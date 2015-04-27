using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLControls.Slider.SelectionSupply
{
    [TemplatePart( Name = "ContentPresenter", Type = typeof( ContentPresenter ) )]
    [TemplatePart( Name = "ContentContainer", Type = typeof( FrameworkElement ) )]

    [TemplateVisualState( GroupName = "CommonStates", Name = "Normal" )]
    [TemplateVisualState( GroupName = "CommonStates", Name = "MouseOver" )]
    [TemplateVisualState( GroupName = "CommonStates", Name = "Pressed" )]
    [TemplateVisualState( GroupName = "SelectionStates", Name = "Selected" )]
    [TemplateVisualState( GroupName = "SelectionStates", Name = "Deselected" )]
    public class SelectableContentControl : ContentControl, ISelectable
    {

        #region Fields

        private bool isMouseOver, isPressed;
        private ContentPresenter ContentPresenter;
        private FrameworkElement ContentContainer;
        public event EventHandler Selected;

        #endregion

        public SelectableContentControl()
        {
            DefaultStyleKey = typeof (SelectableContentControl);
            ContentWidth = double.NaN;
            ContentHeight = double.NaN;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ContentPresenter = (ContentPresenter) GetTemplateChild("ContentPresenter");
            if (ContentPresenter != null)
            {
                ContentPresenter.MouseEnter += ContentPresenter_MouseEnter;
                ContentPresenter.MouseLeave += ContentPresenter_MouseLeave;
                ContentPresenter.MouseLeftButtonDown += ContentPresenter_MouseLeftButtonDown;
                ContentPresenter.MouseLeftButtonUp += ContentPresenter_MouseLeftButtonUp;

                ContentPresenter.Style = ContentStyle;
            }

            ContentContainer = (FrameworkElement) GetTemplateChild("ContentContainer");
            if (ContentContainer != null)
            {
                updateContainerWidth();
                updateContainerHeight();
            }

            // Go to normal state without using any transitions
            goToState(false);
        }

        #region private methods

        private void updateContainerWidth()
        {
            ContentContainer.Width = ContentWidth;
        }
        private void updateContainerHeight()
        {
            ContentContainer.Height = ContentHeight;
        }

        private void goToState(bool useTransitions)
        {
            if (isPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", true);
            }
            else if (isMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            if (IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Deselected", useTransitions);
            }
        }

        #endregion

        #region event handlers

        private void ContentPresenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter.CaptureMouse();
            isPressed = true;
            IsSelected = true;
            goToState(true);
        }

        private void ContentPresenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter.ReleaseMouseCapture();
            isPressed = false;
            goToState(true);
        }

        private void ContentPresenter_MouseLeave(object sender, MouseEventArgs e)
        {
            ReleaseMouseCapture();

            isMouseOver = false;
            SetValue(Canvas.ZIndexProperty, 0);

            goToState(true);
        }

        private void ContentPresenter_MouseEnter(object sender, MouseEventArgs e)
        {
            CaptureMouse();

            isMouseOver = true;
            SetValue(Canvas.ZIndexProperty, 10);

            goToState(true);
        }

        #endregion

        #region ISelectable Members

        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        #endregion

        #region IsSelected property

        public static DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelectedProperty",
                                        typeof (bool),
                                        typeof (SelectableContentControl),
                                        new PropertyMetadata(OnIsSelectedChange));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectableContentControl c = d as SelectableContentControl;
            c.OnIsSelectedChange((bool) e.OldValue, (bool) e.NewValue);
        }

        private void OnIsSelectedChange(bool oldValue, bool newValue)
        {
            EventHandler handler = Selected;

            if (handler != null && newValue)
                handler(this, new EventArgs());

            goToState(true);
        }

        #endregion

        #region ContentWidth property

        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register("ContentWidthProperty",
                                        typeof (double),
                                        typeof (SelectableContentControl),
                                        new PropertyMetadata(OnContentWidthChanged));

        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        protected static void OnContentWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SelectableContentControl).OnContentWidthChanged((double) e.OldValue, (double) e.NewValue);
        }

        internal virtual void OnContentWidthChanged( double oldValue, double newValue )
        {
            if( ContentContainer != null )
                updateContainerWidth();
        }

        #endregion

        #region ContentHeight property

        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register("ContentHeightProperty",
                                        typeof (double),
                                        typeof (SelectableContentControl),
                                        new PropertyMetadata(OnContentHeightChanged));

        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        protected static void OnContentHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SelectableContentControl).OnContentHeightChanged((double) e.OldValue, (double) e.NewValue);
        }

        internal virtual void OnContentHeightChanged(double oldValue, double newValue)
        {
            if (ContentContainer != null)
                updateContainerHeight();
        }

        #endregion

        #region ContentStyle property

        public static readonly DependencyProperty ContentStyleProperty =
            DependencyProperty.Register("ContentStyleProperty",
                                        typeof (Style),
                                        typeof (SelectableContentControl),
                                        new PropertyMetadata(OnContentStyleChanged));

        public Style ContentStyle
        {
            get { return (Style)GetValue(ContentStyleProperty); }
            set { SetValue(ContentStyleProperty, value); }
        }

        protected static void OnContentStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SelectableContentControl).OnContentStyleChanged((Style) e.OldValue, (Style) e.NewValue);
        }

        internal virtual void OnContentStyleChanged(Style oldValue, Style newValue)
        {
            if (ContentPresenter != null)
                ContentPresenter.Style = ContentStyle;
        }

        #endregion
    }
}