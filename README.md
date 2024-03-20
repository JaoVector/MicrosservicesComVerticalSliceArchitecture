# Projeto de Microsserviços Utilizando Vertical Slice Architecture, Kubernetes e Docker com GatewayAPI
## Introdução
Pensando em Microsserviços e como eles devem ser desacoplados e especialistas em suas funções, foi eleborada a ideia de também utilizar a Vertical Slice Architecture para
diminuir a complexidade de cada serviço de forma individual, facilitando a manutenção e também o crescimento futuro de cada serviço, no lugar de utilizar a 
clean architecture por exemplo, que geralmente é destinada a projetos de maior complexidade; e exige que cada alteração seja propagada em suas respectivas camadas.
#
## Descrição
No projeto são duas APIS que se comportam como serviços e uma GatewayAPI. A primeira API é a "Game.Catalogo.API" que funciona como o produtor, onde se econtram os Endpoints para se criar os Items de Batalha,
já a API "Game.Inventario.API" consome os dados que são criados pela Game.Catalogo.API e também faz a associação do ItemBatalha com o Personagem para que cada personagem possa ter acesso ao inventário e possa 
acumular itens 
