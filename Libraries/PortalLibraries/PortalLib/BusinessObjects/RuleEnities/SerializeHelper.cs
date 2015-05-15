using System.IO;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class SerializeHelper<T> where T : Rule
    {
        public string GetXml(T rule)
        {
            using (var stream = new StringWriter())
            {
                var xmlser = new XmlSerializer(typeof(T), new[] { typeof(Rule) });
                xmlser.Serialize(stream, rule);
                return stream.ToString();
            }
        }

        public T GetInstance(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                var xmlser = new XmlSerializer(typeof(T), new[] { typeof(Rule) });
                var obj = xmlser.Deserialize(stream);
                return (T) obj;
            }
        }
    }
}
