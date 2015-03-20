using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Globalization;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// ����� ��� ��� �������.
	/// </summary>
	public class DateClass
	{
		#region ������
		/// <summary>
		/// ���������� ���� ������ ������, � ������� ����������� �������� ����.
		/// </summary>
		/// <param name="date">���� ������ ������.</param>
		/// <param name="weekBeginDay">���� ������, � �������� ������ ����������.</param>
		/// <returns>���� ������ ������, � ������� ����������� �������� ����.</returns>
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
		/// ���������� ��������� ���� �������� ���������.
		/// </summary>
		/// <param name="hour">��� ��������.</param>
		/// <param name="min">������ ��������.</param>
		/// <returns>��������� ���� �������� ���������.</returns>
		public static DateTime GetNextStatisticsDeliveryDate(int hour, int min)
		{
			DateTime nextDeliveryDay;

			DateTime now = GetCurrentTime();

			DateTime today = now.Date;

			// ����� ����, ������� ���� ��������.
			if ((today.Day == 1) || (today.DayOfWeek == DayOfWeek.Monday))
			{
				nextDeliveryDay = new DateTime(now.Year, now.Month, now.Day, hour, min, 0);
				// ����� ���� ����� �������� ��� �� ������.
				if (nextDeliveryDay > now.AddSeconds(30))
					return nextDeliveryDay;
			}

			// �������� ������ ��������� ������.
			nextDeliveryDay = WeekBegin(today).AddDays(7);
			nextDeliveryDay = new DateTime(nextDeliveryDay.Year, nextDeliveryDay.Month, nextDeliveryDay.Day, hour, min, 0);

			// ���� ��� ��� � ������ ������, ������� ��� ������ �����.
			if (nextDeliveryDay.Month != today.Month)
			{
				nextDeliveryDay = today;
				nextDeliveryDay = nextDeliveryDay.AddMonths(1);
				nextDeliveryDay = new DateTime(nextDeliveryDay.Year, nextDeliveryDay.Month, 1, hour, min, 0);
			}

			return nextDeliveryDay;
		}

		/// <summary>
		/// ���������� ���� ������ ������, � ������� ����������� �������� ����.
		/// ������ ���������� � ������������.
		/// </summary>
		/// <param name="date">���� ������ ������.</param>
		/// <returns>���� ������ ������, � ������� ����������� �������� ����.</returns>
		public static DateTime WeekBegin(DateTime date)
		{ return WeekBegin(date, DayOfWeek.Monday); }

		/// <summary>
		/// ���������� ������� ���������� ������.
		/// </summary>
		/// <param name="sBegDate">������ ���������� ������.</param>
		/// <param name="sEndDate">����� ���������� ������.</param>
		public static void GetPeriodLastWeek(out string sBegDate, out string sEndDate)
		{
			// �������� ������� ����.
			DateTime now = DateTime.Now;
			
			// �������� ������ ���������� ������.
			DateTime weekBegin = WeekBegin(now).AddDays(-7);
			
			// �������� ������� ���������� ������.
			DateTime datBegin = new DateTime( weekBegin.Year, weekBegin.Month, weekBegin.Day, now.Hour, now.Minute, now.Second );
			DateTime datEnd = datBegin.AddDays( 6 );

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
		}

		/// <summary>
		/// ���������� ������� ������� ������.
		/// </summary>
		/// <param name="sBegDate">������ ������� ������.</param>
		/// <param name="sEndDate">����� ������� ������.</param>
		public static void GetPeriodCurrentWeek(out string sBegDate, out string sEndDate)
		{
			// �������� ������� ����.
			DateTime now = DateTime.Now;

			// �������� ������ ������� ������.
			DateTime weekBegin = WeekBegin(now);

			// �������� ������� ������� ������.
			DateTime datBegin = new DateTime(weekBegin.Year, weekBegin.Month, weekBegin.Day, now.Hour, now.Minute, now.Second);
			DateTime datEnd = datBegin.AddDays(6);

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
		}

		/// <summary>
		/// ���������� ������� �������� ������.
		/// </summary>
		/// <param name="sBegDate">������ �������� ������.</param>
		/// <param name="sEndDate">����� �������� ������.</param>
		public static void GetPeriodCurrentMonth(out string sBegDate, out string sEndDate)
		{
			// �������� ������� ����.
			DateTime now = DateTime.Now;

			// �������� ������� �������� ������.
			DateTime datBegin = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second);
			DateTime datEnd = datBegin.AddDays( DateTime.DaysInMonth(datBegin.Year, datBegin.Month) - 1 );

			sBegDate = datBegin.ToString( CultureInfo.InvariantCulture.DateTimeFormat );
			sEndDate = datEnd.ToString( CultureInfo.InvariantCulture.DateTimeFormat );

		}

        /// <summary>
        /// ���������� ������� � ������ ������ �� �������� �������.
        /// </summary>
        /// <param name="sBegDate">������ �������� ������.</param>
        /// <param name="sEndDate">����� �������. ������� ������.</param>
        public static void GetPeriodCurrentMonthToNow(out string sBegDate, out string sEndDate)
        {
            // �������� ������� ����.
            DateTime now = DateTime.Now;

            // �������� ������� �������� ������.
            DateTime datBegin = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second);

            sBegDate = datBegin.ToString(CultureInfo.InvariantCulture.DateTimeFormat);
            sEndDate = now.ToString(CultureInfo.InvariantCulture.DateTimeFormat);

        }

		/// <summary>
		/// ���������� ������� ����������� ������.
		/// </summary>
		/// <param name="sBegDate">������ ����������� ������.</param>
		/// <param name="sEndDate">����� ����������� ������.</param>
		public static void GetPeriodLastMonth(out string sBegDate, out string sEndDate)
		{
			// �������� ������� ����.
			DateTime now = DateTime.Now;

			// �������� ������� ����������� ������.
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

