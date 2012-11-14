//using System;
//using System.Diagnostics;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;

//using UlterSystems.PortalLib.DB;
//using Core;
//using Core.ORM;

//namespace UlterSystems.PortalLib.BusinessObjects
//{
//   ///<summary>
//   /// ����� �������.
//   ///</summary>
//   [DBTable("UptimeEvents")]
//   public class UptimeEvent : BasePlainObject
//   {
//      #region Filelds
//      //����
//      private string _Name = "";       //���������
//      private DateTime _BeginTime;     //����� ������
//      private DateTime _EndTime;       //����� ���������
//      private int? _DurationSec;			//����������������� ������� � ��������
//      private int _UserID;					//ID ������������
//      private int? _ProjectID = 1;      //ID �������
//      private int? _WorkCategoryID = 1;    //ID ��������� �����
//      private int _UptimeEventTypeID = 1; //ID ���� �������

//      protected OldDictionaries m_OldDicts = new OldDictionaries();
//      #endregion

//      #region Classes
//      /// <summary>
//      /// ���� �������.
//      /// </summary>
//      public enum EventType
//      {
//         MainWork = 10,
//         TimeOff = 9,
//         LanchTime = 3,
//         OfficeOut = 7,
//         Ill = 11,
//         BusinessTrip = 12,
//         Vacation = 13,
//         TrustIll = 14,
//         StudyEnglish = 15,
//      }
//      #endregion

//      #region Properties
//      /// <summary>
//      /// ���������.
//      /// </summary>
//      [DBRead("Name")]
//      public string Name
//      {
//         [DebuggerStepThrough]
//         get { return _Name; }
//         [DebuggerStepThrough]
//         set { _Name = value; }
//      }

//      /// <summary>
//      /// ����� ������.
//      /// </summary>
//      [DBRead("BeginTime")]
//      public DateTime BeginTime
//      {
//         [DebuggerStepThrough]
//         get { return _BeginTime; }
//         [DebuggerStepThrough]
//         set { _BeginTime = value; }
//      }

//      /// <summary>
//      /// ����� ���������.
//      /// </summary>
//      [DBRead("EndTime")]
//      public DateTime EndTime
//      {
//         [DebuggerStepThrough]
//         get { return _EndTime; }
//         [DebuggerStepThrough]
//         set { _EndTime = value; }
//      }

//      /// <summary>
//      /// ����������������� ������� � ��������.
//      /// </summary>
//      [DBRead("Duration")]
//      [DBNullable]
//      public int? DurationInSeconds
//      {
//         [DebuggerStepThrough]
//         get { return _DurationSec; }
//         [DebuggerStepThrough]
//         set { _DurationSec = value; }
//      }

//      /// <summary>
//      /// ����������������� �������.
//      /// </summary>
//      public TimeSpan Duration
//      {
//         [DebuggerStepThrough]
//         get 
//         {
//            if (DurationInSeconds == null)
//               return new TimeSpan(0);
//            else
//               return new TimeSpan( DurationInSeconds.Value * TimeSpan.TicksPerSecond ); 
//         }
//         [DebuggerStepThrough]
//         set { DurationInSeconds = (int) value.TotalSeconds; }
//      }

//      /// <summary>
//      /// ID ������������.
//      /// </summary>
//      [DBRead("UserID")]
//      public int UserID
//      {
//         [DebuggerStepThrough]
//         get { return _UserID; }
//         [DebuggerStepThrough]
//         set { _UserID = value; }
//      }

//      /// <summary>
//      /// ID �������.
//      /// </summary>
//      [DBRead("ProjectID")]
//      [DBNullable]
//      public int? ProjectID
//      {
//         [DebuggerStepThrough]
//         get { return _ProjectID; }
//         [DebuggerStepThrough]
//         set { _ProjectID = value; }
//      }

//      /// <summary>
//      /// ID ��������� �����.
//      /// </summary>
//      [DBRead("WorkCategoryID")]
//      [DBNullable]
//      public int? WorkCategoryID
//      {
//         [DebuggerStepThrough]
//         get { return _WorkCategoryID; }
//         [DebuggerStepThrough]
//         set { _WorkCategoryID = value; }
//      }

//      /// <summary>
//      /// ID ���� �������.
//      /// </summary>
//      [DBRead("UptimeEventTypeID")]
//      public int UptimeEventTypeID
//      {
//         [DebuggerStepThrough]
//         get { return _UptimeEventTypeID; }
//         [DebuggerStepThrough]
//         set { _UptimeEventTypeID = value; }
//      }

//      /// <summary>
//      /// ��� �������.
//      /// </summary>
//      public UptimeEvent.EventType UptimeEventType
//      {
//         [DebuggerStepThrough]
//         get { return (EventType) UptimeEventTypeID; }
//         [DebuggerStepThrough]
//         set { UptimeEventTypeID = (int) value; }
//      }
//      #endregion
//   }
//}
