using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using UlterSystems.PortalLib.DB;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// ���� �����������.
	/// </summary>
	public enum NotificationType
	{ 
		/// <summary>
		/// ����������� � ���������� ����������� � �������.
		/// </summary>
		NotRegistered = 1,
		/// <summary>
		/// ����������� � ���������� ���������� ������.
		/// </summary>
		CloseEvents = 2
	}

	/// <summary>
	/// ������ E-Mails, �� ������� ������ ���������� �����������.
	/// </summary>
	public class NotificationList : IEnumerable<string>
	{
		#region ����
		private List<string> m_EMails = new List<string>();
		private NotificationType m_Type = NotificationType.CloseEvents;
		#endregion

		#region ��������
		/// <summary>
		/// ��� ����������� ��� ������.
		/// </summary>
		public NotificationType Type
		{
			get { return m_Type; }
		}

		/// <summary>
		/// ����������.
		/// </summary>
		/// <param name="index">������ EMail.</param>
		/// <returns>EMail � �������� ��������.</returns>
		public string this[int index]
		{
			get 
			{
				Debug.Assert(m_EMails != null);
				return m_EMails[index]; 
			}
		}
		#endregion

		#region ������������
		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="type">��� ������.</param>
		/// <param name="table">������� � �������.</param>
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

		#region ������
		/// <summary>
		/// ���������� ������ EMail ��� ����������� �� ��������� ����.
		/// </summary>
		/// <param name="type">��� �����������.</param>
		/// <returns>������ EMail.</returns>
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
