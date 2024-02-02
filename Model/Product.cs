using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Model
{
    internal class Product
    {
        public int Product_ID { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
    }
}
