using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dictionaries
{
	/// <summary>
	/// Интерфейс управления справочниками.
	/// </summary>
	public interface IDictionaryManager
	{
		/// <summary>
		/// Возвращает страницу элементов справочника.
		/// </summary>
		/// <param name="dict">Справочник.</param>
		/// <param name="args">Аргументы пейджинга.</param>
		/// <returns>PagingResult</returns>
		PagingResult GetItemsPage(IDictionary dict, PagingArgs args);

		/// <summary>
		/// Возвращает полное название справочника по любому из многоязычных полей.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		MLString GetFullName(string name);

		/// <summary>
		/// Создаёт справочник по названию.
		/// </summary>
		/// <param name="name">Название справочника.</param>
		/// <returns>Справочник, если удалось его создать, иначе null.</returns>
		IDictionary CreateDictionary(string name);

		/// <summary>
		/// Создаёт справочник по названию.
		/// </summary>
		/// <param name="name">Название справочника.</param>
		/// <returns>Справочник, если удалось его создать, иначе null.</returns>
		IDictionary CreateDictionary(MLString name);

		/// <summary>
		/// Является справочник импортируемым или нет.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <returns>True, если справочник импортируем, иначе False.</returns>
		bool IsImportable(IDictionary dictionary);

		/// <summary>
		/// Является справочник экспортируемым или нет.
		/// </summary>
		/// <param name="dictionary">Справочник.</param>
		/// <returns>True, если справочник экспортируем, иначе False.</returns>
		bool IsExportable(IDictionary dictionary);

		/// <summary>
		/// Возвращает коллекцию словарей, ссылающихся на данный.
		/// </summary>
		/// <param name="type">Словарь.</param>
		/// <returns>Коллекция.</returns>
		DictionaryCollection GetReferenceDictionaries(IDictionary dictionary);
	}
}
