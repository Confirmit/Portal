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
	/// ����� ��� ������ � �������������.
	/// </summary>
    public class ArrangementsManager
    {/*
        #region ����
        private static DateTime m_SelectedDate = DateTime.Today;
        #endregion

        #region ��������
        /// <summary>
        /// ����, ��������� ������������� �� ���������.
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
        
        #region ������
        /// <summary>
        /// ���������� ������ �����������.
		/// </summary>
        /// <returns>������ �����������.</returns>
		public static Arrangement[] GetArrangementsList()
		{
			BaseObjectCollection<Arrangement> coll = (BaseObjectCollection<Arrangement>) BasePlainObject.GetObjects( typeof( Arrangement ) );
			if( coll == null )
				return null;
			else
				return coll.ToArray();
		}

        /// <summary>
        /// ���������� ������ ������ � ������ �����.
        /// </summary>
        /// <param name="OfficeID">ID �����.</param>
        /// <returns>������ ���������� � �������� � ������ �����.</returns>
        public static ConferenceHall[] GetConferenceHallsList(int OfficeID)
        {
            int count;
            BaseObjectCollection<ConferenceHall> coll = new BaseObjectCollection<ConferenceHall>();
            coll.FillFromDataSet(SQLProvider.GetConferenceHallsList(OfficeID));
            return coll.ToArray();
        }

		/// <summary>
        /// ���������� ������ ������ � ������������� �� ��������� ���� � ������ �����.
		/// </summary>
        /// <param name="OfficeID">ID �����.</param>
        /// <returns>������ ���������� � �������� � ������������� �� ��������� ���� � ������ �����.</returns>
        public static ConferenceHall[] GetDayConferenceHallsList(int OfficeID)
		{
            int count;
            BaseObjectCollection<ConferenceHall> coll = new BaseObjectCollection<ConferenceHall>();
            coll.FillFromDataSet(SQLProvider.GetDayConferenceHallsList(OfficeID, SelectedDate));
            return coll.ToArray();
		}

        /// <summary>
        /// ���������� ������ ����������� �� ��������� ���� � ������ �������.
        /// </summary>
        /// <param name="ConferenceHallID">ID �������.</param>
        /// <returns>������ ���������� � ������������ �� ��������� ���� � ������ �������.</returns>
        public static Arrangement[] GetDayArragementsList(int ConferenceHallID)
        {
            int count;
            BaseObjectCollection<Arrangement> coll = new BaseObjectCollection<Arrangement>();
            coll.FillFromDataSet(SQLProvider.GetDayArrangementsList(ConferenceHallID, SelectedDate));
            return coll.ToArray();
        }
        /// <summary>
        /// ��������� ����������� ���������� ����������� � ������ ����������.
        /// </summary>
        /// <param name="ConferenceHallID">ID �������.</param>
        /// <param name="ArrangementID">ID ����������� (��� ��������������).</param>
        /// <param name="dBegin">���� ������.</param>
        /// <param name="dEnd">���� �����.</param>
        /// <returns>�������� �� ����������.</returns>
        public static bool CheckArrangementAdding(int ConferenceHallID, int ArrangementID, DateTime dBegin, DateTime dEnd)
        {
            return SQLProvider.CheckArrangementAdding(ConferenceHallID, ArrangementID, dBegin, dEnd);
        }
		#endregion*/
	}
}

