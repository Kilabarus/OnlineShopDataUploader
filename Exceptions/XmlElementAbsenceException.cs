using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineShopDataUploader.Exceptions
{
    internal class XmlElementAbsenceException : Exception
    {
        public XElement Parent { get; }
        public string ElementName { get; }

        public XmlElementAbsenceException(string message, XElement parent, string elementName) 
            : base(message)
        {
            Parent = parent;
            ElementName = elementName;
        }
    }
}
