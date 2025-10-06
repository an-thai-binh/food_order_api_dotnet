using FoodOrderApi.Models;

namespace FoodOrderApi.Dto.Responses
{
    public class FoodWithTotalQuantity
    {
        public Food? Food { get; set; }
        public int? TotalQuantity { get; set; }
    }
}
