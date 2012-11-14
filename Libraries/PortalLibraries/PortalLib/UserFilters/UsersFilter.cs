using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// ����������� ������ �������������.
	/// </summary>
	public abstract class UsersFilter : IUsersFilter
	{
		#region ����
		private IUsersFilter m_NextFilter = null;
		#endregion

		#region ��������
		/// <summary>
		/// ��������� ������ � �������.
		/// </summary>
		public IUsersFilter NextFilter
		{
			get { return m_NextFilter; }
			set { m_NextFilter = value; }
		}
		#endregion

		#region ������������
		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="nextFilter">��������� ������ � �������.</param>
		public UsersFilter(IUsersFilter nextFilter)
		{ m_NextFilter = nextFilter;	}
		#endregion

		#region IUsersFilter Members

		/// <summary>
		/// �������� �� ������������ � ������.
		/// </summary>
		/// <param name="user">������������.</param>
		/// <returns>�������� �� ������������ � ������.</returns>
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
		/// �������� �� ������������ � ������.
		/// </summary>
		/// <param name="user">������������.</param>
		/// <returns>�������� �� ������������ � ������.</returns>
		protected abstract bool IsUserValid(UlterSystems.PortalLib.BusinessObjects.Person user);

		#endregion
	}
}
