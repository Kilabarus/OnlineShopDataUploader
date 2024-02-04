using OnlineShopDataUploader.DataAccess;
using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Services
{
    public static class PurchasesInserter
    {
        public static void InsertPurchases(List<Purchase> purchases)
        {
            OnlineShopDbContext db = new();

            foreach (Purchase purchase in purchases)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
            }
        }
    }
}
