using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Banana.Services.OrderAPI.Models.Dtos
{
    public class CartDetailsDto
    { 
        public int? CartDetailsId { get; set; }
        public int? CartHeaderId { get; set; } 
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; } 
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
    }
}
