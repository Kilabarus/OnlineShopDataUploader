using OnlineShopDataUploader.Models;
using OnlineShopDataUploader.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Threading.Tasks;

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

        static async Task<List<Purchase>> DeserializePurchasesAsync(string filePath)
        {
            Console.WriteLine();
            Console.WriteLine("Парсинг файла...");

            List<Purchase> purchases = await PurchasesXmlDeserializer.DeserializePurchasesAsync(filePath);            

            return purchases;
        }

        static async Task InsertPurchasesAsync(List<Purchase> purchases)
        {                        
            Console.WriteLine("Подключение к базе данных...");
            PurchasesInserter purchasesInserter = new();
            
            Console.WriteLine($"Добавление записей...");

            int numOfInsertedRecords = 0;
            foreach (Purchase purchase in purchases)
            {        
                await purchasesInserter.InsertPurchaseAsync(purchase);                
                Console.WriteLine($"Добавление записей: {++numOfInsertedRecords} / {purchases.Count}");
            }
        }

        static async Task Main(string[] args)
        {
            string filePath = GetFilePath(args);
            List<Purchase> purchases;

            try
            {
                purchases = await DeserializePurchasesAsync(filePath);

                if (purchases.Count == 0)
                {
                    Console.WriteLine("Указанный файл не содержал записей о покупках");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("При парсинге файла произошла ошибка:");
                Console.WriteLine($"{ex.Message}");

                return;
            }            

            try
            {
                await InsertPurchasesAsync(purchases);                                
                
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
