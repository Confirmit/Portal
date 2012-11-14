using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
	/// <summary>
	/// Вспомогательный класс.
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Транслитерирует символ в строку, например, для 'я' возвращает "ya".
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private static string TransliterateChar( char c )
		{
			char[] chars = new char[] { 
				'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 
				'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я', 
				'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 
				'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я' };
			string[] translit = new string[] { 
				"a", "b", "v", "g", "d", "e", "e", "zh", "z", "i", "i", "k", "l", "m", "n", "o", 
				"p", "r", "s", "t", "u", "f", "h", "c", "ch", "sh", "sch", "", "i", "", "e", "u", "ya", 
				"A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "I", "K", "L", "M", "N", "O", 
				"P", "R", "S", "T", "U", "F", "H", "C", "CH", "SH", "SCH", "", "I", "", "E", "U", "YA" };

			for(int i = 0; i < chars.Length; i++)
			{
				if(chars[i] == c) return translit[i];
			}
			return c.ToString();
		}

		/// <summary>
		/// Возвращает транслитерированное имя файла.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string TransliterateFileName( string fileName )
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < fileName.Length; i++)
			{
				sb.Append( TransliterateChar( fileName[i] ) );
			}
			fileName = sb.ToString();
			fileName = Regex.Replace( fileName, @"[^\w\d\.-]", "_" );
			return fileName;
		}

		/// <summary>
		/// Разбивает строку по словам, так, чтобы длина отдельной строчки была приблизительно равна заданному числу.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string SplitString( string text, int length )
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			string[] words = text.Split( new char[] { ' ' }, StringSplitOptions.None );
			int char_count = 0;
			foreach(string word in words)
			{
				result.Append( word + " " );
				char_count += word.Length + 1;
				if(char_count > length)
				{
					result.Append( "<br>" );
					char_count = 0;
				}
			}
			return result.ToString();
		}
	}
}
