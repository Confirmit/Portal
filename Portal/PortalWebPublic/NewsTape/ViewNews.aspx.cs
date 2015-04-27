using System;

using UlterSystems.PortalLib.NewsTape;

public partial class NewsTape_ViewNews : BaseNewsPage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((CurrentNews != null) && (NewsID != null))
        {
            fullNews.IsInPreviewMode = false;
            fullNews.IsInFullHeightMode = true;

            //if ((!(CurrentUser.IsInRole(Group.GroupsEnum.OfficeNewsEditor))) &&
            //      (!(CurrentUser.IsInRole(Group.GroupsEnum.GeneralNewsEditor))))
            //{
            //    //this.btnEdit.Visible = false;
            //    //this.btnDeleteNews.Visible = false;
            //}

            //TODO: ���� ���� �������� ���� ���������� ��� �������������� ������� �� ������, �� ��� �����.
            //CurrentNews.Time = DateTime.Now;
            fullNews.NewsID = this.NewsID.Value;
            //fullNews.CurrentNews = CurrentNews;
            fullNews.DataBind();
        }
        else
            Response.Redirect("AccessDenied.aspx");

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (NewsID.HasValue)
            Response.Redirect("~/NewsTape/AddNews.aspx?id=" + NewsID.ToString());
    }

    protected void btnDeleteNews_Click(object sender, EventArgs e)
    {
        if (CurrentNews.AuthorID != CurrentUser.ID)//�� ����
        {
            Response.Redirect("~//NewsTape//AccessViolation.aspx");
        }
        else //����
        {
            DateTime expDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            CurrentNews.ExpireTime = expDateTime;
            CurrentNews.Save();
            Response.Redirect("~/NewsTape/FullNewsTape.aspx");
        }
    }
}
