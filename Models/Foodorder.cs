using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Foodorder
{
    public int OrderId { get; set; }

    [JsonIgnore]
    public int? CustomerId { get; set; }

    public string? OrderName { get; set; }

    public string? OrderEmail { get; set; }

    public string? OrderPhoneNumber { get; set; }

    public string? OrderAddress { get; set; }

    public DateTime? OrderTime { get; set; }

    public double? TotalPrice { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    [JsonIgnore]
    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
