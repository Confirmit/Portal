using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// Аргументы для функций связанных с постраничным выводом
	/// </summary>
	public class PagingArgs
	{
		#region Поля 

		private int m_pageIndex;
		private int m_pageSize;
		private string m_sortExpression;
		private bool m_sortOrderASC;

		#endregion

		#region Константы
		/// <summary>
		/// Макс. размер страницы.
		/// Может использоваться для получения всех записей.
		/// </summary>
		public const int MaxPageSize = Int32.MaxValue;

		#endregion

		#region Конструкторы

		/// <summary>
		/// Создает объект входных параметров пейджинга.
		/// </summary>
		/// <param name="page_index">Индекс страницы (начинается с 0).</param>
		/// <param name="page_size">Размер страницы.</param>
		/// <param name="sort_expr">Выражение для сортировки.</param>
		/// <param name="sort_order_asc">Направление сортировки (true - по возрастанию, false - по убыванию).</param>
		/// <param name="data">Дополнительные данные (могут быть null).</param>
		public PagingArgs( int page_index, int page_size, string sort_expr, bool sort_order_asc )
		{
			m_pageIndex = page_index;
			m_pageSize = page_size;
			m_sortExpression = sort_expr;
			m_sortOrderASC = sort_order_asc;
		}

		#endregion

		#region Свойства
		/// <summary>
		/// Индекс страницы (начинается с 0).
		/// </summary>
		public int PageIndex
		{
			get
			{
				return m_pageIndex;
			}
			set
			{
				m_pageIndex = value;
			}
		}

		/// <summary>
		/// Кол-во элементов на странице.
		/// </summary>
		public int PageSize
		{
			get
			{
				return m_pageSize;
			}
			set
			{
				m_pageSize = value;
			}
		}

		/// <summary>
		/// Выражение для сортировки.
		/// </summary>
		public string SortExpression
		{
			get
			{
				return m_sortExpression;
			}
			set
			{
				m_sortExpression = value;
			}
		}

		/// <summary>
		/// Порядок сортировки (сортировка по возрастанию).
		/// </summary>
		public bool SortOrderASC
		{
			get
			{
				return m_sortOrderASC;
			}
			set
			{
				m_sortOrderASC = value;
			}
		}

		#endregion
	}
}
