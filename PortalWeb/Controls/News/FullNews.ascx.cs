using System;
using System.Configuration;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.NewsManager;
using UlterSystems.PortalLib.NewsTape;

public partial class NewsTape_FullNews : BaseUserControl
{
    private News m_currentNews;

    #region —войства

    /// <summary>
    /// –ежим использовани€ контрола FullNews.
    /// Ќаходитс€ ли контрол в режиме предварительного просмотра.
    /// </summary>
    public bool IsInPreviewMode
    {
        set
        {
            ViewState["IsInPreviewMode"] = value;
            if (value)
                hlCaption.NavigateUrl = String.Empty;
        }
        get
        {
            if (ViewState["IsInPreviewMode"] == null)
                ViewState["IsInPreviewMode"] = false;
            return (bool)ViewState["IsInPreviewMode"];
        }
    }
    /// <summary>
    /// –ежим использовани€ контрола FullNews.
    /// Ќаходитс€ ли контрол в режиме полного отображени€ новости.
    /// </summary>
    public bool IsInFullHeightMode
    {
        set { ViewState["IsInFullHeightMode"] = value; }
        get
        {
            if (ViewState["IsInFullHeightMode"] == null)
                ViewState["IsInFullHeightMode"] = false;
            return (bool)ViewState["IsInFullHeightMode"];
        }
    }
    /// <summary>
    /// “екуща€ новость.
    /// </summary>
    public News CurrentNews
    {
        get
        {
            if (m_currentNews == null)
                m_currentNews = new News(NewsID);

            return m_currentNews;
        }
        set { m_currentNews = value; }
    }

    /// <summary>
    /// ID новости.
    /// </summary>
    public int NewsID
    {

        set { ViewState["NewsID"] = value; }
        get
        {
            if (ViewState["NewsID"] == null)
                ViewState["NewsID"] = 0;
            return (int)ViewState["NewsID"];
        }
    }

    #endregion

    /// <summary>
    /// DataBindig row.
    /// </summary>
    public override void DataBind()
    {
        hlCaption.Text = CurrentNews.Caption;
        if (!IsInPreviewMode)
            hlCaption.NavigateUrl = hlCaption.NavigateUrl + NewsID;

        Person author = new Person();
        author.Load(CurrentNews.AuthorID);
        if (!String.IsNullOrEmpty(author.PrimaryEMail))
            hlAuthorName.NavigateUrl = String.Format("mailto:{0}", author.PrimaryEMail);

        lblDateTime.Text = (CurrentNews.CreateTime.Date == DateTime.Now.Date)
                               ? GetGlobalResourceObject("NewsTape", "Today")
                                 + " " + CurrentNews.CreateTime.ToShortTimeString()
                               : CurrentNews.CreateTime.ToShortDateString()
                                 + " " + CurrentNews.CreateTime.ToShortTimeString();
        
        newsText.InnerHtml = CurrentNews.Text;
        hlDeleteNews.CommandArgument = NewsID.ToString();

        String strOffice = (CurrentNews.OfficeID == 0) // общие новости.
                               ? GetGlobalResourceObject("NewsTape", "generalNews").ToString()
                               : CurrentNews.OfficeName;

        imgNewsType.ImageUrl = (CurrentNews.OfficeID == 0) // общие новости.
                                ? ConfigurationManager.AppSettings["generalNewsImage"]
                                : ConfigurationManager.AppSettings["officeNewsImage" + CurrentNews.OfficeName];
        
        imgNewsType.ToolTip = strOffice;
        imgNewsType.AlternateText = strOffice;

        if (IsInFullHeightMode) //режим полного отображени€ новости
        {
            string[] stringSeparators = new string[] {"<LI>", "<BR>", "<li>", "<br>"};
            string[] rows = newsText.InnerHtml.Split(stringSeparators, StringSplitOptions.None);
            if (!(rows.Length > 6 || newsText.InnerHtml.Length > 3000))
                newsText.Style.Add("height", "130px");
        }
        else
            newsText.Style.Add("height", "130px");

        setLinksVisibility();
        setFilesAttachments();
        setText();
    }

    private void setText()
    {
        lblAuthorName.Text = GetLocalResourceObject("lblAuthor.Text").ToString();
        hlAuthorName.Text = CurrentNews.AuthorName;

        hlDiscussOnForum.Text = GetLocalResourceObject("hlDiscussOnForum.Text").ToString();
        hlEditNews.Text = GetLocalResourceObject("hlEditNews.Text").ToString();
        hlSendToArchive.Text = GetLocalResourceObject("hlSendToArchive.Text").ToString();
        hlDeleteNews.Text = GetLocalResourceObject("hlDeleteNews.Text").ToString();
    }

    /// <summary>
    /// Set files attachments.
    /// </summary>
    private void setFilesAttachments()
    {
        gridAttachments.DataSource = CurrentNews.Attachments;
        gridAttachments.DataBind();
    }

    private void setLinksVisibility()
    {
        if (IsInPreviewMode)
        {
            trLinks.Visible = false;
            return;
        }

        bool canEdit = Page.CurrentUser.IsCanEditNews(CurrentNews);
        hlEditNews.PostBackUrl = canEdit ? hlEditNews.PostBackUrl + NewsID
                                         : String.Empty;

        hlEditNews.Visible = canEdit;
        hlDeleteNews.Visible = canEdit;
        hlSendToArchive.Visible = canEdit;
    }

    /// <summary>
    /// ”даление новости со страницы, но не из Ѕƒ.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hlSendToArchive_Click(object sender, EventArgs e)
    {
        NewsManager.SendNewsToArchive(NewsID);
        Response.Redirect("~//NewsTape//FullNewsTape.aspx");
    }

    /// <summary>
    /// ”даление новости из Ѕƒ.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hlDeleteNews_Click(object sender, EventArgs e)
    {
        CurrentNews.Delete();
        Response.Redirect("~//NewsTape//FullNewsTape.aspx");
    }

    protected void hlDiscussOnForum_Click(object sender, EventArgs e)
    {
        if (!CurrentNews.PostID.HasValue)
        {
            // создание на форуме темы дл€ обсуждени€ новости
            Post newPost = new Post();
            newPost.Subject = CurrentNews.Caption;
            newPost.Body = CurrentNews.Text + Environment.NewLine + Environment.NewLine + "[a href=\"" + "../NewsTape/ViewNews.aspx?id=" + CurrentNews.ID + "\"]" + this.GetLocalResourceObject("BackToNews").ToString() + "[/a]";
            newPost.Username = CurrentNews.AuthorID.ToString();
            newPost.ForumID = Int32.Parse(ConfigurationManager.AppSettings["NewsDiscussForumID"]);
            newPost = Posts.AddPost(newPost);

            // обновление новости (новости в соответствие ставитс€ PostID темы из форума)
            CurrentNews.PostID = newPost.PostID;
            CurrentNews.Save();
        }
        Response.Redirect("~//Forums//ShowPost.aspx?PostID=" + CurrentNews.PostID.ToString());
    }
}
