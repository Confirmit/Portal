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

namespace UIPProcess.UIP
{
    /// <summary>
    /// Arguments passed when the NavigateEvent on <code>UIPManager</code> occurs.
    /// </summary>
    public class NavigateEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a NavigateEventArgs object that contains the state.
        /// </summary>
        /// <param name="state">State of the task.</param>
        public NavigateEventArgs(State state)
        {
            _state = state;
        }

        /// <summary>
        /// The state of the task when the navigate event occurred.
        /// </summary>
        public State State 
        {
            get { return _state; }
        }
        private State _state = null;
    }
}