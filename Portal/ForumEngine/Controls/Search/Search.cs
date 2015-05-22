using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;

namespace AspNetForums.Controls.Search
{

    // *********************************************************************
    //  Search
    //
    /// <summary>
    /// This Web control presents the Web controls for searching the Web site.  Additionally,
    /// the search results are displayed via this control.  This Web control allows the user
    /// to visit the page and enter search criteria, and also for search criteria to be passed
    /// through the QueryString or Form collections to have the search results automatically
    /// displayed.
    /// </summary>
    /// 
    // ********************************************************************/ 
    [
    ParseChildren(true)
    ]
    public class Search : WebControl, INamingContainer
    {
        DataGrid searchResultsDataGrid;
        DropDownList resultsPerPage;
        DropDownList matchingWords;
        DropDownList forumsToSearch;
        TextBox textToSearchFor;
        Panel searchResultsPanel;

        const bool defaultAllowSearchAllForums = true;			            // the default choice on whether the user can search all forums or not
        const bool defaultShowBody = true;
        // the default title to show at the top of the search page
        String defaultSearchTitle;

        const int defaultContainingTextBoxMaxLength = 250;		            // the default max # of characters for the Containing textbox
        const int defaultContainingTextBoxColumns = 50;		                // the default # of cols. in the Containing textbox
        // the default text for the submit button
        String defaultSearchButtonText;
        const int defaultMaxBodyLength = 500;				                // how many characters to show of a body, at most

        String defaultNoRecordsMessage;
        String defaultRFVErrorMessage;

        int userTimeZoneOffset = Globals.DBTimezone;                        // store the user's time zone setting

        SearchPagerStyle datagridPagerStyle = new SearchPagerStyle();       // create the DataGrid for displaying the results

        // *********************************************************************
        //  CreateChildControls
        //
        /// <summary>
        /// This event handler adds the children controls.
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void CreateChildControls()
        {
            User user;

            defaultSearchTitle =
                HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "defaultSearchTitle").ToString();

