using System;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace IntegrationTestRules
{
    public class DataBaseHelper
    {
        public void RestoreDatabase()
        {
            ServerConnection connection = new ServerConnection(ServerName, UserName, Password);
            Server sqlServer = new Server(connection);

            Restore rstDatabase = new Restore();
            rstDatabase.Action = RestoreActionType.Database;
            rstDatabase.Database = DataBaseName;

            BackupDeviceItem bkpDevice = new BackupDeviceItem(BackupDatabaseFileName, DeviceType.File);
            rstDatabase.Devices.Add(bkpDevice);
            rstDatabase.ReplaceDatabase = true;
            rstDatabase.SqlRestore(sqlServer);
            
            connection.SqlConnectionObject.Dispose();
            connection.Disconnect();
        }

        private static string BackupDatabaseFileName
        {
            get
            {
                var debugDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                var files = debugDirectory.GetFiles("EmptyPortalBackUp.bak", SearchOption.AllDirectories);
                if (files.Count() == 0) 
                    throw new FileNotFoundException("EmptyPortalBackUp.bak");

                return files.Single().FullName;
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

        private string UserName
        {
            get { return "sa"; }
        }

        private string Password
        {
            get { return "Stupw123!"; }
        }
    }
}