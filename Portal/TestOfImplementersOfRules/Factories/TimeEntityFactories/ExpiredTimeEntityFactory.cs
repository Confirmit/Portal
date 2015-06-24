using System;
using System.Collections.Generic;
using System.Linq;

namespace TestOfImplementersOfRules.Factories.TimeEntityFactories
{
    public class ExpiredTimeEntityFactory : TimeEntityFactory
    {
        public override TimeSpan Experation
        {
            get { return new TimeSpan(0);}
        }

        public override DateTime LaunchDateTime
        {
            get { return DateTime.Now.AddMilliseconds(-1); }
        }

        public override HashSet<DayOfWeek> DaysOfWeek
        {
            get { return new HashSet<DayOfWeek>(Enum.GetNames(typeof(DayOfWeek)).Select(item => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item))); }
        }

        public override DateTime BeginTime
        {
            get { return DateTime.Now.AddDays(-1); }
        }

        public override DateTime EndTime
        {
            get { return DateTime.Now.AddDays(100); }
        }
    }
}