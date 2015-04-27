using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// ��������� ������������� Active Directory
	/// </summary>
	public class ADUserCollection : BaseBindingCollection<ADUser>
	{
		/// <summary>
		/// ���������� ��������� �� ��������� �������������
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{ 
			ADUserCollection result_col = new ADUserCollection();
			if( args.SortExpression != "" )
				this.Sort( new CommonComparer<ADUser>( args.SortExpression, args.SortOrderASC ) );
			return new PagingResult( base.GetPage( args, result_col ), this.Count );
		}

		/// <summary>
		/// ���� ������������ �� ��� ������.
		/// </summary>
		/// <param name="userLogin">����� ������������, �������� ������� �����.</param>
		/// <returns>���� ������������ ������, �� �� ������������. ����� ������������ null.</returns>
		public ADUser FindUserByLogin( string userLogin )
		{
			foreach (ADUser user in this)
				if (user.Login == userLogin)
					return user;

			return null;
		}

	}
}
