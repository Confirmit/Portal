//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using ConfirmIt.PortalLib.BAL;

//namespace UlterSystems.PortalLib.BusinessObjects
//{
//   /// <summary>
//   /// ����� ��� ���������� ��������� ��������� �������������.
//   /// </summary>
//   public class TimesCalculator
//   {
//      private const int m_DinnerMinutesLimit = 10;

//      #region ����
//      private readonly int m_UserID;
//      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//      private readonly UserUptimeEvents m_UserEvents;
//      #endregion

//      #region ������������
//      /// <summary>
//      /// �����������.
//      /// </summary>
//      /// <param name="userID">ID ������������.</param>
//      public TimesCalculator( int userID )
//      {
//         m_UserID = userID;
//         m_UserEvents = new UserUptimeEvents( userID );
//      }

//      /// <summary>
//      /// �����������.
//      /// </summary>
//      /// <param name="user">������������.</param>
//      public TimesCalculator( Person user )
//      {
//         if( user == null )
//            throw new ArgumentNullException( "user" );
//         if( !user.ID.HasValue )
//            throw new Exception();

//         m_UserID = user.ID.Value;
//         m_UserEvents = new UserUptimeEvents( m_UserID );
//      }
//      #endregion

//      #region ������

//      #region ������ ��������� ���������� �������
//      /// <summary>
//      /// ���������� ��������� ����� �� ������ ����.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>��������� ����� �� ������ ����.</returns>
//      public TimeSpan GetDinnerTime( DateTime date )
//      {
//         TimeSpan output = TimeSpan.Zero;

//         // �������� ������� ��������� ���.
//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         if( begin == DateTime.Today )
//         {
//            #region ������� ��� ������������ ���
//            Debug.Assert( m_UserEvents != null );

//            DateTime now = DateTime.Now;

//            // �������� ��� �������.
//            List<UptimeEvent> dateEvents = m_UserEvents.GetEvents( begin, end );
//            if( ( dateEvents == null ) || ( dateEvents.Count == 0 ) )
//            { return TimeSpan.Zero; }

//            foreach( UptimeEvent evnt in dateEvents )
//            {
//               switch( evnt.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.LanchTime:
//                     if( evnt.BeginTime == evnt.EndTime )
//                     { output += ( now - evnt.BeginTime ); }
//                     else
//                     { output += ( evnt.EndTime - evnt.BeginTime ); }
//                     break;
//               }
//            }
//            #endregion
//         }
//         else if( begin > DateTime.Today )
//         {
//            #region ������� ��� ���� � �������
//            // ���� � �������.
//            return TimeSpan.Zero;
//            #endregion
//         }
//         else
//         {
//            #region ������� ���� � �������
//            Debug.Assert( m_UserEvents != null );

//            // �������� ������ ������� �� ����.
//            List<UptimeEvent> dateEvents = m_UserEvents.GetEvents( begin, end );
//            if( ( dateEvents == null ) || ( dateEvents.Count == 0 ) )
//               return new TimeSpan( 0 );

//            foreach( UptimeEvent evnt in dateEvents )
//            {
//               switch( evnt.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.LanchTime:
//                     output += ( evnt.EndTime - evnt.BeginTime );
//                     break;
//               }
//            }
//            #endregion
//         }

//         return output;
//      }

//      /// <summary>
//      /// ���������� ��������� ����� �� �������.
//      /// </summary>
//      /// <returns>��������� ����� �� �������.</returns>
//      public TimeSpan GetTodayDinnerTime()
//      { return GetDinnerTime( DateTime.Today ); }
//      #endregion

//      #region ������ ��� ��������� �������� �������

//      /// <summary>
//      /// ���������� ������ �����, ����������� � �����, �� ������� �� �����.
//      /// � ������ ����� ������ �������, ����������� �� ���� � ��������� �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>������ �����, ����������� � �����, �� ������� �� �����.</returns>
//      public TimeSpan GetTotalTime( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // ������� ���� ������ ���.
//         date = date.Date;

