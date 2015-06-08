using System;
using System.IO;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class SerializeHelper<T> where T : RuleDetails
    {
        public string GetXml(T ruleDetails)
        {
            using (var stream = new StringWriter())
            {
                var xmlSerializer = new XmlSerializer(ruleDetails.GetType(), new[] {typeof(RuleDetails)} );
                xmlSerializer.Serialize(stream, ruleDetails);
                return stream.ToString();
            }
        }

        public T GetInstance(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                var obj = xmlSerializer.Deserialize(stream);
                return (T)obj;
            }
        }
    }
}
