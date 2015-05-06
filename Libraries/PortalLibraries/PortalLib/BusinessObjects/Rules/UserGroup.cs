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
    public class UserGroup : ObjectDataBase, IUserGroup
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

        public string TableAccordName
        {
            get { return "AccordUserGroups"; }
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

        public void AddUserId(int id)
        {
            UsersId.Add(id);
        }

        public void RemoveUserId(int id)
        {
            UsersId.Remove(id);
        }

        public override void Save()
        {
            base.Save();

            var usersFromDataBase = GetUsersIdFromDataBase();

            var nonAddingUsers = UsersId.Except(usersFromDataBase);
            var nonDeletingusers = usersFromDataBase.Except(UsersId);

            AddUsersInDataBase(nonAddingUsers);
            DeleteUsersFromDataBase(nonDeletingusers);
        }

        private void AddUsersInDataBase(IEnumerable<int> usersId)
        {
            if (usersId.Count() == 0) return;

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var userid in usersId)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (idUser, idUserGroup) VALUES  (@idUser, @idUserGroup)", TableAccordName);
                    command.Parameters.AddWithValue("@idUser", userid);
                    command.Parameters.AddWithValue("@idUserGroup", ID);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void DeleteUsersFromDataBase(IEnumerable<int> usersId)
        {
            if (usersId.Count() == 0) return;

            var usersIdForDeleting = string.Join(",", usersId);

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE idUserGroup = @idUserGroup and idUser in ({1})", TableAccordName, usersIdForDeleting);
                command.Parameters.AddWithValue("@idUserGroup", ID);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public override void Delete()
        {
            if (ID == null)
                return;

            base.Delete();
        }

        public List<int> GetUsersId()
        {
            if (ID == null)
                throw new NullReferenceException("ID of instance is null");

            if (UsersId.Count != 0) return UsersId;

            UsersId = GetUsersIdFromDataBase();
            return UsersId;
        }

        private List<int> GetUsersIdFromDataBase()
        {
            var usersId = new List<int>();
            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("Select idUser FROM {0} WHERE idUserGroup = @idUserGroup", TableAccordName);
                command.Parameters.AddWithValue("@idUserGroup", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usersId.Add((int)reader["idUser"]);
                    }
                }

                connection.Close();
            }
            return usersId;
        }

    }
}
