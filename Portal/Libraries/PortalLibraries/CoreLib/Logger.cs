using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Core.Security;
using Core.Exceptions;

namespace Core
{
	#region LogObjectAttribute
	/// <summary>
	/// Атрибут, которым необходимо помечать классы, работу с объектами которых нужно протоколировать в лог-файле.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
	public class LogObjectAttribute : Attribute
	{
		private string m_Name;
		private string m_PropertyName;

		/// <summary>
		/// Название объекта (например, 'объект разработки' или 'скважина').
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Имя свойства класса, отвечающего за значение объекта (например, 'Name','Title' или 'UWI').
		/// </summary>
		public string PropertyName
		{
			get { return m_PropertyName; }
			set { m_PropertyName = value; }
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="description">Название объекта (например, 'объект разработки' или 'скважина').</param>
		/// <param name="propertyName">Имя свойства класса, отвечающего за значение объекта (например, 'Name','Title' или 'UWI').</param>
		public LogObjectAttribute( string name, string propertyName )
		{
			m_Name = name;
			m_PropertyName = propertyName;
		}
	}
	#endregion

	/// <summary>
	/// Класс отвечает за добавление строк в лог системы.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Логгер CommonLogger.
		/// </summary>
		public static ILog Log
		{
			get
			{
				return log4net.LogManager.GetLogger( "CommonLogger" );
			}
		}

		#region Вспомогательные методы
		/// <summary>
		/// Возвращает ссылку на атрибут LogObject, или null, если его нет.
		/// </summary>
		/// <param name="prop">Свойство.</param>
		/// <returns></returns>
		public static LogObjectAttribute GetLogObjectAttribute( Type type )
		{
			object[] attrs = type.GetCustomAttributes( typeof( LogObjectAttribute ), true );
			return attrs.Length > 0 ? (LogObjectAttribute)attrs[0] : null;
		}


		/// <summary>
		/// Возвращает значение объекта.
		/// </summary>
		/// <param name="obj">Объект.</param>
		/// <param name="propertyName">Имя свойства, отвечающего за значение объекта.</param>
		public static string GetLogObjectValue( BasePlainObject obj, string propertyName )
		{
			Type type = obj.GetType();

			PropertyInfo prop = type.GetProperty( propertyName );
			if(prop == null)
			{
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("TypeException", type.FullName, propertyName));
			}

			object value = prop.GetValue( obj, null );
			return value != null ? value.ToString() : String.Empty;
		}

		#endregion

		#region Методы добавления в лог сообщения об операциях над объектами

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта. 
		/// Класс должен быть помечен атрибутом LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoInsertObject( BasePlainObject obj )
		{
			Type type = obj.GetType();

			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// если атрибута нет, значит объект не предназначен для протоколирования
				return;
			}

			string value = GetLogObjectValue( obj, logAttr.PropertyName );

			if(User.Current != null)
			{
				Log.Info( String.Format( @"Пользователь '{0}' добавил объект {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"Добавлен объект {0} ({1}).", logAttr.Name, value ) );
			}
		}

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта. 
		/// Класс должен быть помечен атрибутом LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoUpdateObject( BasePlainObject obj )
		{
			Type type = obj.GetType();

			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// если атрибута нет, значит объект не предназначен для протоколирования
				return;
			}

			string value = GetLogObjectValue( obj, logAttr.PropertyName );

			if(User.Current != null)
			{
				Log.Info( String.Format( @"Пользователь '{0}' изменил объект {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"Изменен объект {0} ({1}).", logAttr.Name, value ) );
			}
		}

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта. 
		/// Класс должен быть помечен атрибутом LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoDeleteObject( Type type, string value )
		{
			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// если атрибута нет, значит объект не предназначен для протоколирования
				return;
			}

			if(User.Current != null)
			{
				Log.Info( String.Format( @"Пользователь '{0}' удалил объект {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"Удален объект {0} ({1}).", logAttr.Name, value ) );
			}
		}

		#endregion

		#region Методы добавления в лог сообщения об операциях над объектами (не используются)

		/*
		/// <summary>
		/// Заносит в лог информацию об удалении объекта.
		/// </summary>
		/// <param name="objectType">Тип объекта (например, Программное обеспечение).</param>
		/// <param name="objectID">Идентификатор объекта.</param>
		public static void InfoDeleteObject( string objectType, int objectID )
		{
			Log.Info(String.Format( @"Пользователь '{0}' удалил объект {1} (ID={2}) из системы.", User.Current.Login, objectType, objectID ));
		}

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта.
		/// </summary>
		/// <param name="objectType">Тип объекта (например, Программное обеспечение).</param>
		/// <param name="objectID">Название (идентификатор) объекта (например, Photoshop).</param>
		public static void InfoInsertObject( string objectType, int objectID )
		{
			InfoInsertObject(objectType,String.Format( "ID={0}", objectID ));
		}

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта.
		/// </summary>
		/// <param name="objectType">Тип объекта (например, Программное обеспечение).</param>
		/// <param name="objectName">Название (идентификатор) объекта (например, Photoshop).</param>
		public static void InfoInsertObject( string objectType, string objectName )
		{
			Log.Info( String.Format( @"Пользователь '{0}' добавил объект {1} ({2}) в систему.", User.Current.Login, objectType, objectName ) );
		}

		/// <summary>
		/// Заносит в лог информацию о добавлении объекта.
		/// </summary>
		/// <param name="objectType">Тип объекта (например, Программное обеспечение).</param>
		/// <param name="objectID">Название (идентификатор) объекта (например, Photoshop).</param>
		public static void InfoUpdateObject( string objectType, int objectID )
		{
			InfoUpdateObject( objectType, String.Format( "ID={0}", objectID ) );
		}
		
		/// <summary>
		/// Заносит в лог информацию об изменении объекта.
		/// </summary>
		/// <param name="objectType">Тип объекта (например, Программное обеспечение).</param>
		/// <param name="objectName">Название (идентификатор) объекта (например, Photoshop).</param>
		public static void InfoUpdateObject( string objectType, string objectName )
		{
			Log.Info( String.Format( @"Пользователь '{0}' изменил объект {1} ({2}).", User.Current.Login, objectType, objectName ) );
		}
		*/

		#endregion
	}
}
