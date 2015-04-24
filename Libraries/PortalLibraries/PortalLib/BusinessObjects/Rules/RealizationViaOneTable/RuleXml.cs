using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Core;
using Core.DB;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{

    [Serializable]
    [DBTable("Rules")]
    public abstract class RuleXml : BasePlainObject
    {
        protected string _xmlInformation;

        [DBRead("IdType")]
        public int IdType
        {
            get { return GetIdType(); }
            set { }
        }

        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get
            {
                return _xmlInformation;
            }
            set
            {
                _xmlInformation = value;
                LoadFromXlm();
            }
        }

        public List<int> RolesId { get; set; }

        public const string TableNameAccord = "AccordRules2";

        public override void Save()
        {
            LoadToXml();
            base.Save();

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var idRole in RolesId)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (idRule, idRole) VALUES  (@idRule, @idRole)", TableNameAccord);
                    command.Parameters.AddWithValue("@idRule", IdType);
                    command.Parameters.AddWithValue("@idRole", idRole);
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
                    string.Format("DELETE FROM {0} WHERE idRule = @idRule", TableNameAccord);
                command.Parameters.AddWithValue("@idRule", ID);
                command.ExecuteNonQuery();

                connection.Close();
            }
            base.Delete();
        }

        public const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        public void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = Connection;
        }

        protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

        protected abstract void LoadToXml();

        protected abstract void LoadFromXlm();

        public abstract int GetIdType();
    }
}
