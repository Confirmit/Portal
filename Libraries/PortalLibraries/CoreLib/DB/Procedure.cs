using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	/// Класс, предоставляющий возможность вызывать хранимые процедуры.
	/// </summary>
	public sealed class Procedure : BaseCommand
	{
		/// <summary>
		/// Инициализирует объект процедуры
		/// </summary>
		/// <param name="procedureName">имя процедуры</param>
		/// <param name="connectionKind">тип подключения</param>
		public Procedure(string procedureName, ConnectionKind connectionKind)
			: base(procedureName, CommandType.StoredProcedure, connectionKind)
		{ }

		/// <summary>
		/// Инициализирует объект процедуры
		/// </summary>
		/// <param name="procedureName">имя процедуры</param>
		public Procedure(string procedureName)
			: base( procedureName, CommandType.StoredProcedure, ConnectionKind.Default )
		{ }
	}
}