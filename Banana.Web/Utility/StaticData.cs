namespace Banana.Web.Utility
{
    public class StaticData
    {
        public static string CouponApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string AuthApiBase { get; set; }
        public static string TokenCookie = "JwtToken";
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
