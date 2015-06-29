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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name = string.Empty;

        [DBRead("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DBRead("Description")]
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        public UserGroup(){}

        public UserGroup(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
