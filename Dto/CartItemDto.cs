using System.Text.Json.Serialization;

namespace FoodOrderApi.Dto
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
