using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.BAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;

namespace ConfirmIt.Portal.Books
{
	/// <summary>
	/// Page of books.
	/// </summary>
	public partial class BooksPage : BaseWebPage
	{
		#region Fields
		/// <summary>
		/// Can user edit book data.
		/// </summary>
		private bool m_CanEdit = false;
		#endregion

		protected void Page_Load( object sender, EventArgs e )
		{
			m_CanEdit = false;
            if (CurrentUser != null)
            {
                m_CanEdit = CurrentUser.IsInRole("Administrator") || CurrentUser.IsInRole("BooksEditor");
            }

			pnlAdminOperations.Visible = m_CanEdit;

			rblLanguages.Items[ 0 ].Text = (string) this.GetLocalResourceObject( "AnyLanguage" );
			ddlOffices.Items[ 0 ].Text =
				(string) this.GetLocalResourceObject( "AnyOffice" );

			gvBooks.Columns[ gvBooks.Columns.Count - 1 ].Visible = m_CanEdit;
			dvSelectedBook.DefaultMode = m_CanEdit ? DetailsViewMode.Insert : DetailsViewMode.ReadOnly;
			if( !m_CanEdit )
				dvSelectedBook.ChangeMode( DetailsViewMode.ReadOnly );

			if( !IsPostBack )
			{
				tbxToPublishingYear.Text = DateTime.Now.Year.ToString();
			}
		}

		protected void dsBooks_Selecting( object sender, ObjectDataSourceSelectingEventArgs e )
		{
			e.InputParameters[ "fromYear" ] = Convert.ToInt32( tbxFromPublishingYear.Text );

			if( string.IsNullOrEmpty( tbxToPublishingYear.Text ) )
				e.InputParameters[ "toYear" ] = DateTime.Now.Year;
			else
				e.InputParameters[ "toYear" ] = Convert.ToInt32( tbxToPublishingYear.Text );

			List<int> selectedThemes = new List<int>();
			foreach( ListItem item in cblThemes.Items )
			{
				if( item.Selected )
					selectedThemes.Add( Convert.ToInt32( item.Value ) );
			}

			e.InputParameters[ "themes" ] = selectedThemes.ToArray();

			if( rblLanguages.SelectedIndex == 0 )
				e.InputParameters[ "language" ] = null;
			else
				e.InputParameters[ "language" ] = rblLanguages.SelectedValue;

			if( ddlOffices.SelectedIndex == 0 )
				e.InputParameters[ "officeID" ] = -1;
			else
				e.InputParameters[ "officeID" ] = Convert.ToInt32( ddlOffices.SelectedValue );
		}

		protected void btnSearch_Click( object sender, EventArgs e )
		{
			gvBooks.SelectedIndex = -1;
			gvBooks.DataBind();
		}

