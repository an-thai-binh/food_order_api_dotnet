using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryImgUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
