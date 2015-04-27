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
	/// �������, ������� ���������� �������� ������, ������ � ��������� ������� ����� ��������������� � ���-�����.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
	public class LogObjectAttribute : Attribute
	{
		private string m_Name;
		private string m_PropertyName;

		/// <summary>
		/// �������� ������� (��������, '������ ����������' ��� '��������').
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// ��� �������� ������, ����������� �� �������� ������� (��������, 'Name','Title' ��� 'UWI').
		/// </summary>
		public string PropertyName
		{
			get { return m_PropertyName; }
			set { m_PropertyName = value; }
		}

		/// <summary>
		/// �����������.
		/// </summary>
		/// <param name="description">�������� ������� (��������, '������ ����������' ��� '��������').</param>
		/// <param name="propertyName">��� �������� ������, ����������� �� �������� ������� (��������, 'Name','Title' ��� 'UWI').</param>
		public LogObjectAttribute( string name, string propertyName )
		{
			m_Name = name;
			m_PropertyName = propertyName;
		}
	}
	#endregion

	/// <summary>
	/// ����� �������� �� ���������� ����� � ��� �������.
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// ������ CommonLogger.
		/// </summary>
		public static ILog Log
		{
			get
			{
				return log4net.LogManager.GetLogger( "CommonLogger" );
			}
		}

		#region ��������������� ������
		/// <summary>
		/// ���������� ������ �� ������� LogObject, ��� null, ���� ��� ���.
		/// </summary>
		/// <param name="prop">��������.</param>
		/// <returns></returns>
		public static LogObjectAttribute GetLogObjectAttribute( Type type )
		{
			object[] attrs = type.GetCustomAttributes( typeof( LogObjectAttribute ), true );
			return attrs.Length > 0 ? (LogObjectAttribute)attrs[0] : null;
		}


		/// <summary>
		/// ���������� �������� �������.
		/// </summary>
		/// <param name="obj">������.</param>
		/// <param name="propertyName">��� ��������, ����������� �� �������� �������.</param>
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

		#region ������ ���������� � ��� ��������� �� ��������� ��� ���������

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������. 
		/// ����� ������ ���� ������� ��������� LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoInsertObject( BasePlainObject obj )
		{
			Type type = obj.GetType();

			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// ���� �������� ���, ������ ������ �� ������������ ��� ����������������
				return;
			}

			string value = GetLogObjectValue( obj, logAttr.PropertyName );

			if(User.Current != null)
			{
				Log.Info( String.Format( @"������������ '{0}' ������� ������ {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"�������� ������ {0} ({1}).", logAttr.Name, value ) );
			}
		}

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������. 
		/// ����� ������ ���� ������� ��������� LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoUpdateObject( BasePlainObject obj )
		{
			Type type = obj.GetType();

			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// ���� �������� ���, ������ ������ �� ������������ ��� ����������������
				return;
			}

			string value = GetLogObjectValue( obj, logAttr.PropertyName );

			if(User.Current != null)
			{
				Log.Info( String.Format( @"������������ '{0}' ������� ������ {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"������� ������ {0} ({1}).", logAttr.Name, value ) );
			}
		}

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������. 
		/// ����� ������ ���� ������� ��������� LogObjectAttribute.
		/// </summary>
		/// <param name="obj"></param>
		public static void InfoDeleteObject( Type type, string value )
		{
			LogObjectAttribute logAttr = GetLogObjectAttribute( type );
			if(logAttr == null)
			{
				// ���� �������� ���, ������ ������ �� ������������ ��� ����������������
				return;
			}

			if(User.Current != null)
			{
				Log.Info( String.Format( @"������������ '{0}' ������ ������ {1} ({2}).", User.Current.Login, logAttr.Name, value ) );
			}
			else
			{
				Log.Info( String.Format( @"������ ������ {0} ({1}).", logAttr.Name, value ) );
			}
		}

		#endregion

		#region ������ ���������� � ��� ��������� �� ��������� ��� ��������� (�� ������������)

		/*
		/// <summary>
		/// ������� � ��� ���������� �� �������� �������.
		/// </summary>
		/// <param name="objectType">��� ������� (��������, ����������� �����������).</param>
		/// <param name="objectID">������������� �������.</param>
		public static void InfoDeleteObject( string objectType, int objectID )
		{
			Log.Info(String.Format( @"������������ '{0}' ������ ������ {1} (ID={2}) �� �������.", User.Current.Login, objectType, objectID ));
		}

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������.
		/// </summary>
		/// <param name="objectType">��� ������� (��������, ����������� �����������).</param>
		/// <param name="objectID">�������� (�������������) ������� (��������, Photoshop).</param>
		public static void InfoInsertObject( string objectType, int objectID )
		{
			InfoInsertObject(objectType,String.Format( "ID={0}", objectID ));
		}

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������.
		/// </summary>
		/// <param name="objectType">��� ������� (��������, ����������� �����������).</param>
		/// <param name="objectName">�������� (�������������) ������� (��������, Photoshop).</param>
		public static void InfoInsertObject( string objectType, string objectName )
		{
			Log.Info( String.Format( @"������������ '{0}' ������� ������ {1} ({2}) � �������.", User.Current.Login, objectType, objectName ) );
		}

		/// <summary>
		/// ������� � ��� ���������� � ���������� �������.
		/// </summary>
		/// <param name="objectType">��� ������� (��������, ����������� �����������).</param>
		/// <param name="objectID">�������� (�������������) ������� (��������, Photoshop).</param>
		public static void InfoUpdateObject( string objectType, int objectID )
		{
			InfoUpdateObject( objectType, String.Format( "ID={0}", objectID ) );
		}
		
		/// <summary>
		/// ������� � ��� ���������� �� ��������� �������.
		/// </summary>
		/// <param name="objectType">��� ������� (��������, ����������� �����������).</param>
		/// <param name="objectName">�������� (�������������) ������� (��������, Photoshop).</param>
		public static void InfoUpdateObject( string objectType, string objectName )
		{
			Log.Info( String.Format( @"������������ '{0}' ������� ������ {1} ({2}).", User.Current.Login, objectType, objectName ) );
		}
		*/

		#endregion
	}
}
