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

using UIPProcess.UIP;
using UIPProcess.UIP.Interceptors;
using UIPProcess.UIP.Navigators;

namespace UIPProcess.Controllers
{
    /// <summary>
    /// This class coordinates the user process.
    /// It represents the controller in a Model-View-Controller pattern.
    /// </summary>
    public abstract class ControllerBase
    {
        #region [ Fields ]

        private readonly Navigator _navigator;

        #endregion

        /// <summary>
        /// Creates a controller.
        /// </summary>
        /// <param name="navigator">Provides navigation services for the controller.</param>
        protected ControllerBase(Navigator navigator)
        {
            _navigator = navigator;
        }

        /// <summary>
        /// Gets the controller state.
        /// </summary>
        public State State
        {
            get { return _navigator.CurrentState; }
        }

        ///// <summary>
        ///// Allows for a default Next button that causes the manager to navigate to the next page.
        ///// </summary>
        //protected virtual void Navigate(Boolean forceNavigationInCurrentAppTransaction) 
        //{
        //    Navigate(_navigator.CurrentState.NavigateValue);
        //}

        /// <summary>
        /// Navigates to the specified view or node.
        /// </summary>
        /// <param name="navigateValue">Name of the node or view to navigate to.</param>
        protected virtual void Navigate(String navigateValue)
        {
            if (_navigator != null)
                _navigator.Navigate(navigateValue);
        }

        /// <summary>
        /// Ends or suspends the current task.
        /// </summary>
        protected virtual void SuspendTask()
        {
            StateCache.RemoveFromCache(State.TaskId);
            State.Clear();
        }

        /// <summary>
        /// The UIPManager calls this method when a new task starts.
        /// </summary>
        /// <param name="taskArguments">A holder for originating the navigation graph and task ID, and an object that
        /// the controller can use to get state information from the previous navigation graph.</param>
        public virtual void EnterTask(TaskArgumentsHolder taskArguments)
        { }

        /// <summary>
        /// The navigator for this task.
        /// </summary>
        public Navigator Navigator
        {
            get { return _navigator; }
        }

        /// <summary>
        /// Completes the task. Completing a task permanently removes any state data for the task.
        /// A completed task cannot be restarted.
        /// </summary>
        public virtual void CompleteTask()
        {
            StateCache.RemoveFromCache(State.TaskId);
            State.Delete();
        }

        /// <summary>
        /// Check if the corresponding property mapped to state.
        /// </summary>
        /// <param name="strPropertyName">The name of the property to check.</param>
        /// <returns><b>True</b> if the corresponding property is mapped to particular state
        /// key in UIP configuration,
        /// <b>false</b> otherwise.</returns>
        /// <remarks>The name of this function is used in <see cref="ControllerActionsInterceptor"/>, 
        /// change it only together with Intercept function of this class.</remarks>
        public virtual bool IsPropertyMappedInConfig(string strPropertyName)
        {
            throw new Exception(string.Format("ExceptionIncorrectInterceptor: {0} - {1}."
                                              , GetType(), "IsPropertyMapped"));
        }

        /// <summary>
        /// Check if the object related to the corresponding property exists in the state.
        /// </summary>
        /// <param name="strPropertyName">The name of the property to check.</param>
        /// <returns><b>True</b> if the corresponding entity exists in the state, 
        /// <b>false</b> otherwise.</returns>
        /// <remarks>The name of this function is used in <see cref="ControllerActionsInterceptor"/>, 
        /// change it only together with Intercept function of this class.</remarks>
        public virtual bool IsMappedEntityExistInState(string strPropertyName)
        {
            throw new Exception(string.Format("ExceptionIncorrectInterceptor: {0} - {1}."
                                              , GetType(), "IsMappedObjectExist"));
        }

        /// <summary>
        /// Map the property of the controller to particular domain object, using the key
        /// of this object in the State.
        /// </summary>
        /// <param name="strPropertyName">The name of the property to map.</param>
        /// <param name="strKeyName">The name of the key in the State to which the property should be mapped.</param>
        /// <remarks>The name of this function is used in <see cref="ControllerActionsInterceptor"/>, 
        /// change it only together with Intercept function of this class.</remarks>
        public virtual void MapPropertyToKey(String strPropertyName, String strKeyName)
        {
            throw new Exception(string.Format("ExceptionIncorrectInterceptor: {0} - {1}."
                                              , GetType(), "MapPropertyToKey"));
        }

        /// <summary>
        /// Event handler function for Load event of the corresponding view.
        /// </summary>
        /// <param name="sender">The control where Load event was occurred.</param>
        /// <param name="e">Event args.</param>
        public virtual void OnLoadView(Object sender, EventArgs e)
        { }

        /// <summary>
        /// Event handler function for PreRender event of the corresponding view.
        /// </summary>
        /// <param name="sender">The control where PreRender event was occurred.</param>
        /// <param name="e">Event args.</param>
        public virtual void OnPreRenderView(Object sender, EventArgs e)
        { }

        /// <summary>
        /// Event handler function for UnLoad event of the corresponding view.
        /// </summary>
        /// <param name="sender">The control where UnLoad event was occurred.</param>
        /// <param name="e">Event args.</param>
        public virtual void OnUnloadView(Object sender, EventArgs e)
        { }
    }
}