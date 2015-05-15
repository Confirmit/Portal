using System;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class AddWorkTimeRule : Rule
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.AddWorkTime; }
        }

        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<AddWorkTimeRule>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<AddWorkTimeRule>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }

        private void BuildThisInstance(AddWorkTimeRule instance)
        {
            this.DayOfWeek = instance.DayOfWeek;
            this.Interval = instance.Interval;
            this.ID = instance.ID;
        }

        public AddWorkTimeRule() { }

        public AddWorkTimeRule(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
        }
    }
}
