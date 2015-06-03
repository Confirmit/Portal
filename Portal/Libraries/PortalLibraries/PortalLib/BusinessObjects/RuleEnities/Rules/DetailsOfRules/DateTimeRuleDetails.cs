using System;
using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class DateTimeRuleDetails : RuleDetails
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime Time { get; set; }
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }

        public DateTimeRuleDetails()
        {
            
        }

        public DateTimeRuleDetails(DateTime time, params DayOfWeek[] daysOfWeek)
        {
            Time = time;
            DaysOfWeek = new HashSet<DayOfWeek>(daysOfWeek);
        }
    }
}