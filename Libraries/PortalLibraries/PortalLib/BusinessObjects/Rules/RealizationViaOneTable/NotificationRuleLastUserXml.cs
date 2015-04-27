using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleLastUserXml : RuleXml
    {
        public string Subject { get; set; }

        public override int GetIdType()
        {
            return 2;
        }

        protected override void LoadToXml()
        {
            using (StringWriter stream = new StringWriter())
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleLastUserXml), new[] { typeof(RuleXml) });
                xmlser.Serialize(stream, this);
                _xmlInformation = stream.ToString();
            }
        }

        protected override void LoadFromXlm()
        {
            using (StringReader stream = new StringReader(_xmlInformation))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleLastUserXml), new[] { typeof(RuleXml) });
                var obj = xmlser.Deserialize(stream);
                BuildThisInstance(obj as NotificationRuleLastUserXml);
            }
        }

        private void BuildThisInstance(NotificationRuleLastUserXml instance)
        {
            this.Subject = instance.Subject;
            this.ID = instance.ID;
            this.IdType = instance.IdType;
        }

        public NotificationRuleLastUserXml()
        {
            GroupsId = new List<int>();
        }

        public NotificationRuleLastUserXml(string subject)
        {
            Subject = subject;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleLastUserXml(string subject, List<int> rolesId)
            : this(subject)
        {
            GroupsId = new List<int>(rolesId);
        }
    }
}
