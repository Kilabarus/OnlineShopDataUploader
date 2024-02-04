namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице PurchaseProduct в базе данных
/// Сгенерирован <c>EFCore</c>'ом через <c>Scaffold-DbContext</c> 
/// на основе созданной через <c>DB_Creation_Script.sql</c> базы данных
/// </summary>
public partial class PurchaseProduct
{
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
