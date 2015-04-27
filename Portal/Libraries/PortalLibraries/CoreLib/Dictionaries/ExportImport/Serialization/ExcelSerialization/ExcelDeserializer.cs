using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using Core.Exceptions;

namespace Core.Dictionaries.ExportImport.Serialization.ExcelSerialization
{
	public class ExcelDeserializer : IDeserializer
	{
		#region IDeserializer Members

		public IEnumerable<DataTable> Deserialize( Stream inputStream )
		{
			// открыть XLS-файл
			XlsFile xlsFile = new XlsFile();
			xlsFile.Open( inputStream );

			// пройти по всем листам...
			for (int sheetIndex = 0; sheetIndex < xlsFile.SheetCount; ++sheetIndex)
			{
				xlsFile.ActiveSheet = sheetIndex + 1;
				// прочитать и возвратить прочитанную таблицу с данными.
				yield return ReadDataTable( xlsFile );
			}
		}

		/// <summary>
		/// Читает данные с текущего листа XLS-файла.
		/// </summary>
		/// <param name="xlsFile">XSL-файл.</param>
		/// <returns>Таблица с прочитанными данными.</returns>
        private static DataTable ReadDataTable(XlsFile xlsFile)
        {
            // установить имя таблицы
            DataTable table = new DataTable(xlsFile.GetSheetName(xlsFile.ActiveSheet));

            // первая строка - названия столбцов
            // вторая строка - типы столбцов
            if (xlsFile.RowCount < 2)
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("InvalidFileFormatException"));

            // прочитать имена столбцов для таблицы
            for (int col = 0; col < xlsFile.ColCount; col++)
            {
                if (xlsFile.GetCellValue(1, col + 1) == null)
                    break;
                // прочитать тип столбца
                Type columnType = Type.GetType(xlsFile.GetCellValue(2, col + 1).ToString(), false);
                if (columnType == null)
                    throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("InvalidFileFormatException"));

                table.Columns.Add(xlsFile.GetCellValue(1, col + 1).ToString(), columnType);
            }

            // прочитать данные
            int rowFrom = 2;
				/* XlsFile тупо обрабатывает
				 * вложенные картинки. Поместить картинку в определённую клетку можно,
				 * а вот взять её из клетки нельзя. Взять картинку можно только из массива
				 * рисунков Excel, начинающегося почему то с 2 (хотя в доке написано ч 1!). 
				 * Будем надеяться, что они там в правильном порядке лежат.
				 */
			int imageCount = 2;
            for (int row = rowFrom; row < xlsFile.RowCount; ++row)
            {
                // Т.к. XlsFile очень часто врёт по поводу количества строк в файле,
                // то мы подстраховываемся таким образом. Считаем, что данные кончились,
                // если колонка 'A' в строке пустая
                if (xlsFile.GetCellValue(row + 1, 1) == null)
                    break;

                object[] values = new object[table.Columns.Count];
				for(int col = 0; col < table.Columns.Count; ++col)
				{
					if(table.Columns[col].DataType == typeof( byte[] ))
					{
						TXlsImgType imageType = TXlsImgType.Unknown;
						values[col] = ConvertValue( xlsFile.GetImage( imageCount++, ref imageType ),
							table.Columns[col].DataType );
					}
					else
					{
						values[col] = ConvertValue( xlsFile.GetCellValue( row + 1, col + 1 ), 
							table.Columns[col].DataType );
					}
				}
                table.Rows.Add(values);
            }

            return table;
        }

		/// <summary>
		/// Преобразовывает объекты из Excel-типов
		/// </summary>
		/// <param name="val">Исходное значение.</param>
		/// <param name="type">Тип, к которому нужно преобразовать.</param>
		/// <returns>Преобразованное значение.</returns>
		private static object ConvertValue( object val, Type type )
		{
			object result = null;
			if(val != null)
			{
				if(type == typeof( DateTime ))
				{
					result = DateTime.FromOADate( (double)val );
				}
				else
				{
					result = val;
				}
			}

			return result;
		}

		#endregion
	}
}
