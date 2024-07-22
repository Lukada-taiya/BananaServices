namespace Banana.Services.OrderAPI.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetailsList { get; set; }
    }
}
 