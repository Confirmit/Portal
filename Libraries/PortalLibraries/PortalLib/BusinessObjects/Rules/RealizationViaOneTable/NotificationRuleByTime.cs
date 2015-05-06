﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleByTime : Rule, INotificationByTime
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public string DayOfWeek { get; set; }

        public override RuleKind GetRuleType()
        {
            return RuleKind.NotificatationByTime;
        }
        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<NotificationRuleByTime>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<NotificationRuleByTime>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }
        private void BuildThisInstance(NotificationRuleByTime instance)
        {
            this.Information = instance.Information;
            this.Time = instance.Time;
            this.DayOfWeek = instance.DayOfWeek;
            this.ID = instance.ID;
        }

        public NotificationRuleByTime()
        {
            GroupsId = new List<int>();
        }

        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
            GroupsId = new List<int>();
            ResolveConnection();
        }

        public NotificationRuleByTime(string information, DateTime time, string dayOfWeek, List<int> groupsId) : this(information, time,  dayOfWeek)
        {
            GroupsId = new List<int>(groupsId);
        }
    }
}
