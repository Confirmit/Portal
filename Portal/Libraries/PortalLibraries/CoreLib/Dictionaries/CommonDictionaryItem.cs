using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Core;
using Core.DB;
using Core.ORM;
using Core.Import;
using Core.Resources;
using Core.Exceptions;
using Core.Types;

namespace Core.Dictionaries
{
	/// <summary>
	/// ������� ����� ������������.
	/// </summary>
	public abstract class CommonDictionaryItem : BasePlainObject, IDictionary
	{
		#region Fields

		private static readonly string[] m_Keys = { "ID" };
		private IDictionaryManager m_Manager = null;

		#endregion

		#region ������������

		public CommonDictionaryItem(IDictionaryManager manager)
			:
			base()
		{
			m_Manager = manager;
		}

		#endregion

		#region ��������

		public virtual IDictionary Dictionary
		{
			get { return (IDictionary)this; }
		}

		public IDictionaryManager Manager
		{
			get
			{
				return m_Manager;
			}
		}
		
		#endregion

		#region IDictionary Members

		public abstract MLString DictionaryName 
		{ 
			get;
		}

		public abstract MLString DictionaryTitle 
		{
			get;
		}

		public virtual string[] Keys
		{
			get
			{
				return m_Keys;
			}
		}

		public virtual object CreateDictionaryItem()
		{
			return Activator.CreateInstance( GetType() );
		}

		public virtual DataTable Export( IDictionaryManager dictManager )
		{
			return MakeExportDataTable( dictManager );
		}

		public virtual ImportResult Import( DataTable table, DictionaryImportContext context )
		{
			ImportResult result = new ImportResult();

			try
			{
				ImportDictionary( table, context, result );
			}
			catch(Exception ex )
			{
				ICoreException coreEx = ex as ICoreException;
				if(coreEx != null)
				{
					result.WriteException( coreEx.MLMessage );
				}
				else
				{
					result.WriteException( new MLString( ex.Message ) );
				}
			}

			return result;
		}

		#endregion

		#region Methods

		/// <summary>
		/// ��������� ������� ����������� �� �����. ������ �������� ������ ������ ����
		/// ������������ � ������������ �� ��������� Keys.
		/// </summary>
		/// <param name="keyValues">�������� �������� ����� � ValueArray.</param>
		/// <returns>True, ���� ������ �������� ������, ����� False.</returns>
		public virtual bool LoadByKeys( ValueArray keyValues )
		{
			return LoadByKeys( keyValues.Values );
		}

		/// <summary>
		/// ��������� ������� ����������� �� �����. ������ �������� ������ ������ ����
		/// ������������ � ����������� �� ��������� Keys.
		/// </summary>
		/// <param name="keyValues">�������� �������� �����.</param>
		/// <returns>True, ���� ������ �������� ������, ����� False.</returns>
		public virtual bool LoadByKeys( object[] keyValues )
		{
			if(Keys.Length != keyValues.Length)
			{
				throw new CoreException( ResourceManager.GetString( "WrongKeyValueListException", DictionaryName ) );
			}

			bool result = false;
			string[] keys = Keys;
			object[] param = new object[keyValues.Length * 2];
			for(int i = 0; i < keyValues.Length; i++)
			{
				param[i*2] = keys[i];
				param[i*2 + 1] = keyValues[i];
			}

			DataRow row = GetObjectByField( param );
			if(row != null)
			{
				ReadFromRow( row );
				result = true;
			}

			return result;
		}

		/// <summary>
		/// ���������� ������ �������� ������, ��������������� �� �������� Keys.
		/// </summary>
		/// <returns>�������� ������.</returns>
		/// <exception cref="Core.Exceptions.DictionaryKeyNotFoundException">���������, ����
		/// �� ������ ���� � �������.</exception>
		public virtual ValueArray GetKeyValues()
		{
			string[] keys = Keys;
			object[] result = new object[keys.Length];

			for(int i = 0; i < keys.Length; i++)
			{
				PropertyInfo prop = ObjectPropertiesMapper.GetDBReadPropertyByName( GetType(), keys[i] );
				if(prop == null)
				{
					throw new DictionaryKeyNotFoundException( keys[i], DictionaryName.ToString() );
				}

				result[i] = prop.GetValue( this, null );
			}

			return new ValueArray( result );
		}

