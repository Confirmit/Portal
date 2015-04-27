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
    public class AdditionRuleWorkTime : Rule
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
            this.IdType = instance.IdType;
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

        public AdditionRuleWorkTime(TimeSpan interval, string dayOfWeek, List<int> rolesId)
            : this(interval, dayOfWeek)
        {
            GroupsId = new List<int>(rolesId);
        }
    }
}
