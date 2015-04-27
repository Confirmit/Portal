using System.Web.UI;

namespace EPAMSWeb
{
	/// <summary>
	/// Summary description for ControlUtil
	/// </summary>
	public static class ControlUtil
	{
		/// <summary>
		/// ���������� ����� �������� � ��������� ID.
		/// ����� ������������ ������� � NamingContainer'�, � ������� ��������� ��������� �������, 
		/// � ����� �� ���� �����������, ������ �� ������ Page.
		/// </summary>
		/// <param name="controlID">ID �������� ��������.</param>
		/// <param name="control">�������, � �������� ���������� �����.</param>
		/// <param name="searchNamingContainers">����������� �� ����� � NamingContainer'��, ����������� ������ �������.</param>
		/// <returns>���� ������� ���������� �� ������, ���������� null.</returns>
		public static Control FindTargetControl( string controlID, Control control, bool searchNamingContainers )
		{
			if(searchNamingContainers)
			{
				Control namingContainer = control;
				Control control2 = null;
				while((control2 == null) && (namingContainer != control.Page))
				{
					namingContainer = namingContainer.NamingContainer;
					if(namingContainer == null)
					{
						return control2;
					}
					control2 = namingContainer.FindControl( controlID );
				}
				return control2;
			}
			return control.FindControl( controlID );
		}

		/// <summary>
		/// ���������� ���� ������� ���������� �� ID � �������� ��������� ��������� ����������.
		/// </summary>
		/// <returns>���� ������� ���������� �� ������, ���������� null.</returns>
		public static Control FindControlRecursively( string controlID, Control searchIn )
		{
			// ���� ��������������� � �������� ����������
			Control control = searchIn.FindControl( controlID );
			if(control != null)
				return control;

			// ���� � �������� ���������
			foreach(Control child in searchIn.Controls)
			{
				control = FindControlRecursively( controlID, child );
				if(control != null)
					return control;
			}

			return null;
		}
	}
}