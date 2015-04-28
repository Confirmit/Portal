using System;
using System.Text;
using System.Collections.Generic;

using Core.Types;
using Core.Exceptions;

namespace Core.Dictionaries
{
	/// <summary>
	/// Класс содержит в себе контекст импорта справочников. В него входят справочники,
	/// уже загруженные в ходе данного импорта и их загруженные элементы, хэшированные по
	/// ключу.
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
		/// Проверяет, содержит ли контекст данные указанного справочника.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <returns>True, если данные содержатся, иначе False.</returns>
		public bool Contains( IDictionary dictionary )
		{
			return m_dictHash.ContainsKey( dictionary );
		}

		/// <summary>
		/// Проверяет, содержит ли контекст элемент справочника.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <param name="keys">Ключевые значения элемента.</param>
		/// <returns>True, если элемент содержится, иначе False.</returns>
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
		/// Добавляет новый элемент в контекст.
		/// </summary>
		/// <param name="dictionary">Словарь.</param>
		/// <param name="keys">Ключевые значения элементов.</param>
		/// <param name="element">Элемент.</param>
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
		/// Возвращает элемент справочника по ключу.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <param name="keyValues">Список значений ключей для поиска.</param>
		/// <returns>Элемент справочника.</returns>
		public object GetDictionaryElement( IDictionary dictionary, ValueArray keyValues )
		{
			return GetDictionaryElement( dictionary, keyValues.Values );
		}

		/// <summary>
		/// Возвращает элемент справочника по ключу.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <param name="keyValues">Список значений ключей для поиска.</param>
		/// <returns>Элемент справочника.</returns>
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
