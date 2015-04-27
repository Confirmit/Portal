using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

using ConfirmIt.Portal.WcfServiceLibrary;
using ConfirmIt.Portal.WcfServiceLibrary.Data;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.Portal.WcfServiceLibrary.Resources;

using Core;
using UlterSystems.PortalLib.BusinessObjects;
using CalendarItem=ConfirmIt.Portal.WcfServiceLibrary.Data.CalendarItem;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
    public class MoscowService : AuthService, IPortalService
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MoscowService()
        {
            Logger.Log.Info(String.Format(Strings.ServiceStarted, GetOfficeName()));

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        /// <summary>
        /// Static MoscowService Constructor.
        /// </summary>
        static MoscowService()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region Методы
        /// <summary>
        /// Возвращает статусы пользователей московского офиса.
        /// </summary>
        /// <returns>Статусы пользователей московского офиса.</returns>
        private IEnumerable<XMLSerializableUserStatusInfo> GetMoscowUsersStatuses()
        {
            MoscowUser[] users = MoscowUser.GetLongServiceUsers();
            
            if ((users == null) || (users.Length == 0))
                return new XMLSerializableUserStatusInfo[0];

            List<XMLSerializableUserStatusInfo> coll = new List<XMLSerializableUserStatusInfo>();
            foreach (MoscowUser user in users)
            {
                MoscowUserEvents[] userEvents = MoscowUserEvents.GetUserEvents(user, DateTime.Today);
                XMLSerializableUserStatusInfo usInfo = GetUserStatus(user, userEvents);

                if (usInfo != null)
                    coll.Add(usInfo);
            }
            return coll;
        }

        /// <summary>
        /// Возвращает статус пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="userEvents">События пользователя.</param>
        /// <returns>Статус пользователя.</returns>
        private XMLSerializableUserStatusInfo GetUserStatus(MoscowUser user, MoscowUserEvents[] userEvents)
        {
            if (user == null)
                return null;

            if (userEvents == null || userEvents.Length == 0)
            {
                XMLSerializableUserStatusInfo absentInfo = new XMLSerializableUserStatusInfo();
                absentInfo.UserID = -1; // Не определен общий идентификатор пользователя.
                absentInfo.USLName = user.USLName;
                absentInfo.UserName = user.FullName;
                absentInfo.EventType = WorkEventType.TimeOff;
                absentInfo.Status = GetEventName(absentInfo.EventType);
                return absentInfo;
            }

            XMLSerializableUserStatusInfo usInfo = new XMLSerializableUserStatusInfo();
            usInfo.UserID = -1; // Не определен общий идентификатор пользователя.
            usInfo.USLName = user.USLName;
            usInfo.UserName = user.FullName;

            WorkEventType? eventType = null;
            DateTime begin = DateTime.MaxValue;
            DateTime now = DateTime.Now;
            TimeSpan workDuration = TimeSpan.Zero;
            bool userWorked = false;
            bool workEventClosed = true;

            foreach (MoscowUserEvents curEvent in userEvents)
            {
                switch (curEvent.EventType)
                {
                    case MoscowEventType.BusinessTrip:
                        eventType = WorkEventType.BusinessTrip;
                        break;

                    case MoscowEventType.Ill:
                        eventType = WorkEventType.Ill;
                        break;

                    case MoscowEventType.TrustIll:
                        eventType = WorkEventType.TrustIll;
                        break;

                    case MoscowEventType.Vacation:
                        eventType = WorkEventType.Vacation;
                        break;

                    case MoscowEventType.OffDay:
                        eventType = WorkEventType.Vacation;
                        break;

                    case MoscowEventType.TakenDay:
                        eventType = WorkEventType.Vacation;
                        break;

                    case MoscowEventType.WorkDay:
                        if (curEvent.BeginTime != null)
                        {
                            userWorked = true;
                            if (begin > curEvent.BeginTime.Value)
                                begin = curEvent.BeginTime.Value;
                            if (curEvent.EndTime != null)
                            {
                                workDuration += (curEvent.EndTime.Value - curEvent.BeginTime.Value);
                            }
                            else
                            {
                                DateTime bt = new DateTime(now.Year, now.Month, now.Day,
                                                           curEvent.BeginTime.Value.Hour,
                                                           curEvent.BeginTime.Value.Minute,
                                                           curEvent.BeginTime.Value.Second);
                                workDuration += (now - bt);
                                workEventClosed = false;
                            }
                        }
                        break;
                }
            }

            if (eventType != null)
            {
                usInfo.EventType = eventType.Value;
                usInfo.Status = GetEventName(usInfo.EventType);
            }
            else
            {
                // Пользователь не работал.
                if (!userWorked)
                {
                    usInfo.EventType = WorkEventType.TimeOff;
                    usInfo.Status = GetEventName(WorkEventType.TimeOff);
                }
                else
                {
                    // Пользователь работал
                    usInfo.BeginWork = begin;

                    if (workEventClosed)
                    {
                        // Если рабочий интервал закрыт.
                        usInfo.EventType = WorkEventType.TimeOff;
                        usInfo.Status = GetEventName(WorkEventType.TimeOff);
                    }
                    else
                    {
                        // Если рабочий интервал не закрыт.
                        usInfo.EventType = WorkEventType.MainWork;
                        usInfo.Status = GetEventName(WorkEventType.MainWork);
                    }
                    // Расчет времени окончания работы.
                    TimeSpan restTime = GetDayRate(user) - workDuration;
                    if (restTime < TimeSpan.Zero)
                        restTime = TimeSpan.Zero;

                    usInfo.EndWork = now + restTime;
                }
            }

            return usInfo;
        }

        /// <summary>
        /// Возвращает имя типа события.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <returns>Имя типа события.</returns>
        private string GetEventName(WorkEventType eventType)
        {
            return Enum.GetName(typeof(WorkEventType), eventType);
        }

        /// <summary>
        /// Возвращает дневную норму пользователя за сегодня.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns>Дневная норма пользователя за сегодня.</returns>
        private TimeSpan GetDayRate(MoscowUser user)
        {
            if (user == null)
                return TimeSpan.Zero;

            CalendarItem cItem = CalendarItem.GetCalendarItem(DateTime.Today);
            TimeSpan output;

            output = (cItem == null)
                         ? new TimeSpan(8, 30, 0)
                         : cItem.WorkTime;

            double seconds = output.TotalSeconds;
            seconds = seconds * user.PartTimeFactor;
            output = TimeSpan.FromSeconds(seconds);
            return output;
        }
        #endregion

        #region Web-методы
        /// <summary>
        /// Метод, возвращающий имя офиса.
        /// </summary>
        /// <returns>Имя офиса.</returns>
        public string GetOfficeName()
        {
			  if (!CheckAuthentication())
				  throw new HttpException(401, Strings.AuthenticationFail);

			  string officeName = string.Empty;
            try
            {
                officeName = ConfigurationManager.AppSettings["OfficeName"];
            }
            catch (Exception ex)
            {
                Logger.Log.Error(Strings.GetOfficeNameError, ex);
            }

            return officeName;
        }

        /// <summary>
        /// Метод, возвращающий статусы пользователей.
        /// </summary>
        /// <returns>Статусы пользователей.</returns>
        public IEnumerable<XMLSerializableUserStatusInfo> GetUserStatuses()
        {
			  if (!CheckAuthentication())
				  throw new HttpException(401, Strings.AuthenticationFail);

			  try
            {
                return GetMoscowUsersStatuses();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(Strings.GetUsersStatusesError, ex);
                return null;
            }
        }
        #endregion
    }
}