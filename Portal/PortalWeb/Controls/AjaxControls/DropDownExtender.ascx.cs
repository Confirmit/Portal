using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using Core;

public partial class DropDownExtender : UserControl
{
    #region Fields
    
    private String m_dataTextField = String.Empty;
    private String m_dataValueField = String.Empty;

    #endregion

    #region ViewState Keys

    /// <summary>
    /// Selected object id ViewState key.
    /// </summary>
    private String objectIdKey
    {
        get { return string.Format("objectIdKey_{0}", ClientID); }
    }

    /// <summary>
    /// Selected Index in list viewstate key.
    /// </summary>
    private String selectedIndexKey
    {
        get { return string.Format("selectedIndexKey_{0}", ClientID); }
    }

    #endregion

    #region Properties

    #region Data configuration

    /// <summary>
    /// Data source for binding.
    /// </summary>
    public Object DataSource
    {
        set { gridView.DataSource = value; }
    }

    /// <summary>
    /// Name of text field information.
    /// </summary>
    public String DataTextField
    {
        set { m_dataTextField = value; }
        get { return m_dataTextField; }
    }

    /// <summary>
    /// Name of ID field.
    /// </summary>
    public String DataValueField
    {
        set { m_dataValueField = value; }
        get { return m_dataValueField; }
    }

    #endregion

    #region Selected values

    /// <summary>
    /// Selected object ID.
    /// </summary>
    public int SelectedObjectID
    {
        set
        {
            ViewState[objectIdKey] = value;

            for (int index = 0; index < gridView.DataKeys.Count; index++)
                if ((int) gridView.DataKeys[index] == SelectedObjectID)
                {
                    SelectedIndex = index;
                    break;
                }
        }
        get
        {
            if (ViewState[objectIdKey] == null)
                return -1;

            return (int)ViewState[objectIdKey];
        }
    }

    /// <summary>
    /// Selected item index.
    /// </summary>
    public int SelectedIndex
    {
        set { ViewState[selectedIndexKey] = value; }
        get
        {
            if (ViewState[selectedIndexKey] == null)
                return -1;

            return (int)ViewState[selectedIndexKey];
        }
    }

    #endregion

    #endregion

    #region Methods

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (SelectedObjectID == -1 || gridView.DataSource == null)
            return;

        setCaptionText();
    }

    /// <summary>
    /// Set text of caption.
    /// </summary>
    private void setCaptionText()
    {
        IEnumerable collection = gridView.DataSource as IEnumerable;
        if (collection == null)
            return;

        IEnumerator enumerator = collection.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            Object id = ReflectionHelper.GetPropertyValue(enumerator.Current,
                                                          DataValueField);
            if (id.Equals(SelectedObjectID))
            {
                lblCaption.Text = ReflectionHelper.GetPropertyValue(
                    enumerator.Current,
                    DataTextField).ToString();

                return;
            }
        }
    }

    /// <summary>
    /// On selected item event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnSelectItemEvent(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton) sender;
        lblCaption.Text = link.Text;

        SelectedObjectID = Int32.Parse(link.CommandArgument);
    }

    #endregion
}
