using System;
using System.Globalization;

namespace Core
{
    /// <summary>
    /// ����� �������� ������, ����������� ��� ����������� �������� �����.
    /// </summary>
    public static class CultureManager
    {
        private static Languages _currentLanguage;

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
			    return _currentLanguage;
			}

		    set
			{
				if(PersistCurrentLanguage != null)
				{
					PersistCurrentLanguage( value );
				}
			    _currentLanguage = value;
				// ������������� �������� ��� �������� ������
				System.Threading.Thread.CurrentThread.CurrentCulture =
					new CultureInfo( value == Languages.Russian ? "ru-RU" : "en-US" );
				System.Threading.Thread.CurrentThread.CurrentUICulture = 
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
