using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SaaSWebAPI
{
    public class Common
    {
        /// <summary>
        /// Serialize an object to xml string
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="obj">The object to serialize</param>
        /// <returns>An XML representation of the object without the XML declaration.</returns>
        ///
        public static string GetXMLFromObject<T>(object obj) where T : class
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer x = new XmlSerializer(typeof(T));
            //XmlWriter myWriter = new StringWriter();

            x.Serialize(writer, obj, namespaces);

            return sb.ToString();
        }
    }
}