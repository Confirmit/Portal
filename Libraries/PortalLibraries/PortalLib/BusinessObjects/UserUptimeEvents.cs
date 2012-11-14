//using System;
//using System.Collections.Generic;
//using System.Data;

//using UlterSystems.PortalLib.DB;
//using System.Diagnostics;
//using ConfirmIt.PortalLib.Properties;

//namespace UlterSystems.PortalLib.BusinessObjects
//{
//   /// <summary>
//   /// Класс событий пользователя.
//   /// </summary>
//   public class UserUptimeEvents
//   {
//      #region Поля
//      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//      private int m_UserID;
//      #endregion

//      #region Свойства
//      /// <summary>
//      /// ID пользователя.
//      /// </summary>
//      public int UserID
//      {
//         [DebuggerStepThrough]
//         get { return m_UserID; }
//         [DebuggerStepThrough]
//         set { m_UserID = value; }
//      }
//      #endregion

//      #region Конструкторы
//      /// <summary>
//      /// Конструктор для загрузки событий конкретного Пользователя.
//      /// </summary>
//      /// <param name="userId">ID пользователя.</param>
//      public UserUptimeEvents(int userId)
//      {
//         UserID = userId;
//      }
//      #endregion

//      #region Методы

//      #region Методы для работы с событиями
//      /// <summary>
//      /// Начать работу.
//      /// </summary>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void StartWork(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // Узнать, есть ли сегодня интервал основной работы.
//         UptimeEvent todayWorkEvent = GetTodayWorkEvent();

//         if (todayWorkEvent == null)
//         {
//            // Открыть новый интервал.
//            todayWorkEvent = new UptimeEvent();
//            todayWorkEvent.Name = name;
//            todayWorkEvent.BeginTime = now.AddMinutes(-5);
//            todayWorkEvent.EndTime = now.AddMinutes(-5);
//            todayWorkEvent.Duration = new TimeSpan(0);
//            todayWorkEvent.UserID = UserID;
//            todayWorkEvent.ProjectID = projectId;
//            todayWorkEvent.WorkCategoryID = workCategoryId;
//            todayWorkEvent.UptimeEventType = UptimeEvent.EventType.MainWork;
//            todayWorkEvent.Save();
//         }
//         else
//         {
//            // Проверить событие.
//            if (todayWorkEvent.BeginTime == todayWorkEvent.EndTime)
//            { throw new Exception("Can't start work before it was finished."); }

//            // Добавить событие отсутствия на работе.
//            UptimeEvent absentEvent = new UptimeEvent();
//            absentEvent.Name = string.Empty;
//            absentEvent.BeginTime = todayWorkEvent.EndTime;
//            absentEvent.EndTime = now;
//            absentEvent.Duration = now - todayWorkEvent.EndTime;
//            absentEvent.UserID = UserID;
//            absentEvent.ProjectID = 1;
//            absentEvent.WorkCategoryID = 1;
//            absentEvent.UptimeEventType = UptimeEvent.EventType.TimeOff;
//            absentEvent.Save();

//            // Изменить событие.
//            todayWorkEvent.EndTime = todayWorkEvent.BeginTime;
//            todayWorkEvent.Duration = new TimeSpan(0);

//            // Сохранить измененное событие.
//            todayWorkEvent.Save();
//         }
//      }

//      /// <summary>
//      /// Закончить работу.
//      /// </summary>
//      public void EndWork()
//      {
//         DateTime now = DateTime.Now;

//         // Получить последнее незакрытое событие.
//         UptimeEvent lastEvent = GetLastEventToday(null);
//         if (lastEvent == null)
//         { throw new Exception("There are no events today."); }

//         // Проверить событие.
//         if (lastEvent.UptimeEventType != UptimeEvent.EventType.MainWork)
//         {
//            // Обрабатывать только незакрытые события.
//            if (lastEvent.BeginTime == lastEvent.EndTime)
//            {
//               lastEvent.EndTime = now;
//               lastEvent.Duration = now - lastEvent.BeginTime;
//               lastEvent.Save();
//            }
//         }

//         // Получить событие работы за сегодня.
//         UptimeEvent todayWorkEvent = GetTodayWorkEvent();
//         if (todayWorkEvent == null)
//         { throw new Exception("There are no work event today."); }

//         // Установить дату окончания события.
//         todayWorkEvent.EndTime = now;
//         todayWorkEvent.Duration = now - todayWorkEvent.BeginTime;
//         todayWorkEvent.Save();
//      }

