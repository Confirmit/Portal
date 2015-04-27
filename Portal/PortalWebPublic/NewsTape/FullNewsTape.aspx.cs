using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using UlterSystems.PortalLib.NewsTape;

public partial class NewsTape_FullNewsTape : BaseWebPage
{
	protected void Page_Load( object sender, EventArgs e )
	{
		gvFullNewsTape.EmptyDataText = GetGlobalResourceObject( "NewsTape", "EmptyActualNews" ).ToString();
	    dsNews.SelectParameters["personID"].DefaultValue = (CurrentUser != null)
	                                                       ? CurrentUser.ID.ToString()
	                                                       : null;
    }

	protected void gvFullNewsTape_RowDataBound( Object sender, GridViewRowEventArgs e )
	{
		if( e.Row.RowType == DataControlRowType.DataRow )
		{
			News news = (News) e.Row.DataItem;
			
			NewsTape_FullNews fullNews = (NewsTape_FullNews) e.Row.FindControl( "fullNews" );
			fullNews.IsInPreviewMode = false;
			fullNews.IsInFullHeightMode = false;
			fullNews.NewsID = news.ID.Value;
			fullNews.CurrentNews = news;
			fullNews.DataBind();

			HtmlAnchor newsID = (HtmlAnchor) e.Row.FindControl( "newsID" );
			newsID.Name = news.ID.Value.ToString();
		}
	}
}
