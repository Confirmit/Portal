using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Threading;
using Core.Exceptions;

namespace Core.DB
{
    /// <summary>
    /// ���� ������������ ����������.
    /// </summary>
    public enum ConnectionKind
    {
        Default,
        FDPTracker, // TODO: ����������, ����� � ������� ���������� �� ����������� �������� �������� ��������
        Finder
    }

    /// <summary>
    /// ��������� ���� ����������.
    /// </summary>
    public enum ConnectionType
    {
        SQLServer,
        OLEDB,
        Odbc
    }

    /// <summary>
    /// �������� ����������. 
    /// ��������� �������� ������ ������������� ���������� 
    /// �� ����������������� �����, ��� ����������� �� ����������.
    /// </summary>
    public static class ConnectionManager
    {
        #region ������ ��� ��������� ������������ ����� ConnectionKind � ConnectionType

        public delegate ConnectionType ConnectionTypeResolveCallback(ConnectionKind kind);

        /// <summary>
        ///   ����� ��� ���������� ����� ����������.
        /// </summary>
        public static ConnectionTypeResolveCallback ConnectionTypeResolve;

        public static DbProviderFactory GetDBFactory(ConnectionKind connectionKind)
        {
            if (ConnectionTypeResolve == null)
                throw new InvalidOperationException("Please define ConnectionTypeResolve method.");

            ConnectionType type = ConnectionTypeResolve(connectionKind);
            switch (type)
            {
                case ConnectionType.SQLServer:
                    return SqlClientFactory.Instance;

                case ConnectionType.OLEDB:
                    return OleDbFactory.Instance;

                case ConnectionType.Odbc:
                    return OdbcFactory.Instance;

                default:
                    return SqlClientFactory.Instance;
            }
        }
        #endregion

        #region ������ � �������� ��� ������ � ConnectionString

        /// <summary>
        /// ������ ����������� �� ���������
        /// </summary>
        public static string DefaultConnectionString
        {
            get { return m_DefaultConnectionString; }
            set { m_DefaultConnectionString = value; }
        }
        /// <summary>
        /// ���������� ������ ConnectionString �� ���� ���������� 
        /// �� ����������������� �����
        /// </summary>
        /// <param name="connKind">��� ����������</param>
        /// <returns></returns>
        public static string GetConnectionString(ConnectionKind connKind)
        {
            return (connKind == ConnectionKind.Default)
                ? m_DefaultConnectionString
                : ConfigurationManager.ConnectionStrings[connKind.ToString()].ConnectionString;
        }

        /// <summary>
        /// ��������� � ���������� ConnectionString.
        /// </summary>
        /// <param name="dataSource">�������� ������</param>
        /// <param name="userID">��� ������������</param>
        /// <param name="password">������</param>
        /// <returns></returns>
        public static string GetConnectionString(string dataSource, string userID, string password)
        {
            return String.Format("Data Source={0}; User ID={1}; Password={2}", dataSource, userID, password);
        }

        private static string m_DefaultConnectionString = null;

        #endregion

        #region ������ ��� ������ � �������� Connection

        /// <summary>
        /// ���������� ���� ��� �������� ���������� � ����.
        /// </summary>
        /// <param name="connectionKind"></param>
        /// <returns></returns>
        private static string GetConnectionCacheKey(ConnectionKind connectionKind)
        {
            return String.Format("Database_Connection_{0}_{1}", connectionKind, Thread.CurrentThread.ManagedThreadId);
        }
        /// <summary>
        /// ���������� ������ ���������� � ��.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        /// <returns>����������.</returns>
        public static IDbConnection GetConnection(ConnectionKind connectionKind)
        {
            IDbConnection conn = null;

            // ���� � ���� ���������� ���� ���������� (���� ����������), 
            // �� ���������� ���, ����� ������� �����.
            string key = GetConnectionCacheKey(connectionKind);
            object obj = CacheManager.Cache[key];
            if (obj == null || !(obj is IDbConnection))
            {
                conn = GetDBFactory(connectionKind).CreateConnection();
                conn.ConnectionString = GetConnectionString(connectionKind);
            }
            else
                conn = (IDbConnection)obj;

            return conn;
        }

