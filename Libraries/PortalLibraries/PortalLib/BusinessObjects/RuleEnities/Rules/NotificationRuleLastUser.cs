using System.Collections.Generic;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [DBTable("Rules")]
    public class NotificationRuleLastUser : Rule
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

        public NotificationRuleLastUser(){}

        public NotificationRuleLastUser(string subject)
        {
            Subject = subject;
        }
    }
}
