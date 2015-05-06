using System;
using System.Diagnostics;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Properties;

namespace UlterSystems.PortalLib.Statistics
{
	/// <summary>
	/// Класс статистики пользователя за день.
	/// </summary>
	[Serializable]
	public class DayUserStatistics
	{
		#region Поля
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime m_Date;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private WorkEventType? m_AbsenceReason = null;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool m_Worked = false;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_BeginTime;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private DateTime m_EndTime;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_TotalTime = TimeSpan.Zero;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_WorkTime = TimeSpan.Zero;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_DinnerTime = TimeSpan.Zero;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_TimeOffTime = TimeSpan.Zero;
		#endregion

		#region Свойства
		/// <summary>
		/// Дата, за которую представлена статистика.
		/// </summary>
		public DateTime Date
		{
			[DebuggerStepThrough]
			get { return m_Date; }
			[DebuggerStepThrough]
			set { m_Date = value; }
		}

		/// <summary>
		/// Причина отсутствия в данный день. Null, если такой причины нет.
		/// </summary>
		/// <remarks>Можно выставить только больничный, отпуск или командировку.</remarks>
		public WorkEventType? AbsenceReason
		{
			[DebuggerStepThrough]
			get { return m_AbsenceReason; }
			[DebuggerNonUserCode]
			set 
			{ 
				if( value == null )
				{ m_AbsenceReason = value; }
				else
				{
					if( WorkEvent.IsAbsenceEvent( (int) value.Value ))
					{ m_AbsenceReason = value; }
					else
					{ throw new Exception( Resources.IllegalAbsenceReason ); }
				}
			}
		}

		/// <summary>
		/// Работал ли пользователь в данную дату.
		/// </summary>
		public bool IsWorked
		{
			[DebuggerStepThrough]
			get { return m_Worked; }
			[DebuggerStepThrough]
			set { m_Worked = value; }
		}

		/// <summary>
		/// Время начала работы за данную дату.
		/// </summary>
		public DateTime BeginTime
		{
			[DebuggerStepThrough]
			get { return m_BeginTime; }
			[DebuggerStepThrough]
			set { m_BeginTime = value; }
		}

		/// <summary>
		/// Время начала работы за данную дату.
		/// </summary>
		public DateTime EndTime
		{
			[DebuggerStepThrough]
			get { return m_EndTime; }
			[DebuggerStepThrough]
			set { m_EndTime = value; }
		}

		/// <summary>
		/// Общее рабочее время за данную дату.
		/// </summary>
		public TimeSpan TotalTime
		{
			[DebuggerStepThrough]
			get { return m_TotalTime; }
			[DebuggerStepThrough]
			set { m_TotalTime = value; }
		}

		/// <summary>
		/// Чистое рабочее время за данную дату.
		/// </summary>
		public TimeSpan WorkTime
		{
			[DebuggerStepThrough]
			get { return m_WorkTime; }
			[DebuggerStepThrough]
			set { m_WorkTime = value; }
		}

		/// <summary>
		/// Обеденное время за данную дату.
		/// </summary>
		public TimeSpan DinnerTime
		{
			get { return m_DinnerTime; }
			set { m_DinnerTime = value; }
		}

		/// <summary>
		/// Нерабочее время за данную дату.
		/// </summary>
		public TimeSpan TimeOffTime
		{
			[DebuggerStepThrough]
			get { return m_TimeOffTime; }
			[DebuggerStepThrough]
			set { m_TimeOffTime = value; }
		}
		#endregion
	}

	/// <summary>
	/// Хранилище для рабочего времени за указанную дату.
	/// </summary>
	public struct DayWorkTime
	{
		#region Поля
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime m_Date;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_WorkTime;
		#endregion

		#region Свойства
		/// <summary>
		/// Дата, за которую рассчитано рабочее время.
		/// </summary>
		public DateTime Date
		{
			[DebuggerStepThrough]
			get { return m_Date; }
			[DebuggerStepThrough]
			set { m_Date = value; }
		}

		/// <summary>
		/// Рабочее время за указанную дату.
		/// </summary>
		public TimeSpan WorkTime
		{
			[DebuggerStepThrough]
			get { return m_WorkTime; }
			[DebuggerStepThrough]
			set { m_WorkTime = value; }
		}
		#endregion
	}
}
