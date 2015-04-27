using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SLControls.Book.Enums;
using SLControls.Book.Page;

namespace SLControls.Book
{
    public delegate void PageTurnedEventHandler(int leftPageIndex, int rightPageIndex);

    public partial class Book : ItemsControl
    {
        #region Constructor

        public Book()
        {
            InitializeComponent();
        }

        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            defaultDataTemplate = (DataTemplate)Resources["defaultDataTemplate"];
            //refreshSheetsContent();
        }

        #region Fields
        
        private DataTemplate defaultDataTemplate;
        private PageStatus _status = PageStatus.None;
        private int _currentSheetIndex = 0;

        public event PageTurnedEventHandler OnPageTurned;

        #endregion

        #region Page events handler

        #region Mouse events handler

        private void leftPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BookPage sheet0 = GetTemplateChild("sheet0") as BookPage;
            BookPage sheet1 = GetTemplateChild("sheet1") as BookPage;

            // устонавливаем порядок рендеринга
            Canvas.SetZIndex(sheet0, 1);
            Canvas.SetZIndex(sheet1, 0);
        }

        private void rightPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BookPage sheet0 = GetTemplateChild("sheet0") as BookPage;
            BookPage sheet1 = GetTemplateChild("sheet1") as BookPage;

            // устонавливаем порядок рендеринга
            Canvas.SetZIndex(sheet0, 0);
            Canvas.SetZIndex(sheet1, 1);
        }

        #endregion

        private void rightPage_PageTurned(object sender, RoutedEventArgs e)
        {
            CurrentSheetIndex++;
        }

        private void leftPage_PageTurned(object sender, RoutedEventArgs e)
        {
            CurrentSheetIndex--;
        }

        #endregion

        #region Methods

        protected virtual bool CheckCurrentSheetIndex()
        {
            return CurrentSheetIndex > (GetItemsCount() / 2);
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (CheckCurrentSheetIndex())
                CurrentSheetIndex = GetItemsCount() / 2;
            else
                refreshSheetsContent();
        }

        internal object GetPage(int index)
        {
            if ((index >= 0) && (index < Items.Count))
                return Items[index];

            return new Canvas();
        }

        public int CurrentSheetIndex
        {
            get { return _currentSheetIndex; }
            set
            {
                if (_status != PageStatus.None)
                    return;

                if (_currentSheetIndex != value)
                {
                    if ((value >= 0) && (value <= GetItemsCount() / 2))
                    {
                        _currentSheetIndex = value;
                        refreshSheetsContent();
                    }
                    else
                        throw new Exception("Index out of bounds");
                }
            }
        }

        public int GetItemsCount()
        {
            if (ItemsSource != null)
            {
                if (ItemsSource is ICollection)
                    return ((ICollection)ItemsSource).Count;

                int count = 0;
                foreach (object o in ItemsSource) 
                    count++;
                
                return count;
            }
            return Items.Count;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            refreshSheetsContent();
        }

        private void refreshSheetsContent()
        {
            BookPage bp0 = GetTemplateChild("sheet0") as BookPage;
            if (bp0 == null)
                return;

            BookPage bp1 = GetTemplateChild("sheet1") as BookPage;

            Visibility bp0Visibility = Visibility.Visible;
            Visibility bp1Visibility = Visibility.Visible;

            bp1.IsTopRightCornerEnabled = true;
            bp1.IsBottomRightCornerEnabled = true;

            Visibility sheet0Page0ContentVisibility = Visibility.Visible;
            Visibility sheet0Page1ContentVisibility = Visibility.Visible;
            Visibility sheet0Page2ContentVisibility = Visibility.Visible;

            Visibility sheet1Page0ContentVisibility = Visibility.Visible;
            Visibility sheet1Page1ContentVisibility = Visibility.Visible;
            Visibility sheet1Page2ContentVisibility = Visibility.Visible;

            DataTemplate dt = ItemTemplate;
            if (dt == null)
                dt = defaultDataTemplate;

            bp0.Page0.ContentTemplate =
                bp0.Page1.ContentTemplate =
                bp0.Page2.ContentTemplate =
                bp1.Page0.ContentTemplate =
                bp1.Page1.ContentTemplate =
                bp1.Page2.ContentTemplate = dt;

            sheet0Page2ContentVisibility = _currentSheetIndex == 1
                                               ? Visibility.Collapsed
                                               : Visibility.Visible;
            int count = GetItemsCount();
            int sheetCount = count / 2;
            bool isOdd = (count % 2) == 1;

            if (_currentSheetIndex == sheetCount)
            {
                if (isOdd)
                {
                    bp1.IsTopRightCornerEnabled = false;
                    bp1.IsBottomRightCornerEnabled = false;
                }
                else
                    bp1Visibility = Visibility.Collapsed;
            }

            if (_currentSheetIndex == sheetCount - 1)
            {
                if (!isOdd)
                    sheet1Page2ContentVisibility = Visibility.Collapsed;
            }

            if (_currentSheetIndex == 0)
            {
                bp0.Page0.Content = null;
                bp0.Page1.Content = null;
                bp0.Page2.Content = null;
                bp0.IsEnabled = false;
                bp0Visibility = Visibility.Collapsed;
            }
            else
            {
                bp0.Page0.Content = GetPage(2 * (CurrentSheetIndex - 1) + 1);
                bp0.Page1.Content = GetPage(2 * (CurrentSheetIndex - 1));
                bp0.Page2.Content = GetPage(2 * (CurrentSheetIndex - 1) - 1);
                bp0.IsEnabled = true;
            }

            bp1.Page0.Content = GetPage(2 * CurrentSheetIndex);
            bp1.Page1.Content = GetPage(2 * CurrentSheetIndex + 1);
            bp1.Page2.Content = GetPage(2 * CurrentSheetIndex + 2);

            bp0.Visibility = bp0Visibility;
            bp1.Visibility = bp1Visibility;

            bp0.Page0.Visibility = sheet0Page0ContentVisibility;
            bp0.Page1.Visibility = sheet0Page1ContentVisibility;
            bp0.Page2.Visibility = sheet0Page2ContentVisibility;

            bp1.Page0.Visibility = sheet1Page0ContentVisibility;
            bp1.Page1.Visibility = sheet1Page1ContentVisibility;
            bp1.Page2.Visibility = sheet1Page2ContentVisibility;

            if (OnPageTurned != null)
            {
                int leftPageIndex = 2 * _currentSheetIndex;
                int rightPageIndex = leftPageIndex + 1;

                if (_currentSheetIndex == 0)
                    leftPageIndex = -1;

                if ((_currentSheetIndex == count / 2) && !isOdd)
                    rightPageIndex = -1;

                OnPageTurned(leftPageIndex, rightPageIndex);
            }
        }

        #endregion

        #region Animation turn page

        public void AnimateToNextPage(int duration)
        {
            if (CurrentSheetIndex + 1 <= GetItemsCount() / 2)
            {
                BookPage bookPage0 = GetTemplateChild("sheet0") as BookPage;
                BookPage bookPage1 = GetTemplateChild("sheet1") as BookPage;

                Canvas.SetZIndex(bookPage0, 0);
                Canvas.SetZIndex(bookPage1, 1);
                bookPage1.AutoTurnPage(CornerOrigin.BottomRight, duration);
            }
        }

        public void AnimateToPreviousPage(int duration)
        {
            if (CurrentSheetIndex > 0)
            {
                BookPage bookPage0 = GetTemplateChild("sheet0") as BookPage;
                BookPage bookPage1 = GetTemplateChild("sheet1") as BookPage;
                
                Canvas.SetZIndex(bookPage1, 0);
                Canvas.SetZIndex(bookPage0, 1);
                bookPage0.AutoTurnPage(CornerOrigin.BottomLeft, duration);
            }
        }

        #endregion
    }
}