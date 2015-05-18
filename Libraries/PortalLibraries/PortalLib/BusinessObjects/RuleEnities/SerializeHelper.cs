using System.IO;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities
{
    public class SerializeHelper<T> where T : RuleInfo
    {
        public string GetXml(T rule)
        {
            using (var stream = new StringWriter())
            {
                var xmlser = new XmlSerializer(rule.GetType());
                xmlser.Serialize(stream, rule);
                return stream.ToString();
            }
        }

        public T GetInstance(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                var xmlser = new XmlSerializer(typeof(T));
                var obj = xmlser.Deserialize(stream);
                return (T) obj;
            }
        }
    }
}
