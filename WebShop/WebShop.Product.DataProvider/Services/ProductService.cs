using WebShop.Infrastructure.Command.Product;
using WebShop.Infrastructure.Event.Product;
using WebShop.Product.DataProvider.Repositories;

namespace WebShop.Product.DataProvider.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            return await _productRepository.AddProduct(product);
        }

        public async Task<ProductCreated> GetProduct(string productId)
        {
            return await _productRepository.GetProduct(productId);
        }
    }
}
