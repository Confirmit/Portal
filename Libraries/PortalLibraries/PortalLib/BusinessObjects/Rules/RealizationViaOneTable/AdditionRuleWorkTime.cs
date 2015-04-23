using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    class AdditionRuleWorkTime : RuleXml
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }
        public override int IdType { get { return 3; } }

        protected override void LoadToXml()
        {
            throw new NotImplementedException();
        }

        protected override void LoadFromXlm()
        {
            throw new NotImplementedException();
        }
        

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            //ResolveConnection();
        }

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek, List<int> rolesId)
            : this(interval, dayOfWeek)
        {
            RolesId = new List<int>(rolesId);
        }
    }
}
