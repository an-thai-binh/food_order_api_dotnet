using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Orderdetail
{
    public int DetailId { get; set; }

    public int? OrderId { get; set; }

    [JsonIgnore]
    public int? FoodId { get; set; }

    public int? Quantity { get; set; }

    public virtual Food? Food { get; set; }

    [JsonIgnore]
    public virtual Foodorder? Order { get; set; }
}