		#region Events of grid view
		protected void gvBooks_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( m_CanEdit )
				dvSelectedBook.ChangeMode( DetailsViewMode.Edit );
			else
				dvSelectedBook.ChangeMode( DetailsViewMode.ReadOnly );
		}
		protected void gvBooks_RowCreated( object sender, GridViewRowEventArgs e )
		{
			if( e.Row.RowType == DataControlRowType.DataRow )
			{
				Book book = e.Row.DataItem as Book;

				ImageButton btn = e.Row.Cells[ e.Row.Cells.Count - 1 ].Controls[ 0 ] as ImageButton;
				if( btn != null )
				{
					btn.ID = "btnGVDelete";
					btn.OnClientClick = string.Format( "if (confirm('{0}') == false) return false; ", this.GetLocalResourceObject( "ConfirmDeleteMessage" ) );
				}

				btn = e.Row.Cells[ e.Row.Cells.Count - 2 ].Controls[ 0 ] as ImageButton;
				if( ( btn != null ) && ( book != null ) )
				{
					btn.ID = "btnGVSelect";
				}

				btn = e.Row.Cells[ e.Row.Cells.Count - 3 ].Controls[ 0 ] as ImageButton;
				if( ( btn != null ) && ( book != null ) )
				{
					btn.ID = "btnGVDownload";

					Office office = Office.GetOfficeByID( book.OfficeID );
					string message =
						string.Format(
							( (string) this.GetLocalResourceObject( "Office.Text" ) ) + @": {0}\n" + ( (string) this.GetLocalResourceObject( "Download.Text" ) ) + ": {1}",
							( office == null ) ? string.Empty : office.OfficeName,
							Server.HtmlEncode( book.DownloadLink.Replace( @"\", @"\\" ) ) );

					btn.OnClientClick = string.Format( "alert('{0}'); return false; ", message );
				}
			}
		}
		protected void gvBooks_RowDeleted( object sender, GridViewDeletedEventArgs e )
		{
			gvBooks.SelectedIndex = -1;
			gvBooks.DataBind();

			if( m_CanEdit )
				dvSelectedBook.ChangeMode( DetailsViewMode.Insert );
			else
				dvSelectedBook.ChangeMode( DetailsViewMode.ReadOnly );
		}
		protected void gvBooks_PageIndexChanging( object sender, GridViewPageEventArgs e )
		{
			if( gvBooks.SelectedIndex != -1 )
			{
				gvBooks.SelectedIndex = -1;
				gvBooks.DataBind();
				if( m_CanEdit )
					dvSelectedBook.ChangeMode( DetailsViewMode.Insert );
				else
					dvSelectedBook.ChangeMode( DetailsViewMode.ReadOnly );
			}
		}
		protected void gvBooks_DataBound( object sender, EventArgs e )
		{
			GridViewRow topPagerRow = gvBooks.TopPagerRow;
			GridViewRow bottomPagerRow = gvBooks.BottomPagerRow;

			ShowPagerData( topPagerRow );
			ShowPagerData( bottomPagerRow );
		}
		#endregion

		#region Events of details view
		protected void dvSelectedBook_ItemInserted( object sender, DetailsViewInsertedEventArgs e )
		{
			gvBooks.SelectedIndex = -1;
			gvBooks.DataBind();
		}
		protected void dvSelectedBook_ItemCommand( object sender, DetailsViewCommandEventArgs e )
		{
			if( e.CommandName == "Cancel" )
			{
				gvBooks.SelectedIndex = -1;
				gvBooks.DataBind();
			}
		}
		protected void dvSelectedBook_ItemUpdated( object sender, DetailsViewUpdatedEventArgs e )
		{
			int bookId = (int) dvSelectedBook.DataKey.Value;

			CheckBoxList cbl = dvSelectedBook.FindControl( "cblDVBookThemes" ) as CheckBoxList;
			if( cbl != null )
			{
				List<int> themeIDs = new List<int>();
				foreach( ListItem item in cbl.Items )
				{
					if( item.Selected )
						themeIDs.Add( Convert.ToInt32( item.Value ) );
				}

				Book.SetThemes( bookId, themeIDs.ToArray() );
			}

			gvBooks.SelectedIndex = -1;
			gvBooks.DataBind();
		}
		protected void dvSelectedBook_DataBound( object sender, EventArgs e )
		{
			Book book = dvSelectedBook.DataItem as Book;
			if( book == null )
				return;

			DropDownList ddl =
				dvSelectedBook.FindControl( "ddlDVLanguage" ) as DropDownList;
			if( ddl != null )
			{
				ddl.SelectedValue = book.Language;
			}

			ddl = dvSelectedBook.FindControl( "ddlDVOffices" ) as DropDownList;
			if( ddl != null )
			{
				ddl.SelectedValue = book.OfficeID.ToString();
			}

			CheckBoxList cbl = dvSelectedBook.FindControl( "cblDVBookThemes" ) as CheckBoxList;
			if( cbl != null )
			{
				BookTheme[] themes = book.Themes;
				List<int> themeIDs = new List<int>( themes.Length );
				foreach( BookTheme theme in themes )
				{
					themeIDs.Add( theme.ID.Value );
				}
				foreach( ListItem item in cbl.Items )
				{
					item.Selected = themeIDs.Contains( Convert.ToInt32( item.Value ) );
				}
			}
		}
		protected void dvSelectedBook_ItemInserting( object sender, DetailsViewInsertEventArgs e )
		{
			if( !m_CanEdit )
			{
				e.Cancel = true;
				return;
			}

			DropDownList ddlLanguage =
				dvSelectedBook.FindControl( "ddlDVLanguage" ) as DropDownList;
			if( ddlLanguage != null )
			{
				e.Values[ "Language" ] = ddlLanguage.SelectedValue;
			}

			CheckBoxList cbl = dvSelectedBook.FindControl( "cblDVBookThemes" ) as CheckBoxList;
			if( cbl != null )
			{
				List<int> themeIDs = new List<int>();
				foreach( ListItem item in cbl.Items )
				{
					if( item.Selected )
					{
						themeIDs.Add( Convert.ToInt32( item.Value ) );
					}
				}
				e.Values[ "Themes" ] = themeIDs.ToArray();
			}

			DropDownList ddl = dvSelectedBook.FindControl( "ddlDVOffices" ) as DropDownList;
			if( ddl != null )
			{
				e.Values[ "OfficeID" ] = Convert.ToInt32( ddl.SelectedValue );
			}
		}
		protected void dvSelectedBook_ItemUpdating( object sender, DetailsViewUpdateEventArgs e )
		{
			if( !m_CanEdit )
			{
				e.Cancel = true;
				return;
			}

			DropDownList ddlLanguage =
				dvSelectedBook.FindControl( "ddlDVLanguage" ) as DropDownList;
			if( ddlLanguage != null )
			{
				e.NewValues[ "Language" ] = ddlLanguage.SelectedValue;
			}

			DropDownList ddl = dvSelectedBook.FindControl( "ddlDVOffices" ) as DropDownList;
			if( ddl != null )
			{
				e.NewValues[ "OfficeID" ] = Convert.ToInt32( ddl.SelectedValue );
			}
		}
		protected void dvSelectedBook_ItemCreated( object sender, EventArgs e )
		{
			if( dvSelectedBook.CurrentMode == DetailsViewMode.Insert )
			{
				CheckBox cbIsElectronicEdit =
					dvSelectedBook.FindControl( "cbDVIsElectronicEdit" ) as CheckBox;
				if( cbIsElectronicEdit != null )
				{
					cbIsElectronicEdit.Checked = true;
				}

				TextBox tbxDownloadLink =
					dvSelectedBook.FindControl( "tbxDVDownloadLink" ) as TextBox;
				if( tbxDownloadLink != null )
				{
					tbxDownloadLink.Text = Globals.Settings.RequestObjects.DownloadBasePath;
				}

				TextBox tbxPublishingYear =
					dvSelectedBook.FindControl( "tbxDVPublishingYear" ) as TextBox;
				if( tbxPublishingYear != null )
				{
					tbxPublishingYear.Text = DateTime.Now.Year.ToString();
				}
			}
		}
		#endregion

		#region Events of pager controls
		protected virtual void OnPageIndexChanged( object sender, EventArgs e )
		{
			DropDownList ddl = (DropDownList) sender;

			gvBooks.PageIndex = Convert.ToInt32( ddl.SelectedValue ) - 1;
		}

		protected virtual void OnPageSizeChanged( object sender, EventArgs e )
		{
			DropDownList ddl = (DropDownList) sender;

			gvBooks.PageSize = Convert.ToInt32( ddl.SelectedValue );
		}
		#endregion

		#region Methods
		/// <summary>
		/// Shows correct pager information.
		/// </summary>
		/// <param name="pagerRow">Pager row.</param>
		private void ShowPagerData( Control pagerRow )
		{
			if( pagerRow == null )
				return;

			DropDownList ddlPages = pagerRow.FindControl( "ddlPage" ) as DropDownList;
			Literal lbl = pagerRow.FindControl( "lblPageCount" ) as Literal;
			DropDownList ddlPSize = pagerRow.FindControl( "ddlPageSize" ) as DropDownList;

			if( ( ddlPages != null ) && ( lbl != null ) && ( ddlPSize != null ) )
			{
				ddlPages.Items.Clear();
				for( int pageIndex = 1; pageIndex <= gvBooks.PageCount; pageIndex++ )
				{
					ListItem item = new ListItem( pageIndex.ToString() );
					if( pageIndex == gvBooks.PageIndex + 1 )
						item.Selected = true;
					ddlPages.Items.Add( item );
				}

				lbl.Text = gvBooks.PageCount.ToString();

				ddlPSize.SelectedValue = gvBooks.PageSize.ToString();
			}
		}
		#endregion
	}
}
