using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Core
{
	/// <summary>
	/// Базовая типизированная коллекция бизнес-объектов, поддерживающая заполнение из БД.
	/// </summary>
	/// <typeparam name="BaseObjectType">Класс элемента коллекции. Должен быть потомком класса BaseObject.</typeparam>
	public class BaseObjectCollection<BaseObjectType> : BaseBindingCollection<BaseObjectType>, ICloneable
		where BaseObjectType : BaseObject
	{
		#region Методы заполнения коллекции из DataTable и DataSet
		/// <summary>
		/// Заполняет коллекцию объектами на основе данных из таблицы.
		/// </summary>
		/// <param name="table">Таблица с данными</param>
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
		/// Заполняет коллекцию объектами на основе данных из датасета (первая таблица).
		/// </summary>
		/// <param name="dataSet">Датасет с данными</param>
		public void FillFromDataSet( DataSet dataSet )
		{
			FillFromDataTable( dataSet.Tables[0] );
		}

		/// <summary>
		/// Заполняет коллекцию объектами на основе данных из таблицы.
		/// </summary>
		/// <param name="table">Таблица с данными</param>
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
		/// Заполняет коллекцию объектами на основе данных из датасета (первая таблица).
		/// </summary>
		/// <param name="dataSet">Датасет с данными</param>
		public void FillFromDataSet( Type type, DataSet dataSet )
		{
			FillFromDataTable( type, dataSet.Tables[0] );
		}

		#endregion

		#region Методы поиска
		/// <summary>
		/// Ищет объект по идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Возвращает найденный объект, или null, если объект не был найден в коллекции.</returns>
		public BaseObjectType Find( int id )
		{
			foreach(BaseObjectType obj in this)
			{
				if(obj.ID == id) return obj;
			}
			return null;
		}

		/// <summary>
		/// Возвращает индекс объекта в коллекции по его идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Возвращает индекс найденного объекта, или null, если объект не был найден в коллекции.</returns>
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

		#region Виртуальные методы класса Collection

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
		
		#region Свойства и методы коллекции для работы с объектами

		/// <summary>
		/// Добавляет объект в коллекцию.
		/// </summary>
		/// <param name="obj"></param>
		public void AddObject( BaseObjectType obj )
		{
			this.Add( obj );
		}

		/// <summary>
		/// Помечает объект коллекции как измененный.
		/// </summary>
		/// <param name="obj"></param>
		public void UpdateObject( BaseObjectType obj )
		{
			int index = this.IndexOf( obj.ID.Value );
			// заменяем объект в коллекции
			if(index>=0) this[index] = obj;
		}

		/// <summary>
		/// Удаляет объект из коллекции.
		/// </summary>
		/// <param name="id"></param>
		public void DeleteObject( int id )
		{
			int index = this.IndexOf( id );
			// удаляем из коллекции
			if(index>=0) this.RemoveAt( index );
		}

		#endregion

		#region Метод GetPage для получения страницы объектов из коллекции
		/// <summary>
		/// Выделяет из коллекцию страницу с заданными параметрами
		/// </summary>
		/// <param name="args">Параметры выделяемой страницы</param>
		/// <param name="target_collection">Коллекция с заданной страницей</param>
		/// <returns>Коллеция с заданной страницей</returns>
		protected BaseObjectCollection<BaseObjectType> GetPage( PagingArgs args, BaseObjectCollection<BaseObjectType> targetCollection )
		{
			// очищаем результирующую коллекцию
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

			// клонируем объекты и заполняем внутренние коллекции
			foreach (BaseObjectType obj in this)
			{
				BaseObjectType clone = (BaseObjectType)obj.Clone();
				// добавляем клонированный объект в себя
				coll.Add( clone );

				/*
				// формируем коллекцию addedObjects
				if(m_addedObjects.IndexOf( obj ) >= 0)
				{
					coll.m_addedObjects.Add( clone );
				}
				// формируем коллекцию updatedObjects
				if(m_updatedObjects.IndexOf( obj ) >= 0)
				{
					coll.m_updatedObjects.Add( clone );
				}
				// формируем коллекцию deletedObjects
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
