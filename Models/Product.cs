using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице Product в базе данных
/// Сгенерирован <c>EFCore</c>'ом через <c>Scaffold-DbContext</c> 
/// на основе созданной через <c>DB_Creation_Script.sql</c> базы данных
/// </summary>
public partial class Product
{
    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
}
