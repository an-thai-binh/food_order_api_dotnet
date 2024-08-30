using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Cart
{
    public int CartId { get; set; }

    [JsonIgnore]
    public int? CustomerId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual Customer? Customer { get; set; }
}