//      /// <summary>
//      /// Отошел с работы.
//      /// </summary>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void TimeOff(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // Создать событие отсутствия на работе.
//         UptimeEvent timeOffEvent = new UptimeEvent();
//         timeOffEvent.Name = name;
//         timeOffEvent.BeginTime = now;
//         timeOffEvent.EndTime = now;
//         timeOffEvent.Duration = new TimeSpan(0);
//         timeOffEvent.UserID = UserID;
//         timeOffEvent.ProjectID = projectId;
//         timeOffEvent.WorkCategoryID = workCategoryId;
//         timeOffEvent.UptimeEventType = UptimeEvent.EventType.TimeOff;
//         timeOffEvent.Save();
//      }

//      /// <summary>
//      /// Вернулся на работу.
//      /// </summary>
//      public void TimeOn()
//      {
//         DateTime now = DateTime.Now;

//         UptimeEvent lastEvent = GetLastEventToday((int)UptimeEvent.EventType.TimeOff);
//         if (lastEvent == null)
//         { throw new Exception("There are no time off events today."); }

//         if (lastEvent.BeginTime != lastEvent.EndTime)
//         { throw new Exception("There are no opened time off events today."); }

//         lastEvent.EndTime = now;
//         lastEvent.Duration = now - lastEvent.BeginTime;
//         lastEvent.Save();
//      }

//      /// <summary>
//      /// Ушел на обед.
//      /// </summary>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void DinnerBegin(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // Создать событие отсутствия на работе.
//         UptimeEvent dinnerEvent = new UptimeEvent();
//         dinnerEvent.Name = name;
//         dinnerEvent.BeginTime = now;
//         dinnerEvent.EndTime = now;
//         dinnerEvent.Duration = new TimeSpan(0);
//         dinnerEvent.UserID = UserID;
//         dinnerEvent.ProjectID = projectId;
//         dinnerEvent.WorkCategoryID = workCategoryId;
//         dinnerEvent.UptimeEventType = UptimeEvent.EventType.LanchTime;
//         dinnerEvent.Save();
//      }

//      /// <summary>
//      /// Вернулся с обеда.
//      /// </summary>
//      public void DinnerEnd()
//      {
//         DateTime now = DateTime.Now;

//         UptimeEvent lastEvent = GetLastEventToday((int)UptimeEvent.EventType.LanchTime);
//         if (lastEvent == null)
//         { throw new Exception("There are no dinner events today."); }

//         if (lastEvent.BeginTime != lastEvent.EndTime)
//         { throw new Exception("There are no opened dinner events today."); }

//         lastEvent.EndTime = now;
//         lastEvent.Duration = now - lastEvent.BeginTime;
//         lastEvent.Save();
//      }

//      /// <summary>
//      /// Start of studying.
//      /// </summary>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void StudyBegin(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // Создать событие отсутствия на работе.
//         UptimeEvent studyEvent = new UptimeEvent();
//         studyEvent.Name = name;
//         studyEvent.BeginTime = now;
//         studyEvent.EndTime = now;
//         studyEvent.Duration = new TimeSpan(0);
//         studyEvent.UserID = UserID;
//         studyEvent.ProjectID = projectId;
//         studyEvent.WorkCategoryID = workCategoryId;
//         studyEvent.UptimeEventType = UptimeEvent.EventType.StudyEnglish;
//         studyEvent.Save();
//      }

//      /// <summary>
//      /// Finishing of studying.
//      /// </summary>
//      public void StudyEnd()
//      {
//         DateTime now = DateTime.Now;

//         UptimeEvent lastEvent = GetLastEventToday((int)UptimeEvent.EventType.StudyEnglish);
//         if (lastEvent == null)
//         { throw new Exception("There are no study events today."); }

//         if (lastEvent.BeginTime != lastEvent.EndTime)
//         { throw new Exception("There are no opened study events today."); }

//         lastEvent.EndTime = now;
//         lastEvent.Duration = now - lastEvent.BeginTime;
//         lastEvent.Save();
//      }

//      /// <summary>
//      /// Устанавливает больничный на данную дату.
//      /// </summary>
//      /// <param name="date">Дата больничного.</param>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void SetIllDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // Получить все события за указанную дату.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // Удалить все события за данный день.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // Установить событие.
//         begin = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
//         end = new DateTime(date.Year, date.Month, date.Day, 20, 30, 0);
//         UptimeEvent illEvent = new UptimeEvent();
//         illEvent.Name = name;
//         illEvent.BeginTime = begin;
//         illEvent.EndTime = end;
//         illEvent.Duration = end - begin;
//         illEvent.UserID = UserID;
//         illEvent.ProjectID = projectId;
//         illEvent.WorkCategoryID = workCategoryId;
//         illEvent.UptimeEventType = UptimeEvent.EventType.Ill;
//         illEvent.Save();
//      }

