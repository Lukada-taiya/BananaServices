namespace Banana.Web.Models
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetailsList { get; set; }
    }
}
 