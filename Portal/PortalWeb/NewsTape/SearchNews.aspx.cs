using System;
using System.Web.UI.WebControls;

using Core;

using UlterSystems.PortalLib.NewsManager;
using UlterSystems.PortalLib.DB;
using UlterSystems.PortalLib.BusinessObjects;
using Confirmit.Portal;

public partial class NewsTape_SearchNews : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //устанавливаем источник данных для NewsGridView
        gvNews.RequestDatasource += new GridRequestDatasourceHandler(OnRequestDatasource);

        if (Page.IsPostBack)
            return;

        ddlAuthors.Items.Clear();
        Person[] newsEditorsList = UserList.GetGeneralNewsEditorsList();
        ddlAuthors.Items.Add(new ListItem((String) this.GetLocalResourceObject("liAll.Text"), "0"));
        
        foreach (Person newsEditor in newsEditorsList)
        {
            ddlAuthors.Items.Add(new ListItem(newsEditor.FullName, newsEditor.ID.ToString()));
        }

        ddlOffices.Items.Clear();
        Office[] offices = Office.GetUserOffices(CurrentUser.ID.Value);
        ddlOffices.Items.Add(new ListItem((String) this.GetLocalResourceObject("liAll.Text"), "-1"));
        ddlOffices.Items.Add(new ListItem((String) this.GetLocalResourceObject("liGeneralNews.Text"), "0"));
        
        foreach (Office office in offices)
        {
            ddlOffices.Items.Add(new ListItem(office.OfficeName, office.ID.ToString()));
        }
    }

    PagingResult OnRequestDatasource(object sender, PagingArgs args)
    {
        PagingResult result = NewsManager.SearchNews(
            args,
            tbSearchText.Text,
            Int32.Parse(ddlAuthors.SelectedItem.Value),
            (DBManager.NewsStatus) ddlNewsStatus.SelectedIndex,
            CurrentUser.ID.Value,
            Int32.Parse(ddlOffices.SelectedItem.Value),
            (DBManager.SearchPeriod) ddlPeriod.SelectedIndex
            );
        return result;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvNews.Visible = true;
        gvNews.RefreshData(true);
    }
}
