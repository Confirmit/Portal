using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleByTime : RuleXml
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public override int IdType
        {
            get { return 1; }
        }

        protected override void LoadToXml()
        {
            throw new NotImplementedException();
        }

        protected override void LoadFromXlm()
        {
            throw new NotImplementedException();
        }
        
        
        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
            RolesId = new List<int>();
            //ResolveConnection();
        }

        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek, List<int> rolesId) : this(information, time,  dayOfWeek)
        {
            RolesId = new List<int>(rolesId);
        }
    }
}
