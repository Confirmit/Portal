using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Core
{
	/// <summary>
	/// Multi-language string
	/// Класс представляет собой строку с поддержкой нескольких языков.
	/// </summary>
	[Serializable]
	public struct MLString : ICloneable
	{
		#region Статические поля
		/// <summary>
		/// Пустое значение.
		/// </summary>
		public static readonly MLString Empty;

		#endregion

		#region Поля

		private Dictionary<CultureManager.Languages, string> m_Values;

		#endregion

		#region Конструкторы

		static MLString()
		{
			Empty = new MLString();
		}


		private MLString(MLString value)
		{
			m_Values = new Dictionary<CultureManager.Languages, string>();
			if(value.m_Values != null)
			{
				foreach(CultureManager.Languages language in value.m_Values.Keys)
				{
					m_Values[language] = value.m_Values[language];
				}
			}
		}

		public MLString( string value ) : this(value, value)
		{
		}

		public MLString( string russianValue, string englishValue )
		{
			m_Values = new Dictionary<CultureManager.Languages, string>();
			m_Values[CultureManager.Languages.Russian] = russianValue;
			m_Values[CultureManager.Languages.English] = englishValue;
		}

		#endregion

		#region Свойства
		/// <summary>
		/// Возвращает значение для указанного языка.
		/// </summary>
		/// <param name="language"></param>
		/// <returns></returns>
		public string this[CultureManager.Languages language]
		{
			get
			{
				return m_Values != null && m_Values.ContainsKey( language ) 
					? (string)m_Values[language] 
					: String.Empty;
			}
		}

		/// <summary>
		/// Возвращает русскоязычное значение.
		/// </summary>
		public string RussianValue
		{
			get
			{
				return this[CultureManager.Languages.Russian];
			}
		}

		/// <summary>
		/// Возвращает англоязычное значение.
		/// </summary>
		public string EnglishValue
		{
			get
			{
				return this[CultureManager.Languages.English];
			}
		}

		#endregion

		#region Методы базового класс Object

		/// <summary>
		/// Возвращает текстовое значение. 
		/// </summary>
		/// <returns>Сначала возвращается значение текущего языка. Если же оно пустое, то возвращается значение альтернативного языка.</returns>
		public override string ToString()
		{
			CultureManager.Languages anotherLanguage =
				(CultureManager.CurrentLanguage == CultureManager.Languages.Russian)
				? CultureManager.Languages.English
				: CultureManager.Languages.Russian;
			return (this[CultureManager.CurrentLanguage] != String.Empty)
				? this[CultureManager.CurrentLanguage]
				: this[anotherLanguage];
		}

		public override int GetHashCode()
		{
			string s = this[CultureManager.Languages.Russian] + "_" + this[CultureManager.Languages.English];
			return s.GetHashCode();
		}

		public override bool Equals( object obj )
		{
			if(ReferenceEquals(obj, null))
			{
				return false;
			}

			if(obj is MLString)
			{
				return Equals( (MLString)obj );
			}

			return false;
		}

		public bool Equals( MLString obj )
		{
			string s1 = this[CultureManager.Languages.Russian] + "_" + this[CultureManager.Languages.English];
			string s2 = obj[CultureManager.Languages.Russian] + "_" + obj[CultureManager.Languages.English];
			return s1.Equals( s2 );
		}

		#endregion

		#region Операторы

		public static bool operator ==( MLString obj1, MLString obj2 )
		{
			return obj1.Equals( obj2 );
		}

		public static bool operator !=( MLString obj1, MLString obj2 )
		{
			return !obj1.Equals( obj2 );
		}

		public static MLString operator+( MLString str1, MLString str2 )
		{
			return new MLString(
				str1[CultureManager.Languages.Russian] + str2[CultureManager.Languages.Russian],
				str1[CultureManager.Languages.English] + str2[CultureManager.Languages.English]
				);
		}

		public static MLString operator +( MLString str1, string str2 )
		{
			return new MLString(
				str1[CultureManager.Languages.Russian] + str2,
				str1[CultureManager.Languages.English] + str2
				);
		}

        public static explicit operator MLText(MLString mlString)
        {
            var mlText = new MLText();
            mlText.AddText("ru", mlString.RussianValue);
            mlText.AddText("en", mlString.EnglishValue);
            return mlText;
        }

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return new MLString( this );
		}

		#endregion
	}
}
