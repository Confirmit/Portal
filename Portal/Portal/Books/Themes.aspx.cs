using System;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;

namespace ConfirmIt.Portal.Books
{
	/// <summary>
	/// Page for book themes management.
	/// </summary>
	public partial class ThemesPage : BaseWebPage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			lblErrorDescription.Text = string.Empty;
		}

		#region Event handlers for grid view
		protected void gvThemes_SelectedIndexChanged( object sender, EventArgs e )
		{
			dvBookTheme.ChangeMode( DetailsViewMode.Edit );
		}

		protected void gvThemes_RowCreated( object sender, GridViewRowEventArgs e )
		{
			if( e.Row.RowType == DataControlRowType.DataRow )
			{
				ImageButton btn = e.Row.Cells[ e.Row.Cells.Count - 1 ].Controls[ 0 ] as ImageButton;
				if( btn != null )
				{
					btn.OnClientClick = string.Format( "if (confirm('{0}') == false) return false; ", this.GetLocalResourceObject( "ConfirmDeleteMessage" ) );
				}
			}
		}

		protected void gvThemes_RowDeleted( object sender, GridViewDeletedEventArgs e )
		{
			gvThemes.SelectedIndex = -1;
			gvThemes.DataBind();
			dvBookTheme.ChangeMode( DetailsViewMode.Insert );
		}
		#endregion

		#region Event handlers for details view
		protected void dvBookTheme_ItemInserted( object sender, DetailsViewInsertedEventArgs e )
		{
			gvThemes.SelectedIndex = -1;
			gvThemes.DataBind();
		}

		protected void dvBookTheme_ItemCommand( object sender, DetailsViewCommandEventArgs e )
		{
			if( e.CommandName == "Cancel" )
			{
				gvThemes.SelectedIndex = -1;
				gvThemes.DataBind();
			}
		}

		protected void dvBookTheme_ItemInserting( object sender, DetailsViewInsertEventArgs e )
		{
			Controls_MLTextBox mltbThemeName = dvBookTheme.FindControl( "mltbThemeName" ) as Controls_MLTextBox;
			if( mltbThemeName != null )
			{
				e.Values[ "Name" ] = mltbThemeName.MultilingualText;
				if( string.IsNullOrEmpty( mltbThemeName.MultilingualText.ToString() ) )
				{
					e.Cancel = true;
					lblErrorDescription.Text = (string) this.GetLocalResourceObject( "NoThemeName" );
					return;
				}
			}
		}

		protected void dvBookTheme_DataBound( object sender, EventArgs e )
		{
			BookTheme theme = dvBookTheme.DataItem as BookTheme;
			if( theme == null )
				return;

			Controls_MLTextBox mltbThemeName = dvBookTheme.FindControl( "mltbThemeName" ) as Controls_MLTextBox;
			if( mltbThemeName != null )
			{
				mltbThemeName.MultilingualText = theme.Name;
			}
		}

		protected void dvBookTheme_ItemUpdating( object sender, DetailsViewUpdateEventArgs e )
		{
			Controls_MLTextBox mltbThemeName = dvBookTheme.FindControl( "mltbThemeName" ) as Controls_MLTextBox;
			if( mltbThemeName != null )
			{
				e.NewValues[ "Name" ] = mltbThemeName.MultilingualText;
				if( string.IsNullOrEmpty( mltbThemeName.MultilingualText.ToString() ) )
				{
					e.Cancel = true;
					lblErrorDescription.Text = (string) this.GetLocalResourceObject( "NoThemeName" );
					return;
				}
			}
		}

		protected void dvBookTheme_ItemUpdated( object sender, DetailsViewUpdatedEventArgs e )
		{
			gvThemes.SelectedIndex = -1;
			gvThemes.DataBind();
		}
		#endregion

	}
}
