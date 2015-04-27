using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Log
{
    /// <summary>
    /// ��������. ���������� ������ ����������� �� �������.
    /// </summary>
    public class CoreAdoNetAppender : log4net.Appender.AdoNetAppender
    {
        public CoreAdoNetAppender()
        {
            this.ConnectionString = Core.DB.ConnectionManager.GetConnectionString(Core.DB.ConnectionKind.FDPTracker);
            this.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data";
        }
    }
}