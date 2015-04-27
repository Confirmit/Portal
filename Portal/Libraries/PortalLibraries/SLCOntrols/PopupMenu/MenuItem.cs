using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SilverlightMenu
{
	[TemplateVisualState(GroupName = "CommonStates", Name = "Normal"),
	 TemplateVisualState(GroupName = "CommonStates", Name = "MouseOver"),
	 TemplateVisualState(GroupName = "CommonStates", Name = "Pressed"),
	 TemplateVisualState(GroupName = "CommonStates", Name = "Disabled"),
	 TemplatePart(Name = MenuItem.MenuItemLayoutElement, Type = typeof(Grid)),
	 TemplatePart(Name = MenuItem.MenuItemsArrowElement, Type = typeof(Path))]
	public class MenuItem : Button
    {
        #region Constructors

        public MenuItem() : base()
		{
			DefaultStyleKey = typeof(MenuItem);
		}

		public MenuItem(string text) : this()
		{
			Text = text;
        }

        #endregion

        #region Fileds

        private const string MenuItemLayoutElement = "MenuItemLayout";
		private const string MenuItemsArrowElement = "MenuItemsArrow";
        private const string MenuItemImageElement = "MenuItemImage";

        private Popup m_popup;
        private MenuPanel m_panel;
        private MenuPopupProvider m_menuPopupProvider = null;

        private Grid LayoutGrid;
        private Shape MenuItemsArrow;
        private Image MenuItemImage;
        protected Direction MenuDirection = Direction.Right;
	    private ImageSource m_ImageSource = null;

        private bool m_isEnabled = true;

        #endregion

        public override void OnApplyTemplate()
		{
			//OverrideDefaultStyle();

			base.OnApplyTemplate();

			LayoutGrid = GetTemplateChild(MenuItemLayoutElement) as Grid;
            MenuItemImage = GetTemplateChild(MenuItemImageElement) as Image;
			MenuItemsArrow = GetTemplateChild(MenuItemsArrowElement) as Shape;

            if (MenuItemImage != null)
            {
                MenuItemImage.Source = m_ImageSource;
                MenuItemImage.Width =
                    MenuItemImage.Height = (m_ImageSource == null)
                                               ? 0
                                               : 20;
            }

            if (m_popup != null && LayoutGrid!= null && !LayoutGrid.Children.Contains(m_popup))
			{
				//Grid par = (Grid)VisualTreeHelper.GetParent(p);
				//if (par != null)
				//   par.Children.Remove(p);
                LayoutGrid.Children.Add(m_popup);
			}

			if (MenuItemsArrow != null)
				MenuItemsArrow.Visibility = !HasSubItems ? Visibility.Collapsed : Visibility.Visible;
		}

        /*		protected virtual void OverrideDefaultStyle()
                {
                    ParentMenu.SetMenuItemStyle(this);
                }*/

        #region Properties

        protected Popup Popup
        {
            set
            {
                m_popup = value;
                m_panel = (MenuPanel)value.Child;
                enablePopupProvider();
            }
        }

	    public ImageSource ImageSource
	    {
	        set { m_ImageSource = value; }
	    }

        public bool HasSubItems
		{
			get
			{
                return (m_panel != null && m_panel.Children.Count > 0);
			}
		}

		public bool IsSubMenuOpen
		{
			get
			{
                if (m_menuPopupProvider == null)
					return false;

                return m_menuPopupProvider.IsPopupOpen;
			}
		}

		internal IList<UIElement> MenuItems
		{
			get { return m_panel == null ? null : m_panel.Children; }
		}

		internal Style PanelMenuStyle { get; set; }

        public string Text
        {
            get { return Content.ToString(); }
            set { Content = value; }
        }

        public new bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                m_isEnabled = value;
                forceVisualState(value);
                //this is not strictly needed if the menu control is implemented as a button and we can
                //'really' disable the control, cause if the control is disabled no event will be fired
                if (value)
                    enablePopupProvider();
                else
                    disablePopupProvider();
            }
        }

        #endregion

        protected override void OnClick()
		{
            if (!m_isEnabled) 
                return;

			base.OnClick();
			RaiseMenuItemClick(this);
		}

        protected virtual void RaiseMenuItemClick(object sender)
        {
            if (HasSubItems)
                return;

            //close the parent popup window
            ParentMenuItem.ForceCloseMenuPopup();

            //rise the menu click event
            onMenuClick();
        }

	    public MenuItem AddSubmenu(string text)
		{
			MenuItem mi = new MenuItem(text);
			//ParentMenu.SetMenuItemStyle(mi);
			AddSubmenu(mi);
			return mi;
		}

		/// <summary>
		/// add a submenu
		/// </summary>
		/// <param name="sm">the menu item to add</param>
		/// <remarks>
		/// internally it creates the stuctures to hold the submenu items only
		/// if they are actually added to the menu itself.
		/// it also will create the popup provider to handle open and close events
		/// </remarks>
		public void AddSubmenu(MenuItem sm)
		{
            if (m_popup == null)
			{
                m_popup = new Popup();
                if (m_panel == null)
				{
                    m_panel = new MenuPanel();
					if (PanelMenuStyle != null)
                        m_panel.Style = PanelMenuStyle;

                    m_panel.LayoutUpdated += pnl_LayoutUpdated;
				}

                m_popup.Child = m_panel;
				if (LayoutGrid != null && !LayoutGrid.Children.Contains(m_popup))
                    LayoutGrid.Children.Add(m_popup);

				if (MenuItemsArrow != null)
					MenuItemsArrow.Visibility = Visibility.Visible;
			}

            m_panel.Children.Add(sm);
			sm.ParentMenuItem = this;
			enablePopupProvider();
		}

		private void pnl_LayoutUpdated(object sender, EventArgs e)
		{
			//set each enabled menuitem in normal state
			//this way we avoid to display the focus on an element of the popup
			//seems that an element of the popup menu can get the focus when it's displayed
            foreach (MenuItem mi in m_panel.Children)
				mi.forceVisualState(mi.IsEnabled);
		}

		private void enablePopupProvider()
		{
            if (m_menuPopupProvider == null)
                m_menuPopupProvider = new MenuPopupProvider(this, this, m_popup, m_panel, MenuDirection);
		}

		private void disablePopupProvider()
		{
            if (m_menuPopupProvider != null)
			{
                m_menuPopupProvider.Dispose();
                m_menuPopupProvider = null;
			}
		}

		/// <summary>
		/// Remove a menu from the list
		/// </summary>
		/// <param name="sm">the menu item to remove</param>
		/// <remarks>internally it will destroy the popup provider so we are sure that
		/// events for opening and closing a menu aren't triggered</remarks>
		public void RemoveSubmenu(MenuItem sm)
		{
			sm.ParentMenuItem = null;
            m_panel.Children.Remove(sm);
            if (m_panel.Children.Count == 0)
			{
				disablePopupProvider();
				if (MenuItemsArrow != null)
					MenuItemsArrow.Visibility = Visibility.Collapsed;
			}
		}

		private void forceVisualState(bool value)
		{
			if (value)
				VisualStateManager.GoToState(this, "Normal", true);
			else
				VisualStateManager.GoToState(this, "Disabled", true);
		}

		public event EventHandler MenuItemClick;

		/// <summary>
		/// the click even is fired only if this menu do not have childrens
		/// </summary>
		private void onMenuClick()
		{
			if (m_isEnabled)
                if (!HasSubItems && MenuItemClick != null)
                    MenuItemClick(this, new EventArgs());
		}

		/// <summary>
		/// close the menu, but the closing can be aborted if the mouse is over the popup menu itself
		/// </summary>
		public void CloseMenuPopup()
		{
            if (m_menuPopupProvider != null)
                m_menuPopupProvider.BeginClosingPopup();
		}

		/// <summary>
		/// close the menu, even if the mouse is over the popup menu itself
		/// </summary>
		public void ForceCloseMenuPopup()
		{
            if (m_menuPopupProvider != null)
                m_menuPopupProvider.ForceBeginClosingPopup();
		}

		public void OpenMenuPopup()
		{
            if (IsEnabled && (m_menuPopupProvider != null) && (m_menuPopupProvider.IsPopupOpen == false))
                m_menuPopupProvider.ShowPopup();
		}

		public MenuItem ParentMenuItem { get; set; }

		/*public virtual MenuBar ParentMenu
		{
			get
			{
				if (ParentMenuItem != null)
				{
					return ParentMenuItem.ParentMenu;
				}
				return null;
			}
			set { throw new ArgumentException("Unexpected use of setted method"); }
		}*/
	}
}
