using Banana.Web.Models;
using Banana.Web.Service.IService;
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class ProductService(IBaseService baseService) : IProductService
    {
        private readonly IBaseService _service = baseService;

        public async Task<ResponseDto> CreateProductAsync(ProductDto productDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = productDto,
                Url = ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = ProductApiBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponseDto> GetProductAsync(int id)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductApiBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto> GetProductByNameAsync(string name)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductApiBase + "/api/product/GetByName/" + name
            });
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto productDto)
        {
            return await _service.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = ProductApiBase + "/api/product",
                Data = productDto
            });
        }
    }
}
