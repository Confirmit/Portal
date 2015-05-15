using System;

public partial class NewsTape_NewsTopMenu : BaseUserControl
{
	protected void Page_Load( object sender, EventArgs e )
	{
		if( Page.CurrentUser != null &&
            (Page.CurrentUser.IsInRole( "GeneralNewsEditor" ) ||
			Page.CurrentUser.IsInRole( "OfficeNewsEditor" )))
		{
			hlAddNews.Visible = true;
		}
		else
			hlAddNews.Visible = false;

        //----Set text
	    search.Text = (string)GetLocalResourceObject("hlSearch.Text");
        archive.Text = (string)GetLocalResourceObject("hlArchive.Text");
        hlAddNews.Text = (string)GetLocalResourceObject("hlAddNews.Text");
	}
}
