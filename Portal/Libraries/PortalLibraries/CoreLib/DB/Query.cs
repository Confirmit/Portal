using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	///  ласс, предоставл€ющий возможность выполн€ть запросы и команды SQL.
	/// </summary>
	public sealed class Query : BaseCommand
	{
		/// <summary>
		/// »нициализирует объект запроса
		/// </summary>
		/// <param name="sql">sql-запрос</param>
		/// <param name="connectionKind">тип подключени€</param>
		public Query(string sql, ConnectionKind connectionKind)
			: base( sql, CommandType.Text, connectionKind )
		{ }

		/// <summary>
		/// »нициализирует объект процедуры
		/// </summary>
		/// <param name="sql">им€ процедуры</param>
		
        public Query(string sql)
			: base( sql, CommandType.Text, ConnectionKind.Default )
		{ }
	}
}