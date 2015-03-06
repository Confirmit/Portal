using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

namespace UlterSystems.PortalLib.Statistics
{
	/// <summary>
	/// This class produces report to Moscow in Excel format.
	/// </summary>
	public class ReportToMoscowProducer
	{
		#region Fields
		private readonly Dictionary<int, Dictionary<DateTime, List<WorkEvent>>> m_EventsCache = new Dictionary<int, Dictionary<DateTime, List<WorkEvent>>>();
		private readonly Dictionary<int, string> m_CodesCache = new Dictionary<int, string>();
		private readonly DataTable m_ReportTable = new DataTable( "ExcelReport" );
		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		[DebuggerNonUserCode]
		public ReportToMoscowProducer()
		{
			m_ReportTable.Columns.Add( "RecordID", typeof( string ) );
			m_ReportTable.Columns.Add( "RegDate", typeof( string ) );
			m_ReportTable.Columns.Add( "USICode", typeof( string ) );
			m_ReportTable.Columns.Add( "DaySign", typeof( int ) );
			m_ReportTable.Columns.Add( "ComeTime", typeof( string ) );
			m_ReportTable.Columns.Add( "LeaveTime", typeof( string ) );
			m_ReportTable.Columns.Add( "Notes", typeof( string ) );
			m_ReportTable.Columns.Add( "Absense", typeof( int ) );
		}

		#endregion

		#region Methods

		/// <summary>
		/// Produces stream with Excel report for Moscow.
		/// </summary>
		/// <param name="beginDate">Date of report beginning.</param>
		/// <param name="endDate">Date of report ending.</param>
		/// <returns>Stream with Excel report for Moscow.</returns>
		public Stream ProduceReport( DateTime beginDate, DateTime endDate )
		{
			Person[] employees = UserList.GetEmployeeList();

			// Construct caches of events and user codes.
			FillCaches( employees, beginDate, endDate );

			for( DateTime date = beginDate.Date; date <= endDate.Date; date = date.AddDays( 1 ) )
			{
				foreach( Person employee in employees )
				{
					if( !m_EventsCache[ employee.ID.Value ].ContainsKey( date ) )
					{ continue; }
					if( !m_CodesCache.ContainsKey( employee.ID.Value ) )
					{ continue; }

					string userCode = m_CodesCache[ employee.ID.Value ];

					List<WorkEvent> dateEvents = m_EventsCache[ employee.ID.Value ][ date ];
					dateEvents.Sort( delegate( WorkEvent x, WorkEvent y )
					{
						return
							DateTime.
								Compare(
								x.BeginTime,
								y.BeginTime );
					} );

					ProcessDayEvents( dateEvents, userCode );
				}
			}

			return PrepareStream();
		}

		/// <summary>
		/// Fills caches of user codes and events.
		/// </summary>
		/// <param name="employees">Array of employees.</param>
		/// <param name="beginDate">Date of report beginning.</param>
		/// <param name="endDate">Date of report ending.</param>
		private void FillCaches( IEnumerable<Person> employees, DateTime beginDate, DateTime endDate )
		{
			Debug.Assert( m_EventsCache != null );
			Debug.Assert( m_CodesCache != null );

			m_EventsCache.Clear();
			m_CodesCache.Clear();

			foreach( Person employee in employees )
			{
                if (employee.USL_name != null)
				{
                    if (employee.USL_name.Length > 0)
						{ m_CodesCache[ employee.ID.Value ] = employee.USL_name; }
						else
						{ m_CodesCache[ employee.ID.Value ] = "???"; }
					}
				
				m_EventsCache[ employee.ID.Value ] = new Dictionary<DateTime, List<WorkEvent>>();

				WorkEvent[] events = WorkEvent.GetEventsOfRange( employee.ID.Value, beginDate, endDate );

				foreach( WorkEvent uptimeEvent in events )
				{
					if( !m_EventsCache[ employee.ID.Value ].ContainsKey( uptimeEvent.BeginTime.Date ) )
					{ m_EventsCache[ employee.ID.Value ][ uptimeEvent.BeginTime.Date ] = new List<WorkEvent>(); }

					m_EventsCache[ employee.ID.Value ][ uptimeEvent.BeginTime.Date ].Add( uptimeEvent );
				}
			}
		}

