using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Core.Dictionaries.ExportImport.Serialization.XmlSerialization;
using Core.ORM.Attributes;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;


namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [Serializable]
    [DBTable("Rules")]
    public class AdditionRuleWorkTimeXml : RuleXml
    {
        public string DayOfWeek { get; set; }
        public TimeSpan Interval { get; set; }

        public override int GetIdType()
        {
            return 3;
        }

        protected override void LoadToXml()
        {
            using (StringWriter stream = new StringWriter())
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(AdditionRuleWorkTimeXml), new []{typeof(RuleXml)});
                xmlser.Serialize(stream, this);
                _xmlInformation = stream.ToString();
            }
        }

        protected override void LoadFromXlm()
        {
            using (StringReader stream = new StringReader(_xmlInformation))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(AdditionRuleWorkTimeXml), new[] { typeof(RuleXml) });
                var obj = xmlser.Deserialize(stream);
                BuildThisInstance(obj as AdditionRuleWorkTimeXml);
            }
        }

        private void BuildThisInstance(AdditionRuleWorkTimeXml instance)
        {
            this.DayOfWeek = instance.DayOfWeek;
            this.Interval = instance.Interval;
            this.ID = instance.ID;
            this.IdType = instance.IdType;
        }

        public AdditionRuleWorkTimeXml()
        {
            GroupsId = new List<int>();
        }

        public AdditionRuleWorkTimeXml(TimeSpan interval, string dayOfWeek)
        {
            Interval = interval;
            DayOfWeek = dayOfWeek;
            ResolveConnection();
        }

        public AdditionRuleWorkTimeXml(TimeSpan interval, string dayOfWeek, List<int> rolesId)
            : this(interval, dayOfWeek)
        {
            GroupsId = new List<int>(rolesId);
        }
    }
}
