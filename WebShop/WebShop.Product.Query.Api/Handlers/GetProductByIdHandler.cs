using MassTransit;
using WebShop.Infrastructure.Event.Product;
using WebShop.Infrastructure.Query.Product;
using WebShop.Product.DataProvider.Services;

namespace WebShop.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var product = await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync<ProductCreated>(product);
        }
    }
}
