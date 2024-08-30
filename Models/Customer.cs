using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateOnly? BirthDate { get; set; }

    public ulong? Gender { get; set; }

    public string? Role { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [JsonIgnore]
    public virtual ICollection<Foodorder> Foodorders { get; set; } = new List<Foodorder>();
}
