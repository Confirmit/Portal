using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    public abstract class BaseSettingCollection
    {
        public virtual void Save(SettingAttribute attribute, object value)
        { }

        public virtual ISetting Load(SettingAttribute attribute)
        {
            throw new Exception("Incorrect usage of Load method of settings.");
        }

        #region ToList methods

        public abstract IList<ISetting> ToList(SettingType type);

        protected virtual List<TSettingType> ToList<TCollection, TSettingType>(SettingType type)
            where TCollection : BaseSettingCollection
            where TSettingType : ISetting
        {
            List<TSettingType> setingsCollection = new List<TSettingType>();
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

            try
            {
                foreach (PropertyInfo property in typeof(TCollection).GetProperties(flags))
                {
                    object[] attr_obj = property.GetCustomAttributes(typeof(SettingAttribute), false);
                    if (attr_obj.Length > 0 && ((SettingAttribute)attr_obj[0]).SettingType == type)
                        setingsCollection.Add((TSettingType)Load((SettingAttribute)attr_obj[0]));
                }
            }
            catch
            {
                return new List<TSettingType>();
            }

            return setingsCollection;
        }

        #endregion
    }
}
