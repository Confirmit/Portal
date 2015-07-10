using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class TimeEntity
    {
        public TimeEntity() { }

        public TimeEntity(TimeSpan expirationTime, TimeSpan launchTime, 
            HashSet<DayOfWeek> daysOfWeek, DateTime beginTime, DateTime endTime)
        {
            ExpirationTime = expirationTime;
            LaunchTime = launchTime;
            DaysOfWeek = daysOfWeek;
            BeginTime = beginTime;
            EndTime = endTime;
        }
       
        [XmlIgnore]
        public TimeSpan ExpirationTime { get; set; }

        public long ExpirationTicks
        {
            get { return ExpirationTime.Ticks; }
            set {  ExpirationTime = new TimeSpan(value);}
        }

        private TimeSpan _launchTime;
        [XmlIgnore]
        public TimeSpan LaunchTime
        {
            get { return _launchTime; }
            set
            {
                if(value.Days >= 1)
                    throw new ArgumentOutOfRangeException("LaunchTime", "The value must be between 0 and 24:00:00");
                _launchTime = value;
            }
        }

        public long LaunchTicks
        {
            get { return LaunchTime.Ticks; }
            set { LaunchTime = new TimeSpan(value); }
        }

        public HashSet<DayOfWeek> DaysOfWeek { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        
    }
}