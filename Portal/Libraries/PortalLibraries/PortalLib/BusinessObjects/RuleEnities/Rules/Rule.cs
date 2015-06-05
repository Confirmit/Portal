using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public class Rule : BasePlainObject
    {
        private string _xmlInformation;
        private DateTime _beginTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;

        protected RuleDetails RuleDetails;

        public Rule()
        {
            
        }

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
        public int TypeId
        {
            get { return (int)RuleType; }
            set { }
        }

        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get { return _xmlInformation; }
            set { _xmlInformation = value; }
        }

        public override bool Load(int id)
        {
            var success = base.Load(id);
            if (success) DeserializeInstance();
            return success;
        }

        public override void Save()
        {
            _xmlInformation = new SerializeHelper<RuleDetails>().GetXml(RuleDetails);
            base.Save();
        }
        
        public virtual void DeserializeInstance() { }

        public virtual RuleKind RuleType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Visit(Visitor visitor)
        {
            
        }
    }
}
