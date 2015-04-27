using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// ��������� ��� ������� ��������, ������ ����� �������� � ����� ����������� ������������.
	/// </summary>
	public interface IAccessible
	{
		/// <summary>
		/// ��������� ����������� ������� ������������ � �������� ��������� � ����������� �� �����������.
		/// </summary>
		/// <param name="user">������������.</param>
		/// <returns>���������� true, ���� ������ ������� �������� ������������, ����� ���������� false.</returns>
		bool CheckAccessibilityToUser( User user );
	}
}
