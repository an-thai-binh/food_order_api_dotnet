namespace FoodOrderApi.Dto
{
    public class FoodDto
    {
        public int CategoryId { get; set; }
        public string? FoodName { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? ImgUrl { get; set; }
    }
}
