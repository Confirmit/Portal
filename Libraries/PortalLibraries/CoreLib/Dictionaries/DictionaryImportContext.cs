using System;
using System.Text;
using System.Collections.Generic;

using Core.Types;
using Core.Exceptions;

namespace Core.Dictionaries
{
	/// <summary>
	/// ����� �������� � ���� �������� ������� ������������. � ���� ������ �����������,
	/// ��� ����������� � ���� ������� ������� � �� ����������� ��������, ������������ ��
	/// �����.
	/// </summary>
	public class DictionaryImportContext
	{
		#region Nested types

		private class DictionaryElements : Dictionary<ValueArray, object>
		{
		}

		#endregion

		#region Fields

		private Dictionary<IDictionary, DictionaryElements> m_dictHash =
			new Dictionary<IDictionary, DictionaryElements>( new DictionaryEqualityComparer() );

		#endregion

		#region Methods

		/// <summary>
		/// ���������, �������� �� �������� ������ ���������� �����������.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <returns>True, ���� ������ ����������, ����� False.</returns>
		public bool Contains( IDictionary dictionary )
		{
			return m_dictHash.ContainsKey( dictionary );
		}

		/// <summary>
		/// ���������, �������� �� �������� ������� �����������.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <param name="keys">�������� �������� ��������.</param>
		/// <returns>True, ���� ������� ����������, ����� False.</returns>
		public bool ContainsElement( IDictionary dictionary, object[] keys )
		{
			bool result = false;
			DictionaryElements elements;

			if(m_dictHash.TryGetValue( dictionary, out elements ))
			{
				result = elements.ContainsKey( new ValueArray( keys ) );
			}
			else
			{
				throw new CoreObjectNotFoundException( dictionary.DictionaryName );
			}

			return result;
		}

		/// <summary>
		/// ��������� ����� ������� � ��������.
		/// </summary>
		/// <param name="dictionary">�������.</param>
		/// <param name="keys">�������� �������� ���������.</param>
		/// <param name="element">�������.</param>
		public void AddElement( IDictionary dictionary, object[] keys, object element )
		{
			DictionaryElements elements;
			if(!m_dictHash.TryGetValue( dictionary, out elements ))
			{
				elements = new DictionaryElements();
				m_dictHash.Add( dictionary, elements );
			}

			ValueArray key = new ValueArray( keys );
			if(elements.ContainsKey( key ))
			{
				throw new DictionaryKeyAlreadyExistsException( 
					new MLString( key.ToString() ), dictionary.DictionaryName );
			}
			else
			{
				elements.Add( key, element );
			}
		}

		/// <summary>
		/// ���������� ������� ����������� �� �����.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <param name="keyValues">������ �������� ������ ��� ������.</param>
		/// <returns>������� �����������.</returns>
		public object GetDictionaryElement( IDictionary dictionary, ValueArray keyValues )
		{
			return GetDictionaryElement( dictionary, keyValues.Values );
		}

		/// <summary>
		/// ���������� ������� ����������� �� �����.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <param name="keyValues">������ �������� ������ ��� ������.</param>
		/// <returns>������� �����������.</returns>
		public object GetDictionaryElement( IDictionary dictionary, object[] keyValues )
		{
			object result = null;
			DictionaryElements elements;

			if(m_dictHash.TryGetValue( dictionary, out elements ))
			{
				elements.TryGetValue( new ValueArray( keyValues ), out result );
			}
			else
			{
				throw new CoreObjectNotFoundException( dictionary.DictionaryName );
			}

			return result;
		}

		#endregion
	}
}
