using System;
using System.Collections.Generic;
using System.Text;
using UlterSystems.PortalLib.BusinessObjects;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// Интерфейс фильтров пользователей.
	/// </summary>
	public interface IUsersFilter
	{
		/// <summary>
		/// Попадает ли пользователь в фильтр.
		/// </summary>
		/// <param name="user">Пользователь.</param>
		/// <returns>Попадает ли пользователь в фильтр.</returns>
		bool IsValid(Person user);
	}
}
