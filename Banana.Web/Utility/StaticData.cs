namespace Banana.Web.Utility
{
    public class StaticData
    {
        public static string CouponApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string ShoppingCartApiBase { get; set; }
        public static string OrderApiBase { get; set; }
        public static string AuthApiBase { get; set; }
        public static string TokenCookie = "JwtToken";
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
