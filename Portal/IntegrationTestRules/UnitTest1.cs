using System;
using System.Configuration;
using System.IO;
using System.Linq;

using Core.DB;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTestRules
{
    public static class Manager
    {
        //public const string Connection = "Data Source=CO-YAR-WS152\\SQLEXPRESS;Initial Catalog=Portal;Integrated Security=True";

        public static void ResolveConnection()
        {
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString; ;
        }

        private static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            Manager.ResolveConnection();
        }
        private static string TestDatabaseDirectory
        {
            get
            {
                var debugDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                DirectoryInfo binDirectory = debugDirectory.Parent;
                DirectoryInfo testProjectDirectory;
                if (binDirectory == null || (testProjectDirectory = binDirectory.Parent) == null)
                {
                    throw new Exception("");
                }
                return Path.Combine(testProjectDirectory.FullName, "Database");
            }
        }	

        [TestMethod]
        public void TestMethod1()
        {                       
            
        }
    }
}
