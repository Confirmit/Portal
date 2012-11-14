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
//   /// Класс событий.
//   ///</summary>
//   [DBTable("UptimeEvents")]
//   public class UptimeEvent : BasePlainObject
//   {
//      #region Filelds
//      //Поля
//      private string _Name = "";       //Пояснение
//      private DateTime _BeginTime;     //Время начала
//      private DateTime _EndTime;       //Время окончания
//      private int? _DurationSec;			//Продолжительность события в секундах
//      private int _UserID;					//ID пользователя
//      private int? _ProjectID = 1;      //ID проекта
//      private int? _WorkCategoryID = 1;    //ID категории работ
//      private int _UptimeEventTypeID = 1; //ID типа события

//      protected OldDictionaries m_OldDicts = new OldDictionaries();
//      #endregion

//      #region Classes
//      /// <summary>
//      /// Типы событий.
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
//      /// Пояснение.
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
//      /// Время начала.
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
//      /// Время окончания.
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
//      /// Продолжительность события в секундах.
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
//      /// Продолжительность события.
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
//      /// ID пользователя.
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
//      /// ID проекта.
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
//      /// ID категории работ.
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
//      /// ID типа события.
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
//      /// Тип события.
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