		/// <summary>
		/// ������ ������� � ����������������� ������� �������.
		/// </summary>
		/// <param name="dictManager">�������� ��������.</param>
		/// <returns>���������������� ������.</returns>
		private DataTable MakeExportDataTable( IDictionaryManager dictManager )
		{
			DataTable result = new DataTable( DictionaryName.ToString() );

            List<PropertyInfo> properties = ObjectPropertiesMapper.GetDBReadProperties(GetType());

			GenerateTableMeta( result, dictManager, properties );

			FillTable( result, properties );

			return result;
		}

		/// <summary>
		/// ������ ���� ������ ������� ��������.
		/// </summary>
		/// <param name="table">������� ��������.</param>
		/// <param name="dictManager">�������� ��������.</param>
		/// <param name="properties">������ �������������� �������.</param>
		private void GenerateTableMeta( DataTable table, IDictionaryManager dictManager, List<PropertyInfo> properties )
		{
			foreach(PropertyInfo prop in properties)
			{
				Type type = GetPropertyType( prop );

				if(type == typeof( MLString ))
				{
					string name = GetColumnName( prop );

					DataColumn columnRU = new DataColumn( name + ObjectMapper.RussianEnding );
					columnRU.DataType = typeof( string );
					table.Columns.Add( columnRU );

					DataColumn columnEN = new DataColumn( name + ObjectMapper.EnglishEnding );
					columnEN.DataType = typeof( string );
					table.Columns.Add( columnEN );
				}
				else
				{
					DataColumn column = new DataColumn( GetColumnName( prop ) );
					column.DataType = type;
					table.Columns.Add( column );
				}
			}
		}

		/// <summary>
		/// ��������� ������� ��������.
		/// </summary>
		/// <param name="table">�������.</param>
		/// <param name="properties">������ �������������� �������.</param>
		private void FillTable( DataTable table, List<PropertyInfo> properties )
		{
			IEnumerable items = (IEnumerable)GetObjects( GetType() );
			foreach(object item in items)
			{
				DataRow row = table.NewRow();

				foreach(PropertyInfo prop in properties)
				{
					object val = prop.GetValue( item, null );
					string columnName = GetColumnName( prop );

					if(prop.PropertyType == typeof( MLString ))
					{
						row[columnName + ObjectMapper.RussianEnding] = ((MLString)val).RussianValue;
						row[columnName + ObjectMapper.EnglishEnding] = ((MLString)val).EnglishValue;
					}
					else
					{
						row[columnName] = ( val == null ? DBNull.Value : val );
					}
				}

				table.Rows.Add( row );
			}
		}

		/// <summary>
		/// ���������� �������� ������� ��� ��������.
		/// </summary>
		/// <param name="property">��������.</param>
		/// <returns>��� �������.</returns>
		private string GetColumnName( PropertyInfo property )
		{
			string linkPrefix = string.Empty;

			if(DBAttributesManager.HasDictionaryLinkAttribute( property ))
			{
				object[] linkAttrs = property.GetCustomAttributes( typeof( DictionaryLinkAttribute ), true );
				IDictionary dict = (IDictionary)Activator.CreateInstance(
					((DictionaryLinkAttribute)linkAttrs[0]).DictionaryLinkType );
				linkPrefix = dict.DictionaryName.ToString();
			}

			return (linkPrefix == string.Empty ? property.Name : linkPrefix + '.' + property.Name);
		}

		/// <summary>
		/// ���������� ����������� � ADO.NET ��� ������� ��� ��������.
		/// </summary>
		/// <param name="property">��������.</param>
		/// <returns>���.</returns>
		private Type GetPropertyType( PropertyInfo property )
		{
			Type result = property.PropertyType;

			if(property.PropertyType.IsGenericType &&
				( property.PropertyType.FullName.Substring( 0, 15 ) == "System.Nullable" ))
			{
				// ������� �������, ��� �������� ��� Nullabel<>, � �� ����� :-(
				result = property.PropertyType.GetGenericArguments()[0];
			}

			return result;
		}

		/// <summary>
		/// ����������� ���������� �� �������.
		/// </summary>
		/// <param name="table">�������.</param>
		/// <param name="context">�������� �������.</param>
		/// <param name="result">��������� �������.</param>
		protected virtual void ImportDictionary( DataTable table, DictionaryImportContext context, ImportResult result )
		{
			Dictionary<PropertyInfo, string> properties = RestoreDBProperties( table );

			foreach(DataRow row in table.Rows)
			{
				ImportRow( properties, context,row,  result );
			}
		}

