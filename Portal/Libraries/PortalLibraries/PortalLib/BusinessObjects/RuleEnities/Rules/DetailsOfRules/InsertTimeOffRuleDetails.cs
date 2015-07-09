using System;
using System.Xml.Serialization;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class InsertTimeOffRuleDetails : RuleDetails
    {
        [XmlIgnore]
        public TimeSpan Interval { get; set; }

        public long ExpirationTicks
        {
            get { return Interval.Ticks; }
            set { Interval = new TimeSpan(value); }
        }

        public InsertTimeOffRuleDetails() { }

        public InsertTimeOffRuleDetails(TimeSpan interval, TimeEntity timeInformation) : base(timeInformation)
        {
            Interval = interval;
        }
    }
}
