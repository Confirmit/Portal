using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Инкапсулируюет работу с выделением в гриде.
	/// </summary>
	public class GridSelection
	{
		#region Конструкторы

		public GridSelection(Control owner)
		{
			m_Owner = owner;
		}

		#endregion

		#region Поля

		private Control m_Owner;

		#endregion

		#region Свойства

		/// <summary>
		/// Грид, для которого создан данный контроллер выделения.
		/// </summary>
		public Control Owner
		{
			get
			{
				return m_Owner;
			}
		}

		/// <summary>
		/// Устанавливает или возвращает флаг выделения строки с указанным ключом.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual bool this[object key]
		{
			get
			{
				return AllSelectedKeys.ContainsKey((object)key.ToString());
			}
			set
			{
				if (value)
				{
					if (!AllSelectedKeys.ContainsKey(key))
					{
						AllSelectedKeys.Add(key, true);
					}
				}
				else
				{
					AllSelectedKeys.Remove(key);
				}
			}
		}

		/// <summary>
		/// Таблица с идентификаторами всех выбранных строк.
		/// </summary>
		private Dictionary<object, bool> AllSelectedKeys
		{
			get
			{
				string key = GetSessionKey(m_Owner.UniqueID);
				Dictionary<object, bool> o = (Dictionary<object, bool>)HttpContext.Current.Session[key];
				if (o == null)
				{
					o = new Dictionary<object, bool>();
					HttpContext.Current.Session[key] = o;
				}
				return o;
			}
		}

		/// <summary>
		/// Коллекция ключей выбранных строк
		/// </summary>
		public virtual DataKeyArray SelectedKeys
		{
			get
			{
				ArrayList list = new ArrayList();
				foreach (KeyValuePair<object, bool> de in AllSelectedKeys)
				{
					OrderedDictionary keyTable = new OrderedDictionary(1);
					keyTable.Add(de.Key, de.Key);
					DataKey key = new DataKey(keyTable);

					list.Add(key);
				}
				return new DataKeyArray(list);
			}
		}

		/// <summary>
		/// Количество выделенных элементов.
		/// </summary>
		public virtual int Count
		{
			get
			{
				return AllSelectedKeys.Count;
			}
		}

		#endregion

		#region Методы
		/// <summary>
		/// Сбрасывает выделение.
		/// </summary>
		public virtual void Clear()
		{
			AllSelectedKeys.Clear();
		}

		/// <summary>
		/// Возвращает строку, сформированную из значений выделенных ключей, перечисленных через запятую.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder keys = new StringBuilder();
			foreach (DataKey key in SelectedKeys)
			{
				if (keys.Length > 0) keys.Append(",");
				keys.Append(key.Value.ToString());
			}
			return keys.ToString();
		}

		/// <summary>
		/// Возвращяет ключ по которому в сессии хранится информация о выделении.
		/// </summary>
		/// <param name="ownerID">Уникальный идентификатор грида, для которого создан контроллер</param>
		/// <returns></returns>
		public static string GetSessionKey(string ownerID)
		{
			return String.Format("AllSelectedKeys_{0}", ownerID);
		}
		#endregion
	}
}