//         // There is no work in future.
//         if( date > DateTime.Today )
//            return TimeSpan.Zero;

//         // �������� ������� ������� �� ��������� ����.
//         UptimeEvent workEvent = m_UserEvents.GetWorkEvent( date );
//         if( workEvent == null )
//            return TimeSpan.Zero;

//         if( date == DateTime.Today )
//         {
//            // �������� ������� �����.
//            DateTime now = DateTime.Now;

//            if( workEvent.EndTime == workEvent.BeginTime )
//            { return ( now - workEvent.BeginTime ); }
//            else
//            { return ( workEvent.EndTime - workEvent.BeginTime ); }
//         }
//         else
//         { return ( workEvent.EndTime - workEvent.BeginTime ); }
//      }

//      /// <summary>
//      /// ���������� ������ �����, ����������� � �����, �� ������� �� ����� �������.
//      /// � ������ ����� ������ �������, ����������� �� ���� � ��������� �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// </summary>
//      /// <returns>������ �����, ����������� � �����, �� ������� �� ����� �������.</returns>
//      public TimeSpan GetTodayTotalTime()
//      { return GetTotalTime( DateTime.Today ); }

//      /// <summary>
//      /// ���������� ������ �����, ����������� � �����, �� ��������� ���������� ���.
//      /// </summary>
//      /// <param name="begin">���� ������ ���������.</param>
//      /// <param name="end">���� ��������� ���������.</param>
//      /// <returns>������ �����, ����������� � �����, �� ��������� ���������� ���.</returns>
//      public TimeSpan GetTotalTime( DateTime begin, DateTime end )
//      {
//         // ��������� ������� ���.
//         begin = begin.Date;
//         end = end.Date.AddSeconds( 1 );

//         TimeSpan output = TimeSpan.Zero;
//         while( begin < end )
//         {
//            output += GetTotalTime( begin );
//            begin = begin.AddDays( 1 );
//         }

//         return output;
//      }

//      /// <summary>
//      /// ���������� ������������ ����� �� �������� �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>������������ ����� �� �������� �����.</returns>
//      public TimeSpan GetWorkedTime( DateTime date )
//      {
//         TimeSpan output = TimeSpan.Zero;

//         // ��������� ������� ������ � ��������� ���������� ���.
//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         if( begin == DateTime.Today )
//         {
//            #region ������� ��� ������������ ���
//            Debug.Assert( m_UserEvents != null );

//            DateTime now = DateTime.Now;

//            // �������� ������ ���� �������.
//            List<UptimeEvent> dateEvents = m_UserEvents.GetEvents( begin, end );
//            if( ( dateEvents == null ) || ( dateEvents.Count == 0 ) )
//            { return output; }

//            foreach( UptimeEvent evnt in dateEvents )
//            {
//               switch( evnt.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.MainWork:
//                  case UptimeEvent.EventType.OfficeOut:
//                     if( evnt.BeginTime == evnt.EndTime )
//                     { output += ( now - evnt.BeginTime ); }
//                     else
//                     { output += ( evnt.EndTime - evnt.BeginTime ); }
//                     break;
//                  case UptimeEvent.EventType.BusinessTrip:
//                  case UptimeEvent.EventType.Vacation:
//                  case UptimeEvent.EventType.Ill:
//                  case UptimeEvent.EventType.TrustIll:
//                     // �� ��������� ������� �������, ������������, �������.
//                     break;
//                  case UptimeEvent.EventType.StudyEnglish:
//                     // Only half of study time is working time.
//                     TimeSpan studyTime;
//                     if( evnt.BeginTime == evnt.EndTime )
//                     { studyTime = ( now - evnt.BeginTime ); }
//                     else
//                     { studyTime = ( evnt.EndTime - evnt.BeginTime ); }
//                     studyTime = TimeSpan.FromSeconds( studyTime.TotalSeconds / 2.0 );
//                     output -= studyTime;
//                     break;
//                  default:
//                     if( evnt.BeginTime == evnt.EndTime )
//                     { output -= ( now - evnt.BeginTime ); }
//                     else
//                     { output -= ( evnt.EndTime - evnt.BeginTime ); }
//                     break;
//               }
//            }
//            #endregion
//         }
//         else if( begin > DateTime.Today )
//         {
//            #region ������� ��� ���� � �������
//            // ���� � �������.
//            return TimeSpan.Zero;
//            #endregion
//         }
//         else
//         {
//            #region ������� ���� � �������
//            Debug.Assert( m_UserEvents != null );

