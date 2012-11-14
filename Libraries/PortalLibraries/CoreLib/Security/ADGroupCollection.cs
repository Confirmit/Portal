using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// ��������� ����� Active Directory
	/// </summary>
	public class ADGroupCollection : BaseBindingCollection<ADGroup>
	{
		/// <summary>
		/// ���������� ��������� �� ��������� �����
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{ 
			ADGroupCollection groups = new ADGroupCollection();
			return new PagingResult( base.GetPage( args, groups ), this.Count );
		}

		/// <summary>
		/// ���� ������ �� ����������� ����� (DN)
		/// </summary>
		/// <param name="groupName">���������� ��� ������ (DN), ������� ������� �����.</param>
		/// <returns>���� ������ �������, �� ��� ������������. ����� ������������ null.</returns>
		public ADGroup FindGroupByDN( string groupDN )
		{
			foreach (ADGroup group in this)
				if (group.DN == groupDN)
					return group;
			
			return null;
		}
	}
}
