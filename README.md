# Projeto de Microsserviços Utilizando Vertical Slice Architecture, Kubernetes e Docker com GatewayAPI
## Introdução
Pensando em Microsserviços e como eles devem ser desacoplados e especialistas em suas funções, foi eleborada a ideia de também utilizar a Vertical Slice Architecture para
diminuir a complexidade de cada serviço de forma individual, facilitando a manutenção e também o crescimento futuro de cada um. 
No lugar de utilizar a 
clean architecture por exemplo, que geralmente é destinada a projetos de maior complexidade; e exige que cada alteração seja propagada em suas respectivas camadas.
#
## Descrição
No projeto são duas APIS que se comportam como serviços e uma GatewayAPI. A primeira API é a "Game.Catalogo.API" que funciona como o produtor, onde se econtram os Endpoints para se criar os Items de Batalha,
já a API "Game.Inventario.API" consome os dados que são criados pela Game.Catalogo.API e também faz a associação do ItemBatalha com o Personagem para que cada personagem possa ter acesso ao inventário e possa 
acumular itens de batalha. E a Game.Gateway.API é responsável por criar um roteamento para os endpoints funcionando como um LoadBalancer de requisições.
#
### Tecnologias Utilizadas
- RabbitMQ
- Masstransit
- .Net 7
- Docker
- Kubernetes
- SQL Server
- Ocelot
#
## Modelo de Dados
### ItemBatalha Categorias
```C#
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemCategoriaEnum
{
   Magia = 1,
   Arma = 2,
   Escudo = 3,
   Armadura = 4
}
```
#
### BaseEntity
```C#
public abstract class BaseEntity
{
    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataAtualizacao { get; set; }
    public DateTimeOffset? DataExclusao { get; set; }
}
```
### ItemBatalha
```C#
public class ItemBatalha : BaseEntity
{
    [Key]
    public Guid ItemId { get; set; }
    [Required]
    [StringLength(65)]
    public string? Nome { get; set; }
    [Required]
    [MaxLength]
    public string? Descricao { get; set; }
    [Required]
    public int Ataque { get; set; }
    [Required]
    public int Defesa { get; set; }
    [Required]
    public ItemCategoriaEnum ItemCategoria { get; set; }
}
```
### ItemInventario
```C#
public class ItemInventario : BaseEntity
{
   [Key]
   public Guid ItemIventId { get; set; }
   public Guid PersonagemId { get; set; }

   public Guid ItemId { get; set; }
   public ItemBatalha? ItemBatalha { get; set; }
}
```

#
### Documento Docker usado para implementar os Microsserviços e também a GatewayAPI.
```yaml
version: '3.9'

services:
  game.gateway.api:
    image: ${DOCKER_REGISTRY-}gamegatewayapi
    container_name: Gateway.Api
    build:
      context: .
      dockerfile: Game.Gateway.Api/Dockerfile
    environment: 
      - ASPNETCORE_ENVIRONMENT=Development
    ports:
      - "5003:80"
    depends_on:
      - rabbitmq
      - catalogo-db
    networks:
      - microsservices

  game.catalogo.api:
    image: ${DOCKER_REGISTRY-}gamecatalogoapi
    container_name: Catalogo.Service
    build:
      context: .
      dockerfile: Game.Catalogo.Api/Dockerfile
    environment: 
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      - rabbitmq
    networks:
      - microsservices

  game.inventario.api:
    image: ${DOCKER_REGISTRY-}gameinventarioapi
    container_name: Inventario.Service
    build:
      context: .
      dockerfile: Game.Inventario.Api/Dockerfile
    environment: 
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      - rabbitmq
    networks:
      - microsservices

  rabbitmq:
    image: rabbitmq:management
    container_name: rabbitmq-amqp
    ports:
      - 5672:5672
      - 15672:15672
    hostname: game-services
    volumes:
      - ./ .containers/queue/data/:/var/lib/rabbitmq
      - ./ .containers/queue/log/:/var/log/rabbitmq
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
    networks:
      - microsservices

  catalogo-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: catalogo-database
    volumes:
      - ./ .containers/catalogo-db:/var/opt/mssql/data
    ports:
      - "1433:1433"
    depends_on:
      - inventario-db
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "<sua-senha>"
      MSSQL_PID: "Express"
      MSSQL_TCP_PORT: "1433"
    networks:
      - microsservices

  inventario-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: inventario-database
    volumes:
      - ./ .containers/inventario-db:/var/opt/mssql/data
    ports:
      - "5432:1433"
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "<sua-senha>"
      MSSQL_PID: "Express"
      MSSQL_TCP_PORT: "5432"
    networks:
      - microsservices

networks:
  microsservices:
     driver: bridge
```
#
### Os arquivos yaml para o Kubernetes podem ser econtrados na pasta K8s.
#
### Configuração do Masstransit
### Produtor
```C#
builder.Services.AddMassTransit(busConfig => 
{
    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.UsingRabbitMq((context, configurator) => 
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), k => 
        {
            k.Username(builder.Configuration["MessageBroker:Username"]);
            k.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});
```
### Consumidor
```C#
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.AddConsumer<ItemBatalhaCriado>();
    busConfig.AddConsumer<ItemBatalhaAtualizado>();
    busConfig.AddConsumer<ItemBatalhaExcluido>();

    busConfig.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), k =>
        {
            k.Username(builder.Configuration["MessageBroker:Username"]);
            k.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});
```
#
### Configuração da GatewayAPI utilizando Ocelot
```json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/gateway/v1/ItemBatalha",
      "UpstreamHttpMethod": [ "Get", "Post" ],
      "DownstreamPathTemplate": "/api/v1/ItemBatalha",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "game.catalogo.api",
          "Port": 80
        }
      ]
    },
    {
      "UpstreamPathTemplate": "/gateway/v1/ItemBatalha/{ItemId}",
      "UpstreamHttpMethod": [ "Get", "Put", "Delete" ],
      "DownstreamPathTemplate": "/api/v1/ItemBatalha/{ItemId}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "game.catalogo.api",
          "Port": 80
        }
      ]
    },

    //Inventario Web Api "api/v1/ItemInventario"
    {
      "UpstreamPathTemplate": "/gateway/v1/ItemInventario",
      "UpstreamHttpMethod": [ "Post" ],
      "DownstreamPathTemplate": "/api/v1/ItemInventario",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "game.inventario.api",
          "Port": 80
        }
      ]
    },
    {
      "UpstreamPathTemplate": "/gateway/v1/ItemInventario/{PersonagemId}",
      "UpstreamHttpMethod": [ "Get" ],
      "DownstreamPathTemplate": "/api/v1/ItemInventario/{PersonagemId}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "game.inventario.api",
          "Port": 80
        }
      ]
    },
    {
      "UpstreamPathTemplate": "/gateway/v1/ItemInventario/{ItemIventId}",
      "UpstreamHttpMethod": [ "Delete" ],
      "DownstreamPathTemplate": "/api/v1/ItemInventario/{ItemIventId}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "game.inventario.api",
          "Port": 80
        }
      ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5003",
    "AdministrationPath": "/administration"
  }
}
```
