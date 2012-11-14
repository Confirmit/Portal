using System;
using System.Text;
using System.Reflection;

using Core.Exceptions;

namespace Core.Types
{
	/// <summary>
	/// Тип, представляющий из себя массив значений.
	/// </summary>
	public class ValueArray
	{
		private readonly object[] m_values;

		#region Constructors

		protected ValueArray()
		{
		}

		/// <summary>
		/// Создаёт объект класса из массива объектов.
		/// </summary>
		/// <param name="values"></param>
		public ValueArray( object[] values )
		{
			if(values == null)
			{
				throw new CoreArgumentNullException( "values" );
			}

			m_values = values;
		}

		/// <summary>
		/// Создаёт объект класса из набора свойств объекта.
		/// </summary>
		/// <param name="properties">Свойства объекта.</param>
		/// <param name="obj">Объект.</param>
		public ValueArray( PropertyInfo[] properties, object obj )
		{
			if(properties == null)
			{
				throw new CoreArgumentNullException( "properties" );
			}

			if(obj == null)
			{
				throw new CoreArgumentNullException( "obj" );
			}

			m_values = new object[properties.Length];
			for(int i = 0; i < properties.Length; i++)
			{
				m_values[i] = properties[i].GetValue( obj, null );
			}
		}

		#endregion

		#region

		public object[] Values
		{
			get
			{
				return m_values;
			}
		}

		/// <summary>
		/// Свойство показывает, это массив значений null или нет.
		/// </summary>
		public bool IsNullArray
		{
			get
			{
				bool result = true;

				foreach(object obj in m_values)
				{
					if(obj != null)
					{
						result = false;
						break;
					}
				}

				return result;
			}
		}

		#endregion

		#region Methods

		public override bool Equals( object obj )
		{
			bool result = false;

			if(obj is Array)
			{
				object[] objs = (object[])obj;
				if(objs.Length == m_values.Length)
				{
					bool tmp = true;

					for(int i = 0; i < m_values.Length; i++)
					{
						if(m_values.GetType() != objs[i].GetType())
						{
							tmp = false;
							break;
						}

						if(m_values[i].GetHashCode() != objs[i].GetHashCode())
						{
							tmp = false;
							break;
						}
					}

					result = tmp;
				}
			}

			return result;
		}

		public override int GetHashCode()
		{
			int result;

			if(m_values.Length == 0)
			{
				result = 0;
			}
			else
			{
				result = m_values[0].GetHashCode();

				for(int i = 1; i < m_values.Length; i++)
				{
					result ^= m_values[i].GetHashCode();
				}
			}

			return result;
		}

		public override string ToString()
		{
			StringBuilder tmp = new StringBuilder();
			tmp.Append( '{' );
			foreach(object key in m_values)
			{
				if(tmp.Length > 1)
				{
					tmp.Append( ',' );
				}
				tmp.AppendFormat( "\'{0}\'", key.ToString() );
			}
			tmp.Append( '}' );

			return tmp.ToString();
		}

		#endregion
	}
}
