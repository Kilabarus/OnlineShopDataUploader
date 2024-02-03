using System;
using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
