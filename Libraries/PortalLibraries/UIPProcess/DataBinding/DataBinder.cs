using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI.WebControls;
using UIPProcess.Reflection;
using UIPProcess.UIP.Views;

namespace UIPProcess.DataBinding
{
    /// <summary>
    /// Class for binding objects.
    /// </summary>
    public class DataBinder
    {
        #region DataBinding

        /* public static void BindObjectToControl(object controlDestination, object objSource)
        {
            if (controlDestination == null || objSource == null)
                return;

            foreach (PropertyInfo propInfo in controlDestination.GetType().GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                object[] attributes = propInfo.GetCustomAttributes(typeof(DataBindingAttribute), false);
                foreach (object attribute in attributes)
                {
                    DataBindingAttribute bindingAttr = attribute as DataBindingAttribute;
                    if (bindingAttr == null)
                        continue;

                    PropertyInfo propertyValue = objSource.GetType().GetProperty(bindingAttr.DataFieldName);
                    if (propertyValue == null)
                        continue;

                    object value = propertyValue.GetValue(objSource, null);
                    if (value == null)
                        continue;

                    propInfo.SetValue(controlDestination, value, null);
                }
            }
        }*/

        /*public static void BindControlToObject(object controlSource, object objDestination)
        {
            if (controlSource == null || objDestination == null)
                return;

            foreach (PropertyInfo propInfo in controlSource.GetType().GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                object[] attributes = propInfo.GetCustomAttributes(typeof(DataBindingAttribute), false);
                foreach (object attribute in attributes)
                {
                    DataBindingAttribute bindingAttr = attribute as DataBindingAttribute;
                    if (bindingAttr == null)
                        continue;

                    object value = propInfo.GetValue(controlSource, null);
                    if (value == null)
                        continue;

                    PropertyInfo propDest = objDestination.GetType().GetProperty(bindingAttr.DataFieldName);
                    if (propDest == null)
                        continue;

                    propDest.SetValue(objDestination, value, null);
                }
            }
        }*/

        #endregion

        #region Bind DropDownList

        /// <summary>
        /// Set the selected value of a drop down.
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="data"></param>
        public static void BindDropDownDataSource(DropDownList dropDownList, ICollection data)
        {
            try
            {
                dropDownList.DataSource = data;
                dropDownList.DataBind();
            }
            catch (ArgumentOutOfRangeException)
            {
                dropDownList.Items.Clear();
                dropDownList.SelectedValue = null;
                dropDownList.DataSource = data;
                dropDownList.DataBind();
            }
        }

        /// <summary>
        /// Set the selected value of a drop down.
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="value"></param>
        public static void SetDropDownListValue(DropDownList dropDownList, Int32 value)
        {
            try
            {
                dropDownList.SelectedValue = ((value > 0) ? value.ToString() : null);
            }
            catch (ArgumentOutOfRangeException)
            {
                dropDownList.SelectedValue = null;
            }
        }

        #endregion

        /// <summary>
        /// Bind data from UI Control to a ViewStorage - a dictionary &lt;property, value&gt;
        /// </summary>
        /// <param name="ctlSource"></param>
        /// <param name="dictDest"></param>
        public static void DataBindFromControlAttributedProps(Object ctlSource, IDictionary<String, Object> dictDest)
        {
            Type classSrc = ctlSource.GetType();
            PropertyInfo[] properties = classSrc.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propSrc in properties)
            {
                Object[] attrBinding = propSrc.GetCustomAttributes(typeof(DataBindingAttribute), false);
                Object[] attrRecursive = propSrc.GetCustomAttributes(typeof(DataBindingRecursiveAttribute), false);

                if ((attrBinding != null && attrBinding.Length > 0)
                    || (attrRecursive != null && attrRecursive.Length > 0))
                {
                    Object valueSrc = null;
                    try
                    {
                        valueSrc = propSrc.GetValue(ctlSource, null);
                    }
                    catch (FormatException)
                    {
                        throw new Exception(String.Format("DataBinding_IcorrectInputFormat - {0}.", propSrc.Name));
                    }

                    DataBindingAttribute binding = (attrBinding != null && attrBinding.Length > 0)
                                                       ? (DataBindingAttribute)attrBinding[0] : null;
                    if (binding != null)
                    {
                        Debug.Assert(dictDest.ContainsKey(binding.DataFieldName) == false);
                        dictDest.Add(binding.DataFieldName, valueSrc);
                    }
                    else
                    {
                        //Object prefix should be added here
                        DataBindFromControlAttributedProps(valueSrc, dictDest);
                    }
                }
            }
        }

