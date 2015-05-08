using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
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
