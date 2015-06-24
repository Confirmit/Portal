using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class TimeEntity
    {
        public TimeEntity() { }

        public TimeEntity(TimeSpan expirationTime, DateTime launchTime, HashSet<DayOfWeek> daysOfWeek, DateTime beginTime, DateTime endTime)
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

        public DateTime LaunchTime { get; set; }
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        
    }
}