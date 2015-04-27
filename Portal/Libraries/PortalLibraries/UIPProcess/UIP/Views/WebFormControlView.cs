//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// WebFormControlView.cs
//
// This file contains the implementations of the WebFormControlView class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;

using UIProcess;
using UIPProcess.Controllers;
using UIPProcess.UIP.Navigators;

namespace UIPProcess.UIP.Views
{
    /// <summary>
    /// Represents a composite user control with Controller used in Web applications.
    /// </summary>
    public class WebFormControlView : WebFormControl, IView
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            initController();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            deinitController();
        }

        public override string ViewName
        {
            set
            {
                if (!String.IsNullOrEmpty(value) && !base.ViewName.Equals(value))
                {
                    base.ViewName = value;

                    deinitController();
                    initController();
                }
            }
        }

        private void initController()
        {
            if (_navigator == null)
                return;

            _controller = _navigator.GetController(this, string.Empty);

            if (_controller == null)
                return;

            Load += _controller.OnLoadView;
            PreRender += _controller.OnPreRenderView;
            Unload += _controller.OnUnloadView;
        }

        private void deinitController()
        {
            if (_controller != null)
            {
                Load -= _controller.OnLoadView;
                PreRender -= _controller.OnPreRenderView;
                Unload -= _controller.OnUnloadView;
            }
        }

        #region IView implementation

        /// <summary>
        /// Gets the view controller.
        /// </summary>
        public ControllerBase Controller
        {
            get { return _controller; }
        }
        private ControllerBase _controller;

        public String ViewType
        {
            get { return URLManager.CutLeftPart(Request.CurrentExecutionFilePath); }
        }

        /// <summary>
        /// Gets the navigator used by this WebFormView.
        /// </summary>
        public Navigator Navigator
        {
            get { return _navigator; }
        }

        /// <summary>
        /// No implementation for this in WebFormViews.
        /// </summary>
        /// <param name="enabled"></param>
        public void Enable(bool enabled)
        {}

        /// <summary>
        /// Initializes the WebFormView.
        /// </summary>
        /// <param name="args">The initialization arguments.</param>
        /// <param name="settings">The settings for the view.</param>
        public virtual void Initialize(TaskArgumentsHolder args, ViewSettings settings)
        {}

        #endregion
    }
}