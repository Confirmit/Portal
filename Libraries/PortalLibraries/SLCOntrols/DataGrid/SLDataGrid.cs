using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Helpers;
using Helpers.Reflection;

using SilverlightMenu;
using SLControls.DataGrid.Events;

namespace SLControls.DataGrid
{
    public class SLDataGrid : System.Windows.Controls.DataGrid
    {
        #region Constructor

        public SLDataGrid() : base()
        {            
            Loaded += OnSLDataGridLoaded;

            m_Menu.Child = new MenuPanel();
            MenuItem item = new MenuItem("Copy");
            item.MenuItemClick += OnCopyItemClick;
            AddMenuItem(item);

            item.ImageSource = ResourceManager.GetBitmap("DataGrid/Images/Copy.png",
                ResourceManager.GetAssemblyName(Assembly.GetExecutingAssembly()));
        }

        #endregion

        #region Fields

        private int m_btnColumnIndex = -1;
        private readonly Popup m_Menu = new Popup();

        #endregion

        #region Menu Methods

        public void AddMenuItem(MenuItem item)
        {
            MenuPanel panel = m_Menu.Child as MenuPanel;
            if (panel == null)
                return;

            if (!panel.Children.Contains(item))
                panel.Children.Add(item);
        }

        #endregion

        #region Events

        public event MenuButton.GridButtonEventHandler MainMenuButtonClick;
        public event SelectedTextChangedEventHandler SelectedTextChanged;
        
        public delegate void SelectedTextChangedEventHandler(String newSelectedText);
        public delegate void MenuItemClickEventHandler(MenuItem sender);
        protected delegate void MenuButtonEventHandler(GridEventArgs e);

        #endregion

        #region DataGrid events

        private void OnSLDataGridLoaded(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = ResourceManager.GetResourceDictionary(
                Assembly.GetExecutingAssembly(),
                "SLControls.Resources.xaml");

            if (rd == null)
                throw new Exception("Can't find resource file.");

            DataTemplate template = rd["leftBtnTemplate"] as DataTemplate;
            if (template == null)
                throw new Exception("Can't find dataTemplate for control in resource file.");

            DataGridTemplateColumn templateColumn = new DataGridTemplateColumn
                                                        {
                                                            MaxWidth = 15,
                                                            CanUserReorder = false,
                                                            CanUserResize = false,
                                                            CanUserSort = false,
                                                            CellTemplate = template
                                                        };
            if (ColumnHeaderStyle == null)
                ColumnHeaderStyle = rd["headerStyle"] as Style;

            if (Columns.Contains(templateColumn))
                return;

            templateColumn.HeaderStyle = rd["basicHeaderStyle"] as Style;
            Columns.Insert(0, templateColumn);
            m_btnColumnIndex = templateColumn.DisplayIndex;
        }

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            Size size = Application.Current.RootVisual.RenderSize;
            IList<UIElement> list =
                (from element in VisualTreeHelper.FindElementsInHostCoordinates(new Rect(0, 0, size.Width, size.Height), this)
                 where element is MenuButton
                       && (element as MenuButton).OnClickSubscribe == false
                 select element).ToList();

            foreach (MenuButton menuButton in list)
            {
                if (menuButton.OnClickSubscribe)
                    continue;

                menuButton.Column = Columns[list.Count - 1 - list.IndexOf(menuButton)];
                menuButton.Click += OnMainMenuButtonClick;
                menuButton.BeforeButtonClick += OnBeforeMenuButtonClick;
            }

            if (m_btnColumnIndex < 0)
                return;

            MenuButton btn = Columns[m_btnColumnIndex].GetCellContent(e.Row)
                                as MenuButton;
            if (btn == null || btn.OnClickSubscribe)
                return;

            btn.Row = e.Row;
            btn.Column = Columns[m_btnColumnIndex];
            btn.Click += OnMainMenuButtonClick;
            btn.BeforeButtonClick += OnBeforeMenuButtonClick;
        }

        #endregion

        #region Menu support

        public virtual void OnCopyItemClick(object sender, EventArgs e)
        {
            MenuButton menuButton = ((MenuItem) sender).ParentMenuItem as MenuButton;
            if (menuButton == null)
                return;

            GridEventArgs eventsArgs = new GridEventArgs(menuButton.Row, menuButton.Column);
            if (menuButton.Row == null && menuButton.Column != null)
            {
                if (menuButton.Column.Equals(Columns[m_btnColumnIndex]))
                    processAllClick();
                else
                    processHeaderButtonClick(eventsArgs);
            }

            if (menuButton.Row != null)
                processRowButtonClick(eventsArgs);
        }

