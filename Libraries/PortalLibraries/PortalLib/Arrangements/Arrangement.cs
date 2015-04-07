using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

using Core;
using Core.ORM.Attributes;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Arrangements
{
    /// <summary>
    /// Класс мероприятия.
    /// </summary>
    [DBTable("Arrangement")]
    public class Arrangement : BasePlainObject 
    {
        #region Поля 
        private string m_Name = string.Empty;
        private string m_Description = string.Empty;
        private int m_ConferenceHallID = 0;
        private string m_ConferenceHallName = string.Empty;
        private int m_OfficeID = 0;
        private string m_OfficeName = string.Empty;
        private DateTime m_TimeBegin;
        private DateTime m_TimeEnd;
        private string m_ListOfGuests = string.Empty;
        private string m_Equipment = string.Empty;
       
        #endregion

        #region Свойства
        /// <summary>
        /// Заголовок мероприятия.
        /// </summary>
        [DBRead("Name")]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        /// <summary>
        /// Описание мероприятия.
        /// </summary>
        [DBRead("Description")]
        [DBNullable]
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }
        /// <summary>
        /// Идентификатор конференц-зала.
        /// </summary>
        [DBRead("ConferenceHallID")]
        public int ConferenceHallID
        {
            get
            {
                return m_ConferenceHallID;
            }
            set
            {
                m_ConferenceHallID = value;
            }
        }
        /// <summary>
        /// Название конференц-зала.
        /// </summary>
        public string ConferenceHallName
        {
            get
            {
                if (m_ConferenceHallName == string.Empty)
                {
                    ConferenceHall hall = new ConferenceHall();
                    hall.Load(m_ConferenceHallID);
                    m_ConferenceHallName = hall.Name;
                }
                return m_ConferenceHallName;
            }
        }
        /// <summary>
        /// ID офиса.
        /// </summary>
        public int OfficeID
        {
            get
            {
                if (m_OfficeID == 0)
                {
                    ConferenceHall hall = new ConferenceHall();
                    hall.Load(m_ConferenceHallID);
                    m_OfficeID = hall.OfficeID;
                }
                return m_OfficeID;
            }
        }
        /// <summary>
        /// Название офиса.
        /// </summary>
        public string OfficeName
        {
            get
            {
                if (m_OfficeName == string.Empty)
                {
                    ConferenceHall hall = new ConferenceHall();
                    hall.Load(m_ConferenceHallID);
                    m_OfficeName = hall.OfficeName;
                }
                return m_OfficeName;
            }
        }
        /// <summary>
        /// Дата начала мероприятия.
        /// </summary>
        [DBRead("TimeBegin")]
        public DateTime TimeBegin
        {
            get
            {
                return m_TimeBegin;
            }
            set
            {
                m_TimeBegin = value;
            }
        }
        /// <summary>
        /// Дата окончания мероприятия.
        /// </summary>
        [DBRead("TimeEnd")]
        public DateTime TimeEnd
        {
            get
            {
                return m_TimeEnd;
            }
            set
            {
                m_TimeEnd = value;
            }
        }
        /// <summary>
        /// Список приглашенных на мероприятие лиц.
        /// </summary>
        [DBRead("ListOfGuests")]
        [DBNullable]
        public string ListOfGuests
        {
            get
            {
                return m_ListOfGuests;
            }
            set
            {
                m_ListOfGuests = value;
            }
        }
        /// <summary>
        /// Оборудование, необходимое для проведения мероприятия.
        /// </summary>
        [DBRead("Equipment")]
        [DBNullable]
        public string Equipment
        {
            get
            {
                return m_Equipment;
            }
            set
            {
                m_Equipment = value;
            }
        }
        #endregion

        #region Конструкторы

        public Arrangement()
        {
        }

        public Arrangement(
            string Name,
            string Description,
            int ConferenceHallID,
            DateTime TimeBegin,
            DateTime TimeEnd,
            string ListOfGuests,
            string Equipment)
        {
            this.Name = Name;
            this.Description = Description;
            this.ConferenceHallID = ConferenceHallID;
            this.TimeBegin = TimeBegin;
            this.TimeEnd = TimeEnd;
            this.ListOfGuests = ListOfGuests;
            this.Equipment = Equipment;
        }

        public Arrangement(XMLSerializableArrangement xArr)
        {
            this.Name = xArr.Name;
            this.Description = xArr.Description;
            this.ConferenceHallID = xArr.ConferenceHallID;
            this.TimeBegin = xArr.TimeBegin;
            this.TimeEnd = xArr.TimeEnd;
            this.ListOfGuests = xArr.ListOfGuests;
            this.Equipment = xArr.Equipment;
        }
        #endregion

    }

    /// <summary>
	/// Класс информации о мероприятиях, пригодный для XML-сериализации.
	/// </summary>
    [Serializable]
    public class XMLSerializableArrangement
    {
        #region Поля 
        private int m_ArrangementID = 0;
        private string m_Name = string.Empty;
        private string m_Description = string.Empty;
        private int m_ConferenceHallID = 0;
        private string m_ConferenceHallName = string.Empty;
        private string m_OfficeName = string.Empty;
        private int m_OfficeID = 0;
        private DateTime m_TimeBegin = DateTime.Today;
        private DateTime m_TimeEnd = DateTime.Today;
        private string m_ListOfGuests = string.Empty;
        private string m_Equipment = string.Empty;
       
        #endregion

        #region Свойства
        /// <summary>
        /// ID мероприятия.
        /// </summary>
        public int ArrangementID
        {
            get
            {   return m_ArrangementID; }
            set
            {   m_ArrangementID = value; }
        }
        /// <summary>
        /// Заголовок мероприятия.
        /// </summary>
        public string Name
        {
            get
            { return m_Name; }
            set
            { m_Name = value; }
        }
        /// <summary>
        /// Описание мероприятия.
        /// </summary>
        public string Description
        {
            get
            { return m_Description; }
            set
            { m_Description = value; }
        }
        /// <summary>
        /// Идентификатор конференц-зала.
        /// </summary>
        public int ConferenceHallID
        {
            get
            { return m_ConferenceHallID; }
            set
            { m_ConferenceHallID = value; }
        }
        /// <summary>
        /// Название конференц-зала.
        /// </summary>
        public string ConferenceHallName
        {
            get
            { return m_ConferenceHallName; }
            set
            { m_ConferenceHallName = value; }
        }
        /// <summary>
        /// ID офиса.
        /// </summary>
        public int OfficeID
        {
            get
            { return m_OfficeID; }
            set
            { m_OfficeID = value; }
        }
        /// <summary>
        /// Название офиса.
        /// </summary>
        public string OfficeName
        {
            get
            { return m_OfficeName; }
            set
            { m_OfficeName = value; }
        }
        /// <summary>
        /// Дата начала мероприятия.
        /// </summary>
        public DateTime TimeBegin
        {
            get
            { return m_TimeBegin; }
            set
            { m_TimeBegin = value; }
        }
        /// <summary>
        /// Дата окончания мероприятия.
        /// </summary>
        public DateTime TimeEnd
        {
            get
            { return m_TimeEnd; }
            set
            { m_TimeEnd = value; }
        }
        /// <summary>
        /// Список приглашенных на мероприятие лиц.
        /// </summary>
        public string ListOfGuests
        {
            get
            { return m_ListOfGuests; }
            set
            { m_ListOfGuests = value; }
        }
        /// <summary>
        /// Оборудование, необходимое для проведения мероприятия.
        /// </summary>
        public string Equipment
        {
            get
            { return m_Equipment; }
            set
            { m_Equipment = value; }
        }
        #endregion

        #region Конструкторы

        public XMLSerializableArrangement()
        {
        }

        public XMLSerializableArrangement(Arrangement arr)
        {
            if ((arr == null)||(!arr.ID.HasValue))
                throw new ArgumentNullException("arr");

            this.ArrangementID = arr.ID.Value;
            this.Name = arr.Name;
            this.Description = arr.Description;
            this.ConferenceHallID = arr.ConferenceHallID;
            this.ConferenceHallName = arr.ConferenceHallName;
            this.OfficeID = arr.OfficeID;
            this.OfficeName = arr.OfficeName;
            this.TimeBegin = arr.TimeBegin;
            this.TimeEnd = arr.TimeEnd;
            this.ListOfGuests = arr.ListOfGuests;
            this.Equipment = arr.Equipment;
        }
        #endregion
    }
}
