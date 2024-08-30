using FoodOrderApi.Models;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Dto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
