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
	/// ������� ����� ��� ���� �������� ������-������.
	/// �������� ������� ������ ������ ������� �� ��.
	/// </summary>
	public abstract class BaseObject : ICloneable
	{
		#region [ Fields ]

		private int? m_ID = null;

		#endregion

		#region [ Properties ]

		/// <summary>
		/// ID �������.
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
		/// ��������, �������� �� ������ � �� ��� ���. ��� ����� ������ ����������� ������� �������� ID.
		/// </summary>
		public virtual bool IsSaved
		{
			[DebuggerStepThrough]
			get { return m_ID.HasValue; }
		}

		#endregion

		#region [ ������ ���������� �������� ]

		/// <summary>
		/// ��������� �������� ������� ���������� �� ������ ��.
		/// </summary>
		/// <param name="row"></param>
        public virtual void ReadFromRow(DataRow row)
        {
            ObjectPropertiesMapper.FillObjectFromRow(this, row);
        }

		/// <summary>
		/// ��������� ������, ��������� ����������.
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
		/// ��������� ������ � ��.
		/// </summary>
		public abstract void Save();

		/// <summary>
		/// ������� ������ �� ��.
		/// </summary>
		public abstract void Delete();

		#endregion

		#region [ ICloneable Members ]

		/// <summary>
		/// �������� ���� �������� ������� � ����. ���� ���������� ������� ��� ������������� ����.
		/// </summary>
		/// <param name="obj">������, � ������� ���������� �������.</param>
		/// <param name="type">��� �������.</param>
        private void CloneTypeFields(object obj, Type type)
        {
            if (type == null)
                return;

            // �������� ��� ���� ���� (����� �����������)
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            // �������� �������� �����
            foreach (FieldInfo field in fields)
            {
                // �������� ��� ����
                Type ftype = field.FieldType;
                // ���� ��� ValueType, �� �������� ���, �.�. ��� ��� ������ MemberwiseClone
                if (ftype.IsValueType) continue;

                // ���� ��������� ������ ������������� ������������ (���������� ������� ShallowCopyAttribute)
                if (field.GetCustomAttributes(typeof(ShallowCopyAttribute), false).Length > 0) continue;

                object value = field.GetValue(this);
                if (value != null)
                {
                    // ���� ���� ����������, �� �������� ��� �����������
                    ICloneable cloneableValue = value as ICloneable;
                    if (cloneableValue != null)
                    {
                        field.SetValue(obj, cloneableValue.Clone());
                    }
                    /*
                    else
                    {
                        // ����� ������ �������� ������ �� ����
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

            // ���� � ������ ���� ������� ���, �� ������������ ��� ����
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

		#region [ ���������� ���������� ������� ]
		
		/// <summary>
		/// ���������� ���������� � �������� ��������� ������� ���������� ����.
		/// </summary>
		/// <param name="type">��� �������.</param>
		/// <param name="includePK">�������� � ������ ��������� ����� ��������.</param>
		/// <param name="extendedList">
		/// ���������� ������� ������ ������ (�������� �������� �������� �������) - false, ���
		/// ������ ������ ������ - true. 
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

                // ��������� ��������� �����, ���� ��� ���� �������
                if (dbReadAttribute.PrimaryKey && !includePK)
                    continue;

                // ��������� ������� Removed
                if (dbReadAttribute.FieldName == "IsRemoved")
                    continue;

                properties.Add(pi);
            }

            return properties.ToArray();
        }

		/// <summary>
		/// ���������� �������� ����, ��������������� ��� ����������� � ���������� ���������
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

                // ��������� ��������� �����, ���� ��� ���� �������
                if (dbReadAttribute.PrimaryKey)
                    continue;

                // ��������� ������� Removed
                if (dbReadAttribute.FieldName == "IsRemoved")
                    continue;

                // ��������� �������������� ��������
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