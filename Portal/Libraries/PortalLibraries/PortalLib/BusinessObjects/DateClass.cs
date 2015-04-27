using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Globalization;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// Класс дат для отчетов.
	/// </summary>
	public class DateClass
	{
		#region Методы
		/// <summary>
		/// Возвращает дату начала недели, к которой принадлежит заданная дата.
		/// </summary>
		/// <param name="date">Дата внутри недели.</param>
		/// <param name="weekBeginDay">День недели, с которого неделя начинается.</param>
		/// <returns>Дата начала недели, к которой принадлежит заданная дата.</returns>
		public static DateTime WeekBegin(DateTime date, DayOfWeek weekBeginDay)
		{
			DateTime weekBegin = date.AddDays( - ( ((int) date.DayOfWeek) - ((int) weekBeginDay) ) );
			if (weekBegin > date)
				weekBegin = weekBegin.AddDays(-7);
			return weekBegin;
		}

		/// <summary>
		/// Returns current time.
		/// </summary>
		/// <returns>Current time.</returns>
		private static DateTime GetCurrentTime()
		{
			return DateTime.Now;
		}

		/// <summary>
		/// Возвращает следующую дату рассылки статистик.
		/// </summary>
		/// <param name="hour">Час рассылки.</param>
		/// <param name="min">Минута рассылки.</param>
		/// <returns>Следующая дата рассылки статистик.</returns>
		public static DateTime GetNextStatisticsDeliveryDate(int hour, int min)
		{
			DateTime nextDeliveryDay;

			DateTime now = GetCurrentTime();

			DateTime today = now.Date;

			// Может быть, сегодня день рассылки.
			if ((today.Day == 1) || (today.DayOfWeek == DayOfWeek.Monday))
			{
				nextDeliveryDay = new DateTime(now.Year, now.Month, now.Day, hour, min, 0);
				// Может быть время рассылки еще не прошло.
				if (nextDeliveryDay > now.AddSeconds(30))
					return nextDeliveryDay;
			}

			// Получить начало следующей недели.
			nextDeliveryDay = WeekBegin(today).AddDays(7);
			nextDeliveryDay = new DateTime(nextDeliveryDay.Year, nextDeliveryDay.Month, nextDeliveryDay.Day, hour, min, 0);

			// Если оно уже в другом месяце, вернуть его первое число.
			if (nextDeliveryDay.Month != today.Month)
			{
				nextDeliveryDay = today;
				nextDeliveryDay = nextDeliveryDay.AddMonths(1);
				nextDeliveryDay = new DateTime(nextDeliveryDay.Year, nextDeliveryDay.Month, 1, hour, min, 0);
			}

			return nextDeliveryDay;
		}

		/// <summary>
		/// Возвращает дату начала недели, к которой принадлежит заданная дата.
		/// Неделя начинается с понедельника.
		/// </summary>
		/// <param name="date">Дата внутри недели.</param>
		/// <returns>Дата начала недели, к которой принадлежит заданная дата.</returns>
		public static DateTime WeekBegin(DateTime date)
		{ return WeekBegin(date, DayOfWeek.Monday); }

		/// <summary>
		/// Возвращает границы предыдущей недели.
		/// </summary>
		/// <param name="sBegDate">Начало предыдущей недели.</param>
		/// <param name="sEndDate">Конец предыдущей недели.</param>
		public static void GetPeriodLastWeek(out string sBegDate, out string sEndDate)
		{
			// Получить текущую дату.
			DateTime now = DateTime.Now;
			
			// Получить начало предыдущей недели.
			DateTime weekBegin = WeekBegin(now).AddDays(-7);
			
			// Получить границы предыдущей недели.
			DateTime datBegin = new DateTime( weekBegin.Year, weekBegin.Month, weekBegin.Day, now.Hour, now.Minute, now.Second );
			DateTime datEnd = datBegin.AddDays( 6 );

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
		}

		/// <summary>
		/// Возвращает границы текущей недели.
		/// </summary>
		/// <param name="sBegDate">Начало текущей недели.</param>
		/// <param name="sEndDate">Конец текущей недели.</param>
		public static void GetPeriodCurrentWeek(out string sBegDate, out string sEndDate)
		{
			// Получить текущую дату.
			DateTime now = DateTime.Now;

			// Получить начало текущей недели.
			DateTime weekBegin = WeekBegin(now);

			// Получить границы текущей недели.
			DateTime datBegin = new DateTime(weekBegin.Year, weekBegin.Month, weekBegin.Day, now.Hour, now.Minute, now.Second);
			DateTime datEnd = datBegin.AddDays(6);

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
		}

		/// <summary>
		/// Возвращает границы текущего месяца.
		/// </summary>
		/// <param name="sBegDate">Начало текущего месяца.</param>
		/// <param name="sEndDate">Конец текущего месяца.</param>
		public static void GetPeriodCurrentMonth(out string sBegDate, out string sEndDate)
		{
			// Получить текущую дату.
			DateTime now = DateTime.Now;

			// Получить границы текущего месяца.
			DateTime datBegin = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second);
			DateTime datEnd = datBegin.AddDays( DateTime.DaysInMonth(datBegin.Year, datBegin.Month) - 1 );

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );

		}

        /// <summary>
        /// Возвращает границы с начала месяца до текущего момента.
        /// </summary>
        /// <param name="sBegDate">Начало текущего месяца.</param>
        /// <param name="sEndDate">Конец периода. Текущей момент.</param>
        public static void GetPeriodCurrentMonthToNow(out string sBegDate, out string sEndDate)
        {
            // Получить текущую дату.
            DateTime now = DateTime.Now;

            // Получить границы текущего месяца.
            DateTime datBegin = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second);

            sBegDate = datBegin.ToString(CultureInfo.InvariantCulture.DateTimeFormat);
            sEndDate = now.ToString(CultureInfo.InvariantCulture.DateTimeFormat);

        }

		/// <summary>
		/// Возвращает границы предыдущего месяца.
		/// </summary>
		/// <param name="sBegDate">Начало предыдущего месяца.</param>
		/// <param name="sEndDate">Конец предыдущего месяца.</param>
		public static void GetPeriodLastMonth(out string sBegDate, out string sEndDate)
		{
			// Получить текущую дату.
			DateTime now = DateTime.Now;

			// Получить границы предыдущего месяца.
			DateTime datBegin = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second).AddMonths(-1);
			DateTime datEnd = datBegin.AddDays( DateTime.DaysInMonth( datBegin.Year, datBegin.Month ) - 1 );

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
		}

		/// <summary>
		/// Tryes to parse dates from request query.
		/// </summary>
		/// <param name="request">Request object.</param>
		/// <param name="begin">Date of begining.</param>
		/// <param name="end">Date of end.</param>
		/// <returns>Was the parsing successfull.</returns>
		public static bool TryParseRequestQueryDates( HttpRequest request, out DateTime begin, out DateTime end )
		{
			begin = DateTime.MinValue;
			end = DateTime.MinValue;

			if( request == null )
				return false;

			if( string.IsNullOrEmpty( request.QueryString[ "BeginDate" ] ) )
				return false;
			if( string.IsNullOrEmpty( request.QueryString[ "EndDate" ] ) )
				return false;

			DateTimeFormatInfo dtfi = CultureInfo.InvariantCulture.DateTimeFormat;

			if( !DateTime.TryParse( request.QueryString[ "BeginDate" ], dtfi, DateTimeStyles.None, out begin ) )
				return false;
			if( !DateTime.TryParse( request.QueryString[ "EndDate" ], dtfi, DateTimeStyles.None, out end ) )
				return false;

			return true;
		}
		#endregion
	}
}

