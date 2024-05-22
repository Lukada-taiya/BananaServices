using Banana.Web.Service.IService;
using Banana.Web.Utility;

namespace Banana.Web.Service
{
    public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticData.TokenCookie);
        }

        public string? GetToken()
        {
            string token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticData.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticData.TokenCookie, token);
        }
    }
}
