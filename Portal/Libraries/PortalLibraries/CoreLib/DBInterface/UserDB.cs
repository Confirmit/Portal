using System;
using System.Data;
using Core.DB;

namespace Core.DBInterface
{
	internal static class UserDB
	{
		/// <summary>
		/// Возвращает идентификатор роли по логину пользователя.
		/// </summary>
		/// <param name="login">Логин.</param>
		/// <returns></returns>
		public static DataRow GetRoleIDByLogin( string login )
		{
			Procedure proc = new Procedure( "userGetRoleIDByUserID" );
			proc.Add( "@UserID", login );

			return proc.ExecDataRow();
		}

		/// <summary>
		/// Добавляет роль пользователю в таблицу RoleToUser. 
		/// </summary>
		/// <param name="login">Логин.</param>
		/// <param name="roleID">Идентификатор роли.</param>
		public static void AddRoleIDByUserID(string login, int roleID)
		{
			Procedure proc = new Procedure("userAddRoleIDByUserID");
			proc.Add("@UserID", login);
			proc.Add("@RoleID", roleID);

			proc.ExecNonQuery();
		}

		/// <summary>
		/// Удаляет роль пользователю из таблицы RoleToUser. 
		/// </summary>
		/// <param name="login">Логин.</param>
		/// <param name="roleID">Идентификатор роли.</param>
		public static void DeleteRoleIDByUserID(string login)
		{
			Procedure proc = new Procedure("userDeleteRoleIDByUserID");
			proc.Add("@UserID", login);

			proc.ExecNonQuery();
		}
	}
}
