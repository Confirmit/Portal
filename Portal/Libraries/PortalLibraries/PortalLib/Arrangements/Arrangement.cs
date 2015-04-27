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
    /// ����� �����������.
    /// </summary>
    [DBTable("Arrangement")]
    public class Arrangement : BasePlainObject 
    {
        #region ���� 
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

        #region ��������
        /// <summary>
        /// ��������� �����������.
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
        /// �������� �����������.
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
        /// ������������� ���������-����.
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
        /// �������� ���������-����.
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
        /// ID �����.
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
        /// �������� �����.
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
        /// ���� ������ �����������.
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
        /// ���� ��������� �����������.
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
        /// ������ ������������ �� ����������� ���.
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
        /// ������������, ����������� ��� ���������� �����������.
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

        #region ������������

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
	/// ����� ���������� � ������������, ��������� ��� XML-������������.
	/// </summary>
    [Serializable]
    public class XMLSerializableArrangement
    {
        #region ���� 
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

        #region ��������
        /// <summary>
        /// ID �����������.
        /// </summary>
        public int ArrangementID
        {
            get
            {   return m_ArrangementID; }
            set
            {   m_ArrangementID = value; }
        }
        /// <summary>
        /// ��������� �����������.
        /// </summary>
        public string Name
        {
            get
            { return m_Name; }
            set
            { m_Name = value; }
        }
        /// <summary>
        /// �������� �����������.
        /// </summary>
        public string Description
        {
            get
            { return m_Description; }
            set
            { m_Description = value; }
        }
        /// <summary>
        /// ������������� ���������-����.
        /// </summary>
        public int ConferenceHallID
        {
            get
            { return m_ConferenceHallID; }
            set
            { m_ConferenceHallID = value; }
        }
        /// <summary>
        /// �������� ���������-����.
        /// </summary>
        public string ConferenceHallName
        {
            get
            { return m_ConferenceHallName; }
            set
            { m_ConferenceHallName = value; }
        }
        /// <summary>
        /// ID �����.
        /// </summary>
        public int OfficeID
        {
            get
            { return m_OfficeID; }
            set
            { m_OfficeID = value; }
        }
        /// <summary>
        /// �������� �����.
        /// </summary>
        public string OfficeName
        {
            get
            { return m_OfficeName; }
            set
            { m_OfficeName = value; }
        }
        /// <summary>
        /// ���� ������ �����������.
        /// </summary>
        public DateTime TimeBegin
        {
            get
            { return m_TimeBegin; }
            set
            { m_TimeBegin = value; }
        }
        /// <summary>
        /// ���� ��������� �����������.
        /// </summary>
        public DateTime TimeEnd
        {
            get
            { return m_TimeEnd; }
            set
            { m_TimeEnd = value; }
        }
        /// <summary>
        /// ������ ������������ �� ����������� ���.
        /// </summary>
        public string ListOfGuests
        {
            get
            { return m_ListOfGuests; }
            set
            { m_ListOfGuests = value; }
        }
        /// <summary>
        /// ������������, ����������� ��� ���������� �����������.
        /// </summary>
        public string Equipment
        {
            get
            { return m_Equipment; }
            set
            { m_Equipment = value; }
        }
        #endregion

        #region ������������

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
