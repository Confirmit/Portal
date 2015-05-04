using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

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
                if (RequestCurrentLanguage != null)
                {
                    return RequestCurrentLanguage();
                }
                if(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en")
                    return Languages.English;

                return Languages.Russian;
            }
		    set
			{
				if(PersistCurrentLanguage != null)
				{
					PersistCurrentLanguage( value );
				}
				// ������������� �������� ��� �������� ������
				Thread.CurrentThread.CurrentCulture =
					new CultureInfo( value == Languages.Russian ? "ru-RU" : "en-US" );
				Thread.CurrentThread.CurrentUICulture = 
					new CultureInfo( value == Languages.Russian ? "ru" : "en" );
			}
		}

        public static void SetLanguage(String language)
        {
            if (language == "en")
                CurrentLanguage = Languages.English;
            else if (language == "ru")
                CurrentLanguage = Languages.Russian;
        }
	}
}
