using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ConfirmIt.PortalLib.Rules;
using Core;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public class GroupProvider : IGroupProvider
    {
        private List<UserGroup> _groups;
        public const string TableAccordName = "AccordRules";
        public IList<UserGroup> GetGroupsByRule(int ruleId)
        {
            if (_groups != null) return _groups;
            _groups = GetGroupsIdFromDataBase(ruleId).Select(GetGroupById).ToList();
            return _groups;
        }

        private IEnumerable<int> GetGroupsIdFromDataBase(int ruleId)
        {
            var groupsId = new List<int>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("Select groupId FROM {0} WHERE ruleId = @ruleId", TableAccordName);
                command.Parameters.AddWithValue("@ruleId", ruleId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groupsId.Add((int)reader["groupId"]);
                    }
                }

                connection.Close();
            }
            return groupsId;
        }
        public void AddToGroupId(int ruleId, IEnumerable<int> groupIdentificates)
        {
            var usersFromDataBase = GetGroupsByRule(ruleId).Select(item => item.ID.Value);
            var nonAddingGroups = groupIdentificates.Except(usersFromDataBase);
            AddGroupsInDataBase(ruleId, nonAddingGroups);
        }

        public void RemoveFromGroupId(int ruleId, IEnumerable<int> groupIdentificates)
        {
            var usersFromDataBase = GetGroupsByRule(ruleId).Select(item => item.ID.Value);
            var nonDeletingGroups = usersFromDataBase.Intersect(groupIdentificates);
            DeleteGroupsFromDataBase(ruleId, nonDeletingGroups);
        }

        private void AddGroupsInDataBase(int ruleId, IEnumerable<int> groupIdentificates)
        {
            if (groupIdentificates.Count() == 0) return;

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var groupId in groupIdentificates)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (ruleId, groupId) VALUES  (@ruleId, @groupId)", TableAccordName);
                    command.Parameters.AddWithValue("@ruleId", ruleId);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void DeleteGroupsFromDataBase(int ruleId, IEnumerable<int> groupIdentificates)
        {
            if (groupIdentificates.Count() == 0) return;

            var groupsIdForDeleting = string.Join(",", groupIdentificates);

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE ruleId = @ruleId and groupId in ({1})", TableAccordName, groupsIdForDeleting);
                command.Parameters.AddWithValue("@ruleId", ruleId);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void SaveGroup(UserGroup group)
        {
            group.Save();
        }

        public void DeleteGroup(int id)
        {
            GetGroupById(id).Delete();
        }

        public UserGroup GetGroupById(int id)
        {
            var instance = new UserGroup();
            instance.Load(id);
            return instance;
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