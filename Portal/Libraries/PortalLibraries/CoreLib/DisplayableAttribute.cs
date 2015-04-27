using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// �������� ��� ������� ��������, ������������, ���� �� ���������� ��������������� �������� � ������
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public class DisplayableAttribute : System.Attribute
	{
		private bool m_display;

		public DisplayableAttribute( bool Display )
		{
			m_display = Display;
		}

		/// <summary>
		/// ���������� �� �������� �������� � �������
		/// </summary>
		public bool Display
		{
			get { return m_display; }
		}
	}
}
