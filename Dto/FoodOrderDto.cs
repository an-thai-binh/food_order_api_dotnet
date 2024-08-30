namespace FoodOrderApi.Dto
{
    public class FoodOrderDto
    {
        public int? CustomerId { get; set; }

        public string? OrderName { get; set; }

        public string? OrderEmail { get; set; }

        public string? OrderPhoneNumber { get; set; }

        public string? OrderAddress { get; set; }

        public DateTime? OrderTime { get; set; }

        public double? TotalPrice { get; set; }

        public string? Status { get; set; }
    }
}
