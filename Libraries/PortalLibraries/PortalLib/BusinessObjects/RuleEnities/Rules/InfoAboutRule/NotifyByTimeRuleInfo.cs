using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.InfoAboutRule
{
    public class NotifyByTimeRuleInfo : RuleInfo
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public NotifyByTimeRuleInfo()
        {
            
        }

        public NotifyByTimeRuleInfo(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
        }
    }
}