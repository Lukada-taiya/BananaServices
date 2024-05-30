using Banana.Services.ShoppingCartAPI.Models.Dto;

namespace Banana.Services.ShoppingCartAPI.Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
    }
}
