using System;

namespace Core.ORM.Attributes
{
    /// <summary>
    /// Атрибут указывает, что данное поле может только читаться из базы.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DBReadOnlyAttribute : Attribute
    {
        public DBReadOnlyAttribute()
        {}
    }
}
