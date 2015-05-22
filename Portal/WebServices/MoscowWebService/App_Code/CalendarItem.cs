using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using UlterSystems.PortalLib;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Класс дня календаря. Содержит информацию о времени работы.
/// </summary>
public class CalendarItem
{
	#region Поля
	private DateTime m_Date;
	private TimeSpan m_WorkTime;
	private static Dictionary<DateTime, CalendarItem> m_Cache = new Dictionary<DateTime, CalendarItem>();
	#endregion

	#region Свойства
	/// <summary>
	/// Дата.
	/// </summary>
	public DateTime Date
	{
		get { return m_Date; }
		set { m_Date  = value; }
	}

	/// <summary>
	/// Рабочее время.
	/// </summary>
	public TimeSpan WorkTime
	{
		get { return m_WorkTime; }
		set { m_WorkTime = value; }
	}
	#endregion

	#region Конструкторы
	/// <summary>
	/// Конструктор.
	/// </summary>
	public CalendarItem()
	{ }
	#endregion

	#region Методы
	/// <summary>
	/// Возвращает информацию обо дне.
	/// </summary>
	/// <param name="date">Дата</param>
	/// <returns>Информация об дате.</returns>
	public static CalendarItem GetCalendarItem(DateTime date)
	{
		Debug.Assert(m_Cache != null);

		if (m_Cache.ContainsKey(date))
		{ return m_Cache[date]; }

		try
		{
			DbProviderFactory dbFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DBConnStr"].ProviderName);
			using (DbConnection connection = dbFactory.CreateConnection())
			{
				connection.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
				using (DbCommand command = dbFactory.CreateCommand())
				{
					command.Connection = connection;
					command.CommandText = "SELECT WorkTime FROM [ET Default Calendar] WHERE (dDay >= @prmBeginDate) AND (dDay <= @prmEndDate)";

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
						if ( reader.Read() )
						{
							CalendarItem item = new CalendarItem();
							item.Date = date.Date;
							item.WorkTime = ((DateTime)reader["WorkTime"]).TimeOfDay;
							m_Cache[date] = item;
							return item;
						}
						else
						{
							m_Cache[date] = null;
							return null; 
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Log.Error(Resources.Strings.GetCalendarItemError, ex);
			return null;
		}
	}
	#endregion
}
