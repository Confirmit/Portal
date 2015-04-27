public partial class ImageLinkButton : BaseUserControl
{
    #region Properties

    public string ImageUrl
    {
        set { img.ImageUrl = value; }
    }

    public int ImageWidth
    {
        set { img.Width = value; }
    }

    public bool IsVisibleText { get; set; }

    public string Text
    {
        set { label.Text = value; }
    }

    public string Href
    {
        set { ahref.HRef = value; }
        get { return ahref.HRef; }
    }

    public string LinkCssClass
    {
        set { label.CssClass = value; }
    }

    #endregion
}
