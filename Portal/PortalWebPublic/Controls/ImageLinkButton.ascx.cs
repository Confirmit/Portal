public partial class ImageLinkButton : BaseUserControl
{
    #region Properties

    public string ImageUrl
    {
        set { img.ImageUrl = value; }
    }

    public string Text
    {
        set { label.Text = value; }
    }

    public string Href
    {
        set { ahref.HRef = value; }
    }

    #endregion
}