		/// <summary>
		/// ����������� ���� ������ �����������.
		/// </summary>
		/// <param name="properties">������ ������������� �������.</param>
		/// <param name="context">�������� �������.</param>
		/// <param name="row">��� � �������.</param>
		/// <param name="result">��������� �������.</param>
		private void ImportRow( Dictionary<PropertyInfo, string> properties, DictionaryImportContext context, 
			DataRow row, ImportResult result )
		{
			try
			{
				ConnectionManager.BeginTransaction();

				Type selfType = GetType();
				object obj = Activator.CreateInstance( selfType );

				foreach(KeyValuePair<PropertyInfo, string> property in properties)
				{
					PropertyInfo prop = property.Key;
					string columnName = property.Value;

					if(GetPropertyType( prop ) == typeof( MLString ))
					{
						if(columnName.Length > 3)
						{
							// �������� ��������� ��� ������������� �������
							string tmpName = columnName.Substring( 0, columnName.Length - 3 );
							string valRU = (string)(row[tmpName + ObjectMapper.RussianEnding] == DBNull.Value ? string.Empty 
								: row[tmpName + ObjectMapper.RussianEnding]);
							string valEN = (string)(row[tmpName + ObjectMapper.EnglishEnding] == DBNull.Value ? string.Empty
								: row[tmpName + ObjectMapper.EnglishEnding]);
							prop.SetValue( obj, new MLString( valRU, valEN ), null );
						}
					}
					else
					{
						object val = row[columnName];
						if(val != DBNull.Value)
						{
							prop.SetValue( obj, val, null );
						}
					}
				}

				CommonDictionaryItem item = (CommonDictionaryItem)obj;
				CommonDictionaryItem existingObj = (CommonDictionaryItem)Activator.CreateInstance( selfType );
				bool isNew = false;
				if(existingObj.LoadByKeys( item.GetKeyValues() ))
				{
					// ����� ������������ ������ �� ����� � ��������� ���
					ObjectMapper.CopyObject( item, existingObj );
				}
				else
				{
					isNew = true;
					existingObj = item;
					// ������ �����, ������� �������� ��� ID
					existingObj.ID = null;
				}

				CheckLinks( existingObj, context );

				existingObj.Save();

				ConnectionManager.CommitTransaction();

				if(isNew)
				{
					result.NewRecords = result.NewRecords + 1;
				}
				else
				{
					result.UpdatedRecords = result.UpdatedRecords + 1;
				}
				result.CorrectRecords = result.CorrectRecords + 1;
			}
			catch(Exception ex )
			{
				ConnectionManager.RollbackTransaction();

				ICoreException coreEx = ex as ICoreException;
				if(coreEx != null)
				{
					result.WriteException( coreEx.MLMessage );
				}
				else
				{
					result.WriteException( new MLString( ex.Message ) );
				}

				result.IncorrectRecords = result.IncorrectRecords + 1;
			}
			finally
			{
				result.ProcessedRows = result.ProcessedRows + 1;
			}
		}

		/// <summary>
		/// �� ��������� �������� � ������� ��������������� �������������� ��������
		/// � ������ �����������.
		/// </summary>
		/// <param name="table">�������.</param>
		/// <returns>������� ������� � �������������� �� �������� ������� � �������.</returns>
		private Dictionary<PropertyInfo, string> RestoreDBProperties( DataTable table )
		{
			Dictionary<PropertyInfo, string> result = new Dictionary<PropertyInfo, string>();

			foreach(DataColumn column in table.Columns)
			{
				string propName = ParsePropertyName( column.ColumnName );
                PropertyInfo prop = ObjectPropertiesMapper.GetDBReadPropertyByName(GetType(), propName);
				if(prop == null)
				{
					throw new CoreException( ResourceManager.GetString( "ColumnNotFoundException", propName, DictionaryName.ToString() ) );
				}

				if(!result.ContainsKey( prop ))
				{
					result.Add( prop, column.ColumnName );
				}
			}

			return result;
		}

