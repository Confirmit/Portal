using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core;
using Core.DB;
using Core.ORM.Attributes;
using UlterSystems.PortalLib.DB;

namespace ConfirmIt.PortalLib.Rules
{
    [DBTable("UserGroups")]
    public class Role : ObjectDataBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description = string.Empty;

        public List<int> UsersId { get; set; }

        [DBRead("Description")]
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        public Role(string description)
        {
            Description = description;
            base.ResolveConnection();
        }

        public Role(string description, List<int> usersId)
            : this(description)
        {
            UsersId = new List<int>(usersId);
        }

        public void AddUserId(int id)
        {
            UsersId.Add(id);
        }

        public override void Save()
        {
            base.Save();

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var userid in UsersId)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (idUser, idUserGroup) VALUES  (@idUser, @idUserGroup)", NameTableAccord);
                    command.Parameters.AddWithValue("@idUser", userid);
                    command.Parameters.AddWithValue("@idUserGroup", ID);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public override void Delete()
        {
            if (ID == null)
                return;

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE idUserGroup = @idUserGroup", NameTableAccord);
                command.Parameters.AddWithValue("@idUserGroup", ID);
                command.ExecuteNonQuery();

                connection.Close();
            }
            base.Delete();
        }


        public override string NameTableAccord
        {
            get { return "GroupingUsers"; }
        }
    }
}
