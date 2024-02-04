using OnlineShopDataUploader.DataAccess;
using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Services
{
    public class PurchasesInserter
    {
        readonly OnlineShopDbContext db = new();

        public async Task InsertPurchaseAsync(Purchase purchase)
        {
            await db.Purchases.AddAsync(purchase);
            await db.SaveChangesAsync();
        }
    }
}
