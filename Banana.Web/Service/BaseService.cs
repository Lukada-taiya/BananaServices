using Banana.Web.Models;
using Banana.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Banana.Web.Utility.StaticData;

namespace Banana.Web.Service
{
    public class BaseService(IHttpClientFactory clientFactory) : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory = clientFactory;
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("BananaAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await httpClient.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound: return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized: return new() { IsSuccess = false, Message = "UnAuthorized" };
                    case HttpStatusCode.Forbidden: return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.InternalServerError: return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }catch(Exception e)
            {
                return new() { IsSuccess = false, Message = e.Message.ToString()};
            }
        }
    }
}
