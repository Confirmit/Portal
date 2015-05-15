using System;

namespace Core.Import
{
	#region ErrorRecordBase

	/// <summary>
	/// Класс записи ошибки в результатах импорта.
	/// </summary>
	[Serializable]
	public abstract class BaseErrorRecord
	{
		#region Поля

		private ImportResult.ErrorType m_ErrorType;
		private MLString m_Description;

		#endregion

		#region Свойства		

		/// <summary>
		/// Тип ошибки.
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
		/// Описание ошибки
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
	/// Класс содержит одну запись об ошибке
	/// </summary>	
	[Serializable]
	public class ErrorRecord : BaseErrorRecord
	{
		#region Поля

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

		#region Свойства

		/// <summary>
		/// Адрес ячейки
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
		/// Значение в ячейке
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
	/// Класс содержит одну запись об ошибке в справочнике
	/// </summary>
	[Serializable]
	public class DictionaryErrorRecord : ErrorRecord
	{
		#region Поля

		private MLString m_DictionaryName;

		#endregion

		#region Конструкторы

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

		#region Свойства

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
	/// Содержит ошибки-исключения.
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
