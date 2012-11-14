using System;
using System.Collections.Generic;
using System.Text;

namespace UlterSystems.PortalLib.Statistics
{
	/// <summary>
	///  ласс дл€ преобразовани€ дат и времен в строки.
	/// </summary>
	public static class DateTimePresenter
	{
		/// <summary>
		/// ¬озвращает строковое представление отрезка времени.
		/// </summary>
		/// <param name="span">ќтрезок времени.</param>
		/// <returns>—троковое представление отрезка времени.</returns>
		public static string GetTime(TimeSpan span)
		{
			bool negative = (span < TimeSpan.Zero);
			if (negative)
				span = -span;

			StringBuilder sb = new StringBuilder();
			int totalHours = (int) span.TotalHours;
			span -= TimeSpan.FromHours(totalHours);
			int totalMinutes = (int)span.TotalMinutes;
			span -= TimeSpan.FromMinutes(totalMinutes);
			int totalSeconds = (int)span.TotalSeconds;

			sb.AppendFormat("{0:00}:{1:00}:{2:00}", totalHours, totalMinutes, totalSeconds);

			if (negative)
				sb.Insert(0, "-");

			return sb.ToString();
		}
	}
}
