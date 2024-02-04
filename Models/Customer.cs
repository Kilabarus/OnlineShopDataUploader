using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице Customer в базе данных
/// Сгенерирован EFCore через <c>Scaffold-DbContext</c>
/// </summary>
public partial class Customer
{
    public int CustomerId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
