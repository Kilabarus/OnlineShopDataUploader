using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице Customer в базе данных
/// Сгенерирован <c>EFCore</c>'ом через <c>Scaffold-DbContext</c> 
/// на основе созданной через <c>DB_Creation_Script.sql</c> базы данных
/// </summary>
public partial class Customer
{
    public int CustomerId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
