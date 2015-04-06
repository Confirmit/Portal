using System;
using System.Globalization;

namespace Core
{
    /// <summary>
    /// Класс содержит методы, необходимые для определения текущего языка.
    /// </summary>
    public static class CultureManager
    {
        private static Languages _currentLanguage;

		/// <summary>
		/// Языки, поддерживаемые системой.
		/// </summary>
		public enum Languages
		{
			Russian,
			English
		}

		/// <summary>
		/// Делегат для возвращения текущего языка.
		/// </summary>
		/// <returns></returns>
		public delegate Languages RequestCurrentLanguageCallback();
		/// <summary>
		/// Делегат для сохранения текущего языка.
		/// </summary>
		/// <returns></returns>
		public delegate void PersistCurrentLanguageCallback( Languages language );

		/// <summary>
		/// Должен возвращать значение текущего языка. 
		/// Приложения, заинтересованные в получении текущего языка, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static RequestCurrentLanguageCallback RequestCurrentLanguage;

		/// <summary>
		/// Должен сохранять значение текущего языка . 
		/// Приложения, заинтересованные в получении текущего языка, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static PersistCurrentLanguageCallback PersistCurrentLanguage;

		/// <summary>
		/// Текущий язык системы.
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
				// устанавливаем культуру для текущего потока
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
		/// Текущая культура UI системы.
		/// </summary>
		public static CultureInfo CurrentUICulture
		{
			get
			{
				return new CultureInfo( CurrentLanguage == Languages.Russian ? "ru" : "en" );
			}
		}

		/// <summary>
		/// Текущая культура системы.
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
