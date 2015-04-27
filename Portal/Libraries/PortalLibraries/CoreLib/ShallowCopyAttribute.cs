using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// �������, ��������� � ���, ��� ���� ����� ����������� ������ �� ������ (������������� ������������).
	/// </summary>
	[AttributeUsage( AttributeTargets.Field, AllowMultiple = false, Inherited = false )]
	public class ShallowCopyAttribute : System.Attribute
	{
		public ShallowCopyAttribute(  )
		{
		}
	}
}
