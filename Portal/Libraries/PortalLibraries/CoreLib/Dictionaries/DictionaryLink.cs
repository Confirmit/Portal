using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dictionaries
{
	/// <summary>
	/// �������� �������� �� ����� �������� ����� �����.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
	public class DictionaryLinkAttribute : Attribute
	{
		#region Fields

		private Type m_dictionaryLinkType = null;
		private string m_propertyName = "ID";

		#endregion

		#region ������������

		/// <summary>
		/// ������ �������� ����� ����� ���������. �� ��������� ������� ����������� �� ����� ID.
		/// </summary>
		/// <param name="dictionaryItemType">��� ���������� �������.</param>
		public DictionaryLinkAttribute( Type dictionaryItemType )
		{
			m_dictionaryLinkType = dictionaryItemType;
		}

		/// <summary>
		/// ������ �������� ����� ����� ���������.
		/// </summary>
		/// <param name="dictionaryItemType">��� ���������� �������.</param>
		/// <param name="propertyName">�������� �������� ���������� �������, �� �������� ��� �����.</param>
		public DictionaryLinkAttribute( Type dictionaryItemType, string propertyName )
		{
			m_dictionaryLinkType = dictionaryItemType;
			m_propertyName = propertyName;
		}

		#endregion

		#region ��������

		/// <summary>
		/// ��� ���������� �������.
		/// </summary>
		public Type DictionaryLinkType
		{
			get
			{ 
				return m_dictionaryLinkType; 
			}
			set
			{
				m_dictionaryLinkType = value;
			}
		}

		/// <summary>
		/// �������� ��� �����.
		/// </summary>
		public string PropertyName
		{
			get
			{
				return m_propertyName;
			}
			set
			{
				m_propertyName = value;
			}
		}

		#endregion
	}
}
