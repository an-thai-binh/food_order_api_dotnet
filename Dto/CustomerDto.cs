namespace FoodOrderApi.Dto
{
    public class CustomerDto
    {
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public string? Role { get; set; }
    }
}
