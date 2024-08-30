using System;
using System.Collections.Generic;

namespace FoodOrderApi.Models;

public partial class Imageinfo
{
    public int ImageId { get; set; }

    public string? Url { get; set; }

    public DateTime? UploadTime { get; set; }
}
