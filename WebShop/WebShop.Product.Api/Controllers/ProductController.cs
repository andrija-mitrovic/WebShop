using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Infrastructure.Command.Product;
using WebShop.Product.DataProvider.Services;

namespace WebShop.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string productId)
        {
            var product = await _productService.GetProduct(productId);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var addedProduct = await _productService.AddProduct(product);

            return Ok(addedProduct);
        }
    }
}
