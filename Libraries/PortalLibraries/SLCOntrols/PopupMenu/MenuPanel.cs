using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace SilverlightMenu
{
	public class MenuPanel : ContentControl
    {
        #region Fileds

        private List<UIElement> m_Children = new List<UIElement>();
        private Panel m_innerPanel = null;

        #endregion

        public MenuPanel()
		{
			this.DefaultStyleKey = typeof(MenuPanel);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

            m_innerPanel = GetTemplateChild("InnerPanel") as Panel;

            foreach (var e in m_Children)
                m_innerPanel.Children.Add(e);
		}

		public IList<UIElement> Children
		{
			get
			{
                if (m_innerPanel != null)
                    return m_innerPanel.Children;

                return m_Children;
			}
		}	
	}
}
