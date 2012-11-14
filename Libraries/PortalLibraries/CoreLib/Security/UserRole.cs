using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;

using Core;
using Core.ORM;
using Core.ORM.Attributes;

namespace Core.Security
{
	/// <summary>
	/// ���� ������������ � ������� ��� ���������� ��������.
	/// </summary>
	[DBTable("Roles")]
	public class UserRole : BasePlainObject
	{
		#region ������������

		public UserRole()
		{
		}

		#endregion

		#region ����

		private MLString m_Name = MLString.Empty;

		#endregion

		#region ��������

		/// <summary>
		/// ��� ����.
		/// </summary>
		[DBRead("Name")]
		public MLString Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		public Role Role
		{
			get
			{
				Role result = Role.Anonymous;

				if(IsSaved)
				{
					try
					{
						result = (Role)Enum.ToObject( typeof( Role ), ID );
					}
					catch(Exception /*ex*/ ) { }
				}

				return result;
			}
		}

		#endregion
	}

	public class UserRoleCollection : BaseObjectCollection<UserRole>
	{
	}
}
