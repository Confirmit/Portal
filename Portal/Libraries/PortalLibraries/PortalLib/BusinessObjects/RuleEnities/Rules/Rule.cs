using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public abstract class Rule :BasePlainObject
    {
        private string _xmlInformation;
        private DateTime _beginTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;

        protected RuleDetails RuleDetails;

        [DBRead("BeginTime")]
        public DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        [DBRead("EndTime")]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        [DBRead("TypeId")]
        public int IdType
        {
            get { return (int) RuleType; }
            set { }
        }

        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get
            {
                return _xmlInformation;
            }
            set
            {
                _xmlInformation = value;
               BuildInstance();
            }
        }

        public override void Save()
        {
            _xmlInformation = GetXmlInformation();
            base.Save();          
        }

        protected string GetXmlInformation()
        {
            return new SerializeHelper<RuleDetails>().GetXml(RuleDetails);
        }
        
        public abstract void BuildInstance();

        public abstract RuleKind RuleType { get; }
    }
}
