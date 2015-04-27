using System;
using System.Collections.Generic;

namespace Core.Import
{
    /// <summary>
    /// ����� �������� ���������� �������
    /// </summary>
	[Serializable]
    public class ImportResult
    {
		/*
		 * ���������� ������ ������ ���� ������������� �� ����������� ������� ����������� ������
		 */

		#region ResultStatus

		/// <summary>
        /// ���������� ������. 
        /// </summary>
        public enum ResultStatus
        {
            /// <summary>
            /// ������ ������ �������
            /// </summary>
            Success = 0,

            /// <summary>
            /// ������ ������ � ��������
            /// </summary>
            Error = 1,

            /// <summary>
            /// �� ����� ������� ��������� ����������� ������
            /// </summary>
            CriticalError = 2,

            /// <summary>
            /// �� ����� ������� ��������� �������������� ��������
            /// </summary>
            Exception = 3
		}

		#endregion

		#region ErrorType

		public enum ErrorType
		{
			/// <summary>
			/// ��������� ������
			/// </summary>
			Critical = 0,

			/// <summary>
			/// �� ��������� ������
			/// </summary>
			NonCritical,

			/// <summary>
			/// ����������� ������ �����������.
			/// </summary>
			DictionaryCritical,

			/// <summary>
			/// ������ �����������.
			/// </summary>
			DictionaryNonCritical,

			/// <summary>
			/// ����������.
			/// </summary>
			Exception

		}

		#endregion

        public static readonly ImportResult Empty;
        private DateTime m_PeriodBegin = DateTime.MinValue;
        private DateTime m_PeriodEnd = DateTime.MinValue;
        private int m_ProcessedRowsCount = 0;
        private int m_SavedRecordsTotal = 0;
        private int m_SavedCorrectRecords = 0;
        private int m_SavedIncorrectRecords = 0;
        private int m_SavedIncompleteRecords = 0;
        private int m_SavedConfirmedRecords = 0;
        private int m_ExtendedRecords = 0;
		private int m_NewRecords = 0;
		private int m_UpdatedRecords = 0;
        private ResultStatus m_ResultStatus;
		private List<ErrorRecord> m_Errors = new List<ErrorRecord>();
		private List<ErrorRecord> m_CriticalErrors = new List<ErrorRecord>();
		private List<ExceptionErrorRecord> m_Exceptions = new List<ExceptionErrorRecord>();
		private List<DictionaryErrorRecord> m_DictionaryErrors = new List<DictionaryErrorRecord>();
		private List<DictionaryErrorRecord> m_DictionaryCriticalErrors = new List<DictionaryErrorRecord>();

        #region ��������

        /// <summary>
        /// ������ ������� �������������� ������
        /// </summary>
        public DateTime PeriodBegin
        {
            get { return m_PeriodBegin; }
            set { m_PeriodBegin = value; }
        }

        /// <summary>
        /// ��������� ������� �������������� ������
        /// </summary>
        public DateTime PeriodEnd
        {
            get { return m_PeriodEnd; }
            set { m_PeriodEnd = value; }
        }

		/// <summary>
		/// �������� �� ��������� ����������� ������
		/// </summary>
        public bool HasCriticalError
        {
            get { return m_CriticalErrors.Count + m_DictionaryCriticalErrors.Count> 0; }
        }

		/// <summary>
		/// �������� �� ��������� ������������� ������
		/// </summary>
        public bool HasNonCriticalError
        {
            get { return m_Errors.Count + m_DictionaryErrors.Count > 0; }
        }

        /// <summary>
        /// �������� �� ��������� ������ ��������� �������� � �����������
        /// </summary>
        public bool HasDictionaryError
        {
            get { return m_DictionaryErrors.Count + m_DictionaryCriticalErrors.Count > 0; }
        }

		/// <summary>
		/// �������� �� ��������� ����������
		/// </summary>
        public bool HasException
        {
            get { return m_Exceptions.Count > 0; }
        }

		/// <summary>
		/// ������ �������
		/// </summary>
        public ResultStatus Result
        {
            get { return m_ResultStatus; }
            set { m_ResultStatus = value; }
        }

		/// <summary>
		/// ���-�� ������������ ����� � �������� �����
		/// </summary>
        public int ProcessedRows
        {
            get { return m_ProcessedRowsCount; }
            set { m_ProcessedRowsCount = value; }
        }

        /// <summary>
        /// ���������� ����������� � ���� �������
        /// </summary>
        public int SavedRecords
        {
            get { return m_SavedRecordsTotal; }
            set { m_SavedRecordsTotal = value; }
        }

        /// <summary>
        /// ���������� ��������� �������
        /// </summary>
        public int ExtendedRecords
        {
            get { return m_ExtendedRecords; }
            set { m_ExtendedRecords = value; }
        }

        /// <summary>
        /// ���������� ����������� ���������� �������
        /// </summary>
        public int CorrectRecords
        {
            get { return m_SavedCorrectRecords; }
            set { m_SavedCorrectRecords = value; }
        }

