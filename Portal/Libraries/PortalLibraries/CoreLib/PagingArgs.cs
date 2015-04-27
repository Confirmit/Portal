using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// ��������� ��� ������� ��������� � ������������ �������
	/// </summary>
	public class PagingArgs
	{
		#region ���� 

		private int m_pageIndex;
		private int m_pageSize;
		private string m_sortExpression;
		private bool m_sortOrderASC;

		#endregion

		#region ���������
		/// <summary>
		/// ����. ������ ��������.
		/// ����� �������������� ��� ��������� ���� �������.
		/// </summary>
		public const int MaxPageSize = Int32.MaxValue;

		#endregion

		#region ������������

		/// <summary>
		/// ������� ������ ������� ���������� ���������.
		/// </summary>
		/// <param name="page_index">������ �������� (���������� � 0).</param>
		/// <param name="page_size">������ ��������.</param>
		/// <param name="sort_expr">��������� ��� ����������.</param>
		/// <param name="sort_order_asc">����������� ���������� (true - �� �����������, false - �� ��������).</param>
		/// <param name="data">�������������� ������ (����� ���� null).</param>
		public PagingArgs( int page_index, int page_size, string sort_expr, bool sort_order_asc )
		{
			m_pageIndex = page_index;
			m_pageSize = page_size;
			m_sortExpression = sort_expr;
			m_sortOrderASC = sort_order_asc;
		}

		#endregion

		#region ��������
		/// <summary>
		/// ������ �������� (���������� � 0).
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
		/// ���-�� ��������� �� ��������.
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
		/// ��������� ��� ����������.
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
		/// ������� ���������� (���������� �� �����������).
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
