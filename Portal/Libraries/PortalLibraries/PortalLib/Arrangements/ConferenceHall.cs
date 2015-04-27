using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using Core;
using Core.ORM.Attributes;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.Arrangements
{
    /// <summary>
    /// ����� ��������� ����.
    /// </summary>
    [DBTable("ConferenceHall")]
    public class ConferenceHall : BasePlainObject 
    {
        #region ����
        private string m_Name = string.Empty;
        private string m_Description = string.Empty;
        private int m_OfficeID = 0;
      	private string m_OfficeName = string.Empty;
        #endregion

        #region ��������
        /// <summary>
        /// ��������� ���������-����.
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
        /// �������� ���������-����.
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
        /// ID �����, � �������� ��������� ���������-���.
        /// </summary>
        [DBRead("OfficeID")]
        public int OfficeID
        {
            get
            {
                return m_OfficeID;
            }
            set
            {
                m_OfficeID = value;
            }
        }
        /// <summary>
        /// �������� �����, � �������� ��������� ���������-���.
        /// </summary>
        public string OfficeName
        {
            get
            {
                if (m_OfficeName == string.Empty)
                {
                    Office office = new Office();
                    office.Load(m_OfficeID);
                    m_OfficeName = office.OfficeName;
                }
                return m_OfficeName;
            }
        }

        #endregion

        #region ������������

        public ConferenceHall()
        {
        }

        public ConferenceHall(
            string Name,
            string Description,
            int OfficeID)
        {
            this.Name = Name;
            this.Description = Description;
            this.OfficeID = OfficeID;
        }
        public ConferenceHall(XMLSerializableConferenceHall xCH)
        {
            this.Name = xCH.Name;
            this.Description = xCH.Description;
            this.OfficeID = xCH.OfficeID;
        }
        #endregion
    }

    /// <summary>
	/// ����� ���������� � ��������� �����, ��������� ��� XML-������������.
	/// </summary>
    [Serializable]
    public class XMLSerializableConferenceHall
    {
        #region ����
        private int m_ID = 0;
        private string m_Name = string.Empty;
        private string m_Description = string.Empty;
        private int m_OfficeID = 0;
      	private string m_OfficeName = string.Empty;
        #endregion

        #region ��������

        /// <summary>
        /// ID ���������-����.
        /// </summary>
        public int ConferenceHallID
        {
            get
            { return m_ID; }
            set
            { m_ID = value; }
        }
        /// <summary>
        /// ��������� ���������-����.
        /// </summary>
        public string Name
        {
            get
            { return m_Name; }
            set
            { m_Name = value; }
        }
        /// <summary>
        /// �������� ���������-����.
        /// </summary>
        public string Description
        {
            get
            { return m_Description; }
            set
            { m_Description = value; }
        }
        /// <summary>
        /// ID �����, � �������� ��������� ���������-���.
        /// </summary>
        public int OfficeID
        {
            get
            { return m_OfficeID; }
            set
            { m_OfficeID = value; }
        }
        /// <summary>
        /// �������� �����, � �������� ��������� ���������-���.
        /// </summary>
        public string OfficeName
        {
            get
            { return m_OfficeName; }
            set
            { m_OfficeName = value; }
        }

        #endregion

        #region ������������

        public XMLSerializableConferenceHall()
        {
        }

        public XMLSerializableConferenceHall(ConferenceHall ch)
        {
            if ((ch == null)||(!ch.ID.HasValue))
                throw new ArgumentNullException("ch");

            this.ConferenceHallID = ch.ID.Value;
            this.Name = ch.Name;
            this.Description = ch.Description;
            this.OfficeID = ch.OfficeID;
            this.OfficeName = ch.OfficeName;
        }
        #endregion
    }
}
