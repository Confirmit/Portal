using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

namespace Core
{
    /// <summary>
    ///  ласс содержит методы, необходимые дл€ определени€ текущего €зыка.
    /// </summary>
    public static class CultureManager
    {
		/// <summary>
		/// языки, поддерживаемые системой.
		/// </summary>
		public enum Languages
		{
			Russian,
			English
		}

		/// <summary>
		/// ƒелегат дл€ возвращени€ текущего €зыка.
		/// </summary>
		/// <returns></returns>
		public delegate Languages RequestCurrentLanguageCallback();
		/// <summary>
		/// ƒелегат дл€ сохранени€ текущего €зыка.
		/// </summary>
		/// <returns></returns>
		public delegate void PersistCurrentLanguageCallback( Languages language );

		/// <summary>
		/// ƒолжен возвращать значение текущего €зыка. 
		/// ѕриложени€, заинтересованные в получении текущего €зыка, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static RequestCurrentLanguageCallback RequestCurrentLanguage;

		/// <summary>
		/// ƒолжен сохран€ть значение текущего €зыка . 
		/// ѕриложени€, заинтересованные в получении текущего €зыка, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static PersistCurrentLanguageCallback PersistCurrentLanguage;

		/// <summary>
		/// “екущий €зык системы.
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
				// устанавливаем культуру дл€ текущего потока
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
