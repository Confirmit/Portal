using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
	/// <summary>
	/// Интерфейс для пометки объектов, котрые могут сообщить о своей доступности пользователю.
	/// </summary>
	public interface IAccessible
	{
		/// <summary>
		/// Проверяет доступность объекта пользователю и изменяет состояние в зависимости от доступности.
		/// </summary>
		/// <param name="user">Пользователь.</param>
		/// <returns>Возвращает true, если данный контрол доступен пользователю, иначе возвращает false.</returns>
		bool CheckAccessibilityToUser( User user );
	}
}
