using OnlineShopDataUploader.Exceptions;
using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineShopDataUploader.Services
{
    /// <summary>
    /// Парсер XML-файла с форматом, заданным в XML-файле DataExample.xml
    /// </summary>
    public class PurchasesXmlDeserializer : IPurchasesFileDeserializer
    {
        /// <summary>
        /// Получение XML-элементов по их имени внутри родительского элемента
        /// </summary>
        /// <param name="parent">Родительский элемент, содержащий необходимые элементы</param>
        /// <param name="elementsName">Имя элементов</param>
        /// <returns>Список найденных элементов</returns>
        /// <exception cref="XmlElementAbsenceException"><paramref name="parent"/> 
        /// не содержал ни одного элемента с именем <paramref name="elementsName"/></exception>
        private IEnumerable<XElement> GetElements(XElement parent, string elementsName)
        {
            IEnumerable<XElement> elements = parent.Elements(elementsName);

            if (!elements.Any())
            {
                throw new XmlElementAbsenceException(
                    $@"Отсутствовали запрошенные XML-элементы ""{elementsName}""",
                    parent, elementsName);
            }

            return elements;
        }

        /// <summary>
        /// Получение XML-элемента по имени внутри родительского элемента
        /// </summary>
        /// <param name="parent">Родительский элемент, содержащий необходимый элемент</param>
        /// <param name="elementName">Имя элемента</param>
        /// <returns>Найденный элемент</returns>
        /// <exception cref="XmlElementAbsenceException"><paramref name="parent"/> не содержал элемента с именем <paramref name="elementName"/></exception>
        private XElement GetElement(XElement parent, string elementName)
        {
            XElement element = parent.Element(elementName)
                ?? throw new XmlElementAbsenceException(
                    $@"Отсутствовал запрошенный XML-элемент ""{elementName}""",
                    parent, elementName);

            return element;
        }

        /// <summary>
        /// Получение значения XML-элемента по имени внутри родительского элемента
        /// </summary>
        /// <param name="parent">Родительский элемент, содержащий необходимый элемент</param>
        /// <param name="elementName">Имя элемента</param>
        /// <returns>Значение найденного элемента</returns>
        private string GetElementValue(XElement parent, string elementName)
        {
            return GetElement(parent, elementName).Value;
        }

        /// <summary>
        /// Асинхронная десериализация XML-файла с покупками
        /// </summary>
        /// <param name="xmlFilePath">Путь к XML-файлу</param>
        /// <returns><c>Task</c> со списком покупок</returns>
        /// <exception cref="XmlFormatException">XML-файл <paramref name="xmlFilePath"/> не соответствовал формату</exception>
        public async Task<List<Purchase>> DeserializePurchasesAsync(string xmlFilePath)
        {
            XDocument xDoc = await XDocument.LoadAsync(File.OpenRead(xmlFilePath),
                LoadOptions.None, new CancellationToken());
            XElement ordersXml = xDoc.Element("orders")
                ?? throw new XmlFormatException($@"Файл {xmlFilePath} не соответствовал формату", xmlFilePath);

            List<Purchase> purchases = [];

            foreach (XElement orderXml in GetElements(ordersXml, "order"))
            {
                purchases.Add(DeserializePurchase(orderXml));
            }

            return purchases;
        }

        /// <summary>
        /// Десериализация покупки и побочная десериализация записи о покупке продукта 
        /// в определенном размере (класс <c>PurchaseProduct</c>)
        /// </summary>
        /// <param name="purchaseXml">XML-элемент с описанием покупки</param>
        /// <returns>Объект <c>Purchase</c> c десериализованной покупкой</returns>
        private Purchase DeserializePurchase(XElement purchaseXml)
        {
            Customer customer = DeserializeCustomer(GetElement(purchaseXml, "user"));

            int purchaseId = int.Parse(GetElementValue(purchaseXml, "no"));
            decimal totalSum = decimal.Parse(GetElementValue(purchaseXml, "sum"));

            DateOnly registrationDate = DateOnly.ParseExact(GetElementValue(purchaseXml, "reg_date"),
                "yyyy.MM.dd", CultureInfo.InvariantCulture);

            Purchase purchase = new()
            {
                PurchaseId = purchaseId,
                TotalSum = totalSum,
                RegistrationDate = registrationDate,

                Customer = customer,
                PurchaseProducts = new List<PurchaseProduct>(),
            };

            // Из-за различающейся структуры XML-файла и базы данных приходится создавать
            // объекты зависящих от друг друга классов таким образом
            foreach (XElement productXml in GetElements(purchaseXml, "product"))
            {
                Product product = DeserializeProduct(productXml);

                int quantity = int.Parse(GetElementValue(productXml, "quantity"));

                PurchaseProduct purchaseProduct = new()
                {
                    PurchaseId = purchase.PurchaseId,
                    Quantity = quantity,

                    Product = product,
                    Purchase = purchase,
                };

                purchase.PurchaseProducts.Add(purchaseProduct);
            }

            return purchase;
        }

        /// <summary>
        /// Десериализация покупателя
        /// </summary>
        /// <param name="customerXml">XML-элемент с описанием покупателя</param>
        /// <returns>Объект <c>Customer</c> c десериализованным покупателем</returns>
        private Customer DeserializeCustomer(XElement customerXml)
        {
            string fullname = GetElementValue(customerXml, "fio");
            string email = GetElementValue(customerXml, "email");

            return new Customer()
            {
                Fullname = fullname,
                Email = email,
            };
        }

        /// <summary>
        /// Десериализация продукта
        /// </summary>
        /// <param name="productXml">XML-элемент с описанием продукта</param>
        /// <returns>Объект <c>Purchase</c> c десериализованным продуктом</returns>
        private Product DeserializeProduct(XElement productXml)
        {
            string title = GetElementValue(productXml, "name");
            decimal price = decimal.Parse(GetElementValue(productXml, "price"));

            return new Product()
            {
                Title = title,
                Price = price,
            };
        }
    }
}
