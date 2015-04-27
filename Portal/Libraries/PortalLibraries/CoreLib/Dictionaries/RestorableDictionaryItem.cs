using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using Core;
using Core.ORM;
using Core.Dictionaries;
using Core.Exceptions;
using Core.Resources;
using Core.ORM.Attributes;

namespace Core.Dictionaries
{
	/// <summary>
	/// Класс элементов справочников, которые удаляются/восстанавливаются в базе
	/// </summary>
	public abstract class RestorableDictionaryItem : CommonDictionaryItem, IRestorableDictionaryItem
	{
		#region Конструкторы

		public RestorableDictionaryItem(IDictionaryManager manager)
			:
			base(manager)
		{
		}

		#endregion

		#region Fields

		private bool m_IsRemoved;

		#endregion

		#region Properties

		/// <summary>
		/// Является ли элемент удаленным из справочника.
		/// </summary>
		[DBRead("IsRemoved")]
		public bool IsRemoved
		{
			get
			{
				return m_IsRemoved; 			
			}
			set
			{
				m_IsRemoved = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Скрывает элемент из справочника.
		/// </summary>
		public virtual void Close()
		{
			if (Check(false))
			{
				m_IsRemoved = true;
				Save();
			}
		}

		/// <summary>
		/// Восстанавливает удаленный элемент.
		/// </summary>
		public void Restore()
		{
			m_IsRemoved = false;
			Save();
		}		

		/// <summary>
		/// Удаляет элемент.
		/// </summary>
		public override void Delete()
		{
			if (Check(true))
			{
				base.Delete();
			}
		}

		private bool Check(bool delete)
		{
			bool result = true;

			DictionaryCollection refDictCollection = Manager.GetReferenceDictionaries(Dictionary);
			Dictionary<IDictionary, PropertyInfo[]> dictionaries = new Dictionary<IDictionary, PropertyInfo[]>();

			foreach (IDictionary dict in refDictCollection)
			{
				dictionaries.Add(dict, ObjectMapper.GetLinkedProperties(Dictionary, dict.GetType()));
			}

			foreach (KeyValuePair<IDictionary, PropertyInfo[]> pair in dictionaries)
			{
				CommonDictionaryItem linkedDictItem = (CommonDictionaryItem)pair.Key.CreateDictionaryItem();
				List<object> param = new List<object>();

				foreach (PropertyInfo property in pair.Value)
				{
                    DictionaryLinkAttribute attrib = DBAttributesManager.GetDictionaryLinkAttribute(property);
					foreach (PropertyInfo prop in ObjectPropertiesMapper.GetDBReadProperties(linkedDictItem.GetType()))
					{
						if (prop.Name == attrib.PropertyName)
						{
							object obj = prop.GetValue(Dictionary, null);

							if (obj != null)
							{
								param.Add(property.Name);
								param.Add(obj);
							}
						}
					}
				}

				if (linkedDictItem.LoadByReference(param.ToArray()))
				{
					result = false;

					if (delete)
					{
						throw new DictionaryElementCouldNotBeDeletedException(
							Dictionary.DictionaryName, 
							GetKeyValues().ToString(), 
							linkedDictItem.DictionaryName, 
							linkedDictItem.GetKeyValues().ToString() );
					}
					else
					{
						throw new DictionaryElementCouldNotBeClosedException(
							Dictionary.DictionaryName,
							GetKeyValues().ToString(), 
							linkedDictItem.DictionaryName,
							linkedDictItem.GetKeyValues().ToString() );
					}
				}
			}

			return result;
		}

		#endregion
	}
}
