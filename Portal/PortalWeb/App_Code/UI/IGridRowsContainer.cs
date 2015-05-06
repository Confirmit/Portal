using System;
using System.Web.UI.WebControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Интерфейс, представляющий элементы управления, содержащие строки.
	/// </summary>
	public interface IGridRowsContainer
	{
		/// <summary>
		/// Возвращает коллекцию строк элемента управления.
		/// </summary>
		GridViewRowCollection Rows { get; }
		/// <summary>
		/// Возвращает строку заголовка элемента управления.
		/// </summary>
		GridViewRow HeaderRow { get;}
		/// <summary>
		/// Коллекция ключей выбранных строк
		/// </summary>
		DataKeyArray SelectedKeys { get;}
	}

}