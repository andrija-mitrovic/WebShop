using WebShop.Infrastructure.Command.Product;
using WebShop.Infrastructure.Event.Product;

namespace WebShop.Product.DataProvider.Services
{
    public interface IProductService
    {
        Task<ProductCreated> GetProduct(string productId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