        /// <summary>
        /// ���������� ����������� ������������ �������
        /// </summary>
        public int IncorrectRecords
        {
            get { return m_SavedIncorrectRecords; }
            set { m_SavedIncorrectRecords = value; }
        }

        /// <summary>
        /// ���������� ����������� �������� �������
        /// </summary>
        public int IncompleteRecords
        {
            get { return m_SavedIncompleteRecords; }
            set { m_SavedIncompleteRecords = value; }
        }

        /// <summary>
        /// ���������� ����������� ����������� �������
        /// </summary>
        public int ConfirmedRecords
        {
            get { return m_SavedConfirmedRecords; }
            set { m_SavedConfirmedRecords = value; }
        }

		/// <summary>
		/// ���������� ����� �������.
		/// </summary>
		public int NewRecords
		{
			get
			{
				return m_NewRecords;
			}
			set
			{
				m_NewRecords = value;
			}
		}

		/// <summary>
		/// ���������� ���������� �������.
		/// </summary>
		public int UpdatedRecords
		{
			get
			{
				return m_UpdatedRecords;
			}
			set
			{
				m_UpdatedRecords = value;
			}
		}

		/// <summary>
		/// ������������� ������
		/// </summary>
		public List<ErrorRecord> Errors
		{
			get 
			{ 
				return m_Errors; 
			}
		}

		/// <summary>
		/// ����������� ������
		/// </summary>
		public List<ErrorRecord> CriticalErrors
		{
			get
			{
				return m_CriticalErrors;
			}
		}

		/// <summary>
		/// ��������� ������
		/// </summary>
		public List<ExceptionErrorRecord> Exceptions
		{
			get
			{
				return m_Exceptions;
			}
		}

        /// <summary>
        /// ������ ���������� �������� � �����������
        /// </summary>
		public List<DictionaryErrorRecord> DictionaryErrors
        {
			get
			{
				return m_DictionaryErrors;
			}
        }

		/// <summary>
		/// ����������� ������ ���������� �������� � �����������
		/// </summary>
		public List<DictionaryErrorRecord> DictionaryCriticalErrors
		{
			get
			{
				return m_DictionaryCriticalErrors;
			}
		}

        #endregion

        #region ������

		#region WriteError

		public void WriteError( ErrorType errorType, int row, int column, string val, string message )
        {
			ErrorRecord record = new ErrorRecord(row, column, val, new MLString(message));
			record.ErrorType = errorType;

			if (errorType == ErrorType.Critical)
			{
				m_CriticalErrors.Add(record);
			}
			else if (errorType == ErrorType.NonCritical)
			{
				m_Errors.Add(record);
			}
        }

        public void WriteError( ErrorType errorType, string columnAddress, string val, string message )
        {
			ErrorRecord record = new ErrorRecord(columnAddress, val, new MLString(message));
			record.ErrorType = errorType;

			if (errorType == ErrorType.Critical)
			{
				m_CriticalErrors.Add(record);
			}
			else if (errorType == ErrorType.NonCritical)
			{
				m_Errors.Add(record);
			}
        }

		public void WriteError(ErrorType errorType, int row, int column, string val, MLString message)
		{
			ErrorRecord record = new ErrorRecord(row, column, val, message);
			record.ErrorType = errorType;

			if (errorType == ErrorType.Critical)
			{
				m_CriticalErrors.Add(record);
			}
			else if (errorType == ErrorType.NonCritical)
			{
				m_Errors.Add(record);
			}
		}

		public void WriteError(ErrorType errorType, string columnAddress, string val, MLString message)
		{
			ErrorRecord record = new ErrorRecord(columnAddress, val, message);
			record.ErrorType = errorType;

			if (errorType == ErrorType.Critical)
			{
				m_CriticalErrors.Add(record);
			}
			else if (errorType == ErrorType.NonCritical)
			{
				m_Errors.Add(record);
			}
		}

		#endregion

		#region WriteDictionaryError

		public void WriteDictionaryError(ErrorType errorType, int row, int column, string val, 
			string message, MLString dictionaryName )
        {
			DictionaryErrorRecord record = new DictionaryErrorRecord(row, column, val, 
				new MLString( message ), dictionaryName);

			record.ErrorType = errorType;

			if (errorType == ErrorType.DictionaryNonCritical)
				m_DictionaryErrors.Add(record);
			else if (errorType == ErrorType.DictionaryCritical)
				m_DictionaryCriticalErrors.Add(record);
		}

		public void WriteDictionaryError(ErrorType errorType, string columnAddress, string val, 
			string message, MLString dictionaryName)
		{
			DictionaryErrorRecord record = new DictionaryErrorRecord(columnAddress, val, 
				new MLString(message), dictionaryName);

			record.ErrorType = errorType;

			if (errorType == ErrorType.DictionaryNonCritical)
				m_DictionaryErrors.Add(record);
			else if (errorType == ErrorType.DictionaryCritical)
				m_DictionaryCriticalErrors.Add(record);
		}

