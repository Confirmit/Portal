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

using UIPProcess.UIP.Navigators;

namespace UIPProcess.UIP
{
    /// <summary>
    /// This class allows the UIPManager to dispense controllers to views, 
    /// sense when controllers have finished, spawn new views,
    /// and coordinate tasks.
    /// </summary>
    public sealed class UIPManager
    {
        #region Variable Declarations

        /// <summary>
        /// Delegates for the NavigateEvent.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A NavigateEventArgs object that contains the event data.</param>
        public delegate void NavigateEventHandler(object sender, NavigateEventArgs e);

        /// <summary>
        /// Occurs when navigation occurs.
        /// </summary>
        public static event NavigateEventHandler NavigateEvent;

        #endregion

        #region Start Task overloads

        /// <summary>
        /// Starts a UIProcess for open navigation.
        /// </summary>
        public static void StartOpenNavigationTask(string name)
        {
            OpenNavigator navigator = new OpenNavigator(name);
            navigator.StartTask();
        }

        /// <summary>
        /// Starts a UIProcess for open navigation.
        /// </summary>
        /// <param name="name">The name of the navigationGraph element in app.config.</param>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        public static void StartOpenNavigationTask(string name, Guid taskId)
        {
            OpenNavigator navigator = new OpenNavigator(name);
            navigator.StartTask(taskId);
        }

        /// <summary>
        /// Starts a UIProcess for open navigation.
        /// </summary>
        /// <param name="name">The name of the navigationGraph element in app.config. </param>
        /// <param name="firstViewName">The first view to be activated.</param>
        public static void StartOpenNavigationTask(string name, string firstViewName)
        {
            OpenNavigator navigator = new OpenNavigator(name);
            navigator.StartTask(firstViewName);
        }

        /// <summary>
        /// Starts a UIProcess for open navigation.
        /// </summary>
        /// <param name="name">The name of the navigationGraph element in app.config. </param>
        /// <param name="firstViewName">The first view to be activated.</param>
        /// <param name="state_params">Initial State parameters</param>
        public static void StartOpenNavigationTask(string name, string firstViewName
                                                   , params KeyValuePair<String, Object>[] state_params)
        {
            OpenNavigator navigator = new OpenNavigator(name);
            navigator.StartTaskWithParams(firstViewName, state_params);
        }

        #endregion

        #region Navigation Code

        /// <summary>
        /// Called by all classes that wish to alert all listeners for NavigateEvent that navigation has occurred.
        /// </summary>
        /// <param name="state">The current state.</param>
        public static void InvokeEventHandlers(State state)
        {
            if(NavigateEvent != null)
            {
                NavigateEventArgs navigateArgs = new NavigateEventArgs(state);
                NavigateEvent(null, navigateArgs);
            }
        }

        #endregion
    }
}