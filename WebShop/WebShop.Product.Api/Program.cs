using MassTransit;
using WebShop.Infrastructure.EventBus;
using WebShop.Infrastructure.Mongo;
using WebShop.Product.Api.Handlers;
using WebShop.Product.DataProvider.Repositories;
using WebShop.Product.DataProvider.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddMongoDb(configuration);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<CreateProductHandler>();

var rabbitmqOption = new RabbitMqOption();
configuration.GetSection("rabbitmq").Bind(rabbitmqOption);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateProductHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitmqOption.ConnectionString), hostconfig =>
        {
            hostconfig.Username(rabbitmqOption.Username);
            hostconfig.Password(rabbitmqOption.Password);
        });

        cfg.ReceiveEndpoint("create_product", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(2, 100);
            });
            ep.ConfigureConsumer<CreateProductHandler>(provider);
        });
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

app.UseHttpsRedirection();

app.UseAuthorization();
var dbInitializer = app.Services.GetService<IDatabaseInitializer>();
await dbInitializer.InitializeAsync();

app.MapControllers();
var busControl = app.Services.GetService<IBusControl>();
busControl.Start();

app.Run();
