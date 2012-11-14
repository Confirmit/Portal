using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	/// Интерфейс объекта "Команда для работы с БД"
	/// </summary>
	public interface IBaseCommand
	{
		IDbCommand Command { get; }
		IDataParameterCollection Parameters { get; }
		IDbDataParameter Add( string name, object value );
		IDbDataParameter Add( string name, object value, DbType type );
		IDbDataParameter AddOutput( string name, object value );
		IDbDataParameter AddReturnValueParameter();
		object GetReturnValue();
		void Clear();
		void Destroy();
		IDataReader ExecReader();
		DataSet ExecDataSet();
		DataTable ExecDataTable();
		DataRow ExecDataRow();
		object ExecScalar();
		void ExecNonQuery();
	}
}