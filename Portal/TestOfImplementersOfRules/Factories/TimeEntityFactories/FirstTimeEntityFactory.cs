using System;
using System.Collections.Generic;

namespace TestOfImplementersOfRules.Factories.TimeEntityFactories
{
    public class FirstTimeEntityFactory : TimeEntityFactory
    {
        public override TimeSpan Experation
        {
            get
            {
                return new TimeSpan(1, 0, 0);
            }

        }

        public override DateTime LaunchDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public override HashSet<DayOfWeek> DaysOfWeek
        {
            get
            {
                return new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            }

        }
    }
}