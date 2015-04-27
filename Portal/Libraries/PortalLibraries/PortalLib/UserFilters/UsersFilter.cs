using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// Абстрактный фильтр пользоваталей.
	/// </summary>
	public abstract class UsersFilter : IUsersFilter
	{
		#region Поля
		private IUsersFilter m_NextFilter = null;
		#endregion

		#region Свойства
		/// <summary>
		/// Следующий фильтр в цепочке.
		/// </summary>
		public IUsersFilter NextFilter
		{
			get { return m_NextFilter; }
			set { m_NextFilter = value; }
		}
		#endregion

		#region Конструкторы
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="nextFilter">Следующий фильтр в цепочке.</param>
		public UsersFilter(IUsersFilter nextFilter)
		{ m_NextFilter = nextFilter;	}
		#endregion

		#region IUsersFilter Members

		/// <summary>
		/// Попадает ли пользователь в фильтр.
		/// </summary>
		/// <param name="user">Пользователь.</param>
		/// <returns>Попадает ли пользователь в фильтр.</returns>
		public bool IsValid(UlterSystems.PortalLib.BusinessObjects.Person user)
		{
			if (user == null)
				return false;

			bool res = IsUserValid(user);
			if (res == false)
				return false;

			if (m_NextFilter != null)
				return m_NextFilter.IsValid(user);
			else
				return true;
		}

		/// <summary>
		/// Попадает ли пользователь в фильтр.
		/// </summary>
		/// <param name="user">Пользователь.</param>
		/// <returns>Попадает ли пользователь в фильтр.</returns>
		protected abstract bool IsUserValid(UlterSystems.PortalLib.BusinessObjects.Person user);

		#endregion
	}
}
