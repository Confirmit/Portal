using System;
using System.Diagnostics;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Properties;

namespace UlterSystems.PortalLib.Statistics
{
	/// <summary>
	/// ����� ���������� ������������ �� ����.
	/// </summary>
	[Serializable]
	public class DayUserStatistics
	{
		#region ����
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
		private TimeSpan m_TimeOffTime = TimeSpan.Zero;
		#endregion

		#region ��������
		/// <summary>
		/// ����, �� ������� ������������ ����������.
		/// </summary>
		public DateTime Date
		{
			[DebuggerStepThrough]
			get { return m_Date; }
			[DebuggerStepThrough]
			set { m_Date = value; }
		}

		/// <summary>
		/// ������� ���������� � ������ ����. Null, ���� ����� ������� ���.
		/// </summary>
		/// <remarks>����� ��������� ������ ����������, ������ ��� ������������.</remarks>
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
		/// ������� �� ������������ � ������ ����.
		/// </summary>
		public bool IsWorked
		{
			[DebuggerStepThrough]
			get { return m_Worked; }
			[DebuggerStepThrough]
			set { m_Worked = value; }
		}

		/// <summary>
		/// ����� ������ ������ �� ������ ����.
		/// </summary>
		public DateTime BeginTime
		{
			[DebuggerStepThrough]
			get { return m_BeginTime; }
			[DebuggerStepThrough]
			set { m_BeginTime = value; }
		}

		/// <summary>
		/// ����� ������ ������ �� ������ ����.
		/// </summary>
		public DateTime EndTime
		{
			[DebuggerStepThrough]
			get { return m_EndTime; }
			[DebuggerStepThrough]
			set { m_EndTime = value; }
		}

		/// <summary>
		/// ����� ������� ����� �� ������ ����.
		/// </summary>
		public TimeSpan TotalTime
		{
			[DebuggerStepThrough]
			get { return m_TotalTime; }
			[DebuggerStepThrough]
			set { m_TotalTime = value; }
		}

		/// <summary>
		/// ������ ������� ����� �� ������ ����.
		/// </summary>
		public TimeSpan WorkTime
		{
			[DebuggerStepThrough]
			get { return m_WorkTime; }
			[DebuggerStepThrough]
			set { m_WorkTime = value; }
		}

		/// <summary>
		/// ��������� ����� �� ������ ����.
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
	/// ��������� ��� �������� ������� �� ��������� ����.
	/// </summary>
	public struct DayWorkTime
	{
		#region ����
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime m_Date;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private TimeSpan m_WorkTime;
		#endregion

		#region ��������
		/// <summary>
		/// ����, �� ������� ���������� ������� �����.
		/// </summary>
		public DateTime Date
		{
			[DebuggerStepThrough]
			get { return m_Date; }
			[DebuggerStepThrough]
			set { m_Date = value; }
		}

		/// <summary>
		/// ������� ����� �� ��������� ����.
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
