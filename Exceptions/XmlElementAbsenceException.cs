using System;
using System.Xml.Linq;

namespace OnlineShopDataUploader.Exceptions
{
    /// <summary>
    /// Исключение для ситуаций, когда в XML-файле отсутствовал необходимый элемент
    /// </summary>
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
