using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

using FlexCel.Core;
using FlexCel.XlsAdapter;

namespace Core.Dictionaries.ExportImport.Serialization.ExcelSerialization
{
	public class ExcelSerializer : ISerializer
	{
		#region Свойства

		private DataSet m_resultDataSet = new DataSet();

		protected DataSet ResultDataSet
		{
			get { return m_resultDataSet; }
		}

		#endregion

		#region ISerializer Members

		public void SerializePart( DataTable data )
		{
			// TODO: ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ. УБРАТЬ!
			// data.TableName += Guid.NewGuid().ToString();


			ResultDataSet.Tables.Add( data.Copy() );	
		}

		/// <summary>
		/// Записывает данные страницы на текущий лист XLS-файла.
		/// </summary>
		/// <param name="table">Таблица с данными.</param>
		/// <param name="xlsFile">XLS-файл.</param>
		private static void WriteDataTable(DataTable table, XlsFile xlsFile)
		{
			// установить имя листа
			xlsFile.SheetName = table.TableName;

			// записать названия столбцов
			for (int i = 0; i < table.Columns.Count; ++i)
			{
				// название столбца
				xlsFile.SetCellValue( 1, i + 1, table.Columns[i].ColumnName );
				// .NET-тип данных столбца
				xlsFile.SetCellValue( 2, i + 1, table.Columns[i].DataType.ToString() );
			}
			// скрыть строку с типами данных
			xlsFile.SetRowHidden( 2, true );

			// записать данные таблицы
			int rowFrom = 3;
			for (int row = 0; row < table.Rows.Count; ++row)
			{
				object[] values = table.Rows[row].ItemArray;
				for(int col = 0; col < values.Length; ++col)
				{
					object val = values[col];
					if(val != DBNull.Value)
					{
						if(table.Columns[col].DataType == typeof( byte[] ))
						{
							Image image = Image.FromStream( new MemoryStream( (byte[])ConvertValue( val ) ) );
							xlsFile.AddImage( row + rowFrom, col + 1, image );
						}
						else
						{
							xlsFile.SetCellValue( row + rowFrom, col + 1, ConvertValue( val ) );
						}
					}
				}
			}
		}

		/// <summary>
		/// Преобразовывает объекты в Excel-совместимые типы.
		/// </summary>
		/// <param name="val">Исходное значение.</param>
		/// <returns>Преобразованное значение.</returns>
		private static object ConvertValue(object val)
		{
			if(val is DateTime)
			{
				return ((DateTime)val).ToOADate();
			}
			
			return val;
		}

		public void WriteSerializationResult( Stream stream )
		{
			XlsFile xlsFile = new XlsFile(true);
			DataTableCollection tables = ResultDataSet.Tables;
			
			// создать новый XLS-файл
			xlsFile.NewFile( tables.Count );

			// записать в файл содержимое каждой таблицы
			for (int tableIndex = 0; tableIndex < tables.Count; ++tableIndex)
			{
				// установить текущий лист
				xlsFile.ActiveSheet = tableIndex + 1;
				WriteDataTable( tables[tableIndex], xlsFile );
			}

			// отправить содержимое файла в поток
			xlsFile.Save( stream );

			// сбросить вспомогательный DataSet
			ResultDataSet.Tables.Clear();
		}

		#endregion
	}
}
