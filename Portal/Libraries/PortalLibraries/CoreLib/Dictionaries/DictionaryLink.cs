using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dictionaries
{
	/// <summary>
	/// Аттрибут отвечает за связь словарей между собой.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
	public class DictionaryLinkAttribute : Attribute
	{
		#region Fields

		private Type m_dictionaryLinkType = null;
		private string m_propertyName = "ID";

		#endregion

		#region Конструкторы

		/// <summary>
		/// Создаёт аттрибут связи между словарями. По умолчанию словари связываются по ключу ID.
		/// </summary>
		/// <param name="dictionaryItemType">Тип связанного словаря.</param>
		public DictionaryLinkAttribute( Type dictionaryItemType )
		{
			m_dictionaryLinkType = dictionaryItemType;
		}

		/// <summary>
		/// Создаёт аттрибут связи между словарями.
		/// </summary>
		/// <param name="dictionaryItemType">Тип связанного словаря.</param>
		/// <param name="propertyName">Название свойства связанного словаря, по которому идёт связь.</param>
		public DictionaryLinkAttribute( Type dictionaryItemType, string propertyName )
		{
			m_dictionaryLinkType = dictionaryItemType;
			m_propertyName = propertyName;
		}

		#endregion

		#region Свойства

		/// <summary>
		/// Тип связанного словаря.
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
		/// Свойство для связи.
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
