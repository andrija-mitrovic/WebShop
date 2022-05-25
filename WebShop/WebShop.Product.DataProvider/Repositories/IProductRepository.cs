using WebShop.Infrastructure.Command.Product;
using WebShop.Infrastructure.Event.Product;

namespace WebShop.Product.DataProvider.Repositories
{
    public interface IProductRepository
    {
        Task<ProductCreated> GetProduct(string productId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
