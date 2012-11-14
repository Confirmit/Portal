using System;
using System.Collections.Generic;

namespace Core.Dictionaries
{
	public class DictionaryEqualityComparer : IEqualityComparer<IDictionary>
	{

		#region IEqualityComparer<IDictionary> Members

		public bool Equals( IDictionary x, IDictionary y )
		{
			return (x.DictionaryName == y.DictionaryName);
		}

		public int GetHashCode( IDictionary obj )
		{
			return obj.DictionaryName.GetHashCode();
		}

		#endregion
	}
}
