using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// Коллекция групп Active Directory
	/// </summary>
	public class ADGroupCollection : BaseBindingCollection<ADGroup>
	{
		/// <summary>
		/// Возвращает коллекцию со страницей групп
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{ 
			ADGroupCollection groups = new ADGroupCollection();
			return new PagingResult( base.GetPage( args, groups ), this.Count );
		}

		/// <summary>
		/// Ищет группу по уникальному имени (DN)
		/// </summary>
		/// <param name="groupName">Уникальное имя группы (DN), которую следует найти.</param>
		/// <returns>Если группа найдена, то она возвращается. Иначе возвращается null.</returns>
		public ADGroup FindGroupByDN( string groupDN )
		{
			foreach (ADGroup group in this)
				if (group.DN == groupDN)
					return group;
			
			return null;
		}
	}
}
