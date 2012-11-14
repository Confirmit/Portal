using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Core.Exceptions;

namespace Core
{
	/// <summary>
	/// Сравнивает объекты по свойству с заданным названием.
	/// </summary>
	public class CommonComparer<ObjectType> : IComparer<ObjectType>
	{
		private string[] m_props;
		private     bool m_isOrderASC;

		/// <summary>
		/// Создает объект по названию свойства и порядку сортировки
		/// </summary>
		/// <param name="property_name">Название свойства, по которому проводится сортировка</param>
		/// <param name="is_order_asc">Порядок сортировки</param>
		public CommonComparer( string property_name, bool is_order_asc )
		{
			m_props = property_name.Split('.');
			m_isOrderASC = is_order_asc;
		}

		#region IComparer<ObjectType> Members

		public int Compare( ObjectType x, ObjectType y )
		{
			object val1 = GetPropertyValue( x );
			object val2 = GetPropertyValue( y );
			int result = 0;
			if( val1 is int )
			{
				if( (int)val1 != (int)val2 )
				{
					if( (int)val1 > (int)val2 )
						result = 1;
					else
						result = -1;
				}
			}
			else if( val1 is double )
			{
				if( (double)val1 != (double)val2 )
				{
					if( (double)val1 > (double)val2 )
						result = 1;
					else
						result = -1;
				}
			}
            else if( val1 is DateTime )
            {
                if( (DateTime) val1 != (DateTime) val2 )
                {
                    if( (DateTime) val1 > (DateTime) val2 )
                        result = 1;
                    else
                        result = -1;
                }
            }
            else
            {
                result = string.Compare( val1.ToString(), (string) val2.ToString() );
            }
			return m_isOrderASC ? result : -result;
		}

		#endregion

		private object GetPropertyValue( object obj )
		{
			PropertyDescriptor prop = null;
			
            for( int i = 0; i < m_props.Length; i++ )
			{
				prop = TypeDescriptor.GetProperties( obj ).Find( m_props[i], true );
                
                if (prop == null)
                    throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("PropertyNotFoundException", m_props[i]));
				
                obj = prop.GetValue( obj );

                if (obj == null)
                    throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("PropertyNullValueException", m_props[i]));
			}

			return obj;
		}
	}
}
