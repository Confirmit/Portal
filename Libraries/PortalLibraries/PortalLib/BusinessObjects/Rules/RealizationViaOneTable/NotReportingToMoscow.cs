using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotReportingToMoscow : Rule
    {
        protected override void LoadToXml()
        {
            _xmlInformation = string.Empty;
        }

        protected override void LoadFromXlm()
        {
            _xmlInformation = string.Empty;
        }

        public override int GetIdType()
        {
            return 4;
        }

        public NotReportingToMoscow()
        {
            GroupsId = new List<int>();
        }

        public NotReportingToMoscow(List<int> groupsId)
        {
            GroupsId = new List<int>(groupsId);
        }

    }
}
