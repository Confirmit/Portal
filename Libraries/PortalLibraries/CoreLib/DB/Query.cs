using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	/// Класс, предоставляющий возможность выполнять запросы и команды SQL.
	/// </summary>
	public sealed class Query : BaseCommand
	{
		/// <summary>
		/// Инициализирует объект запроса
		/// </summary>
		/// <param name="sql">sql-запрос</param>
		/// <param name="connectionKind">тип подключения</param>
		public Query(string sql, ConnectionKind connectionKind)
			: base( sql, CommandType.Text, connectionKind )
		{ }

		/// <summary>
		/// Инициализирует объект процедуры
		/// </summary>
		/// <param name="sql">имя процедуры</param>
		
        public Query(string sql)
			: base( sql, CommandType.Text, ConnectionKind.Default )
		{ }
	}
}