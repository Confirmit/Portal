//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// ControllerFactory.cs
//
// This file contains the implementations of the ControllerFactory class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;
using System.Reflection;
using Castle.Core.Interceptor;

using System.Collections.Generic;
using UIProcess;

namespace UIPProcess.UIP.Interceptors
{
    /// <summary>
    /// The base interceptor class for Controllers' actions.
    /// </summary>
    public class ControllerActionsInterceptor : IControllerActionsInterceptor
    {
        /// <summary>
        /// Intercept method.
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo info = invocation.Method;

            if (info.Name.Equals("IsPropertyMappedInConfig"))
            {
                invocation.ReturnValue = process_IsPropertyMappedInConfig(invocation.Arguments[0] as String);
                return;
            }

            if (info.Name.Equals("IsMappedEntityExistInState"))
            {
                invocation.ReturnValue = process_IsMappedEntityExistInState(invocation, invocation.Arguments[0] as String);
                return;
            }

            if (info.Name.Equals("MapPropertyToKey"))
            {
                String strPropertyName = invocation.Arguments[0] as String;
                String strKeyName = invocation.Arguments[1] as String;

                if (String.IsNullOrEmpty(strPropertyName)
                    || String.IsNullOrEmpty(strKeyName))
                    throw new ArgumentException();
            
                StateKeySettings keySettings = UIPConfiguration.Config.GetStateKeySettingsFromName(strKeyName);
                if (keySettings == null)
                    throw new Exception(string.Format(
                                            "ExceptionControllerStateMappingNullKey: {0} - {1}."
                                            , invocation.InvocationTarget.GetType()
                                            , strPropertyName)
                        );

                _mapPropsKeys[strPropertyName] = keySettings;
                invocation.ReturnValue = true;
                
                return;
            }

            if (info.IsSpecialName && _mapPropsKeys != null && _mapPropsKeys.Count > 0)
            {
                String set_prefix = "set_";
                Boolean set_data =
                    (String.Compare(info.Name, 0, set_prefix, 0, set_prefix.Length) == 0) ?
                                                                                              true : false;
                if (set_data)
                {
                    String strPropertyName = getPropertyName(info.Name, set_prefix);
                    StateKeySettings keySettings = null;

                    if (_mapPropsKeys.TryGetValue(strPropertyName, out keySettings))
                    {
                        if (keySettings == null)
                            throw new Exception(string.Format(
                                                    "ExceptionControllerStateMappingNullKey: {0} - {1}."
                                                    , invocation.InvocationTarget.GetType()
                                                    , strPropertyName)
                                );


                        invocation.ReturnValue = (_state[keySettings.Name] = invocation.Arguments[0]);
                        return;
                    }
                }

                String get_prefix = "get_";
                Boolean get_data =
                    (String.Compare(info.Name, 0, get_prefix, 0, get_prefix.Length) == 0) ?
                                                                                              true : false;
                if (get_data)
                {
                    String strPropertyName = getPropertyName(info.Name, get_prefix);
                    StateKeySettings keySettings = null;

                    if (_mapPropsKeys.TryGetValue(strPropertyName, out keySettings))
                    {
                        invocation.ReturnValue = (_state.Contains(keySettings.Name) ? _state[keySettings.Name] : null);
                        return;
                    }
                }
            }

            invocation.Proceed();
        }

        private Object process_IsPropertyMappedInConfig(String strKey)
        {
            if (_mapPropsKeys == null || _mapPropsKeys.Count < 0)
                return false;

            if (String.IsNullOrEmpty(strKey))
                throw new ArgumentException();

            return _mapPropsKeys.ContainsKey(strKey);
        }

        private Object process_IsMappedEntityExistInState(IInvocation invocation, String strKey)
        {
            if (String.IsNullOrEmpty(strKey))
                throw new ArgumentException();

            if (_mapPropsKeys == null || _mapPropsKeys.Count < 0)
                throw new Exception(string.Format(
                                        "ExceptionControllerStateMappingIncorrect: {0} - {1}."
                                        , invocation.InvocationTarget.GetType()
                                        , strKey)
                    );

            StateKeySettings keySettings = null;
            if (!_mapPropsKeys.TryGetValue(strKey, out keySettings))
                throw new Exception(string.Format(
                                        "ExceptionControllerStateMappingIncorrect: {0} - {1}."
                                        , invocation.InvocationTarget.GetType()
                                        , strKey)
                    );

            return _state.Contains(keySettings.Name);
        }

        private String getPropertyName(String strMethodName, String strPrefix)
        {
            return strMethodName.Substring(strPrefix.Length);
        }

        public virtual State State
        {
            set { _state = value; }
        }
        protected State _state = null;

        public virtual IDictionary<String, StateKeySettings> MapPropsStateKeys 
        {
            set { _mapPropsKeys = value; }
        }
        private IDictionary<String, StateKeySettings> _mapPropsKeys = null;
    }
}