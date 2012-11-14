using System;

namespace Core.Dictionaries
{
	/// <summary>
	/// Интерфейс для базовых объектов(BaseObject), умеющим удалять и восстанавливать себя в списке
	/// </summary>
	public interface IRestorableDictionaryItem
	{
		/// <summary>
		/// Удален ли объект
		/// </summary>
		bool IsRemoved { get; set;  }

		/// <summary>
		/// Восстанавливает объект: устанавливает флаг IsRemoved = false.
		/// </summary>
		/// <returns></returns>
		void Restore();

		/// <summary>
		/// Закрывает объект: устанавливает флаг IsRemoved = true;
		/// </summary>
		/// <returns></returns>
		void Close();
	}
}
