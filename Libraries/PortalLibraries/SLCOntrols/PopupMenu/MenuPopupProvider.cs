using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SilverlightMenu
{
    public enum Direction
    {
        Left,
        Top,
        Right,
        Bottom
    }

	public class MenuPopupProvider : IDisposable
	{
		#region Constructors

		/// <summary>
		/// http://www.codeproject.com/KB/silverlight/Silverlight_Popup_Logic.aspx
		/// 
		/// Encapsulates logic for popup controls so the code does not need to be rewritten.
		/// </summary>
		/// <param name="owner">
		/// The owner is the FrameworkElement that will trigger the popup to close it the
		/// mouse leaves its screan area.  The popup will only remain open after leaving the
		/// owner if the popupChild element is supplied in this constructor and the mouse 
		/// immediately enters the screen area of the popupChild element after leaving the owner.
		/// </param>
		/// <param name="trigger">
		/// The trigger is the framework element that triggers the popup panel to open.
		/// The popup will open on the MouseLeftButtonUp routed event of the trigger.
		/// </param>
		/// <param name="popup">
		/// The popup is the Popup primitive control that contains the content to be displayed.
		/// </param>
		/// <param name="popupChild">
		/// The popupChild is the child control of the popup panel.  The Popup control does not 
		/// raise MouseEnter and MouseLeave events so the child control must be used to detect
		/// if the popup should remain open or closed in conjuction with the owner element.
		/// This value may be left null to create situations where only the owner element
		/// controls whether the popup closes or not.  e.g. an image could trigger a popup that
		/// describes the image and the popup closes when the mouse leaves the image regardless
		/// of whether the mouse enters the description. 
		/// </param>
		/// <param name="placement">
		/// Determines which side of the owner element the popup will appear on.
		/// </param>
		public MenuPopupProvider(FrameworkElement owner, FrameworkElement trigger, Popup popup, FrameworkElement popupChild, Direction placement)
		{
			if (owner == null) throw new ArgumentNullException("owner");
			if (trigger == null) throw new ArgumentNullException("trigger");
			if (popup == null) throw new ArgumentNullException("popup");

			m_owner = owner;
			m_placement = placement;
			m_popup = popup;
			m_popupChild = popupChild;
			m_trigger = trigger;

			m_owner.MouseEnter += _owner_MouseEnter;
			m_owner.MouseLeave += _owner_MouseLeave;
			
            if (m_popupChild != null)
			{
				m_popupChild.MouseEnter += _popupChild_MouseEnter;
				m_popupChild.MouseLeave += _popupChild_MouseLeave;
			}

			//small fix cause buttons do not use MouseLeftButtonUp or MouseLeftButtonDown anymore
			if (m_trigger is ButtonBase)
				((ButtonBase)m_trigger).Click += _trigger_Click;
			else
				m_trigger.MouseLeftButtonUp += _trigger_MouseLeftButtonUp;
			//if the trigger is a menu item we probably want to open the menu when the mouse if over the menu item
			if (m_trigger.GetType() == typeof(MenuItem))
			{
				m_trigger.MouseEnter += _trigger_MouseEnter;
				//_trigger.MouseMove += new MouseEventHandler(_trigger_MouseMove);
			}

			m_closeTimer = new DispatcherTimer();
			m_closeTimer.Interval = new TimeSpan(0, 0, 0, 0, CloseDelay);
			m_closeTimer.Tick += _closeTimer_Tick;
		}

		#endregion

        #region Private Fields

        private const int CloseDelay = 20;
        private DispatcherTimer m_closeTimer;

        private bool m_isPopupOpen;
        private bool m_isPopupClosing;

        private FrameworkElement m_owner;
        private Direction m_placement;
        private Popup m_popup;
        private FrameworkElement m_popupChild;
        private FrameworkElement m_trigger;

        /// <summary>
        /// we cannot close this menu popup is the mouse is over it usually
        /// so we provide a method to force the popup closing
        /// by setting thie member to false..it's useful when we the menu that
        /// use this popup ask to the popupcobntroller to close the menu after a click
        /// </summary>
        private bool _isMouseOverPopupChild;

        public bool IsPopupOpen
        {
            get { return m_isPopupOpen; }
        }

        #endregion

		#region IDisposable Members

		/// <summary>
		/// remove any event handler added
		/// </summary>
		public void Dispose()
		{
			m_owner.MouseEnter -= _owner_MouseEnter;
			m_owner.MouseLeave -= _owner_MouseLeave;
			
            if (m_popupChild != null)
			{
				m_popupChild.MouseEnter -= _popupChild_MouseEnter;
				m_popupChild.MouseLeave -= _popupChild_MouseLeave;
			}
			//small fix cause buttons do not use MouseLeftButtonUp or MouseLeftButtonDown anymore
			if (m_trigger is ButtonBase)
				((ButtonBase)m_trigger).Click -= _trigger_Click;
			else
				m_trigger.MouseLeftButtonUp -= _trigger_MouseLeftButtonUp;

			//if the trigger is a menu item we probably want to open the menu when the mouse if over the menu item
			if (m_trigger.GetType() == typeof(MenuItem))
			{
				m_trigger.MouseEnter -= _trigger_MouseEnter;
				//_trigger.MouseMove -= new MouseEventHandler(_trigger_MouseMove);
			}
		}

		#endregion

		#region Event Handlers

		void _closeTimer_Tick(object sender, EventArgs e)
		{
			//DebugMessage("_closeTimer_Tick");
			ClosePopup();
		}

		void _owner_MouseEnter(object sender, MouseEventArgs e)
		{
			//DebugMessage("_owner_MouseEnter");
			StopClosingPopup();
		}

		void _owner_MouseLeave(object sender, MouseEventArgs e)
		{
			//DebugMessage("_owner_MouseLeave");
			BeginClosingPopup();
		}

		void _popupChild_MouseEnter(object sender, MouseEventArgs e)
		{
			_isMouseOverPopupChild = true;
			//DebugMessage("_popupLayout_MouseEnter");
			StopClosingPopup();
		}

		void _popupChild_MouseLeave(object sender, MouseEventArgs e)
		{
			_isMouseOverPopupChild = false;
			//DebugMessage("_popupLayout_MouseLeave");
			BeginClosingPopup();
		}

		void _trigger_Click(object sender, RoutedEventArgs e)
		{
			ShowPopup();
		}

		void _trigger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ShowPopup();
		}

		void _trigger_MouseEnter(object sender, MouseEventArgs e)
		{
			//now it seems to work even on mouseenter in the trigger
			if (!m_isPopupOpen)
				ShowPopup();
		}

		public void ShowPopup()
		{
			if (!m_isPopupOpen)
			{
                GeneralTransform gt = m_owner.TransformToVisual(Application.Current.RootVisual as UIElement);
                Point offset = gt.Transform(new Point(0, 0));
				Point p;
				switch (m_placement)
				{
					case Direction.Left:
                        p = new Point(offset.X + m_popup.ActualWidth, offset.Y);
						break;
					case Direction.Top:
                        p = new Point(offset.X, offset .Y - m_popup.ActualHeight);
						break;
					case Direction.Bottom:
                        p = new Point(offset.X, offset.Y + m_owner.ActualHeight);
						break;
					case Direction.Right:
                        p = new Point(offset.X + m_owner.ActualWidth, offset.Y);
						break;
					default:
						throw new InvalidOperationException("Placement of popup not defined.");
				}

				m_popup.VerticalOffset = p.Y;
				m_popup.HorizontalOffset = p.X;
				m_isPopupOpen = m_popup.IsOpen = true;
			}
			else
			{
				BeginClosingPopup();
			}
		}

		/*private void DebugMessage(string methodName)
		{
			System.Diagnostics.Debug.WriteLine("{5} {0}: _isPopupOpen({1}) _isPopupClosing({2}) _popup.IsOpen({3}) _closeTimer.IsEnabled({4})", methodName, _isPopupOpen, _isPopupClosing, _popup.IsOpen, _closeTimer.IsEnabled, ((MenuItem)_owner).Text);
		}*/

		#endregion

		#region Private Methods

		public void ForceBeginClosingPopup()
		{
			_isMouseOverPopupChild = false;
			BeginClosingPopup();
		}

		public void BeginClosingPopup()
		{
			if (m_isPopupOpen && !m_isPopupClosing && !_isMouseOverPopupChild)
			{
				m_isPopupClosing = true;
				m_closeTimer.Start();
			}
		}

		private void ClosePopup()
		{
			if (m_isPopupOpen && m_isPopupClosing)
			{
				m_closeTimer.Stop();
				//the popup logically belongs to the owner, so we have to check in one of his children have a popup opened. 
				if (m_owner is MenuItem)
				{
					MenuItem mi = m_owner as MenuItem;
					if (mi.HasSubItems)
						foreach (MenuItem m in mi.MenuItems)
							if (m.IsSubMenuOpen)
							{
								//if so we cancel the closing request
								m_isPopupClosing = false;
								return;
							}
				}

				m_isPopupOpen = m_isPopupClosing = m_popup.IsOpen = false;

				//if this is a menuitem and his parent is not null, we have to ask it to close its menu too,
				//however we have to close the parent menu only if the mouse is not over the menu itself
				if (m_owner.GetType() == typeof(MenuItem))
				{
					MenuItem parent = (m_owner as MenuItem).ParentMenuItem;
					if (parent != null)
						parent.CloseMenuPopup();
				}
			}
		}

		private void StopClosingPopup()
		{
			if (m_isPopupOpen && m_isPopupClosing)
			{
				m_closeTimer.Stop();
				m_isPopupClosing = false;
			}
		}

		#endregion
	}
}
