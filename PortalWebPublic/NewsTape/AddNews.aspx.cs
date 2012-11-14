using System;
using System.Text;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.FilesManagers;

using UlterSystems.PortalLib.NewsManager;
using UlterSystems.PortalLib.NewsTape;
using UlterSystems.PortalLib.Notification;
using Office=UlterSystems.PortalLib.BusinessObjects.Office;

public partial class NewsTape_AddNews : BaseNewsPage
{
    #region Fields

    private readonly string m_uploadObjectName = "uploadFileObject";
    private readonly string m_attachID = "fileUploader";
    private NewsAttachmentManager m_fileManager = new NewsAttachmentManager();

    #endregion

    protected void RegisterJavaScript()
    {
        if (!(Page.ClientScript.IsClientScriptIncludeRegistered("FileUploader")))
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "FileUploader"
                                                          , "FileUploader.js");

        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript' type='text/javascript'>");
        strScript.AppendFormat("var {0} = new FileUploader('{1}', '{2}', '{0}');",
                               m_uploadObjectName,
                               div_newsAttachments.ClientID,
                               m_attachID);
        strScript.Append("</script>");

        if (!(Page.ClientScript.IsStartupScriptRegistered("InitializeUploadObject")))
            Page.ClientScript.RegisterStartupScript(GetType(), "InitializeUploadObject"
                                                          , strScript.ToString());
    }

    /// <summary>
    /// Маркер новости, который используется доступа к редактируемой новости в сессии. 
    /// Значение маркера находится во ViewState.
    /// </summary>
    private string CurrentNewsMarker
    {
        get
        {
            Guid objid;

            if (ViewState["CurrentNewsMarker"] != null)
            {
                // если guid объекта уже сформирован
                objid = (Guid)ViewState["CurrentNewsMarker"];
            }
            else
            {
                // если guid еще не сформирован
                // формируем новый guid
                objid = Guid.NewGuid();
                // кладем во ViewState
                ViewState["CurrentNewsMarker"] = objid;
            }
            return objid.ToString();
        }
    }

    /// <summary>
    /// Редактируемая новость.
    /// </summary>
    public override News CurrentNews
    {
        get
        {
            // берем объект
            object obj = Session[CurrentNewsMarker];
            News news;
            if (obj != null)
            {
                news = (News)obj;
            }
            else
            {
                // если объект не найден, то создаем новый
                news = base.CurrentNews;
                // записываем созданный объект в сессию
                Session[CurrentNewsMarker] = news;
            }
            return news;
        }
        set
        {
            Session[CurrentNewsMarker] = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        RegisterJavaScript();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Проверка доступа
        if (CurrentUser == null)
            Response.Redirect("~//Default.aspx");

        if (!CurrentUser.IsInRole(RolesEnum.GeneralNewsEditor) 
            && !CurrentUser.IsInRole(RolesEnum.OfficeNewsEditor))
            Response.Redirect("~//Default.aspx");

        //формирование переключателя офисов
        if (!Page.IsPostBack)
        {
            String sGeneralNews = GetGlobalResourceObject("NewsTape", "generalNews").ToString();
            rbOffice.Items.Clear();
            ListItem li = new ListItem(sGeneralNews, "0");
            li.Selected = true;
            rbOffice.Items.Add(li);

            Office[] offices = Office.GetUserOffices(CurrentUser.ID.Value);
            foreach (Office office in offices)
            {
                rbOffice.Items.Add(new ListItem(office.OfficeName, office.ID.ToString()));
            }
        }

        setFilesAttachments();

        if (Page.IsPostBack) //страница запрошена впервые
            return;

        if (NewsID.HasValue) //редактирование
        {
            foreach (ListItem li in rbOffice.Items)
            {
                if (Int32.Parse(li.Value) == CurrentNews.OfficeID)
                {
                    li.Selected = true;
                    break;
                }
            }

            if (!CurrentUser.IsCanEditNews(CurrentNews))
                Response.Redirect("~//NewsTape//AccessViolation.aspx");
            else
            {
                tbCaptionNews.Text = CurrentNews.Caption;
                tbTextNews.Text = CurrentNews.Text;
                Calendar.SelectedDate = CurrentNews.ExpireTime;
                lblConfirmExpDate.Visible = (Calendar.SelectedDate <= DateTime.Now);
            }
        }
        else //создание
        {
            Calendar.SelectedDate = DateTime.Now.Date;
            if (Calendar.SelectedDate <= DateTime.Now)
                lblConfirmExpDate.Visible = true;
        }
    }

    /// <summary>
    /// Set files attachments.
    /// </summary>
    private void setFilesAttachments()
    {
        gridAttachments.DataSource = CurrentNews.Attachments;
        gridAttachments.DataBind();
    }
    
    /// <summary>
    /// Опубликование новости.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddNews_Click(object sender, EventArgs e)
    {
        String tag = String.Empty;
        FillNews();

        if (NewsManager.SecurityCheckNews(CurrentNews, out tag) == false)
        {
            if (tag == String.Empty)
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "AddNews",
                                                            "<script type='text/javascript'> alert('" +
                                                            GetLocalResourceObject("TagsAreNotAllowedInCaption").
                                                                ToString() + "')</script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "AddNews",
                                                            "<script type='text/javascript'> alert('" +
                                                            GetLocalResourceObject("NotAllowedTag").ToString() + tag +
                                                            "')</script>");
        }
        else
        {
            attachNewFiles();
            deleteMarkedAttachFiles();

            String redirectURL = String.Empty;

            if (NewsID.HasValue) //редактирование
            {
                CurrentNews.Save();
                redirectURL = "~//NewsTape//FullNewsTape.aspx#" + NewsID.Value.ToString();

                //if (CurrentNews.PostID.HasValue)// если существует обсуждение новости на форуме
                //{
                //    Post newPost = new Post();
                //    newPost.Subject = CurrentNews.Caption;
                //    newPost.Body = CurrentNews.Text;
                //    newPost.Username = CurrentNews.AuthorID.ToString();
                //    Posts.UpdatePost(newPost, false);
                //}
            }
            else //создание
            {
                CurrentNews.CreateTime = DateTime.Now;
                CurrentNews.Save();

                redirectURL = "~//default.aspx";
            }
            sendNewsNotifications();
            Response.Redirect(redirectURL);
        }
    }

    /// <summary>
    /// Заполнение полей объекта Новость.
    /// </summary>
    protected void FillNews()
    {
        CurrentNews.Caption = tbCaptionNews.Text;
        CurrentNews.Text = NewsManager.TextFormatting(tbTextNews.Text);
        CurrentNews.ExpireTime = this.Calendar.SelectedDate;
        CurrentNews.OfficeID = Int32.Parse(rbOffice.SelectedItem.Value);

        if (!NewsID.HasValue)
            CurrentNews.AuthorID = CurrentUser.ID.Value;

        //foreach (String fileName in NewsAttachmentsColl)
        //{
        //    NewsAttachment attach = new NewsAttachment();
        //    attach.FileName = fileName;
        //    CurrentNews.Attachments.Add(attach);
        //}
    }

    /// <summary>
    /// Предварительный просмотр новости.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        String tag = String.Empty;
        FillNews();

        if (NewsManager.SecurityCheckNews(CurrentNews, out tag) == false)
        {
            if (tag == String.Empty)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddNews", "<script type='text/javascript'> alert('" + GetLocalResourceObject("TagsAreNotAllowedInCaption").ToString() + "')</script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddNews", "<script type='text/javascript'> alert('" + GetLocalResourceObject("NotAllowedTag").ToString() + tag + "')</script>");
        }
        else
        {
            if (!NewsID.HasValue)           //создание
                CurrentNews.CreateTime = DateTime.Now;

            string key = CurrentNews.GetHashCode().ToString();
            Session[key] = CurrentNews;
            Response.Redirect(String.Format("~//NewsTape//Preview.aspx?id={0}&news={1}", NewsID, key));
        }
    }

    protected void btnDeleteNews_Click(object sender, EventArgs e)
    {
        if (NewsID.HasValue)
        {
            //DateTime expDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            //CurrentNews.ExpireTime = expDateTime;
            CurrentNews.Delete();
        }
        Response.Redirect("~/NewsTape/FullNewsTape.aspx");
    }

    protected void OnSelectionChanged(object sender, EventArgs e)
    {
        lblConfirmExpDate.Visible = (((System.Web.UI.WebControls.Calendar) sender).SelectedDate <= DateTime.Now.Date)
                                        ? true
                                        : false;
    }

    /// <summary>
    /// Загрузка файлов на сервер.
    /// </summary>
    private void attachNewFiles()
    {
        m_fileManager.SaveNewsAttachmentFiles(Request.Files, CurrentNews);
    }

    /// <summary>
    /// удаление файлов, помеченных на удаление.
    /// </summary>
    private void deleteMarkedAttachFiles()
    {
        m_fileManager.DeleteAttachFiles(CurrentNews.Attachments);
    }

    /// <summary>
    /// Oтправка уведомлений о новости на электронную почту.
    /// </summary>
    private void sendNewsNotifications()
    {
        if (!cbSendNotification.Checked)
            return;

        string Message = CurrentNews.Caption
                         + "[br]"
                         + CurrentNews.Text
                         + "[br]"
                         + GetLocalResourceObject("Author")
                         + CurrentNews.AuthorName
                         + "[br]"
                         + CurrentNews.CreateTime
                         + "[br]"
                         + "[a href=\"http://portal/NewsTape/ViewNews.aspx?id=" 
                         + CurrentNews.ID 
                         + "\"]" 
                         + GetLocalResourceObject("NewsLink") + "[/a]";
        NewsNotification newsNotification = new NewsNotification(Message, (int)CurrentNews.ID);
        newsNotification.SendNews();
    }
}
