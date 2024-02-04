using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Services
{
    /// <summary>
    /// Интерфейс для классов, реализующих парсинг покупок из файла
    /// </summary>
    public interface IPurchasesFileDeserializer
    {
        Task<List<Purchase>> DeserializePurchasesAsync(string xmlFilePath);
    }
}
