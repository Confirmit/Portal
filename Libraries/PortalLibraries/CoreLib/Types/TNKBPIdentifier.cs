using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// ТНК-BP идентификатор.
	/// </summary>
	public class TNKBPIdentifier : IComparable<TNKBPIdentifier>, ICloneable
	{
		#region Поля

		private string m_value = String.Empty;

		#endregion

		#region Свойства

		public string Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		#endregion

		#region Конструкторы

		public TNKBPIdentifier()
		{
		}

		public TNKBPIdentifier( string value )
		{
			m_value = value;
		}

		public TNKBPIdentifier( TNKBPIdentifier value )
		{
			m_value = (string)value.m_value.Clone();
		}

		#endregion

		public override string ToString()
		{
			return m_value;
		}

		public static implicit operator TNKBPIdentifier( string value )
		{
			return new TNKBPIdentifier( value );
		}

		#region IComparable<TNKBPIdentifier> Members

		public int CompareTo( TNKBPIdentifier other )
		{
			return m_value.CompareTo( other.m_value );
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return new TNKBPIdentifier( this );
		}

		#endregion

		/// <summary>
		/// Возвращает пустой идентификатор.
		/// </summary>
		public static TNKBPIdentifier Empty
		{
			get
			{
				return new TNKBPIdentifier();
			}
		}
	}
}
