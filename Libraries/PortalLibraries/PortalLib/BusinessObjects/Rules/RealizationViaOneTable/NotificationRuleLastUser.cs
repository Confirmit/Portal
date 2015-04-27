using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleLastUser : Rule, INotificationLastUser
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
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleLastUser), new[] { typeof(Rule) });
                xmlser.Serialize(stream, this);
                _xmlInformation = stream.ToString();
            }
        }

        protected override void LoadFromXlm()
        {
            using (StringReader stream = new StringReader(_xmlInformation))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(NotificationRuleLastUser), new[] { typeof(Rule) });
                var obj = xmlser.Deserialize(stream);
                BuildThisInstance(obj as NotificationRuleLastUser);
            }
        }

        private void BuildThisInstance(NotificationRuleLastUser instance)
        {
            this.Subject = instance.Subject;
            this.ID = instance.ID;
            this.IdType = instance.IdType;
        }

        public NotificationRuleLastUser()
        {
            GroupsId = new List<int>();
        }

        public NotificationRuleLastUser(string subject)
        {
            Subject = subject;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleLastUser(string subject, List<int> rolesId)
            : this(subject)
        {
            GroupsId = new List<int>(rolesId);
        }
    }
}
