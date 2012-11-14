using System;
using System.Collections;

namespace AspNetForums.Components {
    /// <summary>
    /// Summary description for UserCollection.
    /// </summary>
    public class UserCollection : ArrayList {
	
        // default constructor
        public UserCollection() : base() {}
        public UserCollection(ICollection c) : base(c) {}
    }
}
