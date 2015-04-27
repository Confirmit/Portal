using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

using Core.Import;
using Core.Exceptions;
using Core.Dictionaries.ExportImport.Packing;

namespace Core.Dictionaries.ExportImport
{
	/// <summary>
	/// ��������� ��������/��������� ������������.
	/// </summary>
	public class ExportImportManager
	{
		#region Fields

		private IPacker m_packer;

		#endregion

		#region Constructors

		public ExportImportManager( IPacker packer )
		{
			if(packer == null)
				throw new CoreArgumentException( Resources.ResourceManager.GetString( "PackingException" ) );

			m_packer = packer;
		}

		#endregion

		#region Properties

		protected IPacker Packer
		{
			get { return m_packer; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// ������������ ������ �� ������������ � �����.
		/// </summary>
		/// <param name="dictionaryNames">������ ���� ������������.</param>
		/// <param name="dictionaryManager">�������� ��������.</param>
		/// <param name="stream">�������� �����, � ������� ������������ �������.</param>
		public void ExportDictionary( MLString[] dictionaryNames, IDictionaryManager dictionaryManager, Stream stream )
		{
			m_packer.StartBatch( stream );

			foreach(MLString dictionaryName in dictionaryNames)
			{
				IDictionary dictionary = dictionaryManager.CreateDictionary( dictionaryName );
				m_packer.AddToBatch( dictionary.Export( dictionaryManager ) );
			}

			m_packer.FinishBatch();
		}

		/// <summary>
		/// ����������� ������ � ����������� �������.
		/// </summary>
		/// <param name="stream">�����, ���������� ������ ��� �������.</param>
		public DictionaryImportInfoCollection ImportDictionary( Stream stream, IDictionaryManager dictionaryManager )
		{
			DictionaryImportInfoCollection importInfo = new DictionaryImportInfoCollection();

			try
			{
				DictionaryImportContext context = new DictionaryImportContext();

				// ������� ������ ������� �� ������
				foreach(DataTable table in m_packer.UnPack( stream ))
				{
					IDictionary dictionary = null;

					try
					{
						// ������� ������ ������� �� �����
						dictionary = dictionaryManager.CreateDictionary( table.TableName );
						if(dictionary != null)
						{
							if(!dictionaryManager.IsImportable( dictionary ))
							{
								throw new DictionaryCantBeImportedException( dictionary.DictionaryName );
							}

							DictionaryImportInfo tmp =
								new DictionaryImportInfo( dictionary, dictionary.Import( table, context ) );

							importInfo.Add( tmp );
						}
					}
					catch(Exception ex)
					{
						List<string> errors = new List<string>( 1 );
						errors.Add( ex.Message );

						importInfo.Add( new DictionaryImportInfo( dictionary, 0, 0, 0, 1, errors ) );
					}
				}
			}
			catch(Exception ex)
			{
				List<string> errors = new List<string>( 1 );
				errors.Add( ex.Message );

				importInfo.Add( new DictionaryImportInfo( null, 0, 0, 0, 1, errors ) );
			}

			return importInfo;
		}

		#endregion
	}
}
