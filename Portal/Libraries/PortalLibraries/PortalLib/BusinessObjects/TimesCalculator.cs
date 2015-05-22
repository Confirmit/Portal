//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using ConfirmIt.PortalLib.BAL;

//namespace UlterSystems.PortalLib.BusinessObjects
//{
//   /// <summary>
//   /// Класс для вычисления различных временных характеристик.
//   /// </summary>
//   public class TimesCalculator
//   {
//      private const int m_DinnerMinutesLimit = 10;

//      #region Поля
//      private readonly int m_UserID;
//      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//      private readonly UserUptimeEvents m_UserEvents;
//      #endregion

//      #region Конструкторы
//      /// <summary>
//      /// Конструктор.
//      /// </summary>
//      /// <param name="userID">ID пользователя.</param>
//      public TimesCalculator( int userID )
//      {
//         m_UserID = userID;
//         m_UserEvents = new UserUptimeEvents( userID );
//      }

//      /// <summary>
//      /// Конструктор.
//      /// </summary>
//      /// <param name="user">Пользователь.</param>
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

//      #region Методы

//      #region Методы получения обеденного времени
//      /// <summary>
//      /// Возвращает обеденное время за данную дату.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <returns>Обеденное время за данную дату.</returns>
//      public TimeSpan GetDinnerTime( DateTime date )
//      {
//         TimeSpan output = TimeSpan.Zero;

//         // Получить границы заданного дня.
//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         if( begin == DateTime.Today )
//         {
//            #region Рассчет для сегодняшнего дня
//            Debug.Assert( m_UserEvents != null );

//            DateTime now = DateTime.Now;

//            // Получить все события.
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
//            #region Рассчет для даты в будущем
//            // Дата в будущем.
//            return TimeSpan.Zero;
//            #endregion
//         }
//         else
//         {
//            #region Рассчет даты в прошлом
//            Debug.Assert( m_UserEvents != null );

//            // Получить список событий за дату.
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
//      /// Возвращает обеденное время за сегодня.
//      /// </summary>
//      /// <returns>Обеденное время за сегодня.</returns>
//      public TimeSpan GetTodayDinnerTime()
//      { return GetDinnerTime( DateTime.Today ); }
//      #endregion

//      #region Методы для получения рабочего времени

//      /// <summary>
//      /// Возвращает полное время, проведенное в офисе, от прихода до ухода.
//      /// В полное время входят времена, потраченные на обед и нерабочее время.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <returns>Полное время, проведенное в офисе, от прихода до ухода.</returns>
//      public TimeSpan GetTotalTime( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // Создать дату начала дня.
//         date = date.Date;

//         // There is no work in future.
//         if( date > DateTime.Today )
//            return TimeSpan.Zero;

//         // Получить рабочее событие за указанную дату.
//         UptimeEvent workEvent = m_UserEvents.GetWorkEvent( date );
//         if( workEvent == null )
//            return TimeSpan.Zero;

//         if( date == DateTime.Today )
//         {
//            // Получить текущее время.
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
//      /// Возвращает полное время, проведенное в офисе, от прихода до ухода сегодня.
//      /// В полное время входят времена, потраченные на обед и нерабочее время.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// </summary>
//      /// <returns>Полное время, проведенное в офисе, от прихода до ухода сегодня.</returns>
//      public TimeSpan GetTodayTotalTime()
//      { return GetTotalTime( DateTime.Today ); }

//      /// <summary>
//      /// Возвращает полное время, проведенное в офисе, за указанный промежуток дат.
//      /// </summary>
//      /// <param name="begin">Дата начала интервала.</param>
//      /// <param name="end">Дата окончания интервала.</param>
//      /// <returns>Полное время, проведенное в офисе, за указанный промежуток дат.</returns>
//      public TimeSpan GetTotalTime( DateTime begin, DateTime end )
//      {
//         // Исправить границы дат.
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
//      /// Возвращает отработанное время за заданное число.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <returns>Отработанное время за заданное число.</returns>
//      public TimeSpan GetWorkedTime( DateTime date )
//      {
//         TimeSpan output = TimeSpan.Zero;

//         // Вычислить времена начала и окончания указанного дня.
//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         if( begin == DateTime.Today )
//         {
//            #region Рассчет для сегодняшнего дня
//            Debug.Assert( m_UserEvents != null );

//            DateTime now = DateTime.Now;

//            // Получить список всех событий.
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
//                     // Не учитывать события отпуска, командировки, болезни.
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
//            #region Рассчет для даты в будущем
//            // Дата в будущем.
//            return TimeSpan.Zero;
//            #endregion
//         }
//         else
//         {
//            #region Рассчет даты в прошлом
//            Debug.Assert( m_UserEvents != null );

//            // Получить список событий за дату.
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
//                     // Не учитывать события отпуска, командировки, болезни.
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

