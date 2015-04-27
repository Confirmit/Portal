using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	/// �����, ��������������� ����������� ��������� ������� � ������� SQL.
	/// </summary>
	public sealed class Query : BaseCommand
	{
		/// <summary>
		/// �������������� ������ �������
		/// </summary>
		/// <param name="sql">sql-������</param>
		/// <param name="connectionKind">��� �����������</param>
		public Query(string sql, ConnectionKind connectionKind)
			: base( sql, CommandType.Text, connectionKind )
		{ }

		/// <summary>
		/// �������������� ������ ���������
		/// </summary>
		/// <param name="sql">��� ���������</param>
		
        public Query(string sql)
			: base( sql, CommandType.Text, ConnectionKind.Default )
		{ }
	}
}