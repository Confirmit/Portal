using System;

namespace Core.Import
{
	#region ErrorRecordBase

	/// <summary>
	/// ����� ������ ������ � ����������� �������.
	/// </summary>
	[Serializable]
	public abstract class BaseErrorRecord
	{
		#region ����

		private ImportResult.ErrorType m_ErrorType;
		private MLString m_Description;

		#endregion

		#region ��������		

		/// <summary>
		/// ��� ������.
		/// </summary>
		public ImportResult.ErrorType ErrorType
		{
			get
			{
				return m_ErrorType;
			}
			set
			{
				m_ErrorType = value;
			}
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		public MLString Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
			}
		}

		#endregion
	}

	#endregion

	#region ErrorRecord

	/// <summary>
	/// ����� �������� ���� ������ �� ������
	/// </summary>	
	[Serializable]
	public class ErrorRecord : BaseErrorRecord
	{
		#region ����

		private string m_CellAddress;
		private string m_Value;

		#endregion	

		#region Constructors

		public ErrorRecord()
		{
		}

		public ErrorRecord(string cellAddress, string val, MLString description)
		{
			m_CellAddress = cellAddress;
			m_Value = val;
			Description = description;
		}

		public ErrorRecord(int row, int column, string val, MLString description)
		{
			m_CellAddress = '(' + row.ToString() + ',' + column.ToString() + ')';
			m_Value = val;
			Description = description;
		}

		#endregion		

		#region ��������

		/// <summary>
		/// ����� ������
		/// </summary>
		public string CellAddress
		{
			get
			{
				return m_CellAddress;
			}
			set
			{
				m_CellAddress = value;
			}
		}

		/// <summary>
		/// �������� � ������
		/// </summary>
		public string Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		#endregion
	}

	#endregion

	#region DictionaryErrorRecord

	/// <summary>
	/// ����� �������� ���� ������ �� ������ � �����������
	/// </summary>
	[Serializable]
	public class DictionaryErrorRecord : ErrorRecord
	{
		#region ����

		private MLString m_DictionaryName;

		#endregion

		#region ������������

		public DictionaryErrorRecord()
		{
		}

		public DictionaryErrorRecord(string cellAddress, string val, MLString description, MLString name)
			: base(cellAddress, val, description)
		{
			m_DictionaryName = name;			
		}

		public DictionaryErrorRecord(int row, int column, string val, MLString description, MLString name)
			: base(row, column, val, description)
		{
			m_DictionaryName = name;			
		}

		#endregion

		#region ��������

		public MLString DictionaryName
		{
			get
			{
				return m_DictionaryName;
			}
			set
			{
				m_DictionaryName = value;
			}
		}

		#endregion
	}

	#endregion

	#region ExceptionErrorRecord

	/// <summary>
	/// �������� ������-����������.
	/// </summary>
	[Serializable]
	public class ExceptionErrorRecord : BaseErrorRecord
	{
		public ExceptionErrorRecord()
		{
		}

		public ExceptionErrorRecord(MLString description)
		{
			base.Description = description;
			ErrorType = ImportResult.ErrorType.Exception;
		}
	}

	#endregion
}
