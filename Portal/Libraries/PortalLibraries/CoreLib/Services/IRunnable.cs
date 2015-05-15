using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
	/// <summary>
	/// Интерфейс, определяющий объекты, которые можно "выполнить".
	/// </summary>
	public interface IRunnable
	{
		void Run();
	}
}
