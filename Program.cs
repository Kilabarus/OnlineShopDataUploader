using System;
using System.Collections.Generic;
using System.IO;

using OnlineShopDataUploader.Models;
using OnlineShopDataUploader.Services;

namespace OnlineShopDataUploader
{
    internal class Program
    {        
        static string GetFilePath(string[] args)
        {
            string? input;

            if (args is not null && args.Length > 0)
            {
                input = args[0];
            }
            else
            {
                Console.WriteLine("Укажите путь к XML-файлу с данными: ");
                input = Console.ReadLine();
            }

            while (input is null || !File.Exists(input))
            {
                Console.WriteLine("Не найдено файла по указанному пути, повторите ввод: ");
                input = Console.ReadLine();
            }

            return input;
        }

        static void Main(string[] args)
        {            
            string filePath = GetFilePath(args);
            
            List<Purchase> purchases = PurchasesXmlDeserializer.DeserializePurchases(filePath);

            if (purchases.Count == 0)
            {
                Console.WriteLine("Указанный файл не содержал записей о покупках");
                return;
            }

            try
            {
                PurchasesInserter.InsertPurchases(purchases);
                Console.WriteLine("Записи из указанного файла были успешно добавлены в базу данных");
            }
            catch (Exception e)
            {
                Console.WriteLine("При вставке записей произошла ошибка:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
