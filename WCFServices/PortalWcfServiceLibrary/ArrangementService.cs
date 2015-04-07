using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web;

using Core;
using Core.DB;
using ConfirmIt.Portal.WcfServiceLibrary;
using ConfirmIt.Portal.WcfServiceLibrary.Data;
using ConfirmIt.Portal.WcfServiceLibrary.Resources;
using ConfirmIt.PortalLib.Arrangements;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DAL.SqlClient;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
    /// <summary>
    /// wcf-service, предоставляющий информацию о мероприятиях
    /// </summary>
    //[System.Web.Services.WebServiceBindingAttribute(Name="ArrangementService", Namespace="http://tempuri.org/")]
    public class ArrangementService : IArrangementService
    {    
        #region Fields
        private AuthHeader m_authHeaderValue;
        #endregion

        #region Properties
        /// <summary>
        /// Value of authentication header.
        /// </summary>
        public AuthHeader AuthHeaderValue
        {
            get { return m_authHeaderValue; }
            set { m_authHeaderValue = value; }
        }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ArrangementService()
        {
            log4net.Config.XmlConfigurator.Configure();

            // Инициализировать соединение с базой данных.
            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
        }
        /// <summary>
        /// Static ArrangementService Constructor.
        /// </summary>
        static ArrangementService()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        public ArrangementService(string serviceURL)
        {
            if (string.IsNullOrEmpty(serviceURL))
                throw new ArgumentNullException("serviceURL", "URL Web-сервиса не задан.");

            //this.Url = serviceURL;
        }
        
        #endregion

        #region Методы
        /// <summary>
        /// Процедура привязки соединения к типу сервера.
        /// </summary>
        /// <param name="kind">Тип соединения.</param>
        /// <returns>Тип сервера.</returns>
        protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
        /// <summary>
        /// Checks authentication.
        /// </summary>
        /// <returns>True if user is authenticated; false, otherwise.</returns>
        protected bool CheckAuthentication()
        {
            //return true;
            try
            {
                AuthIDOnlyHeader authHeader =
                    OperationContext.Current.IncomingMessageHeaders.GetHeader<AuthIDOnlyHeader>("AuthIDOnlyHeader", "ConfirmIt.Portal.WcfServiceLibrary");

                if ((Person.GetPersonByID(authHeader.UserID.Value).IsInRole(RolesEnum.OfficeArrangementsEditor) ||
                    Person.GetPersonByID(authHeader.UserID.Value).IsInRole(RolesEnum.GeneralArrangementsEditor)))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Метод, возвращающий список комнат с мероприятиями за заданную дату.
        /// </summary>
        /// <returns>Список комнат.</returns>
        public XMLSerializableConferenceHall[] GetDayConferenceHallsList(int OfficeID, DateTime Date)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                System.Collections.Generic.IList<ConferenceHall> confList = SQLArrangementProvider.GetDayConferenceHallsList(OfficeID, Date);
                XMLSerializableConferenceHall[] xmlConfList = new XMLSerializableConferenceHall[confList.Count];
                int counter = 0;
                foreach (ConferenceHall ch in confList)
                {
                    xmlConfList[counter] = new XMLSerializableConferenceHall(ch);
                    counter++;
                }

                return xmlConfList;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при получении списка комнат.", ex);
                return null;
            }
        }
        /// <summary>
        /// Метод, возвращающий список комнат.
        /// </summary>
        /// <returns>Список комнат.</returns>
        public XMLSerializableConferenceHall[] GetConferenceHallsList(int OfficeID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);
            
            try
            {
                System.Collections.Generic.IList<ConferenceHall> confList = SQLArrangementProvider.GetConferenceHallsList(OfficeID);
                XMLSerializableConferenceHall[] xmlConfList = new XMLSerializableConferenceHall[confList.Count];
                int counter = 0;
                foreach (ConferenceHall ch in confList)
                {
                    xmlConfList[counter] = new XMLSerializableConferenceHall(ch);
                    counter++;
                }

                return xmlConfList;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при получении списка комнат.", ex);
                return null;
            }
        }

        /// <summary>
        /// Метод, возвращающий список мероприятий в данной комнате.
        /// </summary>
        /// <returns>Список мероприятий в данной комнате.</returns>
        public XMLSerializableArrangement[] GetDayArragementsList(int ConferenceHallID, DateTime Date)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                System.Collections.Generic.IList<Arrangement> arrList = SQLArrangementProvider.GetDayArrangementsList(ConferenceHallID, Date);
                XMLSerializableArrangement[] xmlArrList = new XMLSerializableArrangement[arrList.Count];
                int counter = 0;
                foreach (Arrangement arr in arrList)
                {
                    xmlArrList[counter] = new XMLSerializableArrangement(arr);
                    counter++;
                }

                return xmlArrList;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при получении списка мероприятий.", ex);
                return null;
            }
        }
        /// <summary>
        /// Метод, определяющий возможность добавления мероприятия.
        /// </summary>
        /// <param name="ConferenceHallID">ID комнаты.</param>
        /// <param name="ArrangementID">ID мероприятия (при редактировании).</param>
        /// <param name="dBegin">Дата начала.</param>
        /// <param name="dEnd">Дата конца.</param>
        /// <returns>Возможно ли добавление.</returns>
        public bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, DateTime dBegin, DateTime dEnd)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                return SQLArrangementProvider.CheckArrangementAdding(ConferenceHallID, ArrangementID, dBegin, dEnd);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при проверке возможности добавления мероприятия.", ex);
                return false;
            }
        }
        /// <summary>
        /// Метод добавления мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void AddArrangement(string Name, string Description, int ConferenceHallID, DateTime DateBegin, DateTime DateEnd, string ListOfGuests, string Equipment)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement(Name, Description, ConferenceHallID, DateBegin, DateEnd, ListOfGuests, Equipment);
                arr.Save();
                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, DateBegin.Date);
                arrDate.Save();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении мероприятия.", ex);
            }
        }
        /// <summary>
        /// Метод добавления циклического мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void AddDailyCyclicArrangement(string Name, string Description, int ConferenceHallID, DateTime TimeBegin, DateTime TimeEnd,
            int Cycle, DateTime DateEnd, int Count, string ListOfGuests, string Equipment)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement(Name, Description, ConferenceHallID, TimeBegin, TimeEnd, ListOfGuests, Equipment);
                arr.Save();
                DateTime initTime = TimeBegin.Date;
                if (Count != 0)
                {
                    DateTime date = initTime;
                    for (int i = 0; i < Count; i++)
                    {
                        ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                        arrDate.Save();
                        date = date.AddDays(Cycle);
                    }
                }
                else
                {
                    for (DateTime date = initTime; date <= DateEnd; date = date.AddDays(Cycle))
                    {
                        ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                        arrDate.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении циклического мероприятия.", ex);
            }
        }
        /// <summary>
        /// Метод добавления циклического мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void AddWeeklyCyclicArrangement(string Name, string Description, int ConferenceHallID, DateTime TimeBegin, DateTime TimeEnd,
            int WeeksCycle, bool Mo, bool Tu, bool We, bool Th, bool Fr, bool Sa, bool Su, DateTime DateEnd, int Count, string ListOfGuests, string Equipment)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement(Name, Description, ConferenceHallID, TimeBegin, TimeEnd, ListOfGuests, Equipment);
                arr.Save();
                DateTime initTime = TimeBegin.Date;
                if (Count != 0)
                {
                    DateTime date = initTime;
                    for (int i = 0; i < Count;)
                    {
                        for (int day = 1; day <= 7 && i < Count; day++)
                        {
                            if (Mo && date.DayOfWeek == DayOfWeek.Monday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (Tu && date.DayOfWeek == DayOfWeek.Tuesday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (We && date.DayOfWeek == DayOfWeek.Wednesday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (Th && date.DayOfWeek == DayOfWeek.Thursday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (Fr && date.DayOfWeek == DayOfWeek.Friday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (Sa && date.DayOfWeek == DayOfWeek.Saturday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            if (Su && date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                                i++;
                            }
                            date = date.AddDays(1);
                        }
                        date = date.AddDays((WeeksCycle-1) * 7);
                    }
                }
                else
                {
                    for (DateTime date = initTime; date <= DateEnd; date = date.AddDays((WeeksCycle-1) * 7))
                    {
                        int day = 0;
                        switch (date.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                day = 1;
                                break;
                            case DayOfWeek.Tuesday:
                                day = 2;
                                break;
                            case DayOfWeek.Wednesday:
                                day = 3;
                                break;
                            case DayOfWeek.Thursday:
                                day = 4;
                                break;
                            case DayOfWeek.Friday:
                                day = 5;
                                break;
                            case DayOfWeek.Saturday:
                                day = 6;
                                break;
                            case DayOfWeek.Sunday:
                                day = 7;
                                break;
                        }

                        for (; day <= 7 && date <= DateEnd; day++)
                        {
                            if (Mo && date.DayOfWeek == DayOfWeek.Monday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (Tu && date.DayOfWeek == DayOfWeek.Tuesday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (We && date.DayOfWeek == DayOfWeek.Wednesday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (Th && date.DayOfWeek == DayOfWeek.Thursday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (Fr && date.DayOfWeek == DayOfWeek.Friday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (Sa && date.DayOfWeek == DayOfWeek.Saturday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            if (Su && date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ArrangementDate arrDate = new ArrangementDate((int)arr.ID, date);
                                arrDate.Save();
                            }
                            date = date.AddDays(1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении циклического мероприятия.", ex);
            }
        }
        /// <summary>
        /// Метод редактирования мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void EditArrangement(int ArrangementID, string Name, string Description, int ConferenceHallID, DateTime DateBegin, DateTime DateEnd, string ListOfGuests, string Equipment)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement();
                arr.Load(ArrangementID);

                SQLArrangementProvider.DeleteArrangementDate(
                    new ArrangementDate(ArrangementID, arr.TimeBegin.Date));

                arr.Name = Name;
                arr.Description = Description;
                arr.ConferenceHallID = ConferenceHallID;
                arr.TimeBegin = DateBegin;
                arr.TimeEnd = DateEnd;
                arr.ListOfGuests = ListOfGuests;
                arr.Equipment = Equipment;
                arr.Save();

                ArrangementDate arrDate = new ArrangementDate(ArrangementID, DateBegin.Date);
                arrDate.Save();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении мероприятия.", ex);
            }
        }
        /// <summary>
        /// Метод проверки, является ли это мероприятие циклическим
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public bool CheckCyclicArrangement(int ArrID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                return SQLArrangementProvider.isCyclicArrangement(ArrID);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при проверке мероприятия на цикличность.", ex);
                return false;
            }
        }

        /// <summary>
        /// Метод удаления мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void DeleteArrangement(int ArrangementID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement();
                arr.Load(ArrangementID);
                arr.Delete();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при удалении мероприятия.", ex);
            }
        }

        /// <summary>
        /// Метод удаления мероприятия.
        /// </summary>
        /// <param name="arr">Мероприятие.</param>
        public void DeleteOneOfCyclicArrangements(int ArrangementID, DateTime Date)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                SQLArrangementProvider.DeleteArrangementDate(
                   new ArrangementDate(ArrangementID, Date));
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при удалении мероприятия.", ex);
            }
        }

        /// <summary>
        /// Метод, получающий мероприятие по его ID.
        /// </summary>
        /// <returns>Мероприятие.</returns>
        public XMLSerializableArrangement GetArrangement(int ArrangementID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                Arrangement arr = new Arrangement();
                arr.Load(ArrangementID);
                return new XMLSerializableArrangement(arr);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при получении мероприятия.", ex);
                return null;
            }
        }
        /// <summary>
        /// Метод, получающий конференц зал по его ID.
        /// </summary>
        /// <returns>Конференц зал.</returns>
        public XMLSerializableConferenceHall GetConferenceHall(int ConferenceHallID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                ConferenceHall confHall = new ConferenceHall();
                confHall.Load(ConferenceHallID);
                return new XMLSerializableConferenceHall(confHall);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при получении конференц зала.", ex);
                return null;
            }
        }
        /// <summary>
        /// Метод добавления конференц зала.
        /// </summary>
        /// <param name="ch">Конференц зал.</param>
        public void AddConferenceHall(string Name, string Description, int OfficeID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                ConferenceHall tempHall = new ConferenceHall(Name, Description, OfficeID);
                ConferenceHall confHall = SQLArrangementProvider.TryToFindThisConferenceHall(tempHall);
                if (confHall == null)
                    confHall = tempHall;
                else
                    confHall.Description = Description;
                confHall.Save();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении конференц зала.", ex);
            }
        }
        /// <summary>
        /// Метод редактирования конференц зала.
        /// </summary>
        /// <param name="ch">Конференц зал.</param>
        public void EditConferenceHall(int ConferenceHallID, string Name, string Description, int OfficeID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                ConferenceHall confHall = new ConferenceHall();
                confHall.Load(ConferenceHallID);
                confHall.Name = Name;
                confHall.Description = Description;
                confHall.OfficeID = OfficeID;
                confHall.Save();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при добавлении конференц зала.", ex);
            }
        }
        /// <summary>
        /// Метод удаления конференц зала.
        /// </summary>
        /// <param name="ch">Конференц зал.</param>
        public void DeleteConferenceHall(int ConferenceHallID)
        {
            if (!CheckAuthentication())
                throw new HttpException(401, Strings.AuthenticationFail);

            try
            {
                ConferenceHall confHall = new ConferenceHall();
                confHall.Load(ConferenceHallID);
                confHall.Delete();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при удалении конференц зала.", ex);
            }
        }
        #endregion
    }
}
