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
    /// Типы используемых соединений.
    /// </summary>
    public enum ConnectionKind
    {
        Default,
        FDPTracker, // TODO: переделать, чтобы в базовой библиотеке не встречались названия конечных проектов
        Finder
    }

    /// <summary>
    /// Возможные типы соединений.
    /// </summary>
    public enum ConnectionType
    {
        SQLServer,
        OLEDB,
        Odbc
    }

    /// <summary>
    /// Менеджер соединений. 
    /// Позволяет получать строки инициализации соединений 
    /// из конфигурационного файла, или формировать из параметров.
    /// </summary>
    public static class ConnectionManager
    {
        #region Методы для установки соответствия между ConnectionKind и ConnectionType

        public delegate ConnectionType ConnectionTypeResolveCallback(ConnectionKind kind);

        /// <summary>
        ///   Метод для разрешения типов соединения.
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

        #region Методы и свойства для работы с ConnectionString

        /// <summary>
        /// Строка подключения по умолчанию
        /// </summary>
        public static string DefaultConnectionString
        {
            get { return m_DefaultConnectionString; }
            set { m_DefaultConnectionString = value; }
        }
        /// <summary>
        /// Возвращает строку ConnectionString по типу соединения 
        /// из конфигурационного файла
        /// </summary>
        /// <param name="connKind">тип соединения</param>
        /// <returns></returns>
        public static string GetConnectionString(ConnectionKind connKind)
        {
            return (connKind == ConnectionKind.Default)
                ? m_DefaultConnectionString
                : ConfigurationManager.ConnectionStrings[connKind.ToString()].ConnectionString;
        }

        /// <summary>
        /// Формирует и возвращает ConnectionString.
        /// </summary>
        /// <param name="dataSource">источник данных</param>
        /// <param name="userID">имя пользователя</param>
        /// <param name="password">пароль</param>
        /// <returns></returns>
        public static string GetConnectionString(string dataSource, string userID, string password)
        {
            return String.Format("Data Source={0}; User ID={1}; Password={2}", dataSource, userID, password);
        }

        private static string m_DefaultConnectionString = null;

        #endregion

        #region Методы для работы с объектом Connection

        /// <summary>
        /// Возвращает ключ для хранения соединения в кэше.
        /// </summary>
        /// <param name="connectionKind"></param>
        /// <returns></returns>
        private static string GetConnectionCacheKey(ConnectionKind connectionKind)
        {
            return String.Format("Database_Connection_{0}_{1}", connectionKind, Thread.CurrentThread.ManagedThreadId);
        }
        /// <summary>
        /// Возвращает объект соединения с БД.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        /// <returns>Соединение.</returns>
        public static IDbConnection GetConnection(ConnectionKind connectionKind)
        {
            IDbConnection conn = null;

            // Если в кэше соединений есть соединение (идет транзакция), 
            // то возвращаем его, иначе создаем новое.
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
        /// Определяет, есть ли у нас в кэше соединения (идет ли транзакция).
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        /// <returns>Есть ли у нас в кэше соединение.</returns>
        public static bool HasConnectionInCache(ConnectionKind connectionKind)
        {
            // Если в кэше соединений есть соединение (идет транзакция), 
            // то возвращаем true, иначе false.
            string key = GetConnectionCacheKey(connectionKind);
            return (CacheManager.Cache[key] != null) ? true : false;
        }

        /// <summary>
        /// Открывает соединение с БД.
        /// </summary>
        /// <param name="connection">Соединение с базой данных.</param>
        public static void OpenConnection(IDbConnection connection, ConnectionKind connectionKind)
        {

            if (!HasConnectionInCache(connectionKind) || connection.State != ConnectionState.Open)
                connection.Open();
        }

        /// <summary>
        /// Закрывает соединение с БД.
        /// </summary>
        /// <param name="connection">Соединение с базой данных.</param>
        public static void CloseConnection(IDbConnection connection, ConnectionKind connectionKind)
        {
            if (!HasConnectionInCache(connectionKind) || connection.State != ConnectionState.Closed)
                connection.Close();
        }

        #endregion

        #region Методы для работы с транзакциями

        /// <summary>
        /// Возвращает ключ для хранения соединения в кэше.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        /// <returns>Ключ кэша.</returns>
        private static string GetTransactionCacheKey(ConnectionKind connectionKind)
        {
            return String.Format("Database_Transaction_{0}_{1}", connectionKind, Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// Возвращает текущую транзакцию.
        /// </summary>
        /// <returns>Текущая транзакция.</returns>
        public static IDbTransaction GetCurrentTransaction()
        {
            return GetCurrentTransaction(ConnectionKind.Default);
        }
        /// <summary>
        /// Возвращает текущую транзакцию.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        /// <returns>Текущая транзакция.</returns>
        public static IDbTransaction GetCurrentTransaction(ConnectionKind connectionKind)
        {
            object o = CacheManager.Cache[GetTransactionCacheKey(connectionKind)];

            if (o == null || !(o is IDbTransaction))
                return null;
            else
                return (IDbTransaction)o;
        }

        /// <summary>
        /// Начинает транзацию.
        /// </summary>
        public static void BeginTransaction()
        {
            BeginTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// Начинает транзацию.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        public static void BeginTransaction(ConnectionKind connectionKind)
        {
            // получаем соединение
            IDbConnection connection = GetConnection(connectionKind);
            // помещаем соединение в кэш
            CacheManager.Cache[GetConnectionCacheKey(connectionKind)] = connection;

            // получаем транзакцию
            IDbTransaction transaction = GetCurrentTransaction(connectionKind);
            if (transaction == null)
            {
                // если транзакции еще нет, то открываем соединение
                connection.Open();
                // создаем транзакцию
                transaction = connection.BeginTransaction();
                CacheManager.Cache[GetTransactionCacheKey(connectionKind)] = transaction;
            }
            else
            {
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("ParallelTransactionException"));
            }
        }

        /// <summary>
        /// Завершает транзацию.
        /// </summary>
        public static void CommitTransaction()
        {
            CommitTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// Завершает транзакцию.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        public static void CommitTransaction(ConnectionKind connectionKind)
        {
            // получаем транзакцию
            IDbTransaction transaction = GetCurrentTransaction(connectionKind);
            try
            {
                // делаем Commit
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
        /// Откатывает транзацию.
        /// </summary>
        public static void RollbackTransaction()
        {
            RollbackTransaction(ConnectionKind.Default);
        }

        /// <summary>
        /// Откатывает транзакцию.
        /// </summary>
        /// <param name="connectionKind">Тип соединения.</param>
        public static void RollbackTransaction(ConnectionKind connectionKind)
        {
            // получаем транзакцию
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
            // удаляем соединение их кэша
            CacheManager.Cache.Remove(GetConnectionCacheKey(connectionKind));

            // удаляем транзакцию из кэша
            CacheManager.Cache.Remove(GetTransactionCacheKey(connectionKind));
        }

        #endregion
    }
}