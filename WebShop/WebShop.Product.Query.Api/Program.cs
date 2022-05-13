using MassTransit;
using WebShop.Infrastructure.EventBus;
using WebShop.Infrastructure.Mongo;
using WebShop.Product.DataProvider.Repositories;
using WebShop.Product.DataProvider.Services;
using WebShop.Product.Query.Api.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddMongoDb(configuration);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<GetProductByIdHandler>();
//establish connection with rabbitMQ...
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GetProductByIdHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        var rabbitMq = new RabbitMqOption();
        configuration.GetSection("rabbitmq").Bind(rabbitMq);

        cfg.Host(new Uri(rabbitMq.ConnectionString), hostcfg =>
        {
            hostcfg.Username(rabbitMq.Username);
            hostcfg.Password(rabbitMq.Password);
        });
        cfg.ConfigureEndpoints(provider);
    }));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var bus = app.Services.GetService<IBusControl>();
bus.Start();

var dbInitializer = app.Services.GetService<IDatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
