using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

using Core;
using Core.Security;
using System.Text.RegularExpressions;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Колонка таблицы, позволяющая делать биндинг для объектов с подсвойствами.
	/// </summary>
	public abstract class BaseBoundField : System.Web.UI.WebControls.BoundField, IAccessible
	{
		#region Свойства

		/// <summary>
		/// Является ли данная колонка многоязычной (типа MLString). 
		/// Если является, то выражение для сортировки будут формироваться 
		/// на основе указанного значения, а также префикса, соответствующего текущему языку.
		/// </summary>
		public bool IsMultilangual
		{
			get
			{
				object o = ViewState["IsMultilangual"];
				return o != null ? (bool)o : false;
			}
			set
			{
				ViewState["IsMultilangual"] = value;
			}
		}
		
		/// <summary>
		/// Выражение для сортировки.
		/// Для многоязычных колонок к исходному выражению добавляется префикс языка.
		/// </summary>
		public override string SortExpression
		{
			get
			{
				if(base.SortExpression != String.Empty && IsMultilangual)
				{
					return CultureManager.CurrentLanguage == CultureManager.Languages.Russian
						? "r" + base.SortExpression
						: "e" + base.SortExpression;
				}
				return base.SortExpression;
			}
			set
			{
				base.SortExpression = value;
			}
		}

		/// <summary>
		/// Список ролей, для которых разрешен показ данной кнопки. 
		/// Строка, в которой через запятую перечислены названия ролей. 
		/// Если оставить незаполненной или пустой, то контрол будет доступен всем.
		/// </summary>
		public string AllowedRoles
		{
			get
			{
				object o = ViewState["AllowedRoles"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["AllowedRoles"] = value;
			}
		}

		#endregion

		protected override object GetValue( Control controlContainer )
		{
			object data_object = null;
			string data_field_name = this.DataField;

			if(controlContainer == null)
			{
				throw new ApplicationException( "DataControlField_NoContainer" );
			}

			data_object =  DataBinder.GetDataItem( controlContainer );
			if((data_object == null) && !base.DesignMode)
			{
				throw new ApplicationException( "DataItem_Not_Found" );
			}

			if( !data_field_name.Equals( BoundField.ThisExpression ) )
			{
				data_object = GetPropertyExpressionValue( data_object, data_field_name );
			}

			if(!base.DesignMode)
			{
				return data_object;
			}
			else
			{
				return this.GetDesignTimeValue();
			}
		}

		/// <summary>
		/// Возвращает значение выражения из свойств 
		/// (например, сложного свойства, например User.ADUser.OfficeName).
		/// </summary>
		/// <param name="obj">Объект, у которого надо прочитать свойство.</param>
		/// <param name="expression">Выражение.</param>
		/// <returns></returns>
		protected object GetPropertyExpressionValue( object obj, string expression )
		{
			object dataObject = obj;

			string[] props = expression.Split( '.' );
			for(int i = 0; i < props.Length; i++)
			{
				dataObject = GetPropertyValue( dataObject, props[i] );
				if(dataObject == null)
				{
					return String.Empty; // возвращаем пустую строку для случаев, когда подобъект не существует
				}
			}

			return dataObject;
		}

		/// <summary>
		/// Возвращает значение указанного свойства объекта.
		/// </summary>
		/// <param name="obj">Объект, у которого надо прочитать свойство.</param>
		/// <param name="propertyName">Имя свойства, значение которого нужно прочитать.</param>
		/// <returns></returns>
		protected object GetPropertyValue( object obj, string propertyName )
		{
			PropertyDescriptor boundFieldDesc = TypeDescriptor.GetProperties( obj ).Find( propertyName, true );
			if(boundFieldDesc == null && !DesignMode)
			{
				throw new ApplicationException( String.Format( "Can't find property '{0}'", propertyName ) );
			}

			return boundFieldDesc.GetValue( obj );
		}

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			return !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
		}

		#endregion

	}
}