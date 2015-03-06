using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

using Helpers;
using UserPhotos.SLServiceReference;
using System.Collections.Generic;

namespace UserPhotos
{
    public partial class Page : UserControl
    {
        #region Fields

        private int m_userID = -1;
        private bool m_isCanEditPhoto = false;
        private SLServiceClient m_wcfClient = null;

        #endregion

        public Page(String SLService, int userID, bool isCanEditPhoto)
        {
            m_userID = userID;
            m_isCanEditPhoto = isCanEditPhoto;

            InitializeComponent();

            m_wcfClient = new SLServiceClient(new BasicHttpBinding(),
                                              new EndpointAddress(PathHelper.GetAbsoluteUrl(SLService)));

            m_wcfClient.GetUserPhotosAbsoluteURICompleted += wcfClient_GetUserPhotosAbsoluteURICompleted;
            m_wcfClient.GetUserPhotosAbsoluteURIAsync(m_userID);
        }
        
        #region Buttons events

        private void OnImageDeleteLoaded(object sender, RoutedEventArgs e)
        {
            UIElement element = sender as UIElement;
            if (element == null)
                return;

            element.Visibility = m_isCanEditPhoto
                                     ? System.Windows.Visibility.Visible
                                     : System.Windows.Visibility.Collapsed;
        }

        private void OnImageDeleteClick(object sender, RoutedEventArgs e)
        {
            if (m_wcfClient == null)
                return;

            String fileURI = getFileURIFromContext(sender as FrameworkElement);
            m_wcfClient.DeleteUserPhotoAsync(m_userID, fileURI);

            IList<String> list = Slider.ItemsSource as IList<String>;
            list.Remove(fileURI);
        }

        private void OnImagePreviewClick(object sender, RoutedEventArgs e)
        {
            String fileURI = getFileURIFromContext(sender as FrameworkElement);
            HtmlPage.Window.Navigate(new Uri(fileURI, UriKind.Absolute), "Photo");
        }

        #endregion

        #region Service events

        private void wcfClient_GetUserPhotosAbsoluteURICompleted(object sender, GetUserPhotosAbsoluteURICompletedEventArgs e)
        {
            Slider.ItemsSource = e.Result;

            if (e.Result == null || e.Result.Count == 0)
            {
                Slider.Visibility = System.Windows.Visibility.Collapsed;
                imgAnonymnous.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Helpers

        private static String getFileURIFromContext(FrameworkElement fwElement)
        {
            if (fwElement == null)
                return String.Empty;

            return fwElement.DataContext as string;
        }

        #endregion
    }
}
