using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using UlterSystems.PortalLib.DB;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// Типы уведомлений.
	/// </summary>
	public enum NotificationType
	{ 
		/// <summary>
		/// Уведомление о отсутствии регистрации в портале.
		/// </summary>
		NotRegistered = 1,
		/// <summary>
		/// Уведомление о отсутствии завершения работы.
		/// </summary>
		CloseEvents = 2
	}

	/// <summary>
	/// Список E-Mails, по которым должны посылаться уведомления.
	/// </summary>
	public class NotificationList : IEnumerable<string>
	{
		#region Поля
		private List<string> m_EMails = new List<string>();
		private NotificationType m_Type = NotificationType.CloseEvents;
		#endregion

		#region Свойства
		/// <summary>
		/// Тип уведомления для списка.
		/// </summary>
		public NotificationType Type
		{
			get { return m_Type; }
		}

		/// <summary>
		/// Индексатор.
		/// </summary>
		/// <param name="index">Индекс EMail.</param>
		/// <returns>EMail с заданным индексом.</returns>
		public string this[int index]
		{
			get 
			{
				Debug.Assert(m_EMails != null);
				return m_EMails[index]; 
			}
		}
		#endregion

		#region Конструкторы
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="type">Тип списка.</param>
		/// <param name="table">Таблица с данными.</param>
		private NotificationList(NotificationType type, DataTable table)
		{
			m_Type = type;

			if ((table == null) || (table.Rows.Count == 0))
				throw new ArgumentNullException("table");

			Debug.Assert(m_EMails != null);
			m_EMails.Clear();
			foreach (DataRow row in table.Rows)
			{
				string eMail = (string)row["EMail"];
				if( !string.IsNullOrEmpty( eMail ) )
					m_EMails.Add( eMail );
			}
		}
		#endregion

		#region Методы
		/// <summary>
		/// Возвращает список EMail для уведомления по заданному типу.
		/// </summary>
		/// <param name="type">Тип уведомления.</param>
		/// <returns>Список EMail.</returns>
		public static NotificationList GetNotificationList(NotificationType type)
		{
			DataTable dt = DBManager.GetNotificationList((int)type);
			if ((dt == null) || (dt.Rows.Count == 0))
				return null;
			else
				return new NotificationList(type, dt);
		}
		#endregion

		#region IEnumerable<string> Members

		public IEnumerator<string> GetEnumerator()
		{
			return m_EMails.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_EMails.GetEnumerator();
		}

		#endregion
	}
}
