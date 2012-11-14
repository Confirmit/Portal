using System;
using System.Collections.Generic;
using System.Text;
using UlterSystems.PortalLib.BusinessObjects;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// ��������� �������� �������������.
	/// </summary>
	public interface IUsersFilter
	{
		/// <summary>
		/// �������� �� ������������ � ������.
		/// </summary>
		/// <param name="user">������������.</param>
		/// <returns>�������� �� ������������ � ������.</returns>
		bool IsValid(Person user);
	}
}
