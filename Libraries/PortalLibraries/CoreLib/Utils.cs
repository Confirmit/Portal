using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
	/// <summary>
	/// ��������������� �����.
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// ��������������� ������ � ������, ��������, ��� '�' ���������� "ya".
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private static string TransliterateChar( char c )
		{
			char[] chars = new char[] { 
				'�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', 
				'�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', 
				'�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', 
				'�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�' };
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
		/// ���������� ������������������� ��� �����.
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
		/// ��������� ������ �� ������, ���, ����� ����� ��������� ������� ���� �������������� ����� ��������� �����.
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
