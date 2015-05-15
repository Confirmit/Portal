using System;
using System.Collections.Generic;

namespace Core.Import
{
    /// <summary>
    /// Класс содержит результаты импорта
    /// </summary>
	[Serializable]
    public class ImportResult
    {
		/*
		 * Результаты поиска должны быть отсортированы по возрастанию степени критичности ошибки
		 */

		#region ResultStatus

		/// <summary>
        /// Результаты поиска. 
        /// </summary>
        public enum ResultStatus
        {
            /// <summary>
            /// Импорт прошёл успешно
            /// </summary>
            Success = 0,

            /// <summary>
            /// Импорт прошёл с ошибками
            /// </summary>
            Error = 1,

            /// <summary>
            /// Во время импорта произошла критическая ошибка
            /// </summary>
            CriticalError = 2,

            /// <summary>
            /// Во время импорта произошла исключительная ситуация
            /// </summary>
            Exception = 3
		}

		#endregion

		#region ErrorType

		public enum ErrorType
		{
			/// <summary>
			/// Критичная ошибка
			/// </summary>
			Critical = 0,

			/// <summary>
			/// Не критичная ошибка
			/// </summary>
			NonCritical,

			/// <summary>
			/// Критическая ошибка справочника.
			/// </summary>
			DictionaryCritical,

			/// <summary>
			/// Ошибка справочника.
			/// </summary>
			DictionaryNonCritical,

			/// <summary>
			/// Исключение.
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

        #region Свойства

        /// <summary>
        /// Начало периода предоставления данных
        /// </summary>
        public DateTime PeriodBegin
        {
            get { return m_PeriodBegin; }
            set { m_PeriodBegin = value; }
        }

        /// <summary>
        /// Окончание периода предоставления данных
        /// </summary>
        public DateTime PeriodEnd
        {
            get { return m_PeriodEnd; }
            set { m_PeriodEnd = value; }
        }

		/// <summary>
		/// Содержит ли результат критические ошибки
		/// </summary>
        public bool HasCriticalError
        {
            get { return m_CriticalErrors.Count + m_DictionaryCriticalErrors.Count> 0; }
        }

		/// <summary>
		/// Содержит ли результат некритические ошибки
		/// </summary>
        public bool HasNonCriticalError
        {
            get { return m_Errors.Count + m_DictionaryErrors.Count > 0; }
        }

        /// <summary>
        /// Содержит ли результат ошибки отсутсвия значения в справочнике
        /// </summary>
        public bool HasDictionaryError
        {
            get { return m_DictionaryErrors.Count + m_DictionaryCriticalErrors.Count > 0; }
        }

		/// <summary>
		/// Содержит ли результат исключения
		/// </summary>
        public bool HasException
        {
            get { return m_Exceptions.Count > 0; }
        }

		/// <summary>
		/// Статус импорта
		/// </summary>
        public ResultStatus Result
        {
            get { return m_ResultStatus; }
            set { m_ResultStatus = value; }
        }

		/// <summary>
		/// Кол-во обработанных строк в исходном файле
		/// </summary>
        public int ProcessedRows
        {
            get { return m_ProcessedRowsCount; }
            set { m_ProcessedRowsCount = value; }
        }

        /// <summary>
        /// Количество сохранённых в базу записей
        /// </summary>
        public int SavedRecords
        {
            get { return m_SavedRecordsTotal; }
            set { m_SavedRecordsTotal = value; }
        }

        /// <summary>
        /// Количество продлённых записей
        /// </summary>
        public int ExtendedRecords
        {
            get { return m_ExtendedRecords; }
            set { m_ExtendedRecords = value; }
        }

        /// <summary>
        /// Количество сохранённых корректных записей
        /// </summary>
        public int CorrectRecords
        {
            get { return m_SavedCorrectRecords; }
            set { m_SavedCorrectRecords = value; }
        }

        /// <summary>
        /// Количество сохранённых некорректных записей
        /// </summary>
        public int IncorrectRecords
        {
            get { return m_SavedIncorrectRecords; }
            set { m_SavedIncorrectRecords = value; }
        }

        /// <summary>
        /// Количество сохранённых неполных записей
        /// </summary>
        public int IncompleteRecords
        {
            get { return m_SavedIncompleteRecords; }
            set { m_SavedIncompleteRecords = value; }
        }

        /// <summary>
        /// Количество сохранённых утверждённых записей
        /// </summary>
        public int ConfirmedRecords
        {
            get { return m_SavedConfirmedRecords; }
            set { m_SavedConfirmedRecords = value; }
        }

		/// <summary>
		/// Количество новых записей.
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
		/// Количество обновлённых записей.
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
		/// Некритические ошибки
		/// </summary>
		public List<ErrorRecord> Errors
		{
			get 
			{ 
				return m_Errors; 
			}
		}

		/// <summary>
		/// Критические ошибки
		/// </summary>
		public List<ErrorRecord> CriticalErrors
		{
			get
			{
				return m_CriticalErrors;
			}
		}

		/// <summary>
		/// Системные ошибки
		/// </summary>
		public List<ExceptionErrorRecord> Exceptions
		{
			get
			{
				return m_Exceptions;
			}
		}

        /// <summary>
        /// Ошибки отсутствия значения в справочнике
        /// </summary>
		public List<DictionaryErrorRecord> DictionaryErrors
        {
			get
			{
				return m_DictionaryErrors;
			}
        }

		/// <summary>
		/// Критические ошибки отсутствия значения в справочнике
		/// </summary>
		public List<DictionaryErrorRecord> DictionaryCriticalErrors
		{
			get
			{
				return m_DictionaryCriticalErrors;
			}
		}

        #endregion

        #region Методы

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
		/// Сохраняет список ошибок в xls-файл.
		/// Если указан файл импорта, то вписывает в файл ошибок строки из него.
		/// </summary>
		public void GetErrors(ErrorType type, XlsFile file, XlsFile importFile)
		{
			int maxRowCount = 65536;// макс. количество строк в xls-файле

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

					// Копируем строку, в которой надейна ошибка.
					if (importFile != null && record.CellReference != null)
					{
						// Запоминаем ошибки с одинаковыми строками.
						if (!dic.ContainsKey(record.CellReference.Row))
						{
							dic.Add(record.CellReference.Row, j);
							j++;
						}

						// Добавляем ссылку на ошибку.

						TCellAddress ca = new TCellAddress();
						ca.Col = record.CellReference.Col;
						ca.Row = dic[record.CellReference.Row];


						file.AddHyperLink(new TXlsCellRange(i, 1, i, 1),
							new THyperLink(THyperLinkType.CurrentWorkbook,
							"", "", "", "Sheet2!" + ca.CellRef));

						// Копируем строку с ошибкой, 
						// выбираем второй лист, потом возращаемся на первый.
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
					// сохранить в Excel можно не больше 65536 строк
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
