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
	public class BoundField : BaseBoundField
	{
		#region Конструкторы

		public BoundField()
		{
			HtmlEncode = false;
		}

		#endregion

		#region Свойства
		/// <summary>
		/// Содержит ли текст ссылку на почтовый ящик.
		/// </summary>
        public bool HasEmailReference
        {
            get
            {
				object o = ViewState["HasEmailReference"];
				return o != null ? (bool)o : false;
            }
            set
            {
				ViewState["HasEmailReference"] = value;
            }
        }

		/// <summary>
		/// Идентификатор ресурса в файле Messages.resx, который дописывается после значения
		/// </summary>
		public string PostfixMessageResourceName
		{
			get 
			{
				return (string)ViewState["PostfixMessageResourceName"]; 
			}
			set 
			{
				ViewState["PostfixMessageResourceName"] = value; 
			}
		}
		

		/// <summary>
		/// Значения полей, длиннее указанной величины переносятся по строкам.
		/// По умолчанию - не переносим.
		/// </summary>
		public int ValueMaxLength
		{
			get
			{
				object o = ViewState["ValueMaxLength"];
				return o != null ? (int)o : 0;
			}
			set
			{
				ViewState["ValueMaxLength"] = value;
			}
		}

		#endregion

		protected override object GetValue( Control controlContainer )
		{
			object data_object = base.GetValue( controlContainer );

			if(!base.DesignMode)
			{
				// обрабатываем некоторые типы значения, форматируя вывод нужным образом
				if(data_object is DateTime)
				{
					DateTime date = (DateTime)data_object;
					return date.ToShortDateString();
				}
				if(data_object is DateTime?)
				{
					DateTime? date = (DateTime?)data_object;
					return date.HasValue ? date.Value.ToShortDateString() : String.Empty;
				}

				string result = data_object.ToString();
				// ограничиваем выходную строку
				if(ValueMaxLength != 0 && result.Length > ValueMaxLength)
				{
					result = SplitString( result, ValueMaxLength );
				}

				if(!String.IsNullOrEmpty( PostfixMessageResourceName )
					&& !String.IsNullOrEmpty( result ))
				{
					result += " " + (string)HttpContext.GetGlobalResourceObject( "Messages", PostfixMessageResourceName );
				}

				if(HasEmailReference)//если поле содержит ссылку на mail, то делаем ее рабочей
				{
					result = Regex.Replace( result, @"([\w-\.]+)@(([\w-]+\.){1,20})([a-zA-Z]{2,4})", "<a href = mailto:${0}>${0}</a>" );
				}
				return result;
			}
			else
				return this.GetDesignTimeValue();
		}

		/// <summary>
		/// Разбивает строку по словам, так, чтобы длина отдельной строчки была приблизительно равна заданному числу
		/// </summary>
		/// <param name="str"></param>
		/// <param name="lenght"></param>
		/// <returns></returns>
		private string SplitString( string str, int lenght )
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			string[] words = str.Split( new char[] { ' ' }, StringSplitOptions.None );
			int char_count = 0;
			foreach( string word in words )
			{
				result.Append( word + " " );
				char_count += word.Length + 1;
				if( char_count > lenght )
				{
					result.Append( "<br>" );
					char_count = 0;
				}
			}
			return result.ToString();
		}
	}
}