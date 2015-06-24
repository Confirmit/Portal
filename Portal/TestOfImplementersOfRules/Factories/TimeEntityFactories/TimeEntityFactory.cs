using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestOfImplementersOfRules.Factories
{
    public abstract class TimeEntityFactory : ITimeEntityFactory
    {
        private const int defaultNumber = 5;

        public abstract TimeSpan Experation { get; }
        public abstract DateTime LaunchDateTime { get;}
        public abstract HashSet<DayOfWeek> DaysOfWeek { get;}
        
        public List<TimeEntity> GetTimeEntities(int number = defaultNumber)
        {
            var timeEntities = new List<TimeEntity>();
            for (int i = 0; i < number; i++)
            {
                timeEntities.Add(new TimeEntity(Experation, LaunchDateTime, DaysOfWeek));
            }
            return timeEntities;
        }
    }
}