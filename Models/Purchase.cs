using System;
using System.Collections.Generic;

namespace OnlineShopDataUploader.Models;

/// <summary>
/// Класс, соответствующий таблице Purchase в базе данных
/// Сгенерирован EFCore через <c>Scaffold-DbContext</c>
/// </summary>
public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int? CustomerId { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public decimal TotalSum { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();
}
