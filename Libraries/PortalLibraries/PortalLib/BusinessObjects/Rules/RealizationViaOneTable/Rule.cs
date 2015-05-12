using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    [DBTable("Rules")]
    public abstract class Rule : ObjectDataBase
    {
        private string _xmlInformation;
        protected IList<UserGroup> _userGroups;

        private DateTime _beginTime = new DateTime(1759,1,1,12,0,0);
        private DateTime _endTime = new DateTime(9999,12,31,11,59,59);

        protected List<int> GroupIdentifiers { get; set; }

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

        [DBRead("IdType")]
        public int IdType
        {
            get { return (int) GetRuleType(); }
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
                LoadFromXlm();
            }
        }
        

        public bool Contains(int userId)
        {
            if (ID == null)
                throw new NullReferenceException("ID of instance is null");

            if (_userGroups == null || _userGroups.Count == 0)
                BuildUserGroups();

            foreach (var group in _userGroups)
            {
                if (group.GetUsersId().Contains(userId)) return true;
            }

            return false;
        }

        public override void Save()
        {
            _xmlInformation = GetXmlRepresentation();
            base.Save();            
        }

        protected abstract string GetXmlRepresentation();

        protected abstract void LoadFromXlm();

        public abstract RuleKind GetRuleType();
    }
}
