using System;

using UlterSystems.PortalLib.NewsTape;

public partial class NewsTape_Preview : BaseNewsPage
{
    private News m_currentNews;
    /// <summary>
    /// “екуща€ новость.
    /// </summary>
    override public News CurrentNews
    {
        get
        {
            if (m_currentNews == null)
            {
                String key = Request.Params["news"];
                m_currentNews = new News();
                m_currentNews = (News)Session[key];
                if (m_currentNews == null)
                    Response.Redirect("AccessDenied.aspx");
                //int[] officeIDs = NewsManager.GetUserOfficesIDs(CurrentUser.ID.Value);
                //if ( Array.IndexOf(officeIDs, m_currentNews) == -1)
                //    Response.Redirect("AccessDenied.aspx");
            }
            return m_currentNews;
        }
        set
        {
            m_currentNews = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        fullNews.IsInFullHeightMode = true;
        fullNews.IsInPreviewMode = true;
        fullNews.CurrentNews = CurrentNews;
        fullNews.DataBind();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (NewsID.HasValue)
        {
            //NewsManager.DeleteNews(NewsID.Value);
            //DateTime expDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            //CurrentNews.ExpireTime = expDateTime;
            //CurrentNews.Save();
            CurrentNews.Delete();
            Response.Redirect("~/default.aspx");
        }
        else
        {
            Response.Redirect("~/default.aspx");
        }
    }
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        if (NewsID.HasValue)                                    //редактирование
        {
            CurrentNews.Save();
            Response.Redirect("~//NewsTape//FullNewsTape.aspx#" + NewsID.Value.ToString());

        }
        else                                                    //создание
        {
            CurrentNews.CreateTime = DateTime.Now;
            CurrentNews.Save();
            Response.Redirect("~//default.aspx");
        }
    }
}
