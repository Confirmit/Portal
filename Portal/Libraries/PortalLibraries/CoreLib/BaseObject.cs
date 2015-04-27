using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Reflection;

using Core.ORM;
using Core.ORM.Attributes;

namespace Core
{
	/// <summary>
	/// Базовый класс для всех объектов бизнес-логики.
	/// Содержит базовые методы чтения объекта из БД.
	/// </summary>
	public abstract class BaseObject : ICloneable
	{
		#region [ Fields ]

		private int? m_ID = null;

		#endregion

		#region [ Properties ]

		/// <summary>
		/// ID объекта.
		/// </summary>
        [DBRead("ID", true)]
        public virtual int? ID
        {
            [DebuggerStepThrough]
            get { return m_ID; }
            [DebuggerStepThrough]
            set { m_ID = value; }
        }

		/// <summary>
		/// Сообщает, сохранен ли объект в БД или нет. Для этого просто проверяется наличие значения ID.
		/// </summary>
		public virtual bool IsSaved
		{
			[DebuggerStepThrough]
			get { return m_ID.HasValue; }
		}

		#endregion

		#region [ Методы управления объектом ]

		/// <summary>
		/// Заполняет свойства объекта значениями из записи БД.
		/// </summary>
		/// <param name="row"></param>
        public virtual void ReadFromRow(DataRow row)
        {
            ObjectPropertiesMapper.FillObjectFromRow(this, row);
        }

		/// <summary>
		/// Сохраняет объект, используя транзакцию.
		/// </summary>
		public void SaveUsingTransaction()
		{
			DB.ConnectionManager.BeginTransaction();
			try
			{
				Save();
				DB.ConnectionManager.CommitTransaction();
			}
			catch
			{ 
				DB.ConnectionManager.RollbackTransaction();
				throw;
			}
		}

		/// <summary>
		/// Сохраняет объект в БД.
		/// </summary>
		public abstract void Save();

		/// <summary>
		/// Удаляет объект из БД.
		/// </summary>
		public abstract void Delete();

		#endregion

		#region [ ICloneable Members ]

		/// <summary>
		/// Копирует поля текущего объекта в сотв. поля указанного объекта для определенного типа.
		/// </summary>
		/// <param name="obj">Объект, в который копируется текущий.</param>
		/// <param name="type">Тип объекта.</param>
        private void CloneTypeFields(object obj, Type type)
        {
            if (type == null)
                return;

            // получаем все поля типа (кроме статических)
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            // копируем значения полей
            foreach (FieldInfo field in fields)
            {
                // получаем тип поля
                Type ftype = field.FieldType;
                // если это ValueType, не копируем его, т.к. это уже сделал MemberwiseClone
                if (ftype.IsValueType) continue;

                // если разрешено только поверхностное клонирование (установлен атрибут ShallowCopyAttribute)
                if (field.GetCustomAttributes(typeof(ShallowCopyAttribute), false).Length > 0) continue;

                object value = field.GetValue(this);
                if (value != null)
                {
                    // если поле клонируемо, то пытаемся его клонировать
                    ICloneable cloneableValue = value as ICloneable;
                    if (cloneableValue != null)
                    {
                        field.SetValue(obj, cloneableValue.Clone());
                    }
                    /*
                    else
                    {
                        // иначе просто копируем ссылку на поле
                        field.SetValue( obj, value );
                    }
                    */
                }
                /*
                else
                {
                    field.SetValue( obj, null );
                }
                */
            }

            // если у класса есть базовый тип, то обрабатываем его поля
            if (type != typeof(BaseObject))
            {
                CloneTypeFields(obj, type.BaseType);
            }
        }

        public virtual object Clone()
        {
            object obj = MemberwiseClone();
            CloneTypeFields(obj, obj.GetType());

            return obj;
        }

		#endregion

		#region [ Управление свойствами объекта ]
		
		/// <summary>
		/// Возвращает информацию о значимых свойствах объекта указанного типа.
		/// </summary>
		/// <param name="type">Тип объекта.</param>
		/// <param name="includePK">Включать в список первичные ключи объектов.</param>
		/// <param name="extendedList">
		/// Возвратить краткую версию списка (наиболее значимые свойства объекта) - false, или
		/// полную версию списка - true. 
		/// </param>
		/// <returns></returns>
        public static PropertyInfo[] GetObjectProperties(Type type, bool includePK, bool extendedList)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] dbReadAttributes = pi.GetCustomAttributes(typeof(DBReadAttribute), true);
                if (dbReadAttributes.Length == 0)
                    continue;

                DBReadAttribute dbReadAttribute = (DBReadAttribute)dbReadAttributes[0];

                // исключаем первичные ключи, если это было указано
                if (dbReadAttribute.PrimaryKey && !includePK)
                    continue;

                // исключаем признак Removed
                if (dbReadAttribute.FieldName == "IsRemoved")
                    continue;

                properties.Add(pi);
            }

            return properties.ToArray();
        }

		/// <summary>
		/// Возвращает свойства типа, предназначенные для отображения в визуальных элементах
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
        public static PropertyInfo[] GetDisplayableProperties(Type type)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] dbReadAttributes = pi.GetCustomAttributes(typeof(DBReadAttribute), true);
                if (dbReadAttributes.Length == 0)
                    continue;
                DBReadAttribute dbReadAttribute = (DBReadAttribute)dbReadAttributes[0];

                // исключаем первичные ключи, если это было указано
                if (dbReadAttribute.PrimaryKey)
                    continue;

                // исключаем признак Removed
                if (dbReadAttribute.FieldName == "IsRemoved")
                    continue;

                // исключаем неотображаемые элементы
                object[] displ_attrs = pi.GetCustomAttributes(typeof(DisplayableAttribute), true);
                if (displ_attrs.Length > 0 && ((DisplayableAttribute)displ_attrs[0]).Display == false)
                    continue;

                properties.Add(pi);
            }

            return properties.ToArray();
        }

		#endregion
	}
}