namespace Banana.Services.EmailAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetailsList { get; set; }
    }
}
 