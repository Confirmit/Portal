using System;
using System.Data;
using System.Collections.Generic;

using Core.Exceptions;

namespace Core.DB
{
    public class BaseCommandCollection : List<IDbCommand>
    {
        #region [ Fields ]

        IDbConnection m_Connection = null;
        IDbTransaction m_Transaction = null;

        #endregion

        public void ExecNonQuery()
        {
            try
            {
                BeginTransaction();
                {
                    foreach (IDbCommand baseCommand in this)
                    {
                        prepareCommand(baseCommand);
                        baseCommand.ExecuteNonQuery();
                    }
                }
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
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
                object res = null;

                BeginTransaction();
                {
                    foreach (IDbCommand baseCommand in this)
                    {
                        prepareCommand(baseCommand);
                        res = baseCommand.ExecuteScalar();
                    }
                }
                CommitTransaction();

                return res;
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private void prepareCommand(IDbCommand command)
        {
            command.Connection = m_Connection;
            command.Transaction = m_Transaction;
        }

        #region Методы работы с соединением

        private void BeginTransaction()
        {
            try
            {
                ConnectionManager.BeginTransaction();

                m_Connection = ConnectionManager.GetConnection(ConnectionKind.Default);
                m_Transaction = ConnectionManager.GetCurrentTransaction(ConnectionKind.Default);
            }
            catch (Exception ex)
            {
                throw new CoreException(Resources.ResourceManager.GetString("OpenConnectionException", ex.Message), ex);
            }
        }

        private void CommitTransaction()
        {
            try
            {
                ConnectionManager.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw new CoreException(Resources.ResourceManager.GetString("OpenConnectionException", ex.Message), ex);
            }
        }

        private void RollbackTransaction()
        {
            try
            {
                ConnectionManager.RollbackTransaction();
            }
            catch (Exception ex)
            {
                throw new CoreException(Resources.ResourceManager.GetString("OpenConnectionException", ex.Message), ex);
            }
        }

        #endregion
    }
}