using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

using Core;
using Core.ORM.Attributes;
using Core.ORM;

using UlterSystems.PortalLib.BusinessObjects;
using Core.DB;

namespace ConfirmIt.PortalLib.Arrangements
{
    /// <summary>
    /// Класс дата мероприятия.
    /// </summary>
    [DBTable("ArrangementDate")]
    public class ArrangementDate : BasePlainObject
    {
        #region Поля 
        private int m_ArrangementID = 0;
        private DateTime m_Date;
        #endregion

        #region Свойства
        /// <summary>
        /// Идентификатор мероприятия.
        /// </summary>
        [DBRead("ArrangementID")]
        public int ArrangementID
        {
            get
            {
                return m_ArrangementID;
            }
            set
            {
                m_ArrangementID = value;
            }
        }
        /// <summary>
        /// Дата мероприятия.
        /// </summary>
        [DBRead("Date")]
        public DateTime Date
        {
            get
            {
                return m_Date;
            }
            set
            {
                m_Date = value;
            }
        }
        #endregion

        #region Конструкторы

        public ArrangementDate()
        {
        }

        public ArrangementDate(int ArrangementID, DateTime Date)
        {
            this.ArrangementID = ArrangementID;
            this.Date = Date;
        }

        public ArrangementDate(XMLSerializableArrangementDate xArr)
        {
            this.ArrangementID = ArrangementID;
            this.Date = Date;
        }
        #endregion

        #region Методы

        public override void Save()
        {
            if (!IsSaved)
            {
                IBaseCommand insertCommand = GetInsertCommand();
                insertCommand.ExecNonQuery();

                Core.Logger.InfoInsertObject(this);
            }
            else
            {
                IBaseCommand updateCommand = GetUpdateCommand();
                updateCommand.ExecNonQuery();
                Core.Logger.InfoUpdateObject(this);
            }
        }

        private IBaseCommand GetInsertCommand()
        {
            // получить имена стобцов в БД, соответствующих полям объекта и 
            // и соответствующие значения полей.
            //string[] columnNames;
            //object[] values;
            //Type[] types;
            //ObjectPropertiesMapper.GetDBColumnsValues(this, false, false, out columnNames, out values, out types);

            //// формируем текст insert-запроса
            //string insertCommandText = ObjectMapper.GetInsertStatement(this);

            //// создаем команду и добавляем в нее апраметры со значениями
            //IBaseCommand command = new Query(insertCommandText);
            //for (int i = 0; i < columnNames.Length; ++i)
            //{
            //    if (types[i] == typeof(Byte[]))
            //    {
            //        command.Add("@" + columnNames[i], values[i], DbType.Binary);
            //    }
            //    else
            //    {
            //        command.Add("@" + columnNames[i], values[i]);
            //    }
            //}

            //return command;
            return null;
        }

        private IBaseCommand GetUpdateCommand()
        {
            // получить имена стобцов в БД, соответствующих полям объекта и 
            // и соответствующие значения полей.
            //string[] columnNames;
            //object[] values;
            //Type[] types;
            //ObjectPropertiesMapper.GetDBColumnsValues(this, false, false, out columnNames, out values, out types);

            //// формируем текст update-запроса
            //string updateCommandText = ObjectMapper.GetUpdateStatement(this);

            //// создаем команду и добавляем в нее апраметры со значениями
            //IBaseCommand command = new Query(updateCommandText);
            //command.Add("@ID", ID);
            //for (int i = 0; i < columnNames.Length; ++i)
            //{
            //    if (types[i] == typeof(Byte[]))
            //    {
            //        command.Add("@" + columnNames[i], values[i], DbType.Binary);
            //    }
            //    else
            //    {
            //        command.Add("@" + columnNames[i], values[i]);
            //    }
            //}

            //return command;
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Класс информации о мероприятиях, пригодный для XML-сериализации.
    /// </summary>
    [Serializable]
    public class XMLSerializableArrangementDate
    {
        #region Поля
        private int m_ArrangementID = 0;
        private DateTime m_Date = DateTime.Today;
        #endregion

        #region Свойства
        /// <summary>
        /// ID мероприятия.
        /// </summary>
        public int ArrangementID
        {
            get
            { return m_ArrangementID; }
            set
            { m_ArrangementID = value; }
        }
        /// <summary>
        /// Дата мероприятия.
        /// </summary>
        public DateTime Date
        {
            get
            { return m_Date; }
            set
            { m_Date = value; }
        }
        #endregion

        #region Конструкторы

        public XMLSerializableArrangementDate()
        {
        }

        public XMLSerializableArrangementDate(ArrangementDate arr)
        {
            if ((arr == null) || (!arr.ID.HasValue))
                throw new ArgumentNullException("arr");

            this.ArrangementID = arr.ID.Value;
            this.Date = arr.Date;
        }
        #endregion
    }
}
