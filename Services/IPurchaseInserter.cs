using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Services
{
    /// <summary>
    /// Интерфейс для классов, реализующих вставку записей о покупках в базу данных
    /// </summary>
    internal interface IPurchaseInserter
    {
        Task InsertPurchaseAsync(Purchase purchase);
    }
}
