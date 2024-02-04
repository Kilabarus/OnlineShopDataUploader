using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopDataUploader.DataAccess;
using OnlineShopDataUploader.Models;
using OnlineShopDataUploader.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace OnlineShopDataUploader
{
    internal class Program
    {
        static ServiceProvider? serviceProvider;

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

            IPurchasesFileDeserializer purchasesXmlDeserializer = serviceProvider.GetService<IPurchasesFileDeserializer>();
            List<Purchase> purchases = await purchasesXmlDeserializer.DeserializePurchasesAsync(filePath);

            return purchases;
        }

        static async Task InsertPurchasesAsync(List<Purchase> purchases)
        {
            Console.WriteLine("Подключение к базе данных...");
            IPurchaseInserter purchasesInserter = serviceProvider.GetService<IPurchaseInserter>();

            Console.WriteLine($"Добавление записей...");

            int numOfInsertedRecords = 0;
            foreach (Purchase purchase in purchases)
            {
                await purchasesInserter.InsertPurchaseAsync(purchase);
                await Console.Out.WriteLineAsync($"Добавление записей: {++numOfInsertedRecords} / {purchases.Count} {DateTime.Now.Millisecond}");                
            }
        }

        static async Task Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShopDB"].ConnectionString;

            IServiceCollection services = new ServiceCollection()
                .AddDbContext<OnlineShopDbContext>(options => options.UseSqlServer(connectionString))
                .AddTransient<IPurchasesFileDeserializer, PurchasesXmlDeserializer>()
                .AddTransient<IPurchaseInserter, PurchaseInserter>();

            serviceProvider = services.BuildServiceProvider();

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
            catch (Exception e)
            {
                Console.WriteLine("При парсинге файла произошла ошибка:");
                Console.WriteLine(e.Message);

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

                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }
    }
}
