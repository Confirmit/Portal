using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// Фильтр служащих с EMail.
	/// </summary>
	public class HasEMailUsersFilter : UsersFilter
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="nextFilter">Следующий фильтр в цепочке.</param>
		public HasEMailUsersFilter(IUsersFilter nextFilter)
			: base(nextFilter)
		{ }

		/// <summary>
		/// Попадает ли пользователь в фильтр.
		/// </summary>
		/// <param name="user">Пользователь.</param>
		/// <returns>Попадает ли пользователь в фильтр.</returns>
		protected override bool IsUserValid(UlterSystems.PortalLib.BusinessObjects.Person user)
		{
			if (user == null)
				return false;
			else
				return !string.IsNullOrEmpty(user.PrimaryEMail);
		}
	}
}
