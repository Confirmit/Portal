using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Core.Resources
{
    /// <summary>
    /// Класс, предназначенный для работы с ресурсами.
    /// </summary>
    internal static class ResourceManager
    {
        /// <summary>
        /// Возвращает мультиязыковую запись из файла ресурсов.
        /// </summary>
        /// <param name="name">Имя записи.</param>
        /// <returns>Мультиязыковая запись.</returns>
        public static MLString GetString(string name)
        {
            return new MLString(
                Resources.Strings.ResourceManager.GetString(name, new CultureInfo("ru")), 
                Resources.Strings.ResourceManager.GetString(name, new CultureInfo("en"))
                );
        }

        /// <summary>
        /// Возвращает мультиязыковую запись из файла ресурсов.
        /// </summary>
        /// <param name="name">Имя записи.</param>
        /// <param name="args">Параметры.</param>
        /// <returns>Мультиязыковая запись.</returns>
        public static MLString GetString(string name, params object[] args)
        {
            string russian = String.Format(Resources.Strings.ResourceManager.GetString(name, new CultureInfo("ru")), args);
            string english = String.Format(Resources.Strings.ResourceManager.GetString(name, new CultureInfo("en")), args);

            return new MLString(russian, english);
        }

		/// <summary>
		/// Возвращает мультиязыковую запись из файла ресурсов.
		/// </summary>
		/// <param name="name">Имя записи.</param>
		/// <param name="args">Параметры.</param>
		/// <returns>Мультиязыковая запись.</returns>
		public static MLString GetString( string name, params MLString[] args )
		{
			string[] argsRU = new string[args.Length];
			string[] argsEN = new string[args.Length];
			for(int i = 0; i < args.Length; i++)
			{
				argsEN[i] = args[i].EnglishValue;
				argsRU[i] = args[i].RussianValue;
			}

			string russian = String.Format( Resources.Strings.ResourceManager.GetString( name, new CultureInfo( "ru" ) ), argsRU );
			string english = String.Format( Resources.Strings.ResourceManager.GetString( name, new CultureInfo( "en" ) ), argsEN );

			return new MLString( russian, english );
		}
    }
}
