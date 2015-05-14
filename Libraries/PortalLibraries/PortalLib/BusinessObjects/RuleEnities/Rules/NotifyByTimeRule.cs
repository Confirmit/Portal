using System;
using System.Collections.Generic;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [DBTable("Rules")]
    public class NotifyByTimeRule : Rule
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public override RuleKind RuleType
        {
            get { return RuleKind.NotifyByTime; }
        }
        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<NotifyByTimeRule>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<NotifyByTimeRule>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }
        private void BuildThisInstance(NotifyByTimeRule instance)
        {
            this.Information = instance.Information;
            this.Time = instance.Time;
            this.DayOfWeek = instance.DayOfWeek;
            this.ID = instance.ID;
        }

        public NotifyByTimeRule(){}

        public NotifyByTimeRule(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
        }
    }
}
