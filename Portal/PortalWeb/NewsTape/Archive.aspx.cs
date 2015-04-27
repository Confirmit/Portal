using System;
using Core;
using UlterSystems.PortalLib.NewsManager;
using Confirmit.Portal;

public partial class NewsTape_Archive : BaseWebPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        gvArchive.ShowNewsStatus = false;
        // устанавливаем источник данных для NewsGridView
        gvArchive.RequestDatasource += new GridRequestDatasourceHandler(OnRequestDatasource);
    }

    PagingResult OnRequestDatasource(object sender, PagingArgs args)
    {
        int? userID = (CurrentUser != null) ? CurrentUser.ID : null;
        PagingResult result = NewsManager.GetArchiveNews(
            args,
            userID
            );
        return result;
    }
}

