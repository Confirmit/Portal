using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;

using SLControls.Slider.ItemsSupply;

namespace SLControls.Slider.ItemsSupply
{
    public class ItemContainersControl : ItemsSelector
    {
        private Dictionary<object, ItemContainer> _objectToItemContainer;

        public ItemContainersControl()
        {
            ItemWidth = double.NaN;
            ItemHeight = double.NaN;
        }

        #region  ItemWidth property

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidthProperty",
                                        typeof(double),
                                        typeof(ItemContainersControl),
                                        new PropertyMetadata(OnItemWidthChanged));

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        protected static void OnItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ItemContainersControl).OnItemWidthChanged((double)e.OldValue, (double)e.NewValue);
        }

        internal virtual void OnItemWidthChanged(double oldValue, double newValue)
        {
            UpdateContainderWidth();
        }

        protected void UpdateContainderWidth()
        {
            foreach (object obj in ObjectToItemContainer.Values) //GetContainers()
            {
                ItemContainer ItemContainerForObject = this.GetItemContainerForObject(obj);
                ItemContainerForObject.ContentWidth = ItemWidth;
            }
        }

        #endregion

        #region ItemHeight property

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeightProperty",
                                        typeof(double),
                                        typeof(ItemContainersControl),
                                        new PropertyMetadata(OnItemHeightChanged));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        protected static void OnItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ItemContainersControl).OnItemHeightChanged((double)e.OldValue, (double)e.NewValue);
        }

        internal virtual void OnItemHeightChanged(double oldValue, double newValue)
        {
            foreach (object obj in ObjectToItemContainer.Values)//GetContainers()
            {
                ItemContainer ItemContainerForObject = GetItemContainerForObject(obj);
                ItemContainerForObject.ContentHeight = ItemHeight;
            }
        }

        #endregion

        #region ItemStyle property

        public static readonly DependencyProperty ItemStyleProperty =
            DependencyProperty.Register("ItemContentStyleProperty",
                                        typeof (Style),
                                        typeof (ItemContainersControl),
                                        new PropertyMetadata(
                                            new PropertyChangedCallback(OnItemStyleChanged)));

        public Style ItemStyle
        {
            get { return (Style)GetValue(ItemStyleProperty); }
            set { SetValue(ItemStyleProperty, (DependencyObject)value); }
        }

        private static void OnItemStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ItemContainersControl).OnItemStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        internal virtual void OnItemStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
        {
            if (oldItemContainerStyle == newItemContainerStyle)
                return;

            foreach (object obj2 in Items)
            {
                ItemContainer ItemContainerForObject = GetItemContainerForObject(obj2);

                if ((ItemContainerForObject != null) &&
                    ((ItemContainerForObject.Style == null) ||
                     (oldItemContainerStyle == ItemContainerForObject.Style)))
                {
                    if (ItemContainerForObject.Style != null)
                        throw new NotSupportedException(null);

                    ItemContainerForObject.ContentStyle = newItemContainerStyle;
                }
            }
        }

        #endregion

        #region ContainerStyle property

        public static readonly DependencyProperty ContainerStyleProperty =
            DependencyProperty.Register("ItemAnimationContainerStyleProperty",
                                        typeof (Style),
                                        typeof (ItemContainersControl),
                                        new PropertyMetadata(new PropertyChangedCallback(OnContainerStyleChanged)));

        public Style ContainerStyle
        {
            get { return (Style)GetValue(ContainerStyleProperty); }
            set { SetValue(ContainerStyleProperty, (DependencyObject)value); }
        }

        private static void OnContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ItemContainersControl).OnContainerStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        internal virtual void OnContainerStyleChanged(Style oldItemAnimationContainerStyle, Style newItemAnimationContainerStyle)
        {
            if (oldItemAnimationContainerStyle == newItemAnimationContainerStyle)
                return;

            foreach (object obj2 in Items)
            {
                ItemContainer ItemContainerForObject = GetItemContainerForObject(obj2);

                if ((ItemContainerForObject != null) &&
                    ((ItemContainerForObject.Style == null) ||
                     (oldItemAnimationContainerStyle == ItemContainerForObject.Style)))
                {
                    if (ItemContainerForObject.Style != null)
                        throw new NotSupportedException(null);

                    ItemContainerForObject.Style = newItemAnimationContainerStyle;
                }
            }
        }

        #endregion

        #region ItemContainer methods

        protected ItemContainer GetItemContainerForObject(object value)
        {
            ItemContainer item = value as ItemContainer;
            if (item == null)
                ObjectToItemContainer.TryGetValue(value, out item);

            return item;
        }

        protected IDictionary<object, ItemContainer> ObjectToItemContainer
        {
            get
            {
                if (_objectToItemContainer == null)
                    _objectToItemContainer = new Dictionary<object, ItemContainer>();

                return _objectToItemContainer;
            }
        }

        #endregion

        #region overridden methods

        protected override DependencyObject GetContainerForItemOverride()
        {
            ItemContainer item = new ItemContainer();

            if (ContainerStyle != null)
                item.Style = ContainerStyle;

            return item;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ItemContainer);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            ItemContainer item2 = element as ItemContainer;
            bool flag = true;

            if (item2 != item)
            {
                if (ItemTemplate != null)
                {
                    item2.ContentTemplate = ItemTemplate;
                }
                else if (!string.IsNullOrEmpty(DisplayMemberPath))
                {
                    Binding binding = new Binding(DisplayMemberPath);
                    item2.SetBinding(ContentControl.ContentProperty, binding);
                    flag = false;
                }

                if (flag)
                    item2.Content = item;

                ObjectToItemContainer[item] = item2;
            }

            if ((ContainerStyle != null) && (item2.Style == null))
                item2.Style = ContainerStyle;

            if ((ItemStyle != null) && (item2.ContentStyle == null))
                item2.ContentStyle = ItemStyle;

            if (item2 != null)
            {
                item2.ContentWidth = ItemWidth;
                item2.Width = Height*(ItemWidth/ItemHeight);
                item2.Height = Height;
                item2.ContentHeight = ItemHeight;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            ItemContainer item2 = element as ItemContainer;
            if (item == null)
                item = (item2.Content == null)
                           ? item2
                           : item2.Content;

            if (item2 != item)
                ObjectToItemContainer.Remove(item);
        }

        #endregion
    }
}