using System.Web.UI;

namespace EPAMSWeb
{
	/// <summary>
	/// Summary description for ControlUtil
	/// </summary>
	public static class ControlUtil
	{
		/// <summary>
		/// Производит поиск контрола с указанным ID.
		/// Поиск производится сначала в NamingContainer'е, в котором находится указанный контрол, 
		/// а затем во всех вышележащих, вплоть до уровня Page.
		/// </summary>
		/// <param name="controlID">ID искомого контрола.</param>
		/// <param name="control">Контрол, с которого начинается поиск.</param>
		/// <param name="searchNamingContainers">Производить ли поиск в NamingContainer'ах, сождержащих данный контрол.</param>
		/// <returns>Если элемент управления не найден, возвращает null.</returns>
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
		/// Рекурсивно ищет элемент управления по ID в иерархии заданного эелемента управления.
		/// </summary>
		/// <returns>Если элемент управления не найден, возвращает null.</returns>
		public static Control FindControlRecursively( string controlID, Control searchIn )
		{
			// Ищем непосредственно в элементе управления
			Control control = searchIn.FindControl( controlID );
			if(control != null)
				return control;

			// Ищем в дочерних элементах
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