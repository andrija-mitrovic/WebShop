using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebShop.Infrastructure.Command.Product;
using WebShop.Infrastructure.Event.Product;
using WebShop.Infrastructure.Query.Product;

namespace WebShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly IConfiguration _configuration;
        private readonly IRequestClient<GetProductById> _requestClient;

        public ProductController(IBusControl bus, IConfiguration configuration, IRequestClient<GetProductById> requestClient)
        {
            _bus = bus;
            _configuration = configuration;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string productId)
        {
            var product = new GetProductById() { ProductId = productId };
            var response = await _requestClient.GetResponse<ProductCreated>(product);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(product);

            return Ok("Product Created");
        }
    }
}
