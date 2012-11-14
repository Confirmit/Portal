using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// Класс типа события.
	/// </summary>
	public class UptimeEventType
	{
		#region Fields

		private static OldDictionaries m_OldDictionaries = new OldDictionaries();
		private static Dictionary<int, UptimeEventType> m_EventTypes = new Dictionary<int, UptimeEventType>();

		private int m_ID;
		private string m_Name;
		private string m_Description;
		private string m_HTMLColor = "FFFFFF"; // белый по умолчанию.

		#endregion

		#region Свойства
		/// <summary>
		/// ID типа события.
		/// </summary>
		public int ID
		{
			get { return m_ID; }
		}

		/// <summary>
		/// Название типа события.
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Описание типа события.
		/// </summary>
		public string Description
		{
			get { return m_Description; }
		}

		/// <summary>
		/// Ассоциированный с событием цвет в формате HTML.
		/// </summary>
		public string HTMLColor
		{
			get { return m_HTMLColor; }
		}

		/// <summary>
		/// Возвращает цвета события.
		/// </summary>
		public Color Color
		{
			get { return ColorTranslator.FromHtml("#" + m_HTMLColor); }
		}

		#endregion

		#region Конструкторы
		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static UptimeEventType()
		{
			DataTable dt = m_OldDictionaries["UptimeEventTypes"];
			if ((dt == null) || (dt.Rows.Count == 0))
				return;

			foreach (DataRow row in dt.Rows)
			{
				UptimeEventType eventType = new UptimeEventType(row);
				m_EventTypes[eventType.ID] = eventType;
			}
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="row">Строка с данными.</param>
		private UptimeEventType(DataRow row)
		{
			if (row == null)
			{ throw new ArgumentNullException("row"); }

			m_ID = (int)row["ID"];
			m_Name = (string)row["Name"];
			if (row["Description"] != DBNull.Value)
				m_Description = (string)row["Description"];
			if (row["Color"] != DBNull.Value)
				m_HTMLColor = (string)row["Color"];
		}
		#endregion

		#region Методы
		/// <summary>
		/// Возвращает объект типа события с соответствующим ID.
		/// </summary>
		/// <param name="eventTypeID">ID типа события.</param>
		/// <returns>Объект типа события с соответствующим ID.</returns>
		public static UptimeEventType GetEventType(int eventTypeID)
		{
		    return m_EventTypes.ContainsKey(eventTypeID)
		               ? m_EventTypes[eventTypeID]
		               : null;
		}

	    /// <summary>
		/// Преобразует ID типа события в его имя.
		/// </summary>
		/// <param name="obj">ID типа события.</param>
		/// <returns>Имя типа события с заданным ID.</returns>
		public static string ConvertWorkTypeToString(object obj)
		{
			int eventTypeID;
			try
			{ eventTypeID = (int)obj; }
			catch
			{ eventTypeID = -1; }

			if (eventTypeID == -1)
				return string.Empty;

			UptimeEventType eventType = GetEventType(eventTypeID);
	        return eventType == null
	                   ? string.Empty
	                   : eventType.Name;
		}

		/// <summary>
		/// Возвращает список всех типов событий.
		/// </summary>
		/// <returns>Список всех типов событий.</returns>
		public static UptimeEventType[] GetAllEventTypes()
		{
			List<UptimeEventType> output = new List<UptimeEventType>();

			foreach (KeyValuePair<int, UptimeEventType> pair in m_EventTypes)
			{ output.Add(pair.Value); }

			output.Sort(delegate(UptimeEventType x, UptimeEventType y) { return x.ID - y.ID; });

			return output.ToArray();
		}
		#endregion
	}
}