		/// <summary>
		/// Processes events for single day.
		/// </summary>
		/// <param name="dateEvents">Events.</param>
		/// <param name="userCode">3-letter code of user.</param>
		private void ProcessDayEvents( IEnumerable<WorkEvent> dateEvents, string userCode )
		{
			if( dateEvents == null )
				throw new ArgumentNullException( "dateEvents" );
			if( string.IsNullOrEmpty( userCode ) )
				throw new ArgumentNullException( "userCode" );

			WorkEvent mainWorkEvent = null;
			foreach( WorkEvent dateEvent in dateEvents )
			{
				switch( dateEvent.EventType )
				{
					case WorkEventType.BusinessTrip:
						WriteEvent( userCode, dateEvent.BeginTime,
										dateEvent.EndTime, dateEvent.EventType );
						break;
					case WorkEventType.Ill:
						WriteEvent( userCode, dateEvent.BeginTime,
										dateEvent.EndTime, dateEvent.EventType );
						break;
					case WorkEventType.LanchTime:
						break;
					case WorkEventType.MainWork:
						mainWorkEvent = dateEvent;
						break;
					case WorkEventType.OfficeOut:
						if( mainWorkEvent != null )
						{
							WriteEvent( userCode, mainWorkEvent.BeginTime, mainWorkEvent.EndTime, mainWorkEvent.EventType );
						}
						mainWorkEvent = dateEvent;
						break;
					case WorkEventType.StudyEnglish:
						if( mainWorkEvent != null )
						{
							WriteEvent( userCode, mainWorkEvent.BeginTime, dateEvent.BeginTime, mainWorkEvent.EventType );
							TimeSpan duration =
								TimeSpan.FromMilliseconds( dateEvent.Duration.Milliseconds / 2 );
							mainWorkEvent.BeginTime = dateEvent.BeginTime + duration;
						}
						break;
					case WorkEventType.TimeOff:
						if( mainWorkEvent != null )
						{
							WriteEvent( userCode, mainWorkEvent.BeginTime, dateEvent.BeginTime, mainWorkEvent.EventType );
							mainWorkEvent.BeginTime = dateEvent.EndTime;
						}
						break;
					case WorkEventType.TrustIll:
						WriteEvent( userCode, dateEvent.BeginTime,
										dateEvent.EndTime, dateEvent.EventType );
						break;
					case WorkEventType.Vacation:
						WriteEvent( userCode, dateEvent.BeginTime,
										dateEvent.EndTime, dateEvent.EventType );
						break;
				}
			}
			if( mainWorkEvent != null )
			{ WriteEvent( userCode, mainWorkEvent.BeginTime, mainWorkEvent.EndTime, mainWorkEvent.EventType ); }

		}

		/// <summary>
		/// Returns stream with information for Excel.
		/// </summary>
		/// <returns>Stream with information for Excel file.</returns>
		private Stream PrepareStream()
		{
			MemoryStream mStrm = new MemoryStream();
			Debug.Assert( m_ReportTable != null );
			m_ReportTable.WriteXml( mStrm );
			return mStrm;
		}

		/// <summary>
		/// Writes information about event.
		/// </summary>
		/// <param name="userCode">3-symbol code of user.</param>
		/// <param name="eventBegin">Beginning time.</param>
		/// <param name="eventEnd">Ending time.</param>
		/// <param name="eventType">Event type.</param>
		private void WriteEvent( string userCode, DateTime eventBegin, DateTime eventEnd, WorkEventType eventType )
		{
			if( string.IsNullOrEmpty( userCode ))
				throw new ArgumentNullException( "userCode" );

			Debug.Assert( m_ReportTable != null );

			string beginTime = eventBegin.ToString( "HH:mm" );
			string endTime = eventEnd.ToString( "HH:mm" );

			DataRow row = m_ReportTable.NewRow();
			row[ "RecordID" ] = string.Empty;
			row[ "RegDate" ] = eventBegin.ToString( "dd.MM.yy" );
			row[ "USICode" ] = userCode;

			switch( eventType )
			{
				case WorkEventType.BusinessTrip:
					row[ "DaySign" ] = 3;
					row[ "ComeTime" ] = beginTime;
					row[ "LeaveTime" ] = endTime;
					break;
				case WorkEventType.Ill:
					row[ "DaySign" ] = 4;
					row[ "ComeTime" ] = string.Empty;
					row[ "LeaveTime" ] = string.Empty;
					break;
				case WorkEventType.LanchTime:
					break;
				case WorkEventType.MainWork:
					row[ "DaySign" ] = 1;
					row[ "ComeTime" ] = beginTime;
					row[ "LeaveTime" ] = endTime;
					break;
				case WorkEventType.OfficeOut:
					row[ "DaySign" ] = 8;
					row[ "ComeTime" ] = beginTime;
					row[ "LeaveTime" ] = endTime;
					break;
				case WorkEventType.StudyEnglish:
					break;
				case WorkEventType.TimeOff:
					break;
				case WorkEventType.TrustIll:
					row[ "DaySign" ] = 5;
					row[ "ComeTime" ] = string.Empty;
					row[ "LeaveTime" ] = string.Empty;
					break;
				case WorkEventType.Vacation:
					row[ "DaySign" ] = 2;
					row[ "ComeTime" ] = string.Empty;
					row[ "LeaveTime" ] = string.Empty;
					break;
			}

			row[ "Notes" ] = string.Empty;
			row[ "Absense" ] = 0;

			m_ReportTable.Rows.Add( row );
		}

		#endregion
	}
}
