using System;
using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

public partial class PurchaseProduct
{
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