        /// <summary>
        /// ����������, ���� �� � ��� � ���� ���������� (���� �� ����������).
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        /// <returns>���� �� � ��� � ���� ����������.</returns>
        public static bool HasConnectionInCache(ConnectionKind connectionKind)
        {
            // ���� � ���� ���������� ���� ���������� (���� ����������), 
            // �� ���������� true, ����� false.
            string key = GetConnectionCacheKey(connectionKind);
            return (CacheManager.Cache[key] != null) ? true : false;
        }

        /// <summary>
        /// ��������� ���������� � ��.
        /// </summary>
        /// <param name="connection">���������� � ����� ������.</param>
        public static void OpenConnection(IDbConnection connection, ConnectionKind connectionKind)
        {

            if (!HasConnectionInCache(connectionKind) || connection.State != ConnectionState.Open)
                connection.Open();
        }

        /// <summary>
        /// ��������� ���������� � ��.
        /// </summary>
        /// <param name="connection">���������� � ����� ������.</param>
        public static void CloseConnection(IDbConnection connection, ConnectionKind connectionKind)
        {
            if (!HasConnectionInCache(connectionKind) || connection.State != ConnectionState.Closed)
                connection.Close();
        }

        #endregion

        #region ������ ��� ������ � ������������

        /// <summary>
        /// ���������� ���� ��� �������� ���������� � ����.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        /// <returns>���� ����.</returns>
        private static string GetTransactionCacheKey(ConnectionKind connectionKind)
        {
            return String.Format("Database_Transaction_{0}_{1}", connectionKind, Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// ���������� ������� ����������.
        /// </summary>
        /// <returns>������� ����������.</returns>
        public static IDbTransaction GetCurrentTransaction()
        {
            return GetCurrentTransaction(ConnectionKind.Default);
        }
        /// <summary>
        /// ���������� ������� ����������.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        /// <returns>������� ����������.</returns>
        public static IDbTransaction GetCurrentTransaction(ConnectionKind connectionKind)
        {
            object o = CacheManager.Cache[GetTransactionCacheKey(connectionKind)];

            if (o == null || !(o is IDbTransaction))
                return null;
            else
                return (IDbTransaction)o;
        }

        /// <summary>
        /// �������� ���������.
        /// </summary>
        public static void BeginTransaction()
        {
            BeginTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// �������� ���������.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        public static void BeginTransaction(ConnectionKind connectionKind)
        {
            // �������� ����������
            IDbConnection connection = GetConnection(connectionKind);
            // �������� ���������� � ���
            CacheManager.Cache[GetConnectionCacheKey(connectionKind)] = connection;

            // �������� ����������
            IDbTransaction transaction = GetCurrentTransaction(connectionKind);
            if (transaction == null)
            {
                // ���� ���������� ��� ���, �� ��������� ����������
                connection.Open();
                // ������� ����������
                transaction = connection.BeginTransaction();
                CacheManager.Cache[GetTransactionCacheKey(connectionKind)] = transaction;
            }
            else
            {
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("ParallelTransactionException"));
            }
        }

        /// <summary>
        /// ��������� ���������.
        /// </summary>
        public static void CommitTransaction()
        {
            CommitTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// ��������� ����������.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        public static void CommitTransaction(ConnectionKind connectionKind)
        {
            // �������� ����������
            IDbTransaction transaction = GetCurrentTransaction(connectionKind);
            try
            {
                // ������ Commit
                transaction.Commit();
            }
            finally
            {
                IDbConnection connection = GetConnection(connectionKind);
                connection.Close();
                removeFromCache(connectionKind);
            }
        }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        public static void RollbackTransaction()
        {
            RollbackTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// ���������� ����������.
        /// </summary>
        /// <param name="connectionKind">��� ����������.</param>
        public static void RollbackTransaction(ConnectionKind connectionKind)
        {
            // �������� ����������
            IDbTransaction transaction = GetCurrentTransaction(connectionKind);
            try
            {
                transaction.Rollback();
            }
            finally
            {
                IDbConnection connection = GetConnection(connectionKind);
                connection.Close();
                removeFromCache(connectionKind);
            }
        }

        private static void removeFromCache(ConnectionKind connectionKind)
        {
            // ������� ���������� �� ����
            CacheManager.Cache.Remove(GetConnectionCacheKey(connectionKind));

            // ������� ���������� �� ����
            CacheManager.Cache.Remove(GetTransactionCacheKey(connectionKind));
        }

        #endregion
    }
}