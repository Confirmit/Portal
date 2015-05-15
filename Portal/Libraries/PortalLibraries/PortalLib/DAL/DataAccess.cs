using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using Core.ORM;
using System;

namespace ConfirmIt.PortalLib.DAL
{
    /// <summary>
    /// Base class for all data providers.
    /// </summary>
    public abstract class DataAccess
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_ConnectionString;

        #endregion

        #region Properties

        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString
        {
            [DebuggerStepThrough]
            get { return m_ConnectionString; }
            [DebuggerStepThrough]
            set { m_ConnectionString = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes database command non-query.
        /// </summary>
        /// <param name="command">Database command.</param>
        protected virtual int ExecuteNonQuery(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes database command scalar.
        /// </summary>
        /// <param name="command">Database command.</param>
        /// <returns>Scalar result of database command.</returns>
        protected virtual object ExecuteScalar(IDbCommand command)
        {
            return command.ExecuteScalar();
        }

        /// <summary>
        /// Returns data reader of database command results.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Data reader of database command results.</returns>
        protected virtual IDataReader ExecuteReader(IDbCommand command)
        {
            return command.ExecuteReader();
        }

        #endregion

        #region [ GetAllObjectsFromDataTable ]

        protected virtual List<Type> GetAllObjectsFromDataTable<Type>(DataTable table)
            where Type : class
        {
            return GetAllObjectsFromDataTable<Type>(typeof(Type), table);
        }

        protected virtual List<T> GetAllObjectsFromDataTable<T>(Type objType, DataTable table)
            where T : class
        {
            List<T> result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(GetObjectFromRow<T>(objType, row));
            }

            return result;
        }

        protected virtual T GetObjectFromRow<T>(Type objType, DataRow row)
            where T : class
        {
            object obj = Activator.CreateInstance(objType);
            ObjectPropertiesMapper.FillObjectFromRow(obj, row);

            return (T)obj;
        }

        #endregion
    }
}