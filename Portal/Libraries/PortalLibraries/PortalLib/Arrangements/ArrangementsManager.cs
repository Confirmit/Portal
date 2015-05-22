using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ConfirmIt.PortalLib.DAL.SqlClient;
using Core;
using System.Runtime.Serialization;

namespace ConfirmIt.PortalLib.Arrangements
{
	/// <summary>
	/// Класс для работы с мероприятиями.
	/// </summary>
    public class ArrangementsManager
    {/*
        #region Поля
        private static DateTime m_SelectedDate = DateTime.Today;
        #endregion

        #region Свойства
        /// <summary>
        /// Дата, выбранная пользователем на календаре.
        /// </summary>
        public static DateTime SelectedDate
        {
            get
            {
                return m_SelectedDate;
            }
            set
            {
                m_SelectedDate = value;
            }
        }
        #endregion
        
        #region Методы
        /// <summary>
        /// Возвращает список мероприятий.
		/// </summary>
        /// <returns>Список мероприятий.</returns>
		public static Arrangement[] GetArrangementsList()
		{
			BaseObjectCollection<Arrangement> coll = (BaseObjectCollection<Arrangement>) BasePlainObject.GetObjects( typeof( Arrangement ) );
			if( coll == null )
				return null;
			else
				return coll.ToArray();
		}

        /// <summary>
        /// Возвращает список комнат в данном офисе.
        /// </summary>
        /// <param name="OfficeID">ID офиса.</param>
        /// <returns>Список информаций о комнатах в данном офисе.</returns>
        public static ConferenceHall[] GetConferenceHallsList(int OfficeID)
        {
            int count;
            BaseObjectCollection<ConferenceHall> coll = new BaseObjectCollection<ConferenceHall>();
            coll.FillFromDataSet(SQLProvider.GetConferenceHallsList(OfficeID));
            return coll.ToArray();
        }

		/// <summary>
        /// Возвращает список комнат с мероприятиями за указанную дату в данном офисе.
		/// </summary>
        /// <param name="OfficeID">ID офиса.</param>
        /// <returns>Список информаций о комнатах с мероприятиями за указанную дату в данном офисе.</returns>
        public static ConferenceHall[] GetDayConferenceHallsList(int OfficeID)
		{
            int count;
            BaseObjectCollection<ConferenceHall> coll = new BaseObjectCollection<ConferenceHall>();
            coll.FillFromDataSet(SQLProvider.GetDayConferenceHallsList(OfficeID, SelectedDate));
            return coll.ToArray();
		}

        /// <summary>
        /// Возвращает список мероприятий за указанную дату в данной комнате.
        /// </summary>
        /// <param name="ConferenceHallID">ID комнаты.</param>
        /// <returns>Список информаций о мероприятиях за выбранную дату в данной комнате.</returns>
        public static Arrangement[] GetDayArragementsList(int ConferenceHallID)
        {
            int count;
            BaseObjectCollection<Arrangement> coll = new BaseObjectCollection<Arrangement>();
            coll.FillFromDataSet(SQLProvider.GetDayArrangementsList(ConferenceHallID, SelectedDate));
            return coll.ToArray();
        }
        /// <summary>
        /// Проверяет возможность добавления мероприятия с данным интервалом.
        /// </summary>
        /// <param name="ConferenceHallID">ID комнаты.</param>
        /// <param name="ArrangementID">ID мероприятия (при редактировании).</param>
        /// <param name="dBegin">Дата начала.</param>
        /// <param name="dEnd">Дата конца.</param>
        /// <returns>Возможно ли добавление.</returns>
        public static bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, DateTime dBegin, DateTime dEnd)
        {
            return SQLProvider.CheckArrangementAdding(ConferenceHallID, ArrangementID, dBegin, dEnd);
        }
		#endregion*/
	}
}

