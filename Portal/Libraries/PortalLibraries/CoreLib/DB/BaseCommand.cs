using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

using Core.Exceptions;

namespace Core.DB
{
    /// <summary>
    /// Базовый класс для sql-команды.
    /// </summary>
    public abstract class BaseCommand : IBaseCommand
    {
        #region Поля

        private IDbConnection m_Connection;
        private IDbCommand m_Command;
        private ConnectionKind m_ConnectionKind;
        private DbProviderFactory m_DBFactory;

        #endregion

        #region Свойства

        /// <summary>
        /// Параметры команды
        /// </summary>
        public IDataParameterCollection Parameters
        {
            get { return m_Command.Parameters; }
        }

        /// <summary>
        /// Возвращает SQL-команду данной процедуры
        /// </summary>
        public IDbCommand Command
        {
            get { return m_Command; }
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует объект процедуры.
        /// </summary>
        /// <param name="commandText">Текст команды (имя процедуры, SQL-скрипт).</param>
        /// <param name="commandType">Тип команды.</param>
        /// <param name="connectionKind">Тип подключения.</param>
        protected BaseCommand(string commandText, CommandType commandType, ConnectionKind connectionKind)
        {
            m_ConnectionKind = connectionKind;
            m_Connection = ConnectionManager.GetConnection(connectionKind);
            m_DBFactory = ConnectionManager.GetDBFactory(connectionKind);
            
            m_Command = m_Connection.CreateCommand();
            m_Command.Transaction = ConnectionManager.GetCurrentTransaction(connectionKind);
            m_Command.CommandText = commandText;
            m_Command.CommandType = commandType;
        }

        #endregion

        #region Методы работы с соединением

        /// <summary>
        /// Освобождает ресурсы, связанные с процедурой (команда, соединение)
        /// </summary>
        public void Destroy()
        {
            try
            {
                if (m_Command != null)
                    if (m_Command.Connection != null)
                        CloseConnection();

                if (m_Connection != null)
                    if (m_Connection.State == ConnectionState.Open)
                        CloseConnection();
            }
            catch
            { }
        }

        /// <summary>
        /// Открывает соединение к БД.
        /// </summary>
        private void OpenConnection()
        {
            try
            {
                ConnectionManager.OpenConnection(m_Connection, m_ConnectionKind);
            }
            catch (Exception ex)
            {
                throw new CoreException(Resources.ResourceManager.GetString("OpenConnectionException", ex.Message), ex);
            }
        }

        /// <summary>
        /// Закрывает соединение к БД.
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                ConnectionManager.CloseConnection(m_Connection, m_ConnectionKind);
            }
            catch (Exception ex)
            {
                throw new CoreException(Resources.ResourceManager.GetString("OpenConnectionException", ex.Message), ex);
            }
        }

        #endregion

        #region Методы для работы с параметрами

        /// <summary>
        /// Добавляет входной параметр
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        public IDbDataParameter Add(string name, object value)
        {
            return Add(name, value, ParameterDirection.Input);
        }

        /// <summary>
        /// Добавляет входной параметр
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="value">значение</param>
        /// <param name="type">тип</param>
        /// <returns></returns>
        public IDbDataParameter Add(string name, object value, DbType type)
        {
            return Add(name, value, type, ParameterDirection.Input);
        }

        /// <summary>
        /// Добавляет выходной параметр
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        public IDbDataParameter AddOutput(string name, object value)
        {
            return Add(name, value, ParameterDirection.Output);
        }

        /// <summary>
        /// Добавляет параметр
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="value">значение</param>
        /// <param name="direction">направление</param>
        /// <returns></returns>
        protected IDbDataParameter Add(string name, object value, ParameterDirection direction)
        {
            IDbDataParameter parameter = m_Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = direction;
            return Add(parameter);
        }

        /// <summary>
        /// Добавляет параметр
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="value">значение</param>
        /// <param name="type">тип</param>
        /// <param name="direction">направление</param>
        /// <returns></returns>
        protected IDbDataParameter Add(string name, object value, DbType type, ParameterDirection direction)
        {
            IDbDataParameter parameter = m_Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            parameter.Direction = direction;
            return Add(parameter);
        }

        /// <summary>
        /// Добавляет параметр
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDbDataParameter Add(IDbDataParameter param)
        {
            int pos = m_Command.Parameters.Add(param);
            return (IDbDataParameter)m_Command.Parameters[pos];
        }

        /// <summary>
        /// Добавляет параметр - возвращаемое значение
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter AddReturnValueParameter()
        {
            IDbDataParameter parameter = m_Command.CreateParameter();
            parameter.ParameterName = "@RETURN_VALUE";
            parameter.DbType = DbType.Int32;
            parameter.Direction = ParameterDirection.ReturnValue;
            return Add(parameter);
        }

        /// <summary>
        /// Возвращает числовое значение, возвращаемое из процедуры оператором RETURN
        /// </summary>
        /// <returns></returns>
        public object GetReturnValue()
        {
            IDbDataParameter retParam = (IDbDataParameter)m_Command.Parameters["@RETURN_VALUE"];
            return retParam.Value;
        }

        /// <summary>
        /// Очищает список параметров команды
        /// </summary>
        public void Clear()
        {
            m_Command.Parameters.Clear();
        }

        #endregion

        #region Методы для вызова/выполнения команды

        /// <summary>
        /// Выполняет команду
        /// </summary>
        public void ExecNonQuery()
        {
            try
            {
                OpenConnection();
                m_Command.ExecuteNonQuery();
                CloseConnection();
            }
            catch
            {
                Destroy();
                throw;
            }
        }

        /// <summary>
        /// Выполняет команду и возвращает значение 
        /// ячейки первого столбца первой строки первой таблицы.
        /// </summary>
        /// <returns></returns>
        public object ExecScalar()
        {
            try
            {
                OpenConnection();
                object obj = m_Command.ExecuteScalar();
                CloseConnection();
                return obj;
            }
            catch
            {
                Destroy();
                throw;
            }
        }

        /// <summary>
        /// Выполняет процедуру и возвращает DataReader.
        /// Внимание! Соединение необходимо закрывать вручную.
        /// </summary>
        /// <returns></returns>
        public IDataReader ExecReader()
        {
            try
            {
                OpenConnection();
                return m_Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                Destroy();
                throw;
            }
        }

        /// <summary>
        /// Выполняет команду и возвращает заполненный DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ExecDataSet()
        {
            try
            {
                OpenConnection();

                IDbDataAdapter Adapter = m_DBFactory.CreateDataAdapter();
                Adapter.SelectCommand = m_Command;
                DataSet result = new DataSet();
                Adapter.Fill(result);
                CloseConnection();
                return result;
            }
            catch
            {
                Destroy();
                throw;
            }
        }

        /// <summary>
        /// Выполняет команду и возвращает заполненный DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable ExecDataTable()
        {
            DataSet ds = ExecDataSet();
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// Выполняет команду и возвращает первую строку в первой таблице
        /// (если была возвращена хотя бы одна строка), иначе null
        /// </summary>
        /// <returns></returns>
        public DataRow ExecDataRow()
        {
            DataTable dt = ExecDataTable();
            if (dt != null)
            {
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            else return null;
        }

        #endregion
    }
}