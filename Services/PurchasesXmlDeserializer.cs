using OnlineShopDataUploader.Exceptions;
using OnlineShopDataUploader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineShopDataUploader.Services
{
    public static class PurchasesXmlDeserializer
    {        
        private static IEnumerable<XElement> GetElements(XElement parent, string elementsName)
        {
            IEnumerable<XElement> elements = parent.Elements(elementsName)
                ?? throw new XmlElementAbsenceException("Отсутствовали запрошенные XML-элементы", parent, elementsName);

            return elements;
        }

        private static XElement GetElement(XElement parent, string elementName)
        {
            XElement element = parent.Element(elementName)
                ?? throw new XmlElementAbsenceException("Отсутствовал запрошенный XML-элемент", parent, elementName);

            return element;
        }

        private static string GetElementValue(XElement parent, string elementName)
        {            
            return GetElement(parent, elementName).Value;
        }

        public static List<Purchase> DeserializePurchases(string xmlFilePath)
        {
            XDocument xDoc = XDocument.Load(xmlFilePath);
            XElement ordersXml = xDoc.Element("orders") 
                ?? throw new XmlFormatException("Файл не соответствовал формату", xmlFilePath);
            
            List<Purchase> purchases = [];
            
            foreach (XElement orderXml in GetElements(ordersXml, "order"))
            {
                purchases.Add(DeserializePurchase(orderXml));                
            }

            return purchases;
        }
        
        private static Purchase DeserializePurchase(XElement purchaseXml)
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

        private static Customer DeserializeCustomer(XElement customerXml)
        {
            string fullname = GetElementValue(customerXml, "fio");
            string email = GetElementValue(customerXml, "email");

            return new Customer()
            {
                Fullname = fullname,
                Email = email,
            };
        }

        private static Product DeserializeProduct(XElement productXml)
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
