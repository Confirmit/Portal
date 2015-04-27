using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DB
{
	/// <summary>
	/// �����, ��������������� ����������� �������� �������� ���������.
	/// </summary>
	public sealed class Procedure : BaseCommand
	{
		/// <summary>
		/// �������������� ������ ���������
		/// </summary>
		/// <param name="procedureName">��� ���������</param>
		/// <param name="connectionKind">��� �����������</param>
		public Procedure(string procedureName, ConnectionKind connectionKind)
			: base(procedureName, CommandType.StoredProcedure, connectionKind)
		{ }

		/// <summary>
		/// �������������� ������ ���������
		/// </summary>
		/// <param name="procedureName">��� ���������</param>
		public Procedure(string procedureName)
			: base( procedureName, CommandType.StoredProcedure, ConnectionKind.Default )
		{ }
	}
}