//            // �������� ������ ������� �� ����.
//            List<UptimeEvent> dateEvents = m_UserEvents.GetEvents( begin, end );
//            if( ( dateEvents == null ) || ( dateEvents.Count == 0 ) )
//               return output;

//            foreach( UptimeEvent evnt in dateEvents )
//            {
//               switch( evnt.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.MainWork:
//                  case UptimeEvent.EventType.OfficeOut:
//                     output += ( evnt.EndTime - evnt.BeginTime );
//                     break;
//                  case UptimeEvent.EventType.BusinessTrip:
//                  case UptimeEvent.EventType.Vacation:
//                  case UptimeEvent.EventType.Ill:
//                  case UptimeEvent.EventType.TrustIll:
//                     // �� ��������� ������� �������, ������������, �������.
//                     break;
//                  case UptimeEvent.EventType.StudyEnglish:
//                     // Only half of study time is working time.
//                     TimeSpan studyTime = ( evnt.EndTime - evnt.BeginTime );
//                     studyTime = TimeSpan.FromSeconds( studyTime.TotalSeconds / 2.0 );
//                     output -= studyTime;
//                     break;
//                  default:
//                     output -= ( evnt.EndTime - evnt.BeginTime );
//                     break;
//               }
//            }
//            #endregion
//         }

//         // �� ����������� ������������� ������.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// ���������� ������������ ������� �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// </summary>
//      /// <returns>������������ ������� �����.</returns>
//      public TimeSpan GetTodayWorkedTime()
//      { return GetWorkedTime( DateTime.Now ); }

//      /// <summary>
//      /// ���������� ������������ ����� �� �������� �������� ���.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// </summary>
//      /// <param name="begin">���� ������ ���������.</param>
//      /// <param name="end">���� ��������� ���������.</param>
//      /// <returns>���������� ������������ ����� �� �������� �������� ���.</returns>
//      public TimeSpan GetWorkedTime( DateTime begin, DateTime end )
//      {
//         // ��������� ������� ���.
//         begin = begin.Date;
//         end = end.Date.AddSeconds( 1 );

//         TimeSpan output = TimeSpan.Zero;
//         while( begin < end )
//         {
//            output += GetWorkedTime( begin );
//            begin = begin.AddDays( 1 );
//         }

//         return output;
//      }

//      /// <summary>
//      /// ���������� ������������ ����� �� �������� ����� � ������ �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// ����������� ��������� �����.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>���������� ������������ ����� �� �������� ����� � ������ �����.</returns>
//      public TimeSpan GetCorrectWorkedTime( DateTime date )
//      {
//         if( date.Date > DateTime.Today )
//            return TimeSpan.Zero;

//         // �������� ��������� � ������� �����.
//         TimeSpan workedTime;
//         TimeSpan dinnerTime;
//         GetWorkAndDinnerTimes( date, out workedTime, out dinnerTime );

//         TimeSpan output = workedTime;

//         if( output <= TimeSpan.FromHours( 4 ) )
//         {
//            if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//            { output += dinnerTime; }
//         }
//         else
//         {
//            if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//            { output += ( dinnerTime - TimeSpan.FromMinutes( 30 ) ); }
//         }

//         // �� ���������� ������������� �������.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// ���������� ������������ ����� �� ������� � ������ �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// ����������� ��������� �����.
//      /// </summary>
//      /// <returns>���������� ������������ ����� �� ������� � ������ �����.</returns>
//      public TimeSpan GetTodayCorrectWorkedTime()
//      { return GetCorrectWorkedTime( DateTime.Today ); }

//      /// <summary>
//      /// ���������� ������������ ����� �� �������� �������� ��� � ������ �����.
//      /// ��� ���� �� �����������, �������� �� ������ ����� �������� ��� ����������.
//      /// ��� �� �� �����������, ��������� �� ������������ � �������, � ������������, ������.
//      /// ����������� ��������� �����.
//      /// </summary>
//      /// <param name="begin">���� ������ ���������.</param>
//      /// <param name="end">���� ��������� ���������.</param>
//      /// <returns>������������ ����� �� �������� �������� ��� � ������ �����.</returns>
//      public TimeSpan GetCorrectWorkedTime( DateTime begin, DateTime end )
//      {
//         // ��������� ������� ���.
//         begin = begin.Date;
//         end = end.Date.AddSeconds( 1 );

//         TimeSpan output = TimeSpan.Zero;
//         while( begin < end )
//         {
//            output += GetCorrectWorkedTime( begin );
//            begin = begin.AddDays( 1 );
//         }

//         return output;
//      }
//      #endregion

//      #region ������ ��� ��������� �������, ������� �������� ����������
//      /// <summary>
//      /// ���������� ���������� �������, ������� ����� ���������� � ������ �����.
//      /// � ������, ���� ������ ����� - �������� ��� ��������, ������������ 0.
//      /// � ������, ���� �� ������ ����� ������������ � ������������, � �������, ������,
//      /// ������������ 0.
//      /// </summary>
//      /// <param name="date">�����.</param>
//      /// <returns>���������� �������, ������� ����� ���������� � ������ �����.</returns>
//      public TimeSpan GetRestTime( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // ������ ���������.
//         if( CalendarItem.GetHoliday( date ) )
//            return TimeSpan.Zero;

//         // ������ ����������, �������, ������������.
//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         TimeSpan output;

//         // ��������� ���� � �������.
//         if( date.Date > DateTime.Today )
//         { output = new TimeSpan( 8, 30, 0 ); }
//         else if( date.Date == DateTime.Today )
//         { output = GetTodayRestTime(); }
//         else
//         { output = TimeSpan.FromHours( 8 ) - GetCorrectWorkedTime( date ); }

//         // �� ���������� ������������� �����.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// ���������� �����, ������� ����� ���������� �������.
//      /// � ������, ���� ������ ����� - �������� ��� ��������, ������������ 0.
//      /// � ������, ���� �� ������ ����� ������������ � ������������, � �������, ������,
//      /// ������������ 0.
//      /// </summary>
//      /// <returns>�����, ������� ����� ���������� �������.</returns>
//      public TimeSpan GetTodayRestTime()
//      {
//         Debug.Assert( m_UserEvents != null );

//         // �������� ����������� ����.
//         DateTime date = DateTime.Today;

//         // ������ ���������.
//         if( CalendarItem.GetHoliday( date ) )
//            return TimeSpan.Zero;

//         // ������ ����������, �������, ������������.
//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         // �������� ������� � ��������� �����.
//         TimeSpan workTime;
//         TimeSpan dinnerTime;
//         GetWorkAndDinnerTimes( date, out workTime, out dinnerTime );



//         TimeSpan output = CalendarItem.GetWorkTime( date ) - workTime;
//         if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//         { output += ( TimeSpan.FromMinutes( 30 ) - dinnerTime ); }

//         // �� ���������� ������������� �����.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// ���������� ���������� �������, ������� ����� ���������� �� ������ �������� ���.
//      /// � ������, ���� ����� - �������� ��� ��������, ������ ����� - 0.
//      /// � ������, ���� �� ����� ������������ � ������������, � �������, ������,
//      /// ������ ����� - 0.
//      /// </summary>
//      /// <param name="begin">���� ������ ���������.</param>
//      /// <param name="end">���� ��������� ���������.</param>
//      /// <returns>���������� �������, ������� ����� ���������� �� ������ �������� ���.</returns>
//      public TimeSpan GetRestTime( DateTime begin, DateTime end )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // ��������� ������� ���.
//         begin = begin.Date;
//         end = end.Date.AddSeconds( 1 );

//         TimeSpan output = TimeSpan.Zero;
//         while( begin < end )
//         {
//            // ������ ��������, ���������, ���������� � �.�. 
//            if( !( CalendarItem.GetHoliday( begin ) || ( m_UserEvents.HaveAbsenceReason( begin ) ) ) )
//            {
//               output += CalendarItem.GetWorkTime( begin );
//            }

//            if( begin == DateTime.Today )
//            {
//               // �������� ������� � ��������� �����.
//               TimeSpan workTime;
//               TimeSpan dinnerTime;
//               GetWorkAndDinnerTimes( begin, out workTime, out dinnerTime );

//               output -= workTime;
//               if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//               { output += ( TimeSpan.FromMinutes( 30 ) - dinnerTime ); }
//            }
//            else if( begin < DateTime.Today )
//            { output -= GetCorrectWorkedTime( begin ); }

//            begin = begin.AddDays( 1 );
//         }

//         // �� ���������� ������������� �����.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// ���������� �����, ������� ����� ���������� �� ���� ������.
//      /// ����������� �������� � ���������.
//      /// ��� �� ����������� �������, ����������, ������������.
//      /// </summary>
//      /// <returns>�����, ������� ����� ���������� �� ���� ������.</returns>
//      public TimeSpan GetRestTimeForWeek()
//      {
//         DateTime begin = DateClass.WeekBegin( DateTime.Today );
//         DateTime end = begin.AddDays( 6 );

//         return GetRestTime( begin, end );
//      }

//      /// <summary>
//      /// ���������� �����, ������� ����� ���������� � ���� ������.
//      /// </summary>
//      /// <returns>�����, ������� ����� ���������� � ���� ������.</returns>
//      public TimeSpan GetRestTimeForMonth()
//      {
//         DateTime today = DateTime.Today;
//         DateTime begin = new DateTime( today.Year, today.Month, 1, 0, 0, 0 );
//         DateTime end = begin.AddDays( DateTime.DaysInMonth( begin.Year, begin.Month ) - 1 );
//         return GetRestTime( begin, end );
//      }
//      #endregion

//      #region ������ ��� ��������� ��������� ���� (�������, ������� ����� ����������)
//      /// <summary>
//      /// ���������� �����, ������� ����� ���������� � ������ ����.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>�����, ������� ����� ���������� � ������ ����.</returns>
//      public TimeSpan GetTimeRate( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         return CalendarItem.GetWorkTime( date );
//      }
//      #endregion

//      #region ����������� ������
//      /// <summary>
//      /// ���������� ������� � ��������� ����� �� ��������� ����.
//      /// � ������ �� ������� ��������, ���������, ���������� � �.�.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <param name="workTime">������� �����.</param>
//      /// <param name="dinnerTime">��������� �����.</param>
//      public void GetWorkAndDinnerTimes( DateTime date, out TimeSpan workTime, out TimeSpan dinnerTime )
//      {
//         workTime = TimeSpan.Zero;
//         dinnerTime = TimeSpan.Zero;

//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         // � ������� ��� ������ �� ����������.
//         if( begin > DateTime.Today )
//            return;

//         Debug.Assert( m_UserEvents != null );

//         List<UptimeEvent> events = m_UserEvents.GetEvents( begin, end );
//         if( ( events == null ) || ( events.Count == 0 ) )
//            return;

//         if( begin == DateTime.Today )
//         {
//            #region ������ ��� ������������ ���
//            // �������� ������� �����.
//            DateTime now = DateTime.Now;

//            foreach( UptimeEvent curEvent in events )
//            {
//               switch( curEvent.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.MainWork:
//                  case UptimeEvent.EventType.OfficeOut:
//                     if( curEvent.EndTime == curEvent.BeginTime )
//                     { workTime += ( now - curEvent.BeginTime ); }
//                     else
//                     { workTime += ( curEvent.EndTime - curEvent.BeginTime ); }
//                     break;
//                  case UptimeEvent.EventType.LanchTime:
//                     if( curEvent.EndTime == curEvent.BeginTime )
//                     {
//                        workTime -= ( now - curEvent.BeginTime );
//                        dinnerTime += ( now - curEvent.BeginTime );
//                     }
//                     else
//                     {
//                        workTime -= ( curEvent.EndTime - curEvent.BeginTime );
//                        dinnerTime += ( curEvent.EndTime - curEvent.BeginTime );
//                     }
//                     break;
//                  case UptimeEvent.EventType.Ill:
//                  case UptimeEvent.EventType.TrustIll:
//                  case UptimeEvent.EventType.Vacation:
//                  case UptimeEvent.EventType.BusinessTrip:
//                     // ��� ������� �� �����������.
//                     break;
//                  case UptimeEvent.EventType.StudyEnglish:
//                     // Only half of study time is working time.
//                     TimeSpan studyTime;
//                     if( curEvent.EndTime == curEvent.BeginTime )
//                     { studyTime = ( now - curEvent.BeginTime ); }
//                     else
//                     { studyTime = ( curEvent.EndTime - curEvent.BeginTime ); }
//                     studyTime = TimeSpan.FromSeconds( studyTime.TotalSeconds / 2.0 );
//                     workTime -= studyTime;
//                     break;
//                  default:
//                     if( curEvent.EndTime == curEvent.BeginTime )
//                     { workTime -= ( now - curEvent.BeginTime ); }
//                     else
//                     { workTime -= ( curEvent.EndTime - curEvent.BeginTime ); }
//                     break;
//               }
//            }
//            #endregion
//         }
//         else
//         {
//            #region ������ ��� ��� � �������
//            foreach( UptimeEvent curEvent in events )
//            {
//               switch( curEvent.UptimeEventType )
//               {
//                  case UptimeEvent.EventType.MainWork:
//                  case UptimeEvent.EventType.OfficeOut:
//                     workTime += ( curEvent.EndTime - curEvent.BeginTime );
//                     break;
//                  case UptimeEvent.EventType.LanchTime:
//                     workTime -= ( curEvent.EndTime - curEvent.BeginTime );
//                     dinnerTime += ( curEvent.EndTime - curEvent.BeginTime );
//                     break;
//                  case UptimeEvent.EventType.Ill:
//                  case UptimeEvent.EventType.TrustIll:
//                  case UptimeEvent.EventType.Vacation:
//                  case UptimeEvent.EventType.BusinessTrip:
//                     // ��� ������� �� �����������.
//                     break;
//                  case UptimeEvent.EventType.StudyEnglish:
//                     // Only half of study time is working time.
//                     TimeSpan studyTime = ( curEvent.EndTime - curEvent.BeginTime );
//                     studyTime = TimeSpan.FromSeconds( studyTime.TotalSeconds / 2.0 );
//                     workTime -= studyTime;
//                     break;
//                  default:
//                     workTime -= ( curEvent.EndTime - curEvent.BeginTime );
//                     break;
//               }
//            }
//            #endregion
//         }

//         if( workTime < TimeSpan.Zero )
//            workTime = TimeSpan.Zero;
//      }
//      #endregion

//      #region ������ ��� ����������� ������ � ����������

//      /// <summary>
//      /// ���������� ������������ ������� ����� ��� ����������� �� ������� ��������.
//      /// </summary>
//      /// <returns>������������ ������� ����� ��� ����������� �� ������� ��������.</returns>
//      public TimeSpan ShowTodayWorkTime()
//      {
//         // �������� ����������� ����.
//         DateTime date = DateTime.Today;

//         // �������� ����������� ������� � ��������� �������.
//         TimeSpan workTime;
//         TimeSpan dinnerTime;
//         GetWorkAndDinnerTimes( date, out workTime, out dinnerTime );

//         // ���������� ���������.
//         if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//         { workTime += dinnerTime; }

//         return workTime;
//      }
//      #endregion

//      #endregion
//   }
//}
