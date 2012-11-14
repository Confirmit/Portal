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
		#region ��������

		private DataSet m_resultDataSet = new DataSet();

		protected DataSet ResultDataSet
		{
			get { return m_resultDataSet; }
		}

		#endregion

		#region ISerializer Members

		public void SerializePart( DataTable data )
		{
			// TODO: ������ ��� ������������. ������!
			// data.TableName += Guid.NewGuid().ToString();


			ResultDataSet.Tables.Add( data.Copy() );	
		}

		/// <summary>
		/// ���������� ������ �������� �� ������� ���� XLS-�����.
		/// </summary>
		/// <param name="table">������� � �������.</param>
		/// <param name="xlsFile">XLS-����.</param>
		private static void WriteDataTable(DataTable table, XlsFile xlsFile)
		{
			// ���������� ��� �����
			xlsFile.SheetName = table.TableName;

			// �������� �������� ��������
			for (int i = 0; i < table.Columns.Count; ++i)
			{
				// �������� �������
				xlsFile.SetCellValue( 1, i + 1, table.Columns[i].ColumnName );
				// .NET-��� ������ �������
				xlsFile.SetCellValue( 2, i + 1, table.Columns[i].DataType.ToString() );
			}
			// ������ ������ � ������ ������
			xlsFile.SetRowHidden( 2, true );

			// �������� ������ �������
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
		/// ��������������� ������� � Excel-����������� ����.
		/// </summary>
		/// <param name="val">�������� ��������.</param>
		/// <returns>��������������� ��������.</returns>
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
			
			// ������� ����� XLS-����
			xlsFile.NewFile( tables.Count );

			// �������� � ���� ���������� ������ �������
			for (int tableIndex = 0; tableIndex < tables.Count; ++tableIndex)
			{
				// ���������� ������� ����
				xlsFile.ActiveSheet = tableIndex + 1;
				WriteDataTable( tables[tableIndex], xlsFile );
			}

			// ��������� ���������� ����� � �����
			xlsFile.Save( stream );

			// �������� ��������������� DataSet
			ResultDataSet.Tables.Clear();
		}

		#endregion
	}
}
