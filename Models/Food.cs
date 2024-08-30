using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Food
{
    public int FoodId { get; set; }

    [JsonIgnore]
    public int? CategoryId { get; set; }

    public string? FoodName { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public string? ImgUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual Category? Category { get; set; }

    [JsonIgnore]
    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
