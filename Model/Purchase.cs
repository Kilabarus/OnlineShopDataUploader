using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Model
{
    internal class Purchase
    {
        public int Purchase_ID { get; set; }
        public int Customer_ID { get; set; }
        public DateOnly Registration_Date { get; set; }
        public decimal Total_Sum { get; set; }
    }
}
