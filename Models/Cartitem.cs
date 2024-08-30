using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderApi.Models;

public partial class Cartitem
{
    public int ItemId { get; set; }

    [JsonIgnore]
    public int? CartId { get; set; }

    [JsonIgnore]
    public int? FoodId { get; set; }

    public int? Quantity { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Food? Food { get; set; }
}
