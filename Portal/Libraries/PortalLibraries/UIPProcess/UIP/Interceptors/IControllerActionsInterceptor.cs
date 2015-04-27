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
using System.Collections.Generic;

using Castle.Core.Interceptor;
using UIProcess;

namespace UIPProcess.UIP.Interceptors
{
    /// <summary>
    /// Controller actions interceptor interface
    /// </summary>
    public interface IControllerActionsInterceptor : IInterceptor
    {
        /// <summary>
        /// State where business entities are stored during user's session.
        /// </summary>
        State State { set; }
        
        /// <summary>
        /// The configuration of mapping controller properties to State entities via State keys.
        /// </summary>
        IDictionary<String, StateKeySettings> MapPropsStateKeys { set; }
    }
}