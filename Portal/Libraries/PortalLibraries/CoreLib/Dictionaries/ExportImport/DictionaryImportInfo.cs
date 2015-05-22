using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Core.Import;
using Core.Exceptions;

namespace Core.Dictionaries.ExportImport
{
	public class DictionaryImportInfo
	{
		#region Fields

		private IDictionary m_dictionary;
		private int m_newRowsCount = 0;
		private int m_updatedRowsCount = 0;
		private int m_errorsCount = 0;
		private List<string> m_Errors = new List<string>();

		#endregion

		#region Constuctors

		public DictionaryImportInfo( IDictionary dictionary,
			int newRowsCount, int updatedRowsCount, int removedRowsCount, int errorsCount, List<string> errors )
		{
			if(dictionary == null)
			{
				throw new CoreArgumentNullException( "dictionary" );
			}

			m_dictionary = dictionary;
			m_newRowsCount = newRowsCount;
			m_updatedRowsCount = updatedRowsCount;
			m_removedRowsCount = removedRowsCount;
			m_errorsCount = errorsCount;
			m_Errors = errors;
		}

		public DictionaryImportInfo( IDictionary dictionary, ImportResult result )
		{
			if(dictionary == null)
			{
				throw new CoreArgumentNullException( "dictionary" );
			}

			if(result == null)
			{
				throw new CoreArgumentNullException( "result" );
			}

			m_dictionary = dictionary;
			m_newRowsCount = result.NewRecords;
			m_updatedRowsCount = result.UpdatedRecords;

			if(result.HasException)
			{
				FillErrors( result.Exceptions );
			}

			if(result.HasCriticalError)
			{
				FillErrors( result.CriticalErrors );
			}

			if(result.HasNonCriticalError)
			{
				FillErrors( result.Errors );
			}

			if(result.HasDictionaryError)
			{
				FillErrors( result.DictionaryCriticalErrors );
				FillErrors( result.DictionaryErrors );
			}
		}

		#endregion

		#region Свойства

		/// <summary>
		/// Импортированный словарь.
		/// </summary>
		public IDictionary Dictionary
		{
			get { return m_dictionary; }
		}

		/// <summary>
		/// Количество добавленных записей в результате импорта.
		/// </summary>
		public int NewRowsCount
		{
			get { return m_newRowsCount; }
		}

		/// <summary>
		/// Количество измененных записей в результате импорта.
		/// </summary>
		public int UpdatedRowsCount
		{
			get { return m_updatedRowsCount; }
		}

		private int m_removedRowsCount;
		/// <summary>
		/// Количество удаленных записей в результате импорта.
		/// </summary>
		public int RemovedRowsCount
		{
			get { return m_removedRowsCount; }
		}

		/// <summary>
		/// Количество ошибок, возникших в результате импорта.
		/// </summary>
		public int ErrorsCount
		{
			get { return m_errorsCount; }
		}

		/// <summary>
		/// Описания ошибок, возникших в результате импорта
		/// </summary>
		public List<string> Errors
		{
			get { return m_Errors; }
		}

		/// <summary>
		/// Описания первых 10 ошибок, объединенные в одну строку
		/// </summary>
		public string ErrorHtmlDescriptions
		{
			get
			{
				StringBuilder errors = new StringBuilder();
				for(int i = 0; i < Errors.Count && i < 10; i++)
					errors.Append( Errors[i] + "<br>" );
				return errors.ToString();
			}
		}

		#endregion

		#region Methods

		private void FillErrors( IList list )
		{
			m_errorsCount += list.Count;

			foreach(object obj in list)
			{
				BaseErrorRecord record = obj as BaseErrorRecord;
				if(record != null)
				{
					m_Errors.Add( record.Description.ToString() );
				}
			}
		}

		#endregion
	}

	public class DictionaryImportInfoCollection : BaseBindingCollection<DictionaryImportInfo>
	{
		/// <summary>
		/// Возвращает коллекцию со страницей результатов импортирования справочников.
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{
			DictionaryImportInfoCollection resultCollection = new DictionaryImportInfoCollection();
			if(args.SortExpression != "")
				this.Sort( new CommonComparer<DictionaryImportInfo>( args.SortExpression, args.SortOrderASC ) );
			return new PagingResult( base.GetPage( args, resultCollection ), this.Count );
		}
	}
}
