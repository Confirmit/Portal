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
using System.Diagnostics;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

using UIPProcess.Controllers;
using UIPProcess.UIP.Interceptors;
using UIPProcess.UIP.Navigators;

namespace UIProcess
{
    /// <summary>
    /// Factory for creating controllers.
    /// </summary>
    public sealed class ControllerFactory
    {
        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ControllerFactory()
        {
        }

        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        private ControllerFactory() { }
        #endregion

        /// <summary>
        /// Creates the controller for the view.
        /// </summary>
        /// <param name="view">The name of the view in the Views section of the configuration file.</param>
        /// <param name="navigator">The navigator that the controller uses for navigation services.</param>
        /// <returns></returns>
        public static ControllerBase Create(String view, Navigator navigator)
        {
            String viewName = getViewName(view);
            ControllerSettings typeSettings = UIPConfiguration.Config.GetControllerSettings(viewName);
            if (typeSettings == null)
                throw new Exception(string.Format("ExceptionControllerNotFound: {0}.", viewName));
            
            return createController(typeSettings, navigator);
        }

        public static ControllerBase CreateByName(String controllerName, Navigator navigator)
        {
            ControllerSettings typeSettings = 
                UIPConfiguration.Config.GetControllersSettingsFromName(controllerName);
            
            if (typeSettings == null)
                throw new Exception(string.Format("ExceptionControllerNotFound: {0}.", controllerName));

            return createController(typeSettings, navigator);
        }

        private static String getViewName(String view)
        {
            Int32 nQuerySign = view.IndexOf('?');
            String viewName = (nQuerySign == -1) ? view : view.Substring(0, nQuerySign);
            return viewName;
        }

        private static ControllerBase createController(ControllerSettings typeSettings, Navigator navigator)
        {
            IControllerActionsInterceptor interceptor = null;
            if (!String.IsNullOrEmpty(typeSettings.Interceptor))
            {
                ObjectTypeSettings typeInterceptor = (ObjectTypeSettings)UIPConfiguration.Config.InterceptorCollection[typeSettings.Interceptor];
                interceptor = (IControllerActionsInterceptor)
                    GenericFactory.Create(typeInterceptor);
            }
            else
            {
                interceptor = new ControllerActionsInterceptor();
            }

            interceptor.State = navigator.CurrentState;
            interceptor.MapPropsStateKeys = typeSettings.MapPropsKeys;

            Type typeController = GenericFactory.CreateTypeInstance(typeSettings);
            Debug.Assert(typeController != null);

            return (ControllerBase) proxyGenerator.CreateClassProxy(
                                                    typeController, new IInterceptor[] {interceptor},
                                                    new object[] {navigator});
        }
    }
}
