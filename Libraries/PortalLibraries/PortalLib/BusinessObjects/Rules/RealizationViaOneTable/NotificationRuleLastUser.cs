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

        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<NotificationRuleLastUser>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<NotificationRuleLastUser>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }

        public override RuleKind GetRuleType()
        {
            return RuleKind.NotificationLastUser;
        }

        private void BuildThisInstance(NotificationRuleLastUser instance)
        {
            this.Subject = instance.Subject;
            this.ID = instance.ID;
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

        public NotificationRuleLastUser(string subject, List<int> groupsId)
            : this(subject)
        {
            GroupsId = new List<int>(groupsId);
        }
    }
}
