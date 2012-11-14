using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

using SLControls.Slider.ItemsSupply;

namespace SLControls.Slider
{
    public class AnimatedSlider : ItemContainersControl
    {
        #region Fields

        private RepeatButton m_PreviousButton;
        private RepeatButton m_NextButton;
        private Storyboard m_ToPreviousStoryboard;
        private Storyboard m_ToNextStoryboard;

        private Canvas m_View;
        private ItemsPresenter m_ItemsPresenter;

        private double m_areaWidth;
        private Point m_lastMousePos;
        private bool m_IsMouseDrag = false;

        #endregion

        public AnimatedSlider()
        {
            DefaultStyleKey = typeof(AnimatedSlider);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_PreviousButton = (RepeatButton)GetTemplateChild("LeftButton");
            if (m_PreviousButton != null)
            {
                m_PreviousButton.Click += LeftButton_Click;
                m_PreviousButton.Style = LeftButtonStyle;
            }

            m_NextButton = (RepeatButton)GetTemplateChild("RightButton");
            if (m_NextButton != null)
            {
                m_NextButton.Click += RightButton_Click;
                m_NextButton.Style = RightButtonStyle;
            }

            m_ToPreviousStoryboard = (Storyboard)GetTemplateChild("ToLeftStoryboard");
            m_ToNextStoryboard = (Storyboard)GetTemplateChild("ToRightStoryboard");

            m_View = (Canvas)GetTemplateChild("View");
            if (m_View != null)
            {
                m_View.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                m_View.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                m_View.MouseMove += Canvas_MouseMove;
            }

            m_ItemsPresenter = (ItemsPresenter)GetTemplateChild("ItemsPresenter");
            LayoutUpdated += AnimatedSlider_LayoutUpdated;
        }

        private void AnimatedSlider_LayoutUpdated(object sender, EventArgs e)
        {
            LayoutUpdated -= AnimatedSlider_LayoutUpdated;

            m_areaWidth = ActualWidth - m_PreviousButton.ActualWidth - m_NextButton.ActualWidth;
            RectangleGeometry visibleArea = new RectangleGeometry();
            visibleArea.Rect = new Rect(0, 0, m_areaWidth, ActualHeight);
            m_View.Clip = visibleArea;
        }

        #region event handlers

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if ((double)m_ItemsPresenter.GetValue(Canvas.LeftProperty) + 40 >= 0)
            {
                m_ItemsPresenter.SetValue(Canvas.LeftProperty, (double)0);
            }
            else if (m_ToPreviousStoryboard != null)
                m_ToPreviousStoryboard.Begin();
            else // Strange bug calls recursively LeftButton_Click when Canvas.LeftProperty is set to positive
                m_ItemsPresenter.SetValue(Canvas.LeftProperty,
                                          (double)m_ItemsPresenter.GetValue(Canvas.LeftProperty) + ItemWidth);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            double dist = (double) m_ItemsPresenter.GetValue(Canvas.LeftProperty)
                          - m_areaWidth;
                          //- m_NextButton.ActualWidth;

            // -30 - value of BY property of animation.
            if (dist - 30 /*+ 10*/ <= -(m_ItemsPresenter.ActualWidth))
            {
                m_ItemsPresenter.SetValue(Canvas.LeftProperty, m_areaWidth - (m_ItemsPresenter.ActualWidth));
            }
            else if (m_ToNextStoryboard != null)
            {
                m_ToNextStoryboard.Begin();
            }
            else // Strange bug calls recursively RightButton_Click when Canvas.LeftProperty is set to positive
                m_ItemsPresenter.SetValue(Canvas.LeftProperty,
                                          (double)m_ItemsPresenter.GetValue(Canvas.LeftProperty) - ItemWidth);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_IsMouseDrag = true;
            m_lastMousePos = e.GetPosition(m_View);

            FrameworkElement c = sender as FrameworkElement;
            if (c == null)
                return;

            c.CaptureMouse();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_IsMouseDrag = false;

            FrameworkElement c = sender as FrameworkElement;
            if (c == null)
                return;

            c.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_IsMouseDrag)
                return;

            Point toViewCoordinates = e.GetPosition(m_View);
            double deltaX = toViewCoordinates.X - m_lastMousePos.X;
            double newLeft = (double)m_ItemsPresenter.GetValue(Canvas.LeftProperty) + deltaX;

            if (newLeft <= 0 && newLeft >= -(m_ItemsPresenter.ActualWidth - m_areaWidth))
                m_ItemsPresenter.SetValue(Canvas.LeftProperty, newLeft);

            m_lastMousePos = toViewCoordinates;
        }

        #endregion

        #region  LeftButtonStyle

        public static readonly DependencyProperty LeftButtonStyleProperty =
            DependencyProperty.Register("LeftButtonStyleProperty",
                                        typeof(Style),
                                        typeof(AnimatedSlider),
                                        new PropertyMetadata(OnLeftButtonStyleChanged));

        public Style LeftButtonStyle
        {
            get { return (Style)GetValue(LeftButtonStyleProperty); }
            set { SetValue(LeftButtonStyleProperty, value); }
        }

        protected static void OnLeftButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AnimatedSlider).OnLeftButtonStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        internal virtual void OnLeftButtonStyleChanged(Style oldValue, Style newValue)
        {
            if (m_PreviousButton != null)
                m_PreviousButton.Style = newValue;
        }

        #endregion

        #region  RightButtonStyle

        public static readonly DependencyProperty RightButtonStyleProperty =
            DependencyProperty.Register("RightButtonStyleProperty",
                                        typeof (Style),
                                        typeof (AnimatedSlider),
                                        new PropertyMetadata(OnRightButtonStyleChanged));

        public Style RightButtonStyle
        {
            get { return (Style)GetValue(RightButtonStyleProperty); }
            set { SetValue(RightButtonStyleProperty, value); }
        }

        protected static void OnRightButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AnimatedSlider).OnRightButtonStyleChanged((Style)e.OldValue, (Style)e.NewValue);
        }

        internal virtual void OnRightButtonStyleChanged(Style oldValue, Style newValue)
        {
            if (m_NextButton != null)
                m_NextButton.Style = newValue;
        }

        #endregion
    }
}