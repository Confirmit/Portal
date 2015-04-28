using System;
using System.Collections.Generic;
using System.Text;

using Core.ORM.Attributes;
using Core;

namespace UlterSystems.PortalLib.Notification
{
	/// <summary>
	/// Типы рассылок.
	/// </summary>
	public enum Delivery
	{
		User = 1,
		Office = 2,
	}

	/// <summary>
	/// Типы представления данных в рассылках.
	/// </summary>
	public enum DeliveryPresentation
	{
		HTML = 1,
		XML = 2,
		Excel = 3,
	}

	/// <summary>
	/// Класс информации о рассылке для пользователя.
	/// </summary>
	[DBTable("UsersDeliveries")]
	public class UserDelivery : BasePlainObject
	{
		#region Поля
		private int m_UserID;
		private int m_DeliveryID;
		private int m_DeliveryPresentation;
		private int? m_StatisticsUserID;
		#endregion

		#region Свойства
		/// <summary>
		/// ID пользователя, получателя рассылки.
		/// </summary>
		[DBRead("UserID")]
		public int UserID
		{
			get { return m_UserID; }
			set { m_UserID = value; }
		}

		/// <summary>
		/// Идентификатор рассылки.
		/// </summary>
		[DBRead("DeliveryID")]
		public int DeliveryID
		{
			get { return m_DeliveryID; }
			set { m_DeliveryID = value; }
		}

		/// <summary>
		/// Тип рассылки.
		/// </summary>
		public Delivery Delivery
		{
			get { return (Delivery)m_DeliveryID; }
			set { m_DeliveryID = (int)value; }
		}

		/// <summary>
		/// Идентификатор представления рассылки.
		/// </summary>
		[DBRead("DeliveryPresentation")]
		public int DeliveryPresentationID
		{
			get { return m_DeliveryPresentation; }
			set { m_DeliveryPresentation = value; }
		}

		/// <summary>
		/// Представление рассылки.
		/// </summary>
		public DeliveryPresentation DeliveryPresentation
		{
			get { return (DeliveryPresentation)m_DeliveryPresentation; }
			set { m_DeliveryPresentation = (int)value; }
		}

		/// <summary>
		/// ID пользователя, чья статистика должна быть прислана.
		/// Если null, то присылается статистика того пользователя, которому
		/// принадлежит объект.
		/// </summary>
		[DBRead("StatisticsUserID")]
		[DBNullable()]
		public int? StatisticsUserID
		{
			get { return m_StatisticsUserID; }
			set { m_StatisticsUserID = value; }
		}
		#endregion

		#region Методы
		/// <summary>
		/// Возвращает все рассылки, на которые подписан пользователь.
		/// </summary>
		/// <param name="userID">ID пользователя.</param>
		/// <returns>Все рассылки, на которые подписан пользователь.</returns>
		public static UserDelivery[] GetUserDeliveries(int userID)
		{
			BaseObjectCollection<UserDelivery> coll = (BaseObjectCollection<UserDelivery>)BasePlainObject.GetObjects(typeof(UserDelivery), "UserID", userID);
			if (coll == null)
				return null;
			else
				return coll.ToArray();
		}

		/// <summary>
		/// Возвращает конкретную рассылку пользователя, если он на нее подписан.
		/// </summary>
		/// <param name="userID">ID пользователя.</param>
		/// <param name="delivery">Тип рассылки.</param>
		/// <returns>Конкретная рассылка пользователя.</returns>
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
