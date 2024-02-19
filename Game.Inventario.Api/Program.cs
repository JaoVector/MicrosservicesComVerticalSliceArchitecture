using Carter;
using Game.Common;
using Game.Inventario.Api.Database;
using Game.Inventario.Api.Features.ItensBatalha;
using Game.Inventario.Api.Features.ItensBatalha.Repository;
using Game.Inventario.Api.Features.ItensInventario.Repository;
using Game.Inventario.Api.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(sql => sql.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
builder.Services.AddCarter();

builder.Services.AddScoped<IItemInventarioRepository, ItemInventarioRepository>();
builder.Services.AddScoped<IItemBatalhaRepository, ItemBatalhaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();

//app.UseAuthorization();
//app.MapControllers();

app.Run();