		/// <summary>
		/// �� ����� ������� ��������������� �������������� ��� ��������.
		/// </summary>
		/// <param name="columnName">�������� �������.</param>
		/// <returns>�������� ��������.</returns>
		private string ParsePropertyName( string columnName )
		{
			string result = string.Empty;

			/* ���� ����� � �������� �������. ���� ��� ��� ����, 
			   ������ �������� ������� ��������� ������ ���������� �������.
			 */
			int dotPosition = columnName.LastIndexOf( '.' );
			if(dotPosition == -1)
			{
				result = columnName;
			}
			else if(columnName.Length > dotPosition + 1)
			{
				// �������� ������� ������� �� �������� ���������� �������
				result = columnName.Substring( dotPosition + 1 );
			}
			else
			{
				throw new CoreException( ResourceManager.GetString( "WrongColumnNameException", columnName, DictionaryName.ToString() ) );
			}

			if(result.Length > 3)
			{
				// �������� ��������� � ������������ �������
				string ending = result.Substring( result.Length - 3 );
				if(ending == ObjectMapper.RussianEnding || ending == ObjectMapper.EnglishEnding)
				{
					result = result.Remove( result.Length - 3 );
				}
			}

			return result;
		}

		/// <summary>
		/// ��������� ����� � ������� ���������.
		/// </summary>
		/// <param name="obj">������.</param>
		/// <param name="context">�������� �������.</param>
		private void CheckLinks( CommonDictionaryItem obj, DictionaryImportContext context )
		{
			Dictionary<IDictionary, PropertyInfo[]> linkedDict =
				ObjectMapper.GetLinkedDictionariesAndProperties( GetType() );

			foreach(KeyValuePair<IDictionary, PropertyInfo[]> keyValue in linkedDict)
			{
				IDictionary dict = keyValue.Key;
				PropertyInfo[] props = keyValue.Value;
				ValueArray keys = new ValueArray( props, obj );

				/* ���������, ���� � ��� ������ ��� ���. ���� ���� ������, �� �� ���������,
				 * ��� ������. ���� ���� ���� ������, ���� �� ���� �� ������ ���� (��������
				 * Nullable �� ����������), �� ��� �� ����� ������, �.�. � ���� ������
				 * ����� ������ ��� ���������� �������, ��� �� ����������� �� Nullable ��������.
				 */
				if(!keys.IsNullArray)
				{
					CommonDictionaryItem item1;
					CommonDictionaryItem item2 = (CommonDictionaryItem)dict.CreateDictionaryItem();
					if(context.Contains( dict ) &&
						(item1 = (CommonDictionaryItem)context.GetDictionaryElement( dict, keys )) != null)
					{
						/* ����� ��������� ������� � ��� ������������.
						 * ��������� ����� ������ �������� � ���������, �.�.
						 * ������ ����� ����������� �� ��������� ������ � ����� �������, � ��� 
						 * ����� ���������� �� �������� (�������� ���� ID c indentity increment
						 * �������� �������� �������������).
						 */
						UpdateLinks( obj, props, item1 );
					}
					else if(item2.LoadByKeys( keys ))
					{
						// ����� ��������� ������� � ����
					}
					else
					{
						// �� ����� ����� �����
						throw new DictionaryLinkNotFoundException( obj.DictionaryName, GetKeyValues().ToString(),
							dict.DictionaryName, keys.ToString() );
					}
				}
			}
		}

		private void UpdateLinks( CommonDictionaryItem objToBeUpdated, PropertyInfo[] objForUpdateLinks, 
			CommonDictionaryItem objUpdateFrom )
		{
			ValueArray values = objUpdateFrom.GetKeyValues();

			if(objForUpdateLinks.Length != values.Values.Length)
			{
				throw new CoreException( ResourceManager.GetString( "WrongKeyValueListException", objUpdateFrom.DictionaryName ) );
			}

			for(int i = 0; i < values.Values.Length; i++)
			{
				objForUpdateLinks[i].SetValue( objToBeUpdated, values.Values[i], null );
			}
		}

		#endregion
	}

	/// <summary>
	/// ��������� ������������.
	/// </summary>
	public class CommonDictionaryItemCollection : BaseObjectCollection<CommonDictionaryItem>
	{
		/// <summary>
		/// ���������� ��������� �� ��������� ������������
		/// </summary>
		public PagingResult GetPage( PagingArgs args )
		{
			CommonDictionaryItemCollection resultCollection = new CommonDictionaryItemCollection();
			if(args.SortExpression != "")
				Sort( new CommonComparer<CommonDictionaryItem>( args.SortExpression, args.SortOrderASC ) );
			return new PagingResult( base.GetPage( args, resultCollection ), Count );
		}

	}
}