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
			// ������� XLS-����
			XlsFile xlsFile = new XlsFile();
			xlsFile.Open( inputStream );

			// ������ �� ���� ������...
			for (int sheetIndex = 0; sheetIndex < xlsFile.SheetCount; ++sheetIndex)
			{
				xlsFile.ActiveSheet = sheetIndex + 1;
				// ��������� � ���������� ����������� ������� � �������.
				yield return ReadDataTable( xlsFile );
			}
		}

		/// <summary>
		/// ������ ������ � �������� ����� XLS-�����.
		/// </summary>
		/// <param name="xlsFile">XSL-����.</param>
		/// <returns>������� � ������������ �������.</returns>
        private static DataTable ReadDataTable(XlsFile xlsFile)
        {
            // ���������� ��� �������
            DataTable table = new DataTable(xlsFile.GetSheetName(xlsFile.ActiveSheet));

            // ������ ������ - �������� ��������
            // ������ ������ - ���� ��������
            if (xlsFile.RowCount < 2)
                throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("InvalidFileFormatException"));

            // ��������� ����� �������� ��� �������
            for (int col = 0; col < xlsFile.ColCount; col++)
            {
                if (xlsFile.GetCellValue(1, col + 1) == null)
                    break;
                // ��������� ��� �������
                Type columnType = Type.GetType(xlsFile.GetCellValue(2, col + 1).ToString(), false);
                if (columnType == null)
                    throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("InvalidFileFormatException"));

                table.Columns.Add(xlsFile.GetCellValue(1, col + 1).ToString(), columnType);
            }

            // ��������� ������
            int rowFrom = 2;
				/* XlsFile ���� ������������
				 * ��������� ��������. ��������� �������� � ����������� ������ �����,
				 * � ��� ����� � �� ������ ������. ����� �������� ����� ������ �� �������
				 * �������� Excel, ������������� ������ �� � 2 (���� � ���� �������� � 1!). 
				 * ����� ���������, ��� ��� ��� � ���������� ������� �����.
				 */
			int imageCount = 2;
            for (int row = rowFrom; row < xlsFile.RowCount; ++row)
            {
                // �.�. XlsFile ����� ����� ��� �� ������ ���������� ����� � �����,
                // �� �� ����������������� ����� �������. �������, ��� ������ ���������,
                // ���� ������� 'A' � ������ ������
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
		/// ��������������� ������� �� Excel-�����
		/// </summary>
		/// <param name="val">�������� ��������.</param>
		/// <param name="type">���, � �������� ����� �������������.</param>
		/// <returns>��������������� ��������.</returns>
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
