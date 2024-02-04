using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице Product в базе данных
/// Сгенерирован EFCore через <c>Scaffold-DbContext</c>
/// </summary>
public partial class Product
{
    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
}
