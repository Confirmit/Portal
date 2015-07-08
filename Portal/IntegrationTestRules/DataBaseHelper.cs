using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace IntegrationTestRules
{
    public class DataBaseHelper
    {
        public void RestoreDatabaseFromOriginal()
        {
            KillDatabase();
            CopyFiles();
            AttachDatabase();
        }

        private static string BackupDatabaseDirectory
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
                return Path.Combine(testProjectDirectory.FullName, "BackupPortal");
            }
        }

        private static string TestDataBaseDirectory
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
                return Path.Combine(testProjectDirectory.FullName, "DataBaseTestPortal");
            }
        }

        private string DataBaseName
        {
            get { return "TestPortal"; }
        }

        private string ServerName
        {
            get { return @"CO-YAR-WS152\SQLEXPRESS"; }
        }

        private string BackupDatabaseFileName
        {
            get { return Path.Combine(BackupDatabaseDirectory, DataBaseName + ".mdf"); }
        }

        private string TestDataBaseFileName
        {
            get { return Path.Combine(TestDataBaseDirectory, DataBaseName + ".mdf"); }
        }

        private static string GetLogFileName(string databaseFileName)
        {
            return new Regex(".mdf$", RegexOptions.IgnoreCase).Replace(databaseFileName, "_log.ldf");
        }

        private void KillDatabase()
        {
            Server server = new Server(new ServerConnection(ServerName, "sa", "Stupw123!"));

            SqlConnection.ClearAllPools();
            if (server.Databases.Contains(DataBaseName))
            {
                server.KillDatabase(DataBaseName);
            }
        }

        private void CopyFiles()
        {
            File.Copy(BackupDatabaseFileName, TestDataBaseFileName, true);

            string logFileName = GetLogFileName(TestDataBaseFileName);
            File.Copy(GetLogFileName(BackupDatabaseFileName),logFileName, true);

            File.SetAttributes(TestDataBaseFileName, FileAttributes.Normal);
            File.SetAttributes(logFileName, FileAttributes.Normal);
        }

        private void AttachDatabase()
        {
            Server server = new Server(new ServerConnection(ServerName, "sa", "Stupw123!"));
            
            if (!server.Databases.Contains(DataBaseName))
            {
                server.AttachDatabase(DataBaseName, new StringCollection { TestDataBaseFileName, GetLogFileName(TestDataBaseFileName) });
            }
        }
    }
}