        /// <summary>
        /// Bind data from ViewStorage - a dictionary &lt;property, value&gt; to UI Control.
        /// </summary>
        /// <param name="dictSrc"></param>
        /// <param name="ctlDest"></param>
        public static void DataBindToControlAttributedProps(
            IDictionary<String, Object> dictSrc, Object ctlDest)
        {
            if (dictSrc == null)
                return;

            Type classDest = ctlDest.GetType();
            PropertyInfo[] properties = classDest.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propDest in properties)
            {
                Object[] attrBinding = propDest.GetCustomAttributes(typeof(DataBindingAttribute), false);
                DataBindingAttribute binding = (attrBinding != null && attrBinding.Length > 0)
                                                   ? (DataBindingAttribute)attrBinding[0] : null;
                if (binding != null)
                {
                    if (dictSrc.ContainsKey(binding.DataFieldName))
                        propDest.SetValue(ctlDest, dictSrc[binding.DataFieldName], null);
                }

                Object[] attrRecursive = propDest.GetCustomAttributes(typeof(DataBindingRecursiveAttribute), false);
                DataBindingRecursiveAttribute recursive = (attrRecursive != null && attrRecursive.Length > 0)
                                                              ? (DataBindingRecursiveAttribute)attrRecursive[0] : null;
                if (recursive != null)
                {
                    //Object prefix should be processed here
                    DataBindToControlAttributedProps(dictSrc, propDest.GetValue(ctlDest, null));
                }
            }
        }

        /// <summary>
        /// Binding from Control (control from UI) to Entity (data object from Domain).
        /// </summary>
        /// <param name="ctlSource"></param>
        /// <param name="dataDest"></param>
        public static void DataBindFromControlAttributedProps(Object ctlSource, Object dataDest)
        {
            Type classSrc = ctlSource.GetType();
            PropertyInfo[] properties = classSrc.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propSrc in properties)
            {
                Object[] attrBinding = propSrc.GetCustomAttributes(typeof(DataBindingAttribute), false);
                DataBindingAttribute binding = (attrBinding != null && attrBinding.Length > 0)
                                                   ? (DataBindingAttribute)attrBinding[0] : null;

                Object[] attrRecursive = propSrc.GetCustomAttributes(typeof(DataBindingRecursiveAttribute), false);
                DataBindingRecursiveAttribute recursive = (attrRecursive != null && attrRecursive.Length > 0)
                                                              ? (DataBindingRecursiveAttribute)attrRecursive[0] : null;

                if (binding != null || recursive != null)
                {
                    Object valueSrc = null;
                    try
                    {
                        valueSrc = propSrc.GetValue(ctlSource, null);
                    }
                    catch (FormatException)
                    {
                        throw new Exception(String.Format("DataBinding_IcorrectInputFormat - {0}.", propSrc.Name));
                    }

                    if (binding != null)
                    {
                        PropertyInfo propDest = (PropertyInfo)MemberFindHelper.FindPropertyByName(
                                                                  dataDest, binding.DataFieldName);

                        // Sometime it's required to bind only the part of the properties from the
                        // whole form to 'proxy' object.
                        if (propDest != null)
                        {
                            try
                            {
                                setDestObjectPropName(dataDest, propDest
                                                      , binding.IsObjectProperty
                                                      , (binding.ObjectPropType ?? propDest.PropertyType)
                                                      , valueSrc);
                            }
                            catch (TargetInvocationException ex)
                            {
                                throw new Exception(String.Empty, ex.InnerException);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(recursive.DataFieldName))
                        {
                            PropertyInfo propDest = (PropertyInfo)MemberFindHelper.FindPropertyByName(
                                                                      dataDest, recursive.DataFieldName);

                            // при проверке LifeTimeEntity происходит частичный байндинг в прокси объект, 
                            // поэтому часть полей не байндится (метод processLifeTimeErrorIfAny) 
                            if (propDest == null)
                                continue;
                            //throw new DataBindingException(String.Format(
                            //    CommonCoreExceptions.DataBindingRecursive_UnableToFindDestinationField
                            //        , recursive.DataFieldName));

                            Object destMember = propDest.GetValue(dataDest, null);
                            if (destMember == null)
                                throw new Exception(String.Format("DataBindingRecursive_NullDestinationField - {0}", recursive.DataFieldName));

                            DataBindFromControlAttributedProps(valueSrc, destMember);
                        }
                        else
                            DataBindFromControlAttributedProps(valueSrc, dataDest);
                    }
                }
            }
        }

        /// <summary>
        /// Binding from Entity (data object from Domain) to Control (control from UI).
        /// </summary>
        /// <param name="dataSrc">Source entity (data object).</param>
        /// <param name="ctlDest">Destination control (UI control).</param>
        public static void DataBindToControlAttributedProps(Object dataSrc, Object ctlDest)
        {
            Type classDest = ctlDest.GetType();
            PropertyInfo[] properties = classDest.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propDest in properties)
            {
                Object[] attrBinding = propDest.GetCustomAttributes(typeof(DataBindingAttribute), false);
                DataBindingAttribute binding = (attrBinding != null && attrBinding.Length > 0)
                                                   ? (DataBindingAttribute)attrBinding[0] : null;

                if (binding != null)
                {
                    if (dataSrc == null)
                    {
                        if (binding.BindOnlyForNotNullEntity == false)
                            propDest.SetValue(ctlDest, binding.DefaultValue, null);
                    }
                    else
                    {
                        PropertyInfo propSrc = MemberFindHelper.FindPropertyByName(
                                                   dataSrc, binding.DataFieldName) as PropertyInfo;

                        // Check the existence of the source property to process
                        // User Password control
                        if (propSrc != null)
                        {
                            Object valueSrc = propSrc.GetValue(dataSrc, null);
                            //if (binding.IsObjectProperty)
                            //    valueSrc = (valueSrc == null) ? 0 : ((IPersistentEntity)valueSrc).Id;

                            try
                            {
                                propDest.SetValue(ctlDest, valueSrc, null);
                            }
                            catch (ArgumentException e)
                            {
                                throw new Exception(String.Format("DataBinding_ErrorDestTypeConversion - {0}.", binding.DataFieldName) + e.Message);
                            }
                        }
                    }
                }

                Object[] attrRecursive = propDest.GetCustomAttributes(typeof(DataBindingRecursiveAttribute), false);
                DataBindingRecursiveAttribute recursive = (attrRecursive != null && attrRecursive.Length > 0)
                                                              ? (DataBindingRecursiveAttribute)attrRecursive[0] : null;
                if (recursive != null)
                {
                    if (!String.IsNullOrEmpty(recursive.DataFieldName) && dataSrc != null)
                    {
                        PropertyInfo propSrc = MemberFindHelper.FindPropertyByName(
                                                   dataSrc, recursive.DataFieldName) as PropertyInfo;
                        Debug.Assert(propSrc != null);

                        Object valueSrc = propSrc.GetValue(dataSrc, null);
                        DataBindToControlAttributedProps(valueSrc, propDest.GetValue(ctlDest, null));
                    }
                    else
                        DataBindToControlAttributedProps(dataSrc, propDest.GetValue(ctlDest, null));
                }
            }
        }

        private static void setDestObjectPropName(
            Object objDest, PropertyInfo propDest,
            Boolean isObjectProperty, Object valueSrc)
        {
            if (propDest == null)
                return;

            setDestObjectPropName(
                objDest, propDest
                , isObjectProperty, propDest.PropertyType
                , valueSrc);
        }

        private static void setDestObjectPropName(Object objDest, PropertyInfo propDest
                                                    , Boolean isObjectProperty, Type propertyType
                                                    , Object valueSrc)
        {
            if (propDest == null)
                return;

            /*if (isObjectProperty)
            {
                Int32 id = (Int32)valueSrc;

                if (id > 0)
                {
                    // Fix the error here when somebody delete the Parameter in parallel.
                    EntitySerializer serializer = new EntitySerializer();
                    valueSrc = serializer.LoadObject(propertyType, manager, id);
                }
                else
                {
                    valueSrc = null;
                }
            }*/

            //Debug.Assert(propDest.CanWrite == true);
            if (propDest.CanWrite)
                propDest.SetValue(objDest, valueSrc, null);
        }

        #region Deprecated functions

        public static void DataBindControl(IWebControl ctlSource, Object objDestination)
        {
            PropertyInfo[] pi = InterfaceHelper.GetProperties(ctlSource.GetType(), ctlSource.GetType());

            foreach (PropertyInfo propSrc in pi)
            {
                if (!propSrc.CanRead)
                    continue;

                Object valueSrc = propSrc.GetValue(ctlSource, null);

                Boolean isObjectProperty = false;
                String strDestPropName = getDestObjectPropName(propSrc.Name, propSrc.PropertyType, out isObjectProperty);
                PropertyInfo propDest = (PropertyInfo)MemberFindHelper.FindPropertyByName(objDestination, strDestPropName);
                setDestObjectPropName(objDestination, propDest, isObjectProperty, valueSrc);
            }
        }

        private static String getDestObjectPropName(String strSrcPropName, Type typSrcProp, out Boolean isObjectProperty)
        {
            String strDestPropName = strSrcPropName;

            String strIdSuffix = strDestPropName.Substring(strDestPropName.Length - 2);
            isObjectProperty = false;

            if (strIdSuffix.ToLower().Equals("id")
                && (typSrcProp.Equals(typeof(Int32)) || typSrcProp.Equals(typeof(Int64))))
            {
                strDestPropName = strDestPropName.Substring(0, strDestPropName.Length - 2);
                isObjectProperty = true;
            }

            return strDestPropName;
        }

        #endregion
    }
}