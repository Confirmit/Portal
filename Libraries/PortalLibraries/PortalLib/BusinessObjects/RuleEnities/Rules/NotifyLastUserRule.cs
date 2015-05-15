using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class NotifyLastUserRule : Rule
    {
        public string Subject { get; set; }

        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<NotifyLastUserRule>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<NotifyLastUserRule>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyLastUser; }
        }

        private void BuildThisInstance(NotifyLastUserRule instance)
        {
            this.Subject = instance.Subject;
            this.ID = instance.ID;
        }

        public NotifyLastUserRule() { }

        public NotifyLastUserRule(string subject)
        {
            Subject = subject;
        }
    }
}
