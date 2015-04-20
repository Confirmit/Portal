using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ConfirmIt.PortalLib.Rules;
using Core.DB;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules
{
    public abstract class Rule : ObjectDataBase
    {
        public abstract int IdRule { get; }

        public List<int> GroupsId { get; set; }

        public void AddGroupId(int id)
        {
            GroupsId.Add(id);
        }

        public override string NameTableAccord
        {
            get { return "AccordRules"; }
        }

        public override void Save()
        {
            base.Save();

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var groupId in GroupsId)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (idRule, idGroup, idInstance) VALUES  (@idRule, @idGroup, @idInstance)", NameTableAccord);
                    command.Parameters.AddWithValue("@idRule", IdRule);
                    command.Parameters.AddWithValue("@idGroup", groupId);
                    command.Parameters.AddWithValue("@idInstance", ID);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
