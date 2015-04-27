using System;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Controls.HotGridView
{
    [Serializable]
    public class BoundSelectionFieldData
    {
        private String _navigateURL = String.Empty;
        private String _text = String.Empty;
        private String _image = String.Empty;
        private String _altText = String.Empty;

        public Boolean IsEmpty()
        {
            return String.IsNullOrEmpty(_navigateURL) && String.IsNullOrEmpty(_text);
        }

        public HyperLink CreateControlLinkFromCachedData()
        {
            HyperLink link = new HyperLink();
            link.NavigateUrl = _navigateURL;
            link.Text = _text;

            if (!String.IsNullOrEmpty(_image))
                link.Controls.Add(CreateImage(_image, _altText));

            return link;
        }

        public HyperLink CreateControlLink(DataControlFieldCell cell
            , String IdPropertyName
            , String controllerObjectName
            , String onClickClientEvent
            , String imageUrl
            , String altText
            , Int32 columnIndex
            , String businessKeyProperty
            )
        {
            GridViewRow row = (GridViewRow)cell.BindingContainer;
            if (row == null)
                return null;

            String value = GetObjectValue(row.DataItem, IdPropertyName);
            String businessKeyPropertyValue = GetObjectValue(row.DataItem, businessKeyProperty);

            if (String.IsNullOrEmpty(cell.Text) || String.Compare(cell.Text, "&nbsp;") == 0 ||
                    String.IsNullOrEmpty(value))
                return null;

            HyperLink link = new HyperLink();

            link.Text = cell.Text;
            link.NavigateUrl = GetNavigationUrl(value, businessKeyPropertyValue, controllerObjectName, onClickClientEvent, columnIndex);

            if (!String.IsNullOrEmpty(imageUrl))
                link.Controls.Add(CreateImage(imageUrl, altText));

            _navigateURL = link.NavigateUrl;
            _text = link.Text;
            _image = imageUrl;

            return link;
        }

        public static String GetNavigationUrl(String objectValue, String businessKeyProperty,
                                                String controllerObjectName, String onClickClientEvent, 
                                                Int32 columnIndex)
        {
                return "javascript:"
                        + controllerObjectName
                        + (!String.IsNullOrEmpty(controllerObjectName) ? "." : String.Empty)
                        + (String.IsNullOrEmpty(onClickClientEvent) ?
                                     String.Format("OnClick('{0}','{1}','{2}', true)", columnIndex, objectValue, businessKeyProperty)
                                   : String.Format(onClickClientEvent, objectValue, businessKeyProperty));
        }

        public static String GetObjectValue(object item, string propertyname)
        {
            Type type = item.GetType();
            IList<PropertyInfo> listP = type.GetProperties();
            foreach (PropertyInfo property in listP)
            {
                if (property.Name == propertyname)
                {
                    Object value = property.GetValue(item, null);
                    if (value != null)
                        return value.ToString();
                }
            }

            return null;
        }

        public static Image CreateImage(String url, String altText)
        {
            Image image = new Image();
            image.ImageUrl = url;
            image.AlternateText = String.IsNullOrEmpty(altText) ? "Edit" : altText;
            return image;
        }
    }
}
