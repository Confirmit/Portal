using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Core
{
	/// <summary>
	/// ������� �������������� ��������� ������-��������, �������������� ���������� �� ��.
	/// </summary>
	/// <typeparam name="BaseObjectType">����� �������� ���������. ������ ���� �������� ������ BaseObject.</typeparam>
	public class BaseObjectCollection<BaseObjectType> : BaseBindingCollection<BaseObjectType>, ICloneable
		where BaseObjectType : BaseObject
	{
		#region ������ ���������� ��������� �� DataTable � DataSet
		/// <summary>
		/// ��������� ��������� ��������� �� ������ ������ �� �������.
		/// </summary>
		/// <param name="table">������� � �������</param>
		public virtual void FillFromDataTable( DataTable table )
		{
			foreach(DataRow row in table.Rows)
			{
			    BaseObjectType obj = Activator.CreateInstance<BaseObjectType>();
			    obj.ReadFromRow( row );
			    Add( obj );
			}
		}

		/// <summary>
		/// ��������� ��������� ��������� �� ������ ������ �� �������� (������ �������).
		/// </summary>
		/// <param name="dataSet">������� � �������</param>
		public void FillFromDataSet( DataSet dataSet )
		{
			FillFromDataTable( dataSet.Tables[0] );
		}

		/// <summary>
		/// ��������� ��������� ��������� �� ������ ������ �� �������.
		/// </summary>
		/// <param name="table">������� � �������</param>
		public void FillFromDataTable( Type type, DataTable table )
		{
			foreach (DataRow row in table.Rows)
			{
				BaseObjectType obj = (BaseObjectType)Activator.CreateInstance( type );
				obj.ReadFromRow( row );
				Add( obj );
			}
		}

		/// <summary>
		/// ��������� ��������� ��������� �� ������ ������ �� �������� (������ �������).
		/// </summary>
		/// <param name="dataSet">������� � �������</param>
		public void FillFromDataSet( Type type, DataSet dataSet )
		{
			FillFromDataTable( type, dataSet.Tables[0] );
		}

		#endregion

		#region ������ ������
		/// <summary>
		/// ���� ������ �� ��������������.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>���������� ��������� ������, ��� null, ���� ������ �� ��� ������ � ���������.</returns>
		public BaseObjectType Find( int id )
		{
			foreach(BaseObjectType obj in this)
			{
				if(obj.ID == id) return obj;
			}
			return null;
		}

		/// <summary>
		/// ���������� ������ ������� � ��������� �� ��� ��������������.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>���������� ������ ���������� �������, ��� null, ���� ������ �� ��� ������ � ���������.</returns>
		public int IndexOf( int id )
		{
			for (int i=0; i<this.Count; i++)
			{
				BaseObjectType obj = this[i];
				if(obj.ID == id) return i;
			}
			return -1;
		}

		#endregion

		#region ����������� ������ ������ Collection

		/*
		protected override void InsertItem( int index, BaseObjectType item )
		{
			base.InsertItem( index, item );
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		protected override void RemoveItem( int index )
		{
			base.RemoveItem( index );
		}

		protected override void  SetItem(int index, BaseObjectType item)
		{
			base.SetItem( index, item );
		}
		 */

		#endregion		
		
		#region �������� � ������ ��������� ��� ������ � ���������

		/// <summary>
		/// ��������� ������ � ���������.
		/// </summary>
		/// <param name="obj"></param>
		public void AddObject( BaseObjectType obj )
		{
			this.Add( obj );
		}

		/// <summary>
		/// �������� ������ ��������� ��� ����������.
		/// </summary>
		/// <param name="obj"></param>
		public void UpdateObject( BaseObjectType obj )
		{
			int index = this.IndexOf( obj.ID.Value );
			// �������� ������ � ���������
			if(index>=0) this[index] = obj;
		}

		/// <summary>
		/// ������� ������ �� ���������.
		/// </summary>
		/// <param name="id"></param>
		public void DeleteObject( int id )
		{
			int index = this.IndexOf( id );
			// ������� �� ���������
			if(index>=0) this.RemoveAt( index );
		}

		#endregion

		#region ����� GetPage ��� ��������� �������� �������� �� ���������
		/// <summary>
		/// �������� �� ��������� �������� � ��������� �����������
		/// </summary>
		/// <param name="args">��������� ���������� ��������</param>
		/// <param name="target_collection">��������� � �������� ���������</param>
		/// <returns>�������� � �������� ���������</returns>
		protected BaseObjectCollection<BaseObjectType> GetPage( PagingArgs args, BaseObjectCollection<BaseObjectType> targetCollection )
		{
			// ������� �������������� ���������
			targetCollection.Clear();

			for(int i = 0; i < args.PageSize && (args.PageIndex * args.PageSize + i) < Count; i++)
			{
				targetCollection.Add( this[args.PageIndex * args.PageSize + i] );
			}
			return targetCollection;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			//BaseObjectCollection<BaseObjectType> coll = (BaseObjectCollection<BaseObjectType>)MemberwiseClone();
			BaseObjectCollection<BaseObjectType> coll = 
				(BaseObjectCollection<BaseObjectType>)Activator.CreateInstance( this.GetType() );

			// ��������� ������� � ��������� ���������� ���������
			foreach (BaseObjectType obj in this)
			{
				BaseObjectType clone = (BaseObjectType)obj.Clone();
				// ��������� ������������� ������ � ����
				coll.Add( clone );

				/*
				// ��������� ��������� addedObjects
				if(m_addedObjects.IndexOf( obj ) >= 0)
				{
					coll.m_addedObjects.Add( clone );
				}
				// ��������� ��������� updatedObjects
				if(m_updatedObjects.IndexOf( obj ) >= 0)
				{
					coll.m_updatedObjects.Add( clone );
				}
				// ��������� ��������� deletedObjects
				if(m_deletedObjects.IndexOf( obj ) >= 0)
				{
					coll.m_deletedObjects.Add( clone );
				}
				*/
			}

			return coll;
		}

		#endregion
	}
}