//      /// <summary>
//      /// Устанавливает больничный по доверию на данную дату.
//      /// </summary>
//      /// <param name="date">Дата больничного.</param>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void SetTrustIllDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // Получить все события за указанную дату.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // Удалить все события за данный день.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // Установить событие.
//         begin = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
//         end = new DateTime(date.Year, date.Month, date.Day, 20, 30, 0);
//         UptimeEvent trustIllEvent = new UptimeEvent();
//         trustIllEvent.Name = name;
//         trustIllEvent.BeginTime = begin;
//         trustIllEvent.EndTime = end;
//         trustIllEvent.Duration = end - begin;
//         trustIllEvent.UserID = UserID;
//         trustIllEvent.ProjectID = projectId;
//         trustIllEvent.WorkCategoryID = workCategoryId;
//         trustIllEvent.UptimeEventType = UptimeEvent.EventType.TrustIll;
//         trustIllEvent.Save();
//      }

//      /// <summary>
//      /// Устанавливает отпуск на данную дату.
//      /// </summary>
//      /// <param name="date">Дата отпуска.</param>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void SetVacationDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // Получить все события за указанную дату.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // Удалить все события за данный день.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // Установить событие.
//         begin = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
//         end = new DateTime(date.Year, date.Month, date.Day, 20, 30, 0);
//         UptimeEvent vacationEvent = new UptimeEvent();
//         vacationEvent.Name = name;
//         vacationEvent.BeginTime = begin;
//         vacationEvent.EndTime = end;
//         vacationEvent.Duration = end - begin;
//         vacationEvent.UserID = UserID;
//         vacationEvent.ProjectID = projectId;
//         vacationEvent.WorkCategoryID = workCategoryId;
//         vacationEvent.UptimeEventType = UptimeEvent.EventType.Vacation;
//         vacationEvent.Save();
//      }

//      /// <summary>
//      /// Устанавливает командировку на данную дату.
//      /// </summary>
//      /// <param name="date">Дата командировки.</param>
//      /// <param name="name">Название для события.</param>
//      /// <param name="projectId">ID проекта.</param>
//      /// <param name="workCategoryId">ID категории работ.</param>
//      public void SetBusinessTripDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // Получить все события за указанную дату.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // Удалить все события за данный день.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // Установить событие.
//         begin = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);
//         end = new DateTime(date.Year, date.Month, date.Day, 20, 30, 0);
//         UptimeEvent businessTripEvent = new UptimeEvent();
//         businessTripEvent.Name = name;
//         businessTripEvent.BeginTime = begin;
//         businessTripEvent.EndTime = end;
//         businessTripEvent.Duration = end - begin;
//         businessTripEvent.UserID = UserID;
//         businessTripEvent.ProjectID = projectId;
//         businessTripEvent.WorkCategoryID = workCategoryId;
//         businessTripEvent.UptimeEventType = UptimeEvent.EventType.BusinessTrip;
//         businessTripEvent.Save();
//      }

//      /// <summary>
//      /// Возвращает список событий за заданный период.
//      /// </summary>
//      /// <param name="begin">Дата начала периода.</param>
//      /// <param name="end">Дата окончания периода.</param>
//      /// <returns>Список событий за заданный период.</returns>
//      public List<UptimeEvent> GetEvents(DateTime begin, DateTime end)
//      {
//         int rowsCount = 0;
//         List<UptimeEvent> output = new List<UptimeEvent>();
//         DataTable dt = DBManager.GetUserEvents(UserID, begin, end, out rowsCount);

//         if (rowsCount > 0)
//         {
//            for (int i = 0; i < rowsCount; i++)
//            {
//               int id = (int)dt.Rows[i]["ID"];
//               UptimeEvent utEvent = new UptimeEvent();
//               if (utEvent.Load(id))
//                  output.Add(utEvent);
//            }
//         }
//         return output;
//      }

//      /// <summary>
//      /// Возвращает список сегодняшних событий.
//      /// </summary>
//      /// <returns>Список сегодняшних событий.</returns>
//      public List<UptimeEvent> GetTodayEvents()
//      {
//         // Получить границы дня.
//         DateTime dayBegin = DateTime.Today;
//         DateTime dayEnd = DateTime.Today.AddDays(1).AddSeconds(-1);

//         return GetEvents(dayBegin, dayEnd);
//      }

//      /// <summary>
//      /// Возвращает сегодняшнее событие начала работы.
//      /// </summary>
//      /// <returns>Сегодняшнее событие начала работы.</returns>
//      public UptimeEvent GetTodayWorkEvent()
//      {
//         return GetWorkEvent(DateTime.Today);
//      }

