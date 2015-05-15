using Core;

namespace Migration.Utility.PersonNameType
{
    public class UserList
    {
        public static Person<T>[] GetUserList<T>()
        {
            BaseObjectCollection<Person<T>> coll = (BaseObjectCollection<Person<T>>)BasePlainObject.GetObjects(typeof(Person<T>));
            if (coll == null)
                return null;
            
            return coll.ToArray();
        }
    }
}
