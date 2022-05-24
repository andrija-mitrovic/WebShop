using MassTransit;
using WebShop.Infrastructure.Command.Product;
using WebShop.Product.DataProvider.Services;

namespace WebShop.Product.Api.Handlers
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private readonly IProductService _service;

        public CreateProductHandler(IProductService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            await _service.AddProduct(context.Message);
            await Task.CompletedTask;
        }
    }
}
