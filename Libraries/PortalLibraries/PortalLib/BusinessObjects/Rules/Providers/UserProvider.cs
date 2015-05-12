using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.Providers
{
    public class UserProvider : IUserProvider
    {
        private List<int> _users;
        public string TableAccordName
        {
            get { return "AccordUserGroups"; }
        }

        public IList<int> GetUsersByGroup(int groupId)
        {
            if (_users != null) return _users;
            _users = GetUserIdentificates(groupId);
            return _users;
        }

        private List<int> GetUserIdentificates(int groupId)
        {
            var usersId = new List<int>();
            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("Select userId FROM {0} WHERE groupId = @groupId", TableAccordName);
                command.Parameters.AddWithValue("@groupId", groupId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usersId.Add((int)reader["userId"]);
                    }
                }

                connection.Close();
            }
            return usersId;
        }

        public void AddUserIdentificates(int groupId, IEnumerable<int> userIdentificates)
        {
            var usersFromDataBase = GetUsersByGroup(groupId);
            var nonAddingUsers = userIdentificates.Except(usersFromDataBase);
            AddUsersInDataBase(groupId, nonAddingUsers);
        }

        public void RemoveUserIdentificates(int groupId, IEnumerable<int> userIdentificates)
        {
            var usersFromDataBase = GetUsersByGroup(groupId);
            var nonDeletingusers = usersFromDataBase.Intersect(userIdentificates);
            DeleteUsersFromDataBase(groupId, nonDeletingusers);
        }

        private void AddUsersInDataBase(int groupId, IEnumerable<int> userIdentificates)
        {
            if (userIdentificates.Count() == 0) return;

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var userId in userIdentificates)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (userId, groupId) VALUES  (@userId, @groupId)", TableAccordName);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void DeleteUsersFromDataBase(int groupId, IEnumerable<int> userIdentificates)
        {
            if (userIdentificates.Count() == 0) return;

            var usersIdForDeleting = string.Join(",", userIdentificates);

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE groupId = @groupId and userId in ({1})", TableAccordName, usersIdForDeleting);
                command.Parameters.AddWithValue("@groupId", groupId);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }



        private const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        private void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        private ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
    }
}
