//using System;
//using System.Collections.Generic;
//using System.Data;

//using UlterSystems.PortalLib.DB;
//using System.Diagnostics;
//using ConfirmIt.PortalLib.Properties;

//namespace UlterSystems.PortalLib.BusinessObjects
//{
//   /// <summary>
//   /// ����� ������� ������������.
//   /// </summary>
//   public class UserUptimeEvents
//   {
//      #region ����
//      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//      private int m_UserID;
//      #endregion

//      #region ��������
//      /// <summary>
//      /// ID ������������.
//      /// </summary>
//      public int UserID
//      {
//         [DebuggerStepThrough]
//         get { return m_UserID; }
//         [DebuggerStepThrough]
//         set { m_UserID = value; }
//      }
//      #endregion

//      #region ������������
//      /// <summary>
//      /// ����������� ��� �������� ������� ����������� ������������.
//      /// </summary>
//      /// <param name="userId">ID ������������.</param>
//      public UserUptimeEvents(int userId)
//      {
//         UserID = userId;
//      }
//      #endregion

//      #region ������

//      #region ������ ��� ������ � ���������
//      /// <summary>
//      /// ������ ������.
//      /// </summary>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void StartWork(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // ������, ���� �� ������� �������� �������� ������.
//         UptimeEvent todayWorkEvent = GetTodayWorkEvent();

//         if (todayWorkEvent == null)
//         {
//            // ������� ����� ��������.
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
//            // ��������� �������.
//            if (todayWorkEvent.BeginTime == todayWorkEvent.EndTime)
//            { throw new Exception("Can't start work before it was finished."); }

//            // �������� ������� ���������� �� ������.
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

//            // �������� �������.
//            todayWorkEvent.EndTime = todayWorkEvent.BeginTime;
//            todayWorkEvent.Duration = new TimeSpan(0);

//            // ��������� ���������� �������.
//            todayWorkEvent.Save();
//         }
//      }

//      /// <summary>
//      /// ��������� ������.
//      /// </summary>
//      public void EndWork()
//      {
//         DateTime now = DateTime.Now;

//         // �������� ��������� ���������� �������.
//         UptimeEvent lastEvent = GetLastEventToday(null);
//         if (lastEvent == null)
//         { throw new Exception("There are no events today."); }

//         // ��������� �������.
//         if (lastEvent.UptimeEventType != UptimeEvent.EventType.MainWork)
//         {
//            // ������������ ������ ���������� �������.
//            if (lastEvent.BeginTime == lastEvent.EndTime)
//            {
//               lastEvent.EndTime = now;
//               lastEvent.Duration = now - lastEvent.BeginTime;
//               lastEvent.Save();
//            }
//         }

//         // �������� ������� ������ �� �������.
//         UptimeEvent todayWorkEvent = GetTodayWorkEvent();
//         if (todayWorkEvent == null)
//         { throw new Exception("There are no work event today."); }

//         // ���������� ���� ��������� �������.
//         todayWorkEvent.EndTime = now;
//         todayWorkEvent.Duration = now - todayWorkEvent.BeginTime;
//         todayWorkEvent.Save();
//      }