            defaultSearchButtonText = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "defaultSearchButtonText").ToString();
            defaultNoRecordsMessage = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "defaultNoRecordsMessage").ToString();
            defaultRFVErrorMessage = Globals.HtmlNewLine + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "defaultRFVErrorMessage").ToString();


            // Get the timezone offset
            if (Page.Request.IsAuthenticated)
            {
                user = Users.GetLoggedOnUser();
                //userTimeZoneOffset = user.Timezone;
            }

            // Create the table used to display the search options
            Controls.Add(CreateSearchOptionsControl());

            // create the label that will display the "No Records Found" message, if needed
            Label lblTmp = new Label();
            lblTmp.CssClass = "normalTextSmallBold";
            lblTmp.ID = "lblNoResults";
            lblTmp.Visible = false;
            lblTmp.Text = NoRecordsFoundMessage;
            Controls.Add(lblTmp);

            // Create the panel that displays the search results
            CreateSearchResultsPanel();

        }


        // *********************************************************************
        //  DataGrid_PageIndexChange
        //
        /// <summary>
        /// This event handler fires when the user clicks the Next or Prev page
        /// links on the search page.  It simply assigns the new Page Index to
        /// the DataGrid's CurrentPageIndex and rebinds the data
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void DataGrid_PageIndexChange(Object sender, DataGridPageChangedEventArgs e)
        {
            searchResultsDataGrid.CurrentPageIndex = e.NewPageIndex;		// set the page index
            DisplaySearchResults();		// rebind the data
        }

        // *********************************************************************
        //  CreateSearchResultsPanel
        //
        /// <summary>
        /// This function creates the panel to display the search results in.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void CreateSearchResultsPanel()
        {
            Table table;
            TableHeaderCell th;
            TableRow tr;
            TableCell td;
            Label searchResultsText;

            // Create the panel
            searchResultsPanel = new Panel();
            searchResultsPanel.ID = "panelSearchResults";
            searchResultsPanel.Visible = false;

            // Create and set some properties on the table
            table = new Table();
            table.CellPadding = 3;
            table.CellSpacing = 1;
            table.Width = Unit.Percentage(100);
            table.CssClass = "tableBorder";

            // Display the title
            tr = new TableRow();
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.HorizontalAlign = HorizontalAlign.Left;
            searchResultsText = new Label();
            searchResultsText.ID = "lblInformation";
            th.Controls.Add(searchResultsText);
            tr.Controls.Add(th);
            table.Controls.Add(tr);

            // Add some white space
            searchResultsPanel.Controls.Add(new LiteralControl("<P></P>"));

            // Add the search results
            tr = new TableRow();
            td = new TableCell();

            // add the dataGrid
            searchResultsDataGrid = new DataGrid();
            searchResultsDataGrid.ID = "searchResultsDataGrid";
            searchResultsDataGrid.CellPadding = 5;
            searchResultsDataGrid.CellSpacing = 0;
            searchResultsDataGrid.BorderWidth = 0;
            searchResultsDataGrid.AllowCustomPaging = true;
            searchResultsDataGrid.AllowPaging = true;
            searchResultsDataGrid.PageIndexChanged += new DataGridPageChangedEventHandler(DataGrid_PageIndexChange);

            // Set the pager style attributes
            searchResultsDataGrid.PagerStyle.CopyFrom(datagridPagerStyle);
            searchResultsDataGrid.PagerStyle.CssClass = "searchPager";
            searchResultsDataGrid.PagerStyle.Position = datagridPagerStyle.Position;
            searchResultsDataGrid.PagerStyle.Mode = datagridPagerStyle.Mode;
            searchResultsDataGrid.PagerStyle.NextPageText = datagridPagerStyle.NextPageText;
            searchResultsDataGrid.PagerStyle.PageButtonCount = datagridPagerStyle.PageButtonCount;
            searchResultsDataGrid.PagerStyle.PrevPageText = datagridPagerStyle.PrevPageText;
            searchResultsDataGrid.PagerStyle.Visible = datagridPagerStyle.Visible;
            searchResultsDataGrid.PagerStyle.HorizontalAlign = HorizontalAlign.Right;

            // Set the item styles
            searchResultsDataGrid.ItemStyle.CssClass = "searchItem";
            searchResultsDataGrid.AlternatingItemStyle.CssClass = "searchAlternatingItem";

            // Set some display properties
            searchResultsDataGrid.Width = Unit.Percentage(100.0);
            searchResultsDataGrid.AutoGenerateColumns = false;
            searchResultsDataGrid.ShowHeader = false;

            // Add a template column to the Columns property - this column is used to display count
            TemplateColumn searchResultsCountColumn = new TemplateColumn();
            searchResultsCountColumn.ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildCountItemTemplate));
            searchResultsCountColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            searchResultsCountColumn.ItemStyle.VerticalAlign = VerticalAlign.Top;
            searchResultsDataGrid.Columns.Add(searchResultsCountColumn);

            // Add a template column to the Columns property - this column is used to display results
            TemplateColumn searchResultsColumn = new TemplateColumn();
            searchResultsColumn.ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildItemTemplate));
            searchResultsDataGrid.Columns.Add(searchResultsColumn);

            // Add the datagrid with our search results
            td.Controls.Add(searchResultsDataGrid);
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add the table
            searchResultsPanel.Controls.Add(table);

            this.Controls.Add(searchResultsPanel);
        }

        // *********************************************************************
        //  BuildItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for the datalist.
        /// It calls BeginBuildItemTemplate, which creates the DataBoundLiteralControl
        /// needed.  It then adds this control to the parser object
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void BuildItemTemplate(Control _ctrl)
        {

            // add the DataBoundLiteralControl to the parser
            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(BeginBuildItemTemplate());
        }

        // *********************************************************************
        //  BuildCountItemTemplate
        //
        /// <summary>
        /// This function is called to create a template for the datalist.
        /// This column displays the count for items returned.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void BuildCountItemTemplate(Control _ctrl)
        {
            Label label;

            // add the DataBoundLiteralControl to the parser
            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.DataBinding += new System.EventHandler(DataBindResultCount);

            __parser.AddParsedSubObject(label);
        }

        // *********************************************************************
        //  BeginBuildItemTemplate
        //
        /// <summary>
        /// This function creates a new DataBoundLiteralControl and populates
        /// the Static Strings.  Additionally, the DataBinding event handler is
        /// wired up to a local event handler.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private Control BeginBuildItemTemplate()
        {
            PlaceHolder placeHolder;
            PlaceHolder postDetails;
            HyperLink hyperlink;
            Label postBody;

            // Create the PlaceHolder control
            postDetails = new PlaceHolder();

            // Display the title of the post
            hyperlink = new HyperLink();
            hyperlink.CssClass = "normalTextSmallBold";
            hyperlink.DataBinding += new System.EventHandler(DataBindTitle);
            postDetails.Controls.Add(hyperlink);

            // Line break
            postDetails.Controls.Add(new LiteralControl("<br>"));

            // Display post details
            placeHolder = new PlaceHolder();
            placeHolder.DataBinding += new System.EventHandler(DataBindPostDetails);
            postDetails.Controls.Add(placeHolder);

            // Are we displaying the post body?
            if (ShowBody)
            {
                // Line break
                postDetails.Controls.Add(new LiteralControl("<br>"));

                postBody = new Label();
                postBody.CssClass = "normalTextSmaller";
                postBody.DataBinding += new System.EventHandler(DataBindPostBody);
                postDetails.Controls.Add(postBody);
            }

            return postDetails;
        }

        // *********************************************************************
        //  DataBindResultCount
        //
        /// <summary>
        /// Handles databind for counting results.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void DataBindResultCount(Object sender, EventArgs e)
        {
            int count = 0;
            Label countLabel = (Label)sender;
            DataGridItem container = (DataGridItem)countLabel.NamingContainer;

            count = searchResultsDataGrid.PageSize * searchResultsDataGrid.CurrentPageIndex;
            count += +(container.ItemIndex + 1);
            countLabel.Text = count.ToString() + ".";
        }


        // *********************************************************************
        //  DataBindPostBody
        //
        /// <summary>
        /// Handles databind for the post body.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void DataBindPostBody(Object sender, EventArgs e)
        {
            Label body = (Label)sender;
            DataGridItem container = (DataGridItem)body.NamingContainer;
            Post post = (Post)container.DataItem;
            String postBody = Globals.FormatPostBody(post.Body);

            // We only want to display some of the body
            if (postBody.Length > MaxBodyLengthToDisplay)
            {
                int whitespace = 0;
                // Clip the body
                postBody = postBody.Substring(0, MaxBodyLengthToDisplay);

                // Find the last occurence of a space
                whitespace = postBody.LastIndexOf(" ");

                // Rebuild postBody string
                postBody = postBody.Substring(0, whitespace) + " ...";
            }

            body.Text = postBody;
        }

        // *********************************************************************
        //  DataBindTitle
        //
        /// <summary>
        /// Handles databind for the post title.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void DataBindTitle(Object sender, EventArgs e)
        {
            HyperLink title = (HyperLink)sender;
            DataGridItem container = (DataGridItem)title.NamingContainer;
            Post post = (Post)container.DataItem;

            title.Text = post.Subject;
            title.NavigateUrl = Globals.UrlShowPost + post.PostID;

            int c = Threads.GetTotalPostsForThread(post.ThreadID);


            if ((c + 1) > Globals.PageSize)
            {
                int totalPages = Paging.CalculateTotalPages(c, Globals.PageSize, Threads.GetTotalPinnedPostsForThread(post.ThreadID));

                // Newest post will be on the last page
                title.NavigateUrl = Globals.UrlShowPost + post.ThreadID + "&PageIndex=" + totalPages + "#" + post.PostID;

            }
            else
            {
                title.NavigateUrl = Globals.UrlShowPost + post.ThreadID + "#" + post.PostID;
            }

        }

        // *********************************************************************
        //  DataBindPostDetails
        //
        /// <summary>
        /// Handles databind for the post details, e.g. who and at what time.
        /// </summary>
        /// 
        // ********************************************************************/ 
        private void DataBindPostDetails(Object sender, EventArgs e)
        {
            Label label;
            PlaceHolder postDetails = (PlaceHolder)sender;
            DataGridItem container = (DataGridItem)postDetails.NamingContainer;
            Post post = (Post)container.DataItem;
            DateTime postDate;

            // Construct post details
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "posted").ToString();
            postDetails.Controls.Add(label);

            // Author
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.Text = Users.GetUserInfo(post.Username).DisplayName;
            postDetails.Controls.Add(label);

            // Posted on date
            postDate = post.PostDate;
            label = new Label();
            label.CssClass = "normalTextSmall";

            // Is the post an announcement
            if (post.IsAnnouncement)
            {
                label.Text = " <b>(" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "Announcement").ToString() + ")</b>";
            }
            else if (DateTime.Now < postDate)
            {
                label.Text = " <b>(" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "Pined").ToString() + ")</b>";
            }
            else
            {
                label.Text = " " + post.PostDate.ToString();
            }

            postDetails.Controls.Add(label);

        }


        // *********************************************************************
        //  CreateSearchOptionsControl
        //
        /// <summary>
        /// This function returns the control that displays the search options.
        /// </summary>
        /// <returns>Control used to render search options.</returns>
        /// 
        // ********************************************************************/ 
        private Control CreateSearchOptionsControl()
        {

            Table table;
            TableRow tr;
            TableCell td;
            TableHeaderCell th;
            Button button;
            RequiredFieldValidator fieldValidator;
            CheckBox checkbox;

            // Create a new table and set default properties
            table = new Table();
            table.CssClass = "tableBorder";
            table.CellPadding = 3;
            table.CellSpacing = 1;
            table.Width = Unit.Percentage(100);

            // Column 1 - Search Text
            tr = new TableRow();

            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.HorizontalAlign = HorizontalAlign.Left;

            // Search Text
            th.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "Search").ToString();
            tr.Controls.Add(th);
            table.Controls.Add(tr);

            tr = new TableRow();
            tr.ID = "DisplayAdvancedSearchOptions";
            td = new TableCell();

            // Column 2 - Search Options
            td.Wrap = false;
            td.CssClass = "forumRow";
            td.HorizontalAlign = HorizontalAlign.Center;

            // Forums to search
            forumsToSearch = new DropDownList();
            forumsToSearch.ID = "forumsToSearch";
            forumsToSearch.DataTextField = "Name";
            forumsToSearch.DataValueField = "ForumID";

            // Databind based on postback
            if (!Page.IsPostBack)
            {
                forumsToSearch.DataSource = Forums.GetAllForums();
                forumsToSearch.DataBind();
            }

            // Add the "All Forums" option if the user is allowed to search all forums
            if (AllowSearchAllForums)
                forumsToSearch.Items.Insert(0, new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "AllForums").ToString(), "-1"));

            td.Controls.Add(forumsToSearch);

            // White space
            td.Controls.Add(new LiteralControl("&nbsp;"));

            // Results / page
            resultsPerPage = new DropDownList();
            resultsPerPage.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "resultsPerPage1").ToString(), "10"));
            resultsPerPage.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "resultsPerPage2").ToString(), "25"));
            resultsPerPage.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "resultsPerPage3").ToString(), "50"));
            td.Controls.Add(resultsPerPage);

            // White space
            td.Controls.Add(new LiteralControl("&nbsp;"));

            // White space
            td.Controls.Add(new LiteralControl("&nbsp;"));

            // Match
            matchingWords = new DropDownList();
            matchingWords.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "matchingWords1").ToString(), "0"));
            matchingWords.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "matchingWords2").ToString(), "1"));
            matchingWords.Items.Add(new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "matchingWords3").ToString(), "2"));
            td.Controls.Add(matchingWords);

            tr.Controls.Add(td);

            // Are we displaying advanced search options?
            if (DisplayAdvancedSearch)
                tr.Visible = true;
            else
                tr.Visible = false;

            // Add row1 to table
            table.Controls.Add(tr);


            // Search Box
            tr = new TableRow();

            // Text to search for
            td = new TableCell();
            td.Wrap = false;
            td.CssClass = "forumRow";
            td.HorizontalAlign = HorizontalAlign.Center;

            // Search text box
            textToSearchFor = new TextBox();
            textToSearchFor.ID = "textToSearchFor";
            textToSearchFor.Columns = ContainingTextBoxColumns;
            textToSearchFor.MaxLength = defaultContainingTextBoxMaxLength;
            textToSearchFor.Width = Unit.Pixel(300);
            td.Controls.Add(textToSearchFor);

            // White space
            td.Controls.Add(new LiteralControl("&nbsp;"));

            button = new Button();
            button.Text = defaultSearchButtonText;
            button.Click += new EventHandler(SearchButton_Click);
            td.Controls.Add(button);

            // White space
            td.Controls.Add(new LiteralControl("&nbsp;"));

            // Add the advanced features checkbox
            checkbox = new CheckBox();
            checkbox.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "AdvancedOptions").ToString();
            checkbox.CssClass = "normalTextSmall";
            checkbox.AutoPostBack = true;
            checkbox.CheckedChanged += new System.EventHandler(DisplayAdvancedOptions);
            td.Controls.Add(checkbox);

            // Validate the search box
            fieldValidator = new RequiredFieldValidator();
            fieldValidator.CssClass = "validationWarningSmall";
            fieldValidator.ControlToValidate = "textToSearchFor";
            fieldValidator.Display = ValidatorDisplay.Dynamic;
            fieldValidator.ErrorMessage = defaultRFVErrorMessage;
            td.Controls.Add(fieldValidator);
            tr.Controls.Add(td);

            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  DisplayAdvancedOptions
        //
        /// <summary>
        /// Event handler raised when the checkbox for displaying advanced options
        /// is clicked.
        /// </summary>
        /// <param name="sender">Checkbox that raised the event</param>
        /// <param name="e">Event arguments from the checkbox</param>
        private void DisplayAdvancedOptions(Object sender, EventArgs e)
        {
            CheckBox checkbox;
            Control advancedSearch;

            // Get the checkbox that raised us
            checkbox = (CheckBox)sender;

            // Get the advanced search option table ro
            advancedSearch = FindControl("DisplayAdvancedSearchOptions");

            // Change setting based on status of checkbox
            if (checkbox.Checked)
            {
                advancedSearch.Visible = false;
                DisplayAdvancedSearch = false;
            }
            else
            {
                advancedSearch.Visible = true;
                DisplayAdvancedSearch = true;
            }
        }


        // *********************************************************************
        //  SearchButton_Click
        //
        /// <summary>
        /// This event handler is called when the user clicks on the submit button,
        /// firing off the search.  It resets the DataGrid's CurrentPageIndex to 0
        /// and Displays the Search Results.
        /// </summary>
        // ***********************************************************************/
        private void SearchButton_Click(Object sender, EventArgs e)
        {

            // Exit if the page is invalid
            if (!Page.IsValid)
                return;

            // set the datagrid to start paging at the beginning
            searchResultsDataGrid.CurrentPageIndex = 0;

            // set the number of Records per page
            searchResultsDataGrid.PageSize = Convert.ToInt32(resultsPerPage.SelectedItem.Value);

            // display the search results
            DisplaySearchResults();

        }

        // *********************************************************************
        //  DisplaySearchResults
        //
        /// <summary>
        /// This function grabs the dataset of search results for the particular page.
        /// If there are records in the DataSet, it is bound to the DataGrid, else
        /// a message is displayed explaining that no results were found.
        /// </summary>
        // ***********************************************************************/
        private void DisplaySearchResults()
        {
            // get the search results
            ToSearchEnum ToSearch;
            SearchWhatEnum SearchWhat;

            ToSearch = ToSearchEnum.PostsSearch;


            // determine how we want to search
            switch (Convert.ToInt32(matchingWords.SelectedItem.Value))
            {
                case 1:
                    SearchWhat = SearchWhatEnum.SearchAnyWord;
                    break;

                case 2:
                    SearchWhat = SearchWhatEnum.SearchExactPhrase;
                    break;

                default:
                    SearchWhat = SearchWhatEnum.SearchAllWords;
                    break;
            }

            // Clean up search text
            textToSearchFor.Text = textToSearchFor.Text.Replace("'", "''");

            // Perform the Search
            PostCollection posts = AspNetForums.Search.PerformSearch(ToSearch, SearchWhat,
               Convert.ToInt32(forumsToSearch.SelectedItem.Value),
               textToSearchFor.Text, searchResultsDataGrid.CurrentPageIndex + 1, searchResultsDataGrid.PageSize);

            // if the dataset is empty, show an appropriate message
            if (posts.Count == 0)
            {
                ((Label)FindControl("lblNoResults")).Visible = true;
                ((Panel)FindControl("panelSearchResults")).Visible = false;
            }
            else
            {
                // we have results, bind it to the DataGrid.
                ((Panel)FindControl("panelSearchResults")).Visible = true;
                ((Label)FindControl("lblNoResults")).Visible = false;

                // see if we have more records to show...
                searchResultsDataGrid.VirtualItemCount = posts.TotalRecordCount;

                searchResultsDataGrid.DataSource = posts;
                searchResultsDataGrid.DataBind();

                // display how many results we got and what page we're viewing.
                ((Label)FindControl("lblInformation")).Text =
                    HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "Found1").ToString() +
                    String.Format("{0:d}", posts.TotalRecordCount) +
                    HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "Found2").ToString() +
                    (searchResultsDataGrid.CurrentPageIndex + 1) +
                    HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "of").ToString() +
                    searchResultsDataGrid.PageCount;
            }
        }

        // *********************************************************************
        //  OnPreRender
        //
        /// <summary>
        /// This event handler checks to see if its the first time the page has
        /// been loaded - if it is, the data is bound.
        /// </summary>
        // ***********************************************************************/
        protected override void OnPreRender(EventArgs e)
        {

            // see if this is our first visit to the page
            if (!Page.IsPostBack)
            {
                DropDownList forumsToSearch = (DropDownList)FindControl("forumsToSearch");

                // If we found the control
                if (forumsToSearch != null)
                {
                    forumsToSearch.DataSource = Forums.GetAllForums();
                    forumsToSearch.DataBind();
                }

                // add the "All Forums" option if the user is allowed to search all forums
                if (AllowSearchAllForums)
                    forumsToSearch.Items.Insert(0, new ListItem(HttpContext.GetLocalResourceObject(Globals.SkinsDir + "Search", "AllForums").ToString(), "-1"));

                if (CheckForPassedInParameters())
                    DisplaySearchResults();
            }
        }


        // *********************************************************************
        //  CheckForPassedInParameters
        //
        /// <summary>
        /// This function checks to see if any querystring parameters were passed
        /// in.  Since we want to be able to invoke the search via a direct link, we need
        /// to allow querystring/form parameters to trip the search.  If a querystring/
        /// form parameter Contents is passed in, the search is automagically performed.
        /// </summary>
        // ***********************************************************************/
        private bool CheckForPassedInParameters()
        {

            if (Context.Request.Params["SearchText"] != null)
            {
                // we have parameter values!
                textToSearchFor.Text = Context.Request.Params["SearchText"].ToString();

                if (Context.Request.Params["Forum"] != null)
                    try
                    {
                        forumsToSearch.Items.FindByValue(Context.Request.Params["Forum"].ToString()).Selected = true;
                    }
                    catch (Exception)
                    {
                        forumsToSearch.Items[0].Selected = true;
                    }
                else
                    forumsToSearch.Items[0].Selected = true;

                if (Context.Request.Params["Results"] != null)
                    try
                    {
                        resultsPerPage.Items.FindByValue(Context.Request.Params["Results"].ToString()).Selected = true;
                    }
                    catch (Exception)
                    {
                        resultsPerPage.Items[1].Selected = true;
                    }
                else
                    resultsPerPage.Items[1].Selected = true;

                if (Context.Request.Params["Matching"] != null)
                    try
                    {
                        matchingWords.Items.FindByValue(Context.Request.Params["Matching"].ToString()).Selected = true;
                    }
                    catch (Exception)
                    {
                        matchingWords.Items[0].Selected = true;
                    }
                else
                    matchingWords.Items[0].Selected = true;

                // Set the Records per page
                searchResultsDataGrid.CurrentPageIndex = 0;
                searchResultsDataGrid.PageSize = Convert.ToInt32(resultsPerPage.SelectedItem.Value);

                return true;
            }

            return false;		// no querystring values
        }


        /// <summary>
        /// The message to display when no results are found for a search.
        /// </summary>
        [
        Category("Style"),
        Description("The message to display when no results are found for a search."),
        DefaultValue("No records were found matching your search query.")
        ]
        public String NoRecordsFoundMessage
        {
            get
            {
                if (ViewState["noRecsFoundMsg"] == null) return defaultNoRecordsMessage;
                return (String)ViewState["noRecsFoundMsg"];
            }
            set { ViewState["noRecsFoundMsg"] = value; }
        }

        /// <summary>
        /// The title that should appear at the top of the search page.
        /// </summary>
        [
        Category("Style"),
        Description("The title that should appear at the top of the search page."),
        DefaultValue("Search")
        ]
        public String SearchTitle
        {
            get
            {
                if (ViewState["searchTitle"] == null) return defaultSearchTitle;
                return (String)ViewState["searchTitle"];
            }
            set { ViewState["searchTitle"] = value; }
        }

        /// <summary>
        /// Used to display advanced search options.
        /// </summary>
        [
        Description("Display advanced search options."),
        DefaultValue("Search")
        ]
        public bool DisplayAdvancedSearch
        {
            get
            {
                if (ViewState["displayAdvancedSearch"] == null) return true;
                return (bool)ViewState["displayAdvancedSearch"];
            }
            set { ViewState["displayAdvancedSearch"] = value; }
        }


        /// <summary>
        /// This property determines whether or not the user can search all forums or must search
        /// from a particular forum.  It defaults to true.
        /// </summary>
        /// <remarks>For sites with a large number of forums and a large number of posts with a large
        /// user base performing many searches, setting this property to false may help the overall
        /// performance of the site.</remarks>
        [
        Category("Style"),
        Description("Whether or not a user can search all forums."),
        DefaultValue(true)
        ]
        public bool AllowSearchAllForums
        {
            get
            {
                if (ViewState["allowSearchAllForums"] == null) return defaultAllowSearchAllForums;
                return (bool)ViewState["allowSearchAllForums"];
            }
            set { ViewState["allowSearchAllForums"] = value; }
        }

        /// <summary>
        /// Specifies the text for the button to launch the search.
        /// </summary>
        [
        Category("Style"),
        Description("Specifies the text for the button to launch the search."),
        DefaultValue(" Поиск ")
        ]
        public String SearchButtonText
        {
            get
            {
                if (ViewState["searchButtonTxt"] == null) return defaultSearchButtonText;
                return (String)ViewState["searchButtonTxt"];
            }
            set { ViewState["searchButtonTxt"] = value; }
        }


        /// <summary>
        /// Specifies how many columns the Containing text box should have.
        /// </summary>
        [
        Category("Style"),
        Description("Specifies how many columns the Containing text box should have."),
        DefaultValue(50)
        ]
        public int ContainingTextBoxColumns
        {
            get
            {
                if (ViewState["containingTextBoxColumns"] == null) return defaultContainingTextBoxColumns;
                return (int)ViewState["containingTextBoxColumns"];
            }
            set { ViewState["containingTextBoxColumns"] = value; }
        }

        /// <summary>
        /// Determines how many characters of the Body should be displayed.  This property setting only
        /// takes affect if the ShowBody property is set to true.
        /// <seealso cref="ShowBody"/>
        /// </summary>
        [
        Category("Style"),
        Description("Determines how many characters of the Body should be displayed."),
        DefaultValue(500)
        ]
        public int MaxBodyLengthToDisplay
        {
            get
            {
                if (ViewState["mblToDisplay"] == null) return defaultMaxBodyLength;
                return (int)ViewState["mblToDisplay"];
            }
            set { ViewState["mblToDisplay"] = value; }
        }

        /// <summary>
        /// Determines whether or not the Body of the message should be displayed in the search results.
        /// </summary>
        [
        Category("Style"),
        Description("Determines whether or not the Body of the message should be displayed."),
        DefaultValue(true)
        ]
        public bool ShowBody
        {
            get
            {
                if (ViewState["showBody"] == null) return defaultShowBody;
                return (bool)ViewState["showBody"];
            }
            set { ViewState["showBody"] = value; }
        }

        /// <summary>
        /// Specifies the UI and stylistic settings of the search results Pager.  The Pager allows the
        /// end user to navigate between multiple pages of search results.
        /// </summary>
        [
        Category("Style"),
        Description("Specifies the UI and stylistic settings of the search results Pager.")
        ]
        public SearchPagerStyle PagerStyle
        {
            get { return datagridPagerStyle; }
        }
        /*******************************************************/
    }
}