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
	/// ����� ��������� ������������, ������� ���������/����������������� � ����
	/// </summary>
	public abstract class RestorableDictionaryItem : CommonDictionaryItem, IRestorableDictionaryItem
	{
		#region ������������

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
		/// �������� �� ������� ��������� �� �����������.
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
		/// �������� ������� �� �����������.
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
		/// ��������������� ��������� �������.
		/// </summary>
		public void Restore()
		{
			m_IsRemoved = false;
			Save();
		}		

		/// <summary>
		/// ������� �������.
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