//      /// <summary>
//      /// ������ � ������.
//      /// </summary>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void TimeOff(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // ������� ������� ���������� �� ������.
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
//      /// �������� �� ������.
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
//      /// ���� �� ����.
//      /// </summary>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void DinnerBegin(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // ������� ������� ���������� �� ������.
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
//      /// �������� � �����.
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
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void StudyBegin(
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime now = DateTime.Now;

//         // ������� ������� ���������� �� ������.
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
//      /// ������������� ���������� �� ������ ����.
//      /// </summary>
//      /// <param name="date">���� �����������.</param>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void SetIllDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // �������� ��� ������� �� ��������� ����.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // ������� ��� ������� �� ������ ����.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // ���������� �������.
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
//      /// ������������� ���������� �� ������� �� ������ ����.
//      /// </summary>
//      /// <param name="date">���� �����������.</param>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void SetTrustIllDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // �������� ��� ������� �� ��������� ����.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // ������� ��� ������� �� ������ ����.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // ���������� �������.
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
//      /// ������������� ������ �� ������ ����.
//      /// </summary>
//      /// <param name="date">���� �������.</param>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void SetVacationDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // �������� ��� ������� �� ��������� ����.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // ������� ��� ������� �� ������ ����.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // ���������� �������.
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
//      /// ������������� ������������ �� ������ ����.
//      /// </summary>
//      /// <param name="date">���� ������������.</param>
//      /// <param name="name">�������� ��� �������.</param>
//      /// <param name="projectId">ID �������.</param>
//      /// <param name="workCategoryId">ID ��������� �����.</param>
//      public void SetBusinessTripDay(
//         DateTime date,
//         string name,
//         int projectId,
//         int workCategoryId
//         )
//      {
//         DateTime begin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
//         DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

//         // �������� ��� ������� �� ��������� ����.
//         List<UptimeEvent> dateEvents = GetEvents(begin, end);
//         if ((dateEvents != null) && (dateEvents.Count > 0))
//         {
//            // ������� ��� ������� �� ������ ����.
//            for (int i = 0; i < dateEvents.Count; i++)
//            {
//               UptimeEvent evnt = dateEvents[i];
//               evnt.Delete();
//            }
//         }

//         // ���������� �������.
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
//      /// ���������� ������ ������� �� �������� ������.
//      /// </summary>
//      /// <param name="begin">���� ������ �������.</param>
//      /// <param name="end">���� ��������� �������.</param>
//      /// <returns>������ ������� �� �������� ������.</returns>
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
//      /// ���������� ������ ����������� �������.
//      /// </summary>
//      /// <returns>������ ����������� �������.</returns>
//      public List<UptimeEvent> GetTodayEvents()
//      {
//         // �������� ������� ���.
//         DateTime dayBegin = DateTime.Today;
//         DateTime dayEnd = DateTime.Today.AddDays(1).AddSeconds(-1);

//         return GetEvents(dayBegin, dayEnd);
//      }

//      /// <summary>
//      /// ���������� ����������� ������� ������ ������.
//      /// </summary>
//      /// <returns>����������� ������� ������ ������.</returns>
//      public UptimeEvent GetTodayWorkEvent()
//      {
//         return GetWorkEvent(DateTime.Today);
//      }

//      /// <summary>
//      /// ���������� ������� ������ ������ �� ��������� ����.
//      /// </summary>
//      /// <param name="date">���� �������.</param>
//      /// <returns>������� ������ ������ �� ��������� ����.</returns>
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
//      /// ���������� ��������� �� ������� ������� ��������� ����.
//      /// </summary>
//      /// <param name="uptimeEventTypeID">��� �������. ���� NULL, �� �����.</param>
//      /// <returns>��������� �� ������� ������� ��������� ����.</returns>
//      public UptimeEvent GetLastEventToday(int? uptimeEventTypeID)
//      { return GetLastEvent(uptimeEventTypeID, DateTime.Today); }

//      /// <summary>
//      /// ���������� ��������� ������� ��������� ���� �� ��������� ����.
//      /// </summary>
//      /// <param name="uptimeEventTypeID">��� �������. ���� NULL, �� �����.</param>
//      /// <param name="date">���� ������� �������.</param>
//      /// <returns>��������� ������� ��������� ���� �� ��������� ����.</returns>
//      public UptimeEvent GetLastEvent(int? uptimeEventTypeID, DateTime date)
//      {
//         // �������� ��� ������� �� ��������� ����.
//         DateTime dayBegin = date.Date;
//         DateTime dayEnd = dayBegin.AddDays(1).AddSeconds(-1);

//         List<UptimeEvent> dateEvents = GetEvents(dayBegin, dayEnd);
//         if ((dateEvents == null) || (dateEvents.Count == 0))
//            return null;

//         UptimeEvent lastEvent = null;

//         // ����� ������ ������� ��������� ����.
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

//         // ����� ��������� ������� ��������� ����.
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

//      #region ������ ��� ��������� �������� �������

//      /// <summary>
//      /// ����� �� ������������ ������� �� �������� � ������ ����.
//      /// � ����� �������� ��������� �������, ������������, ������.
//      /// </summary>
//      /// <param name="date">����.</param>
//      /// <returns>����� �� ������������ ������� �� �������� � ������ ����.</returns>
//      public bool HaveAbsenceReason(DateTime date)
//      {
//         // �������� ������� ���������� ���.
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

//         // �������� ��� ������� �� ��������� ����.
//         List<UptimeEvent> events = GetEvents(begin, end);
//         if ((events == null) || (events.Count == 0))
//         {
//            Cache.Add(cacheKey, false);
//            return false;
//         }

//         // ����������� ��� ������� � ������� �������, ������������, �������.
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
