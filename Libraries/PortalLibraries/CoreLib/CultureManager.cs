using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Core
{
    /// <summary>
    /// ����� �������� ������, ����������� ��� ����������� �������� �����.
    /// </summary>
    public static class CultureManager
    {
		/// <summary>
		/// �����, �������������� ��������.
		/// </summary>
		public enum Languages
		{
			Russian,
			English
		}

		/// <summary>
		/// ������� ��� ����������� �������� �����.
		/// </summary>
		/// <returns></returns>
		public delegate Languages RequestCurrentLanguageCallback();
		/// <summary>
		/// ������� ��� ���������� �������� �����.
		/// </summary>
		/// <returns></returns>
		public delegate void PersistCurrentLanguageCallback( Languages language );

		/// <summary>
		/// ������ ���������� �������� �������� �����. 
		/// ����������, ���������������� � ��������� �������� �����, 
		/// ������ �������� ���� ���������� � ����� �������.
		/// </summary>
		public static RequestCurrentLanguageCallback RequestCurrentLanguage;

		/// <summary>
		/// ������ ��������� �������� �������� ����� . 
		/// ����������, ���������������� � ��������� �������� �����, 
		/// ������ �������� ���� ���������� � ����� �������.
		/// </summary>
		public static PersistCurrentLanguageCallback PersistCurrentLanguage;

		/// <summary>
		/// ������� ���� �������.
		/// </summary>
		public static Languages CurrentLanguage
		{
			get
			{
				if(RequestCurrentLanguage != null)
				{
					return RequestCurrentLanguage();
				}
				else
				{
					return Languages.Russian;
				}
			}
			set
			{
				if(PersistCurrentLanguage != null)
				{
					PersistCurrentLanguage( value );
				}
				// ������������� �������� ��� �������� ������
				System.Threading.Thread.CurrentThread.CurrentCulture =
					new CultureInfo( value == Languages.Russian ? "ru-RU" : "en-US" );
				System.Threading.Thread.CurrentThread.CurrentUICulture = 
					new CultureInfo( value == Languages.Russian ? "ru" : "en" );
			}
		}

		/// <summary>
		/// ������� �������� UI �������.
		/// </summary>
		public static CultureInfo CurrentUICulture
		{
			get
			{
				return new CultureInfo( CurrentLanguage == Languages.Russian ? "ru" : "en" );
			}
		}

		/// <summary>
		/// ������� �������� �������.
		/// </summary>
		public static CultureInfo CurrentCulture
		{
			get
			{
				return new CultureInfo( CurrentLanguage == Languages.Russian ? "ru-RU" : "en-US" );
			}
		}
	}
}
