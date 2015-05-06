using System;
using System.Collections.Generic;
using System.IO;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using Core.ORM.Attributes;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;


namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [Serializable]
    [DBTable("Rules")]
    public class AdditionRuleWorkTime : Rule, IAdditionWork
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public override RuleKind GetRuleType()
        {
            return RuleKind.AdditionalWorkTime;
        }

        protected override string GetXmlRepresentation()
        {
            var helper = new SerializeHelper<AdditionRuleWorkTime>();
            return helper.GetXml(this);
        }

        protected override void LoadFromXlm()
        {
            var helper = new SerializeHelper<AdditionRuleWorkTime>();
            BuildThisInstance(helper.GetInstance(XmlInformation));
        }

        private void BuildThisInstance(AdditionRuleWorkTime instance)
        {
            this.DayOfWeek = instance.DayOfWeek;
            this.Interval = instance.Interval;
            this.ID = instance.ID;
        }

        public AdditionRuleWorkTime()
        {
            GroupsId = new List<int>();
        }

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            ResolveConnection();
        }

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek, List<int> groupsId)
            : this(interval, dayOfWeek)
        {
            GroupsId = new List<int>(groupsId);
        }
    }
}
