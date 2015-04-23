using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.Rules;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{

    [DBTable("Rules")]
    public abstract class RuleXml : BasePlainObject
    {
        [DBRead("IdType")]
        public abstract int IdType { get; }

        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get
            {
                if (_xmlInformation != null)
                    return _xmlInformation;

                LoadToXml();
                return _xmlInformation;
            }
            set
            {
                _xmlInformation = value;
                LoadFromXlm();
            }
        }

        public List<int> RolesId { get; set; }

        protected string _xmlInformation;

        protected abstract void LoadToXml();

        protected abstract void LoadFromXlm();
    }
}
