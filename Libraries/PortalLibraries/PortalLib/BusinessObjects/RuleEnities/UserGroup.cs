using System.Collections.Generic;
using System.Diagnostics;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.Rules
{
    [DBTable("UserGroups")]
    public class UserGroup : BasePlainObject
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description = string.Empty;

        [DBRead("Description")]
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        public UserGroup(){}

        public UserGroup(string description)
        {
            Description = description;
        }
    }
}