        /// <summary>
        /// Event on menu button click. Desides what button clicked and call process functions.
        /// </summary>
        /// <param name="sender">Menu button.</param>
        /// <param name="e">Event args.</param>
        public virtual void OnMainMenuButtonClick(object sender, GridEventArgs e)
        {
            //setStyle(null, CurrentColumn, new SolidColorBrush(Colors.Black));
            
            if (e.Row == null && e.Column != null)
            {
                //setStyle(null, e.Column, new SolidColorBrush(Colors.Red));
                CurrentColumn = e.Column;
            }

            if (e.Row != null)
                SelectedIndex = e.Row.GetIndex();

            if (MainMenuButtonClick != null)
                MainMenuButtonClick(sender, e);
        }

        private void OnBeforeMenuButtonClick(object sender, GridEventArgs e)
        {
            MenuButton btnClicked = sender as MenuButton;
            if (btnClicked == null)
                return;

            MenuPanel panel = m_Menu.Child as MenuPanel;
            if (panel == null)
                return;

            foreach (MenuItem menuItem in panel.Children)
            {
                menuItem.ParentMenuItem = btnClicked;
            }

            btnClicked.Popup = m_Menu;
        }

        #region Process menu buttons click

        private void processHeaderButtonClick(GridEventArgs e)
        {
            if (e.Column == null)
                return;

            StringBuilder result = new StringBuilder();
            result.Append("<table>");

            foreach (Object item in ItemsSource)
            {
                Object value = getColumnValue(item, e.Column);
                result.AppendFormat("<tr><td>{0}</td></tr>", value);
            }
            result.Append("</table>");

            if (SelectedTextChanged != null)
                SelectedTextChanged(result.ToString());
        }

        private void processRowButtonClick(GridEventArgs e)
        {
            if (e.Row == null)
                return;

            StringBuilder result = new StringBuilder();
            result.Append("<table><tr>");

            foreach (DataGridColumn column in Columns)
            {
                Object value = getColumnValue(SelectedItem, column);
                if (value == null)
                    continue;

                result.AppendFormat("<td>{0}</td>", value);
            }
            result.Append("</tr></table>");

            if (SelectedTextChanged != null)
                SelectedTextChanged(result.ToString());
        }

        private void processAllClick()
        {
            StringBuilder data = new StringBuilder();
            StringBuilder header = new StringBuilder();
            header.Append("<tr>");

            foreach (DataGridColumn column in Columns)
            {
                if (column.Header != null)
                    header.AppendFormat("<td><b>{0}</b></td>", column.Header);
            }
            header.Append("</tr>");

            foreach (Object item in ItemsSource)
            {
                data.Append("<tr>");
                foreach (DataGridColumn column in Columns)
                {
                    Object value = getColumnValue(item, column);
                    if (value == null)
                        continue;

                    data.AppendFormat("<td>{0}</td>", value);
                }
                data.Append("</tr>");
            }

            if (SelectedTextChanged != null)
                SelectedTextChanged(String.Format("<table>{0}{1}</table>", header, data));
        }

        #endregion

        #region Select support

        /// <summary>
        /// Get value of cell in column.
        /// </summary>
        /// <param name="source">Binding object to grid row.</param>
        /// <param name="column">Column.</param>
        /// <returns>Value object.</returns>
        private Object getColumnValue(Object source, DataGridColumn column)
        {
            DataGridBoundColumn bound = column as DataGridBoundColumn;
            TemplateColumn templateColumn = column as TemplateColumn;
            
            string propName = bound == null
                                  ? (templateColumn != null)
                                        ? templateColumn.BindingObjectValue
                                        : "Text"
                                  : bound.Binding.Path.Path;

            object value = ReflectionHelper.GetPropertyValue(source, propName);
            if (templateColumn != null && templateColumn.Converter != null)
            {
                value = templateColumn.Converter.Convert(value,
                                                         typeof (String),
                                                         templateColumn.ConverterParameter,
                                                         Thread.CurrentThread.CurrentCulture);
            }

            return value;
        }

        private void setStyle(DataGridRow row, DataGridColumn column, Brush brush)
        {
            if (row != null)
            {
                
            }

            if (column != null)
            {
                foreach (Object item in ItemsSource)
                {
                    FrameworkElement cellContent = column.GetCellContent(item);
                    DataGridCell cell = cellContent.Parent as DataGridCell;
                    if (cell != null)
                        cell.Foreground = brush;
                }
            }
        }

        #endregion

        #endregion
    }
}