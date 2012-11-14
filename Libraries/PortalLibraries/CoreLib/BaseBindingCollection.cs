using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Core
{
	/// <summary>
	/// ������� �������������� ���������, �������������� ������������� � �������� DataSource.
	/// </summary>
	/// <typeparam name="ObjectType">��� ��������.</typeparam>
	public abstract class BaseBindingCollection<ObjectType> : List<ObjectType>
	{
		/// <summary>
		/// �������� �� ��������� �������� � ��������� �����������
		/// </summary>
		/// <param name="args">��������� ���������� ��������</param>
		/// <param name="target_collection">��������� � �������� ���������</param>
		/// <returns>�������� � �������� ���������</returns>
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
