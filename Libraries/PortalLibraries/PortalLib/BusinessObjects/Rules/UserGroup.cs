using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.Rules
{
    [DBTable("UserGroups")]
    public class UserGroup : ObjectDataBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description = string.Empty;

        protected List<int> UsersId { get; set; }

        [DBRead("Description")]
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        public UserGroup()
        {
            UsersId = new List<int>();
        }

        public UserGroup(string description)
            : this()
        {
            Description = description;
            base.ResolveConnection();
        }

        public UserGroup(string description, List<int> usersId)
            : this(description)
        {
            UsersId = new List<int>(usersId);
        }
    }
}
