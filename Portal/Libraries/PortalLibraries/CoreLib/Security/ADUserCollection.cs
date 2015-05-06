using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// Коллекция пользователей Active Directory
	/// </summary>
	public class ADUserCollection : BaseBindingCollection<ADUser>
	{
		/// <summary>
		/// Возвращает коллекцию со страницей пользователей
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{ 
			ADUserCollection result_col = new ADUserCollection();
			if( args.SortExpression != "" )
				this.Sort( new CommonComparer<ADUser>( args.SortExpression, args.SortOrderASC ) );
			return new PagingResult( base.GetPage( args, result_col ), this.Count );
		}

		/// <summary>
		/// Ищет пользователя по его логину.
		/// </summary>
		/// <param name="userLogin">Логин пользоваетля, которого следует найти.</param>
		/// <returns>Если пользователь найден, то он возвращается. Иначе возвращается null.</returns>
		public ADUser FindUserByLogin( string userLogin )
		{
			foreach (ADUser user in this)
				if (user.Login == userLogin)
					return user;

			return null;
		}

	}
}
