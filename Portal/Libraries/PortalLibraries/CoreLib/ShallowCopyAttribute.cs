using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// Атрибут, говорящий о том, что поле нужно клонировать только по ссылке (поверхностное клонирование).
	/// </summary>
	[AttributeUsage( AttributeTargets.Field, AllowMultiple = false, Inherited = false )]
	public class ShallowCopyAttribute : System.Attribute
	{
		public ShallowCopyAttribute(  )
		{
		}
	}
}
