using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Core
{
	/// <summary>
	/// ��������� �������� � ����������, ��������� � ������������ �������
	/// </summary>
	public class PagingResult
	{
		#region ����

		private object m_result;
		private    int m_totalCount;

		#endregion

		#region ������������

		/// <summary>
		/// ������� ��������� ������ �� �������� ������� � ������ ���-�� ���������
		/// </summary>
		/// <param name="result"></param>
		/// <param name="total_count"></param>
		public PagingResult( object result, int total_count )
		{
			m_result = result;
			m_totalCount = total_count;
		}

		/// <summary>
		/// ������� ��������� ������ �� ��������� ������� ������, ������� �� ���� ������ ��������
		/// </summary>
		/// <param name="array"></param>
		/// <param name="page_number"></param>
		/// <param name="page_size"></param>
		public PagingResult( IList array, int page_number, int page_size )
		{
			ArrayList list = new ArrayList();
			for( int i = page_number * page_size; (i < ( page_number + 1 ) * page_size) && i < array.Count; i++ )
				list.Add( array[i] );
			m_result = list;
			m_totalCount = array.Count;
		}

		#endregion

		#region ��������

		/// <summary>
		/// ��������� �������� (DataSource)
		/// ������, ��������� �������� � ������ ��������
		/// </summary>
		public object Result
		{
			get
			{
				return m_result;
			}
			set
			{
				m_result = value;
			}
		}

		/// <summary>
		/// ����� ���-�� �������� �� ���� ���������
		/// </summary>
		public int TotalCount
		{
			get
			{
				return m_totalCount;
			}
			set
			{
				m_totalCount = value;
			}
		}

		#endregion

		/// <summary>
		/// ������ ��������
		/// </summary>
		public static PagingResult Empty
		{
			get
			{
				return new PagingResult( new object[0], 0 );
			}
		}
	}
}
