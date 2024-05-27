using Banana.Web.Models;

namespace Banana.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetProductAsync(int id);
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductByNameAsync(string name);
        Task<ResponseDto> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto> DeleteProductAsync(int id);
    }
}
