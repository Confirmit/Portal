using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleByTimeXml : RuleXml
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public override int GetIdType()
        {
            return 1; 
        }

        protected override void LoadToXml()
        {
            using (StringWriter stream = new StringWriter())
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleByTimeXml), new[] { typeof(RuleXml) });
                xmlser.Serialize(stream, this);
                _xmlInformation = stream.ToString();
            }
        }

        protected override void LoadFromXlm()
        {
            using (StringReader stream = new StringReader(_xmlInformation))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleByTimeXml), new[] { typeof(RuleXml) });
                var obj = xmlser.Deserialize(stream);
                BuildThisInstance(obj as NotificationRuleByTimeXml);
            }
        }

        private void BuildThisInstance(NotificationRuleByTimeXml instance)
        {
            this.Information = instance.Information;
            this.Time = instance.Time;
            this.DayOfWeek = instance.DayOfWeek;
            this.ID = instance.ID;
            this.IdType = instance.IdType;
        }
        
        
        public NotificationRuleByTimeXml(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
            RolesId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleByTimeXml(string information, DateTime time, string dayOfWeek, List<int> rolesId) : this(information, time,  dayOfWeek)
        {
            RolesId = new List<int>(rolesId);
        }
    }
}
