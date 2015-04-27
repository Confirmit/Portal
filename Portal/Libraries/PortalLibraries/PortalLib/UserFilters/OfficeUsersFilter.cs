using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// ������ �������� �� ����� � ���������.
	/// </summary>
	public class YaroslavlOfficeUsersFilter : UsersFilter
	{
		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="nextFilter">��������� ������ � �������.</param>
		public YaroslavlOfficeUsersFilter(IUsersFilter nextFilter)
			: base(nextFilter)
		{ }

		/// <summary>
		/// �������� �� ������������ � ������.
		/// </summary>
		/// <param name="user">������������.</param>
		/// <returns>�������� �� ������������ � ������.</returns>
		protected override bool IsUserValid(UlterSystems.PortalLib.BusinessObjects.Person user)
		{
			if (user == null)
				return false;
			else
				return !user.EmployeesUlterSYSMoscow;
		}
	}
}
