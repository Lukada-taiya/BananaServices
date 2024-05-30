using Banana.Services.ShoppingCartAPI.Models.Dto;
using Banana.Services.ShoppingCartAPI.Services.IService;
using Newtonsoft.Json;

namespace Banana.Services.ShoppingCartAPI.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var client = _httpClientFactory.CreateClient("Products");
            var response = await client.GetAsync("/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            return new List<ProductDto>();
        }
    }
}
