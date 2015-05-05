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

        protected override void LoadToXml()
        {
            using (StringWriter stream = new StringWriter())
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(AdditionRuleWorkTime), new []{typeof(Rule)});
                xmlser.Serialize(stream, this);
                _xmlInformation = stream.ToString();
            }
        }

        protected override void LoadFromXlm()
        {
            using (StringReader stream = new StringReader(_xmlInformation))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(AdditionRuleWorkTime), new[] { typeof(Rule) });
                var obj = xmlser.Deserialize(stream);
                BuildThisInstance(obj as AdditionRuleWorkTime);
            }
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
