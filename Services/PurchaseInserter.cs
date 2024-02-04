using OnlineShopDataUploader.DataAccess;
using OnlineShopDataUploader.Models;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Services
{
    /// <summary>
    /// Простая обертка над сгенерированным EFCore'ом <c>OnlineShopDbContext</c>'ом, 
    /// вставляющая в базу данных записи
    /// </summary>
    public class PurchaseInserter : IPurchaseInserter
    {        
        readonly OnlineShopDbContext db;

        public PurchaseInserter(OnlineShopDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Вставка записей о покупке в базу данных
        /// </summary>
        /// <param name="purchase">Покупка, записи о которой требуется вставить в базу данных</param>
        /// <returns><c>Task</c>, представляющую асинхронную операцию вставки записей о покупке</returns>
        public async Task InsertPurchaseAsync(Purchase purchase)
        {
            await db.Purchases.AddAsync(purchase);
            await db.SaveChangesAsync();
        }
    }
}
