using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{
    public class NotificationRuleLastUser : RuleXml
    {
        public string Subject { get; set; }

        public override int IdType
        {
            get { return 2; }
        }

        protected override void LoadToXml()
        {
            throw new NotImplementedException();
        }

        protected override void LoadFromXlm()
        {
            throw new NotImplementedException();
        }

        public NotificationRuleLastUser(string subject)
        {
            Subject = subject;
            RolesId = new List<int>();
            //ResolveConnection();
        }

        public NotificationRuleLastUser(string subject, List<int> rolesId)
            : this(subject)
        {
            RolesId = new List<int>(rolesId);
        }
    }
}
