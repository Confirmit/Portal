using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.UserFilters
{
	/// <summary>
	/// ������ �������� � EMail.
	/// </summary>
	public class HasEMailUsersFilter : UsersFilter
	{
		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="nextFilter">��������� ������ � �������.</param>
		public HasEMailUsersFilter(IUsersFilter nextFilter)
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
				return !string.IsNullOrEmpty(user.PrimaryEMail);
		}
	}
}
