using System;

namespace OnlineShopDataUploader.Exceptions
{
    /// <summary>
    /// Исключение для ситуаций, когда формат XML-файла не соответствует заданному примером
    /// </summary>
    public class XmlFormatException : Exception
    {
        public string XmlFilePath { get; }

        public XmlFormatException(string message, string xmlFilePath) : base(message)
        {
            XmlFilePath = xmlFilePath;
        }
    }
}
