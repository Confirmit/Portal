using System;
using System.Collections.Generic;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities
{
    public class TimeEntity
    {
        public TimeEntity()
        {
            
        }

        public TimeEntity(TimeSpan expirationTime, DateTime launchTime, HashSet<DayOfWeek> daysOfWeek)
        {
            ExpirationTime = expirationTime;
            LaunchTime = launchTime;
            DaysOfWeek = daysOfWeek;
        }

        public TimeSpan ExpirationTime { get; set; }
        public DateTime LaunchTime { get; set; }
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }
    }
}