		public void WriteDictionaryError(ErrorType errorType, int row, int column, string val, 
			MLString message, MLString dictionaryName)
		{
			DictionaryErrorRecord record = new DictionaryErrorRecord(row, column, val, message, dictionaryName);

			record.ErrorType = errorType;

			if (errorType == ErrorType.DictionaryNonCritical)
				m_DictionaryErrors.Add(record);
			else if (errorType == ErrorType.DictionaryCritical)
				m_DictionaryCriticalErrors.Add(record);
		}

		public void WriteDictionaryError(ErrorType errorType, string columnAddress, string val, 
			MLString message, MLString dictionaryName)
		{
			DictionaryErrorRecord record = new DictionaryErrorRecord(columnAddress, val,  message, dictionaryName);

			record.ErrorType = errorType;

			if (errorType == ErrorType.DictionaryNonCritical)
				m_DictionaryErrors.Add(record);
			else if (errorType == ErrorType.DictionaryCritical)
				m_DictionaryCriticalErrors.Add(record);
		}


		#endregion

		#region Write Exception

		public void WriteException( string description )
        {			
            m_Exceptions.Add( new ExceptionErrorRecord(new MLString(description)) );
        }

		public void WriteException(MLString description)
		{
			m_Exceptions.Add(new ExceptionErrorRecord(description));
		}

		#endregion

		/*/// <summary>
		/// ��������� ������ ������ � xls-����.
		/// ���� ������ ���� �������, �� ��������� � ���� ������ ������ �� ����.
		/// </summary>
		public void GetErrors(ErrorType type, XlsFile file, XlsFile importFile)
		{
			int maxRowCount = 65536;// ����. ���������� ����� � xls-�����

			List<ErrorRecord> list = new List<ErrorRecord>();
			
			if (type == ErrorType.Critical)
			{
				list.AddRange(m_CriticalErrors);

				foreach (DictionaryErrorRecord record in m_DictionaryCriticalErrors)
				{
					list.Add(record as ErrorRecord);
				}
			}
			else if (type == ErrorType.NonCritical)
			{
				list.AddRange(m_Errors);

				foreach (DictionaryErrorRecord record in m_DictionaryErrors)
				{
					list.Add(record as ErrorRecord);
				}
			}				

			Dictionary<int, int> dic = new Dictionary<int, int>();
			int j = 1;

			if (list.Count > 0)
			{
				int i = 1;

				foreach (ErrorRecord record in list)
				{
					file.SetCellValue(i, 1, record.CellAddress);
					file.SetCellValue(i, 2, record.Value);
					file.SetCellValue(i, 3, record.Description);

					// �������� ������, � ������� ������� ������.
					if (importFile != null && record.CellReference != null)
					{
						// ���������� ������ � ����������� ��������.
						if (!dic.ContainsKey(record.CellReference.Row))
						{
							dic.Add(record.CellReference.Row, j);
							j++;
						}

						// ��������� ������ �� ������.

						TCellAddress ca = new TCellAddress();
						ca.Col = record.CellReference.Col;
						ca.Row = dic[record.CellReference.Row];


						file.AddHyperLink(new TXlsCellRange(i, 1, i, 1),
							new THyperLink(THyperLinkType.CurrentWorkbook,
							"", "", "", "Sheet2!" + ca.CellRef));

						// �������� ������ � �������, 
						// �������� ������ ����, ����� ����������� �� ������.
						file.ActiveSheet = 2;

						if (file.IsEmptyRow(dic[record.CellReference.Row]))
						{
							System.Text.StringBuilder sb = new System.Text.StringBuilder();
							System.IO.Stream xlsStream = new System.IO.MemoryStream();

							importFile.CopyToClipboardFormat(
								new TXlsCellRange(record.CellReference.Row, 1,
								record.CellReference.Row, importFile.ColCount), sb, xlsStream);

							file.PasteFromTextClipboardFormat(
								dic[record.CellReference.Row], 1, TFlxInsertMode.None,
								sb.ToString());
						}

						file.ActiveSheet = 1;
					}

					i++;
					// ��������� � Excel ����� �� ������ 65536 �����
					if (i > maxRowCount) break;
				}
			}
		}*/

		
		//public string[] GetErrors( ErrorType type )
		//{
		//    string[] result = null;
		//    List<ErrorRecord> list = type == ErrorType.Critical ? m_CriticalErrors : m_Errors;

		//    if( list.Count > 0 )
		//    {
		//        result = new string[ list.Count ];
		//        int i = 0;

		//        foreach( ErrorRecord record in list )
		//            result[i++] = record.CellAddress + " : " + record.Value + " : " + record.Description.ToString();
		//    }

		//    return result;
		//}			

        #endregion
    }
}