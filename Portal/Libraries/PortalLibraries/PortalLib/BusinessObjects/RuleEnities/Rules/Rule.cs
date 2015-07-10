using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public abstract class Rule : BasePlainObject
    {
        protected RuleDetails RuleDetails;

        public TimeEntity TimeInformation
        {
            get { return RuleDetails.TimeInformation; }
            set { RuleDetails.TimeInformation = value; }
        }

        private string _description;
        [DBRead("Description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DBRead("TypeId")]
        public int TypeId
        {
            get { return (int)RuleType; }
            set { }
        }

        private string _xmlInformation;
        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get { return _xmlInformation; }
            set { _xmlInformation = value; }
        }

        protected Rule(string description)
        {
            _description = description;
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

        public abstract void DeserializeInstance();

        public abstract RuleKind RuleType { get; }

        public abstract void Visit(RuleVisitor ruleVisitor, RuleInstance ruleInstance);
    }
}
