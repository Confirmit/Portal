using System;
using System.Data;
using Core.DB;

namespace Core.DBInterface
{
	internal static class UserDB
	{
		/// <summary>
		/// ���������� ������������� ���� �� ������ ������������.
		/// </summary>
		/// <param name="login">�����.</param>
		/// <returns></returns>
		public static DataRow GetRoleIDByLogin( string login )
		{
			Procedure proc = new Procedure( "userGetRoleIDByUserID" );
			proc.Add( "@UserID", login );

			return proc.ExecDataRow();
		}

		/// <summary>
		/// ��������� ���� ������������ � ������� RoleToUser. 
		/// </summary>
		/// <param name="login">�����.</param>
		/// <param name="roleID">������������� ����.</param>
		public static void AddRoleIDByUserID(string login, int roleID)
		{
			Procedure proc = new Procedure("userAddRoleIDByUserID");
			proc.Add("@UserID", login);
			proc.Add("@RoleID", roleID);

			proc.ExecNonQuery();
		}

		/// <summary>
		/// ������� ���� ������������ �� ������� RoleToUser. 
		/// </summary>
		/// <param name="login">�����.</param>
		/// <param name="roleID">������������� ����.</param>
		public static void DeleteRoleIDByUserID(string login)
		{
			Procedure proc = new Procedure("userDeleteRoleIDByUserID");
			proc.Add("@UserID", login);

			proc.ExecNonQuery();
		}
	}
}
