using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Core
{
	/// <summary>
	/// Базовая типизированная коллекция, поддерживающая использование в качестве DataSource.
	/// </summary>
	/// <typeparam name="ObjectType">Тип элемента.</typeparam>
	public abstract class BaseBindingCollection<ObjectType> : List<ObjectType>
	{
		/// <summary>
		/// Выделяет из коллекцию страницу с заданными параметрами
		/// </summary>
		/// <param name="args">Параметры выделяемой страницы</param>
		/// <param name="target_collection">Коллекция с заданной страницей</param>
		/// <returns>Коллеция с заданной страницей</returns>
		protected BaseBindingCollection<ObjectType> GetPage( PagingArgs args, BaseBindingCollection<ObjectType> target_collection )
		{
			target_collection.Clear();

			for(int i = 0; i < args.PageSize && (args.PageIndex * args.PageSize + i) < this.Count; i++)
			{
				target_collection.Add( this[args.PageIndex * args.PageSize + i] );
			}
			return target_collection;
		}
	}

}
