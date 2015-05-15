using System;
using System.Diagnostics;
using System.Reflection;

namespace UIPProcess.DataValidating
{
    /// <summary>
    /// Validator class provides a set of validation functions, based on reflection mechanism.
    /// </summary>
    public static class DataValidator
    {
        /// <summary>
        /// Do the validation of the corresponding control.
        /// </summary>
        /// <param name="ctlValidate">Control to validate.</param>
        /// <returns></returns>
        public static Boolean Validate(Object ctlValidate)
        {
            Type classDest = ctlValidate.GetType();
            MemberInfo[] members = classDest.GetMembers(
                BindingFlags.Public | BindingFlags.Instance);

            Boolean fValid = true;
            ClearValidators(ctlValidate);

            foreach (MemberInfo member in members)
            {
                MethodInfo methodValidate = member as MethodInfo;
                if (methodValidate == null)
                    continue;

                Object[] attr = methodValidate.GetCustomAttributes(typeof(DataValidatingAttribute), false);
                DataValidatingAttribute validating = (attr != null && attr.Length > 0) ?
                                                                                           (DataValidatingAttribute)attr[0] : null;
                if (validating != null && !validating.IsClearingMethod)
                {
                    Boolean fCurrentFieldValid = (Boolean)methodValidate.Invoke(ctlValidate, null);
                    fValid = fValid && fCurrentFieldValid;
                }
            }

            PropertyInfo[] properties = classDest.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                Object[] attr = property.GetCustomAttributes(typeof(DataValidatingRecursiveAttribute), false);
                DataValidatingRecursiveAttribute recursive = (attr != null && attr.Length > 0) ?
                                                                                                   (DataValidatingRecursiveAttribute)attr[0] : null;
                if (recursive != null)
                {
                    Boolean fCurrentControlValid = DataValidator.Validate(property.GetValue(ctlValidate, null));
                    fValid = fValid && fCurrentControlValid;
                }
            }

            return fValid;
        }

        /// <summary>
        /// Clear validators of the corresponding control.
        /// </summary>
        /// <param name="ctlValidate">Control to clear validators.</param>
        public static void ClearValidators(Object ctlValidate)
        {
            Type classDest = ctlValidate.GetType();
            MemberInfo[] members = classDest.GetMembers(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (MemberInfo member in members)
            {
                MethodInfo methodValidate = member as MethodInfo;
                if (methodValidate == null)
                    continue;

                Object[] attr = methodValidate.GetCustomAttributes(typeof(DataValidatingAttribute), false);
                if (attr == null || attr.Length == 0)
                    continue;

                DataValidatingAttribute attrValidating = attr[0] as DataValidatingAttribute;
                Debug.Assert(attrValidating != null);

                if (attrValidating.IsClearingMethod)
                    methodValidate.Invoke(ctlValidate, null);
            }

            PropertyInfo[] properties = classDest.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                Object[] attr = property.GetCustomAttributes(typeof(DataValidatingRecursiveAttribute), false);
                DataValidatingRecursiveAttribute recursive = (attr != null && attr.Length > 0) ?
                                                                                                   (DataValidatingRecursiveAttribute)attr[0] : null;
                if (recursive != null)
                {
                    DataValidator.ClearValidators(property.GetValue(ctlValidate, null));
                }
            }
        }
    }
}