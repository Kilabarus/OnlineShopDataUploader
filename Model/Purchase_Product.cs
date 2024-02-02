using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Model
{
    internal class Purchase_Product
    {
        public int Purchase_ID { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
    }
}
