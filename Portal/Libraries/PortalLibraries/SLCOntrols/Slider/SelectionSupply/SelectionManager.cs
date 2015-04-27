using System;
using System.Collections.ObjectModel;

namespace SLControls.Slider.SelectionSupply
{
    public interface ISelectable
    {
        event EventHandler Selected;
        void Select();
        void Deselect();
    }

    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs(int indexOfSelectedItem)
        {
            selectedItemIndex = indexOfSelectedItem;
        }

        public int selectedItemIndex;
    }

    public class SelectionManager
    {
        private ObservableCollection<ISelectable> items;
        public event EventHandler<SelectionChangedEventArgs> SelectionChange;

        public void HookOnSelectionChangedEvent(ObservableCollection<ISelectable> items)
        {
            this.items = items;
            items.CollectionChanged += CollectionChanged;

            foreach (ISelectable current in this.items)
            {
                current.Selected += OnItemSelected;
            }

        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (ISelectable newItem in e.NewItems)
                {
                    newItem.Selected += OnItemSelected;
                }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (ISelectable newItem in e.OldItems)
                {
                    newItem.Selected -= OnItemSelected;
                }
            }
        }

        public void OnItemSelected(object sender, EventArgs e)
        {
            ISelectable selected = sender as ISelectable;
            foreach (ISelectable current in items)
            {
                if (current != selected)
                    current.Deselect();
            }

            if (SelectionChange != null)
                SelectionChange(this, new SelectionChangedEventArgs(items.IndexOf(selected)));
        }
    }
}