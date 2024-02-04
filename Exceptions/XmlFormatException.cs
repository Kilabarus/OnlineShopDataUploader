using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDataUploader.Exceptions
{

	public class XmlFormatException : Exception
	{
        public string XmlFilePath { get; }

        public XmlFormatException(string message, string xmlFilePath) : base(message) 
        {
            XmlFilePath = xmlFilePath;
        }
    }
}
