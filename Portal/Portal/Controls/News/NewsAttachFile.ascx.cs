using System;
using Resources;

using ConfirmIt.PortalLib.NewsTape;
using ConfirmIt.PortalLib.FilesManagers;
using ConfirmIt.PortalLib.WebServiceSupport;

using UlterSystems.PortalLib.NewsTape;

public partial class NewsAttachFile : BaseUserControl
{
    #region Fileds

    private readonly NewsAttachmentManager m_fileManager = new NewsAttachmentManager();

    #endregion

    #region Properties

    #region ViewState keys

    private String IsEditModeKey
    {
        get { return String.Format("isEditMode_key"); }
    }

    #endregion

    public bool IsEditMode
    {
        set { ViewState[IsEditModeKey] = value; }
        get
        {
            return ViewState[IsEditModeKey] == null
                       ? false
                       : (bool)ViewState[IsEditModeKey];
        }
    }

    public NewsAttachment Attachment
    {
        get { return m_attachment; }
        set { m_attachment = value; }
    }
    private NewsAttachment m_attachment = null;

    #endregion

    #region Events

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        linkDelete.Click += OnDeleteClick;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (Attachment.IsDeleted)
        {
            Visible = false;
            return;
        }

        setAttachment(Attachment);
    }

    void OnDeleteClick(object sender, EventArgs e)
    {
        if (Attachment == null)
            return;

        Attachment.IsDeleted = true;
    }

    #endregion

    #region Methods

    private void setAttachment(NewsAttachment attachment)
    {
        News curNews = new News(attachment.NewsID);
        tblDelete.Visible = (Page.CurrentUser.IsCanEditNews(curNews) && IsEditMode);

        setImage(attachment);
        hlFileName.Text = attachment.ShortFileName;

        hlFileName.NavigateUrl = m_fileManager.GetFileURL(attachment.FileName);
    }

    /// <summary>
    /// Set the image url of file.
    /// </summary>
    /// <param name="attachment">News Attachment.</param>
    private void setImage(NewsAttachment attachment)
    {
        String strFileURL = m_fileManager.GetFileURL(attachment.FileName);

        if (String.IsNullOrEmpty(strFileURL))
        {
            imgAttachImage.ImageUrl = Settings.ATTACH_FILE_NOT_FOUND_IMAGE_PATH;
            return;
        }

        imgAttachImage.ImageUrl = isImageFile(attachment.FileName)
                                      ? strFileURL
                                      : Settings.DEFAULT_ATTACH_IMAGE_PATH;
    }

    private bool isImageFile(String strFileName)
    {
        FileProperties prop = m_fileManager.GetFileProperties(strFileName);
        if (!prop.IsImageFile)
            return false;

        if (prop.Width > 100)
            if (prop.Height > prop.Width)
                imgAttachImage.Height = 100;
            else
                imgAttachImage.Width = 100;
        else
            imgAttachImage.Width = prop.Width;

        return true;
    }

    #endregion
}