//      /// <summary>
//      /// Возвращает событие начала работы за указанную дату.
//      /// </summary>
//      /// <param name="date">Дата события.</param>
//      /// <returns>Событие начала работы за указанную дату.</returns>
//      public UptimeEvent GetWorkEvent(DateTime date)
//      {
//         DataRow row = DBManager.GetWorkEvent(UserID, date);
//         if (row == null)
//         { return null; }
//         else
//         {
//            int id = (int)row["ID"];
//            UptimeEvent todayWorkEvent = new UptimeEvent();
//            if (!todayWorkEvent.Load(id))
//               return null;
//            if (todayWorkEvent.UptimeEventType != UptimeEvent.EventType.MainWork)
//               throw new Exception("Incorrect type of work event.");
//            return todayWorkEvent;
//         }
//      }

//      /// <summary>
//      /// Возвращает последнее за сегодня событие заданного типа.
//      /// </summary>
//      /// <param name="uptimeEventTypeID">Тип события. Если NULL, то любой.</param>
//      /// <returns>Последнее за сегодня событие заданного типа.</returns>
//      public UptimeEvent GetLastEventToday(int? uptimeEventTypeID)
//      { return GetLastEvent(uptimeEventTypeID, DateTime.Today); }

//      /// <summary>
//      /// Возвращает последнее событие заданного типа за указанную дату.
//      /// </summary>
//      /// <param name="uptimeEventTypeID">Тип события. Если NULL, то любой.</param>
//      /// <param name="date">Дата выборки событий.</param>
//      /// <returns>Последнее событие заданного типа за указанную дату.</returns>
//      public UptimeEvent GetLastEvent(int? uptimeEventTypeID, DateTime date)
//      {
//         // Получить все события за указанную дату.
//         DateTime dayBegin = date.Date;
//         DateTime dayEnd = dayBegin.AddDays(1).AddSeconds(-1);

//         List<UptimeEvent> dateEvents = GetEvents(dayBegin, dayEnd);
//         if ((dateEvents == null) || (dateEvents.Count == 0))
//            return null;

//         UptimeEvent lastEvent = null;

//         // Найти первое событие заданного типа.
//         if (uptimeEventTypeID != null)
//         {
//            foreach (UptimeEvent evnt in dateEvents)
//            {
//               if (evnt.UptimeEventTypeID == uptimeEventTypeID.Value)
//               {
//                  lastEvent = evnt;
//                  break;
//               }
//            }
//            if (lastEvent == null)
//               return null;
//         }
//         else
//         { lastEvent = dateEvents[0]; }

//         // Найти последнее событие заданного типа.
//         foreach (UptimeEvent evnt in dateEvents)
//         {
//            if (evnt.BeginTime > lastEvent.BeginTime)
//            {
//               if (uptimeEventTypeID != null)
//               {
//                  if (evnt.UptimeEventTypeID == uptimeEventTypeID.Value)
//                  { lastEvent = evnt; }
//               }
//               else
//               { lastEvent = evnt; }
//            }
//         }

//         return lastEvent;
//      }

//      #endregion

//      #region Методы для получения отрезков времени

//      /// <summary>
//      /// Имеет ли пользователь причину не работать в данный день.
//      /// К таким причинам относятся болезнь, командировка, отпуск.
//      /// </summary>
//      /// <param name="date">День.</param>
//      /// <returns>Имеет ли пользователь причину не работать в данный день.</returns>
//      public bool HaveAbsenceReason(DateTime date)
//      {
//         // Получить границу указанного дня.
//         DateTime begin = date.Date;
//         DateTime end = begin.AddDays( 1 ).AddSeconds( -1 );

//         // Check if value is in the cache.
//         string cacheKey = String.Format("UserID {0} haveAbsenceReason {1}", UserID, begin.ToShortDateString());
//         if (Cache.Contains(cacheKey))
//         {
//            DateTime now = DateTime.Now;
//            DateTime insertTime = Cache.InsertDate(cacheKey).Value;
//            if( insertTime < ( now - Settings.Default.AbsenceReasonExpireTime ) )
//            { Cache.Remove( cacheKey ); }
//            else
//            { return (bool) Cache.GetObject(cacheKey); }
//         }

//         // Получить все события за указанный день.
//         List<UptimeEvent> events = GetEvents(begin, end);
//         if ((events == null) || (events.Count == 0))
//         {
//            Cache.Add(cacheKey, false);
//            return false;
//         }

//         // Просмотреть все события в поисках болезни, командировки, отпуска.
//         foreach (UptimeEvent curEvent in events)
//         {
//            switch (curEvent.UptimeEventType)
//            {
//               case UptimeEvent.EventType.BusinessTrip:
//               case UptimeEvent.EventType.Vacation:
//               case UptimeEvent.EventType.Ill:
//               case UptimeEvent.EventType.TrustIll:
//                  Cache.Add(cacheKey, true);
//                  return true;
//            }
//         }

//         Cache.Add(cacheKey, false);
//         return false;
//      }

//      #endregion

//      #endregion
//   }
//}
