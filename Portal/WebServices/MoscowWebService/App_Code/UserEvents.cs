using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using UlterSystems.PortalLib;
using System.Data.Common;

public enum MoscowEventType : short
{
	WorkDay = 1,		//	Рабочий день
	Vacation = 2,		// Отпуск
	BusinessTrip = 3,	// Командировка
	Ill = 4,				// Больничный
	TrustIll = 5,		// Больничный "по доверию"
	TakenDay = 6,		// День за свой счет
	OffDay = 7,			// Отгул
	HomeWork = 8,		// Работа дома
	Holiday = 9,		// Выходной
}

/// <summary>
/// Класс событий пользователя.
/// </summary>
public class MoscowUserEvents
{
	#region Поля
	private string m_UserCode;
	private DateTime m_Date;
	private DateTime? m_BeginTime;
	private DateTime? m_EndTime;
	private short m_EventType;
	#endregion

	#region Свойства
	/// <summary>
	/// Код пользователя.
	/// </summary>
	public string USLName
	{
		get { return m_UserCode; }
		set { m_UserCode = value; }
	}

	/// <summary>
	/// Дата события.
	/// </summary>
	public DateTime Date
	{
		get { return m_Date; }
		set { m_Date = value; }
	}

	/// <summary>
	/// Время начала события.
	/// </summary>
	public DateTime? BeginTime
	{
		get { return m_BeginTime; }
		set { m_BeginTime = value; }
	}

	/// <summary>
	/// Время окончания события.
	/// </summary>
	public DateTime? EndTime
	{
		get { return m_EndTime; }
		set { m_EndTime = value; }
	}

	/// <summary>
	/// Код типа события.
	/// </summary>
	public short EventTypeCode
	{
		get { return m_EventType; }
		set { m_EventType = value; }
	}

	/// <summary>
	/// Тип события.
	/// </summary>
	public MoscowEventType EventType
	{
		get { return (MoscowEventType)m_EventType; }
		set { m_EventType = (short)value; }
	}
	#endregion

	#region Конструкторы
	/// <summary>
	/// Конструктор.
	/// </summary>
	public MoscowUserEvents()
	{ }
	#endregion

	#region Методы
	/// <summary>
	/// Возвращает все события пользователя за указанную дату.
	/// </summary>
	/// <param name="user">Пользователь.</param>
	/// <param name="date">Дата.</param>
	/// <returns>Все события пользователя за указанную дату.</returns>
	public static MoscowUserEvents[] GetUserEvents(MoscowUser user, DateTime date)
	{
		if (user == null)
			return new MoscowUserEvents[0];
		return GetUserEvents(user.USLName, date);
	}

	/// <summary>
	/// Возвращает все события пользователя за указанную дату.
	/// </summary>
	/// <param name="userCode">Код пользователя.</param>
	/// <param name="date">Дата.</param>
	/// <returns>Все события пользователя за указанную дату.</returns>
	public static MoscowUserEvents[] GetUserEvents(string userCode, DateTime date)
	{
		if (string.IsNullOrEmpty(userCode))
			return new MoscowUserEvents[0];

		List<MoscowUserEvents> coll = new List<MoscowUserEvents>();
		try
		{
			DbProviderFactory dbFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DBConnStr"].ProviderName);
			using (DbConnection connection = dbFactory.CreateConnection())
			{
				connection.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
				using (DbCommand command = dbFactory.CreateCommand())
				{
					command.Connection = connection;
					command.CommandText = "SELECT DaySign, ComeTime, LeaveTime FROM [ET Work Table] WHERE (USICode = @prmUserCode) AND (RegDate >= @prmBeginDate) AND (RegDate <= @prmEndDate)";

					DbParameter prmUserCode = dbFactory.CreateParameter();
					prmUserCode.ParameterName = "@prmUserCode";
					prmUserCode.Value = userCode;
					command.Parameters.Add(prmUserCode);

					DbParameter prmBeginDate = dbFactory.CreateParameter();
					prmBeginDate.ParameterName = "@prmBeginDate";
					prmBeginDate.DbType = DbType.DateTime;
					prmBeginDate.Value = date.Date;
					command.Parameters.Add(prmBeginDate);

					DbParameter prmEndDate = dbFactory.CreateParameter();
					prmEndDate.ParameterName = "@prmEndDate";
					prmEndDate.DbType = DbType.DateTime;
					prmEndDate.Value = date.Date.AddDays(1).AddSeconds(-1);
					command.Parameters.Add(prmEndDate);

					connection.Open();

					using (DbDataReader reader = command.ExecuteReader())
					{
						while( reader.Read() )
						{
							MoscowUserEvents curEvent = new MoscowUserEvents();
							curEvent.USLName = userCode;
							curEvent.Date = date.Date;
							curEvent.EventTypeCode = (short)reader["DaySign"];
							if (reader["ComeTime"] != DBNull.Value)
							{ curEvent.BeginTime = (DateTime)reader["ComeTime"]; }
							else
							{ curEvent.BeginTime = null; }
							if (reader["LeaveTime"] != DBNull.Value)
							{ curEvent.EndTime = (DateTime)reader["LeaveTime"]; }
							else
							{ curEvent.EndTime = null; }
							coll.Add(curEvent);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{ Logger.Log.Error(String.Format(Resources.Strings.GerUserEventsError, userCode, date.ToShortDateString()), ex); }

		return coll.ToArray();
	}
	#endregion
}
