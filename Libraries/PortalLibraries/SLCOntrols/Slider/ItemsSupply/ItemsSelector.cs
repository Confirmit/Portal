using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using SLControls.Slider.SelectionSupply;
using SelectionChangedEventArgs=SLControls.Slider.SelectionSupply.SelectionChangedEventArgs;

namespace SLControls.Slider.ItemsSupply
{
    public class ItemsSelector : ItemsControl
    {

        #region private members

        protected ObservableCollection<ISelectable> m_selectableCollection;
        private SelectionManager m_selectionManager;
        public event EventHandler<SelectionChangedEventArgs> SelectionChange;

        #endregion

        public ItemsSelector()
        {
            m_selectableCollection = new ObservableCollection<ISelectable>();
            m_selectionManager = new SelectionManager();

            m_selectionManager.HookOnSelectionChangedEvent(m_selectableCollection);
            m_selectionManager.SelectionChange += _selectManager_SelectionChange;
        }

        private void _selectManager_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChange != null)
                SelectionChange(this, e);
        }

        #region overridden methods

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ISelectable item2 = element as ISelectable;
            m_selectableCollection.Add(item2);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ISelectable item2 = element as ISelectable;
            m_selectableCollection.Remove(item2);
        }

        #endregion
    }
}