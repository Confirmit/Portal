using Core;

namespace Migration.UserTableMigration
{
    public class UserList
    {
        public static Person<T>[] GetUserList<T>()
        {
            BaseObjectCollection<Person<T>> coll = (BaseObjectCollection<Person<T>>)BasePlainObject.GetObjects(typeof(Person<T>));
            if (coll == null)
                return null;
            else
                return coll.ToArray();
        }
    }
}
