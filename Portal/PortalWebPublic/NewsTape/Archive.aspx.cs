using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UlterSystems.PortalLib.DB;
using UlterSystems.PortalLib.NewsTape;
using UlterSystems.PortalLib.NewsManager;
using Core;

public partial class NewsTape_Archive : BaseWebPage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        
        gvNews.ShowNewsStatus = false;
        //устанавливаем источник данных для NewsGridView
        gvNews.RequestDatasource += delegate(object gridSender, PagingArgs args)
        {
           
            PagingResult result = NewsManager.GetArchiveNews(
                args,
                CurrentUser.ID.Value
                );
            return result;
        };
    }
}
