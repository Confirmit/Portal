using System;
using System.Data;
using System.Collections.Generic;

using Core;
using Core.Import;

namespace Core.Dictionaries
{
	/// <summary>
	/// Справочник.
	/// </summary>
	public interface IDictionary
	{
		/// <summary>
		/// Имя справочника.
		/// </summary>
		MLString DictionaryName { get; }

		/// <summary>
		/// Локализованное название справочника.
		/// </summary>
		MLString DictionaryTitle { get; }

		/// <summary>
		/// Список ключевых полей словаря.
		/// </summary>
		string[] Keys { get; }

		/// <summary>
		/// Экспортирует информацию из справочника в таблицу.
		/// </summary>
		/// <param name="dictManager">Менеджер справочников.</param>
		/// <returns></returns>
		DataTable Export( IDictionaryManager dictManager );

		/// <summary>
		/// Импортирует информацию из таблицы в справочник.
		/// </summary>
		/// <param name="table">Таблица с данными.</param>
		/// <param name="context">Контекст импорта справочника.</param>
		/// <returns>Результат импорта.</returns>
		ImportResult Import( DataTable table, DictionaryImportContext context );

		/// <summary>
		/// Создаёт пустой элемент справочника.
		/// </summary>
		/// <returns>Элемент справочника.</returns>
		object CreateDictionaryItem();
	}

	public class DictionaryCollection : BaseBindingCollection<IDictionary>
	{
	}
}
