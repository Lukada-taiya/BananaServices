using Banana.Services.OrderAPI.Models.Dtos;

namespace Banana.Services.OrderAPI.Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
    }
}
