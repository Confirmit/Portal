using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace TestOfImplementersOfRules.Factories.TimeEntityFactories
{
    public class TimeEntityFactory
    {
        public TimeEntity GetExpiredTimeEntity()
        {
            var launchTime = DateTime.Now.TimeOfDay;
            var experation = new TimeSpan(0);
            var daysOfweek = new HashSet<DayOfWeek>(Enum.GetNames(typeof(DayOfWeek)).Select(item => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item)));
            var beginTime = DateTime.Now.AddDays(-100);
            var endTime = DateTime.Now.AddDays(100);

            return new TimeEntity(experation, launchTime, daysOfweek, beginTime, endTime);
        }

        public TimeEntity GetDefaultTimeEntity()
        {
            var experation = new TimeSpan(1, 0, 0);
            var launchTime = DateTime.Now.TimeOfDay;
            var daysOfweek = new HashSet<DayOfWeek>(Enum.GetNames(typeof(DayOfWeek)).Select(item => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item)));
            var beginTime = DateTime.Now.AddDays(-10); 
            var endTime = DateTime.Now.AddDays(10);

            return new TimeEntity(experation, launchTime, daysOfweek, beginTime, endTime);
        }

        public TimeEntity GetLongExpirationTimeEntity()
        {
            var experation = new TimeSpan(9999, 9, 9, 9); 
            var launchDateTime = DateTime.Today.AddDays(1).AddMilliseconds(-1).TimeOfDay; 
            var daysOfweek = new HashSet<DayOfWeek>(Enum.GetNames(typeof(DayOfWeek)).Select(item => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), item)));
            var beginTime = new DateTime(2000, 1, 1);
            var endTime = new DateTime(3000, 1, 1); 

            return new TimeEntity(experation, launchDateTime, daysOfweek, beginTime, endTime);
        }

        public IList<TimeEntity> GetTimeEntities(int count, Func<TimeEntity> timeEntityFunc)
        {
            var timeEntities = new List<TimeEntity>();

            for (int i = 0; i < count; i++)
            {
                timeEntities.Add(timeEntityFunc.Invoke());
            }
            return timeEntities;
        }

    }
}