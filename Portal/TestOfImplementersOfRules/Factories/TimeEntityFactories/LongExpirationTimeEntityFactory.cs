using System;
using System.Collections.Generic;
using System.Linq;

namespace TestOfImplementersOfRules.Factories.TimeEntityFactories
{
    public class LongExpirationTimeEntityFactory : TimeEntityFactory
    {
        public override TimeSpan Experation
        {
            get { return new TimeSpan(9999,9,9,9); }
        }

        public override DateTime LaunchDateTime
        {
            get { return DateTime.Today.AddDays(1).AddMilliseconds(-1); }
        }

        public override HashSet<DayOfWeek> DaysOfWeek
        {
            get { return new HashSet<DayOfWeek>(Enum.GetNames(typeof(DayOfWeek))
                .Select(item => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item)));; }
        }

        public override DateTime BeginTime
        {
            get { return new DateTime(2000,1,1);}
        }

        public override DateTime EndTime
        {
            get { return new DateTime(3000, 1, 1); }
        }
    }
}