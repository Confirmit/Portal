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
    /// ������� ����� ��� sql-�������.
    /// </summary>
    public abstract class BaseCommand : IBaseCommand
    {
        #region ����

        private IDbConnection m_Connection;
        private IDbCommand m_Command;
        private ConnectionKind m_ConnectionKind;
        private DbProviderFactory m_DBFactory;

        #endregion

        #region ��������

        /// <summary>
        /// ��������� �������
        /// </summary>
        public IDataParameterCollection Parameters
        {
            get { return m_Command.Parameters; }
        }

        /// <summary>
        /// ���������� SQL-������� ������ ���������
        /// </summary>
        public IDbCommand Command
        {
            get { return m_Command; }
        }

        #endregion

        #region ������������

        /// <summary>
        /// �������������� ������ ���������.
        /// </summary>
        /// <param name="commandText">����� ������� (��� ���������, SQL-������).</param>
        /// <param name="commandType">��� �������.</param>
        /// <param name="connectionKind">��� �����������.</param>
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

        #region ������ ������ � �����������

        /// <summary>
        /// ����������� �������, ��������� � ���������� (�������, ����������)
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
        /// ��������� ���������� � ��.
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
        /// ��������� ���������� � ��.
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

        #region ������ ��� ������ � �����������

        /// <summary>
        /// ��������� ������� ��������
        /// </summary>
        /// <param name="name">��� ���������</param>
        /// <param name="value">��������</param>
        /// <returns></returns>
        public IDbDataParameter Add(string name, object value)
        {
            return Add(name, value, ParameterDirection.Input);
        }

        /// <summary>
        /// ��������� ������� ��������
        /// </summary>
        /// <param name="name">��� ���������</param>
        /// <param name="value">��������</param>
        /// <param name="type">���</param>
        /// <returns></returns>
        public IDbDataParameter Add(string name, object value, DbType type)
        {
            return Add(name, value, type, ParameterDirection.Input);
        }

        /// <summary>
        /// ��������� �������� ��������
        /// </summary>
        /// <param name="name">��� ���������</param>
        /// <param name="value">��������</param>
        /// <returns></returns>
        public IDbDataParameter AddOutput(string name, object value)
        {
            return Add(name, value, ParameterDirection.Output);
        }

        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// <param name="name">��� ���������</param>
        /// <param name="value">��������</param>
        /// <param name="direction">�����������</param>
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
        /// ��������� ��������
        /// </summary>
        /// <param name="name">��� ���������</param>
        /// <param name="value">��������</param>
        /// <param name="type">���</param>
        /// <param name="direction">�����������</param>
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
        /// ��������� ��������
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDbDataParameter Add(IDbDataParameter param)
        {
            int pos = m_Command.Parameters.Add(param);
            return (IDbDataParameter)m_Command.Parameters[pos];
        }

        /// <summary>
        /// ��������� �������� - ������������ ��������
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
        /// ���������� �������� ��������, ������������ �� ��������� ���������� RETURN
        /// </summary>
        /// <returns></returns>
        public object GetReturnValue()
        {
            IDbDataParameter retParam = (IDbDataParameter)m_Command.Parameters["@RETURN_VALUE"];
            return retParam.Value;
        }

        /// <summary>
        /// ������� ������ ���������� �������
        /// </summary>
        public void Clear()
        {
            m_Command.Parameters.Clear();
        }

        #endregion

        #region ������ ��� ������/���������� �������

        /// <summary>
        /// ��������� �������
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
        /// ��������� ������� � ���������� �������� 
        /// ������ ������� ������� ������ ������ ������ �������.
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
        /// ��������� ��������� � ���������� DataReader.
        /// ��������! ���������� ���������� ��������� �������.
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
        /// ��������� ������� � ���������� ����������� DataSet
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
        /// ��������� ������� � ���������� ����������� DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable ExecDataTable()
        {
            DataSet ds = ExecDataSet();
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// ��������� ������� � ���������� ������ ������ � ������ �������
        /// (���� ���� ���������� ���� �� ���� ������), ����� null
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