//         // Не возвращаеть отрицательных времен.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// Возвращает отработанное сегодня время.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// </summary>
//      /// <returns>Отработанное сегодня время.</returns>
//      public TimeSpan GetTodayWorkedTime()
//      { return GetWorkedTime( DateTime.Now ); }

//      /// <summary>
//      /// Возвращает отработанное время за заданный диапазон дат.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// </summary>
//      /// <param name="begin">Дата начала интервала.</param>
//      /// <param name="end">Дата окончания интервала.</param>
//      /// <returns>Возвращает отработанное время за заданный диапазон дат.</returns>
//      public TimeSpan GetWorkedTime( DateTime begin, DateTime end )
//      {
//         // Исправить границы дат.
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
//      /// Возвращает отработанное время за заданное число с учетом обеда.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// Учитывается обеденное время.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <returns>Возвращает отработанное время за заданное число с учетом обеда.</returns>
//      public TimeSpan GetCorrectWorkedTime( DateTime date )
//      {
//         if( date.Date > DateTime.Today )
//            return TimeSpan.Zero;

//         // Получить обеденное и рабочее время.
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

//         // Не возвращать отрицательные времена.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// Возвращает отработанное время за сегодня с учетом обеда.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// Учитывается обеденное время.
//      /// </summary>
//      /// <returns>Возвращает отработанное время за сегодня с учетом обеда.</returns>
//      public TimeSpan GetTodayCorrectWorkedTime()
//      { return GetCorrectWorkedTime( DateTime.Today ); }

//      /// <summary>
//      /// Возвращает отработанное время за заданный диапазон дат с учетом обеда.
//      /// При этом не учитываются, является ли данное число выходным или праздником.
//      /// Так же не учитывается, находится ли пользователь в отпуске, в командировке, болеет.
//      /// Учитывается обеденное время.
//      /// </summary>
//      /// <param name="begin">Дата начала интервала.</param>
//      /// <param name="end">Дата окончания интервала.</param>
//      /// <returns>Отработанное время за заданный диапазон дат с учетом обеда.</returns>
//      public TimeSpan GetCorrectWorkedTime( DateTime begin, DateTime end )
//      {
//         // Исправить границы дат.
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

//      #region Методы для получения времени, которое осталось отработать
//      /// <summary>
//      /// Возвращает количество времени, которое нужно отработать в данное число.
//      /// В случае, если данное число - выходной или праздник, возвращается 0.
//      /// В случае, если на данное число пользователь в командировке, в отпуске, болеет,
//      /// возвращается 0.
//      /// </summary>
//      /// <param name="date">Число.</param>
//      /// <returns>Количество времени, которое нужно отработать в данное число.</returns>
//      public TimeSpan GetRestTime( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // Учесть праздники.
//         if( CalendarItem.GetHoliday( date ) )
//            return TimeSpan.Zero;

//         // Учесть больничные, отпуска, командировки.
//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         TimeSpan output;

//         // Обработка даты в будущем.
//         if( date.Date > DateTime.Today )
//         { output = new TimeSpan( 8, 30, 0 ); }
//         else if( date.Date == DateTime.Today )
//         { output = GetTodayRestTime(); }
//         else
//         { output = TimeSpan.FromHours( 8 ) - GetCorrectWorkedTime( date ); }

//         // Не возвращать отрицательные имена.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// Возвращает время, которое нужно отработать сегодня.
//      /// В случае, если данное число - выходной или праздник, возвращается 0.
//      /// В случае, если на данное число пользователь в командировке, в отпуске, болеет,
//      /// возвращается 0.
//      /// </summary>
//      /// <returns>Время, которое нужно отработать сегодня.</returns>
//      public TimeSpan GetTodayRestTime()
//      {
//         Debug.Assert( m_UserEvents != null );

//         // Получить сегодняшнюю дату.
//         DateTime date = DateTime.Today;

//         // Учесть праздники.
//         if( CalendarItem.GetHoliday( date ) )
//            return TimeSpan.Zero;

//         // Учесть больничные, отпуска, командировки.
//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         // Получить рабочее и обеденное время.
//         TimeSpan workTime;
//         TimeSpan dinnerTime;
//         GetWorkAndDinnerTimes( date, out workTime, out dinnerTime );



//         TimeSpan output = CalendarItem.GetWorkTime( date ) - workTime;
//         if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//         { output += ( TimeSpan.FromMinutes( 30 ) - dinnerTime ); }

//         // Не возвращать отрицательные имена.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// Возвращает количество времени, которое нужно отработать за данный диапазон дат.
//      /// В случае, если число - выходной или праздник, нужное время - 0.
//      /// В случае, если на число пользователь в командировке, в отпуске, болеет,
//      /// нужное время - 0.
//      /// </summary>
//      /// <param name="begin">Дата начала интервала.</param>
//      /// <param name="end">Дата окончания интервала.</param>
//      /// <returns>Количество времени, которое нужно отработать за данный диапазон дат.</returns>
//      public TimeSpan GetRestTime( DateTime begin, DateTime end )
//      {
//         Debug.Assert( m_UserEvents != null );

//         // Исправить границы дат.
//         begin = begin.Date;
//         end = end.Date.AddSeconds( 1 );

//         TimeSpan output = TimeSpan.Zero;
//         while( begin < end )
//         {
//            // Учесть выходные, праздники, больничные и т.д. 
//            if( !( CalendarItem.GetHoliday( begin ) || ( m_UserEvents.HaveAbsenceReason( begin ) ) ) )
//            {
//               output += CalendarItem.GetWorkTime( begin );
//            }

//            if( begin == DateTime.Today )
//            {
//               // Получить рабочее и обеденное время.
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

//         // Не возвращать отрицательные имена.
//         if( output < TimeSpan.Zero )
//            output = TimeSpan.Zero;

//         return output;
//      }

//      /// <summary>
//      /// Возвращает время, которое нужно отработать на этой неделе.
//      /// Учитываются выходные и праздники.
//      /// Так же учитываются отпуска, больничные, командировки.
//      /// </summary>
//      /// <returns>Время, которое нужно отработать на этой неделе.</returns>
//      public TimeSpan GetRestTimeForWeek()
//      {
//         DateTime begin = DateClass.WeekBegin( DateTime.Today );
//         DateTime end = begin.AddDays( 6 );

//         return GetRestTime( begin, end );
//      }

//      /// <summary>
//      /// Возвращает время, которое нужно отработать в этом месяце.
//      /// </summary>
//      /// <returns>Время, которое нужно отработать в этом месяце.</returns>
//      public TimeSpan GetRestTimeForMonth()
//      {
//         DateTime today = DateTime.Today;
//         DateTime begin = new DateTime( today.Year, today.Month, 1, 0, 0, 0 );
//         DateTime end = begin.AddDays( DateTime.DaysInMonth( begin.Year, begin.Month ) - 1 );
//         return GetRestTime( begin, end );
//      }
//      #endregion

//      #region Методы для получения временных норм (времени, которое нужно отработать)
//      /// <summary>
//      /// Возвращает время, которое нужно отработать в данную дату.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <returns>Время, которое нужно отработать в данную дату.</returns>
//      public TimeSpan GetTimeRate( DateTime date )
//      {
//         Debug.Assert( m_UserEvents != null );

//         if( m_UserEvents.HaveAbsenceReason( date ) )
//            return TimeSpan.Zero;

//         return CalendarItem.GetWorkTime( date );
//      }
//      #endregion

//      #region Специальные методы
//      /// <summary>
//      /// Возвращает рабочее и обеденное время за указанную дату.
//      /// В расчет не берутся выходные, праздники, больничные и т.п.
//      /// </summary>
//      /// <param name="date">Дата.</param>
//      /// <param name="workTime">Рабочее время.</param>
//      /// <param name="dinnerTime">Обеденное время.</param>
//      public void GetWorkAndDinnerTimes( DateTime date, out TimeSpan workTime, out TimeSpan dinnerTime )
//      {
//         workTime = TimeSpan.Zero;
//         dinnerTime = TimeSpan.Zero;

//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         // В будущем еше ничего не отработано.
//         if( begin > DateTime.Today )
//            return;

//         Debug.Assert( m_UserEvents != null );

//         List<UptimeEvent> events = m_UserEvents.GetEvents( begin, end );
//         if( ( events == null ) || ( events.Count == 0 ) )
//            return;

//         if( begin == DateTime.Today )
//         {
//            #region Расчет для сегодняшнего дня
//            // Получить текущее время.
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
//                     // Эти события не учитываются.
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
//            #region Расчет для дня в прошлом
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
//                     // Эти события не учитываются.
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

//      #region Методы для отображения данных в интерфейсе

//      /// <summary>
//      /// Возвращает отработанное сегодня время для отображения на главной странице.
//      /// </summary>
//      /// <returns>Отработанное сегодня время для отображения на главной странице.</returns>
//      public TimeSpan ShowTodayWorkTime()
//      {
//         // Получить сегодняшнюю дату.
//         DateTime date = DateTime.Today;

//         // Получить сегодняшние рабочее и обеденное времена.
//         TimeSpan workTime;
//         TimeSpan dinnerTime;
//         GetWorkAndDinnerTimes( date, out workTime, out dinnerTime );

//         // Произвести коррекцию.
//         if( dinnerTime < TimeSpan.FromMinutes( m_DinnerMinutesLimit ) )
//         { workTime += dinnerTime; }

//         return workTime;
//      }
//      #endregion

//      #endregion
//   }
//}
