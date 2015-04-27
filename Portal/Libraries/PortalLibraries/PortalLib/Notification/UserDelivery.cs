using System;
using System.Collections.Generic;
using System.Text;

using Core.ORM.Attributes;
using Core;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// ���� ��������.
	/// </summary>
	public enum Delivery
	{
		User = 1,
		Office = 2,
	}

	/// <summary>
	/// ���� ������������� ������ � ���������.
	/// </summary>
	public enum DeliveryPresentation
	{
		HTML = 1,
		XML = 2,
		Excel = 3,
	}

	/// <summary>
	/// ����� ���������� � �������� ��� ������������.
	/// </summary>
	[DBTable("UsersDeliveries")]
	public class UserDelivery : BasePlainObject
	{
		#region ����
		private int m_UserID;
		private int m_DeliveryID;
		private int m_DeliveryPresentation;
		private int? m_StatisticsUserID;
		#endregion

		#region ��������
		/// <summary>
		/// ID ������������, ���������� ��������.
		/// </summary>
		[DBRead("UserID")]
		public int UserID
		{
			get { return m_UserID; }
			set { m_UserID = value; }
		}

		/// <summary>
		/// ������������� ��������.
		/// </summary>
		[DBRead("DeliveryID")]
		public int DeliveryID
		{
			get { return m_DeliveryID; }
			set { m_DeliveryID = value; }
		}

		/// <summary>
		/// ��� ��������.
		/// </summary>
		public Delivery Delivery
		{
			get { return (Delivery)m_DeliveryID; }
			set { m_DeliveryID = (int)value; }
		}

		/// <summary>
		/// ������������� ������������� ��������.
		/// </summary>
		[DBRead("DeliveryPresentation")]
		public int DeliveryPresentationID
		{
			get { return m_DeliveryPresentation; }
			set { m_DeliveryPresentation = value; }
		}

		/// <summary>
		/// ������������� ��������.
		/// </summary>
		public DeliveryPresentation DeliveryPresentation
		{
			get { return (DeliveryPresentation)m_DeliveryPresentation; }
			set { m_DeliveryPresentation = (int)value; }
		}

		/// <summary>
		/// ID ������������, ��� ���������� ������ ���� ��������.
		/// ���� null, �� ����������� ���������� ���� ������������, ��������
		/// ����������� ������.
		/// </summary>
		[DBRead("StatisticsUserID")]
		[DBNullable()]
		public int? StatisticsUserID
		{
			get { return m_StatisticsUserID; }
			set { m_StatisticsUserID = value; }
		}
		#endregion

		#region ������
		/// <summary>
		/// ���������� ��� ��������, �� ������� �������� ������������.
		/// </summary>
		/// <param name="userID">ID ������������.</param>
		/// <returns>��� ��������, �� ������� �������� ������������.</returns>
		public static UserDelivery[] GetUserDeliveries(int userID)
		{
			BaseObjectCollection<UserDelivery> coll = (BaseObjectCollection<UserDelivery>)BasePlainObject.GetObjects(typeof(UserDelivery), "UserID", userID);
			if (coll == null)
				return null;
			else
				return coll.ToArray();
		}

		/// <summary>
		/// ���������� ���������� �������� ������������, ���� �� �� ��� ��������.
		/// </summary>
		/// <param name="userID">ID ������������.</param>
		/// <param name="delivery">��� ��������.</param>
		/// <returns>���������� �������� ������������.</returns>
		public static UserDelivery[] GetSpecificUserDelivery(int userID, Delivery delivery)
		{
			BaseObjectCollection<UserDelivery> coll = (BaseObjectCollection<UserDelivery>)BasePlainObject.GetObjects(typeof(UserDelivery), "UserID", userID, "DeliveryID", (int) delivery);
			if (coll == null)
				return null;
			else
				return coll.ToArray();
		}
		#endregion
	}
}
