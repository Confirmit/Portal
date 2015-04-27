using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.IO;
using AspNetForums.Controls.BaseClasses;

namespace AspNetForums.Controls
{


    // *********************************************************************
    //  AllUsers
    //
    /// <summary>
    /// This control extends DataList to provide a custom rendering and build in
    /// style/skin templates for Ulter Systems forums
    /// </summary>
    // ***********************************************************************/
    public class UserList : ForumDataListControl
    {

        int userCount = 1;
        bool userCountIsAscending = true;

        // *********************************************************************
        //  RenderHeaderTempalte
        //
        /// <summary>
        /// Renders each the header using a default template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderHeaderTemplate()
        {
            Table table;
            TableRow tr;
            TableHeaderCell th;

            table = new Table();
            tr = new TableRow();

            // Column - # of users
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.HorizontalAlign = HorizontalAlign.Left;
            th.Text = "&nbsp;#&nbsp;";
            tr.Controls.Add(th);

            // Column - Username
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "UserList", "Username").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Left;
            tr.Controls.Add(th);

            // Column - ID
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "UserList", "Domain").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Left;
            tr.Controls.Add(th);

            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderItemTempalte
        //
        /// <summary>
        /// Renders each item using a default template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderItemTemplate()
        {

            Table table;
            TableRow tr;
            TableCell td;
            Label label;
            HyperLink link;

            // Create a new table
            table = new Table();

            // Create a new table row
            tr = new TableRow();

            // Column - User Count
            td = new TableCell();
            td.Width = 25;
            td.HorizontalAlign = HorizontalAlign.Center;
            td.VerticalAlign = VerticalAlign.Middle;
            td.CssClass = "forumRow";
            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.DataBinding += new System.EventHandler(HandleDataBindingForUserCount);
            td.Controls.Add(label);
            tr.Controls.Add(td);

            // Column - Username
            td = new TableCell();
            td.CssClass = "forumRow";
            link = new HyperLink();
            link.DataBinding += new System.EventHandler(HandleDatabindingForUsername);
            td.Controls.Add(link);
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Controls.Add(td);

            // Column - ID
            td = new TableCell();
            td.CssClass = "forumRowHighlight";
            td.HorizontalAlign = HorizontalAlign.Left;
            link = new HyperLink();
            link.Target = "_blank";
            link.DataBinding += new System.EventHandler(HandleDatabindingForID);
            td.Controls.Add(link);
            tr.Controls.Add(td);

            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderFooterTempalte
        //
        /// <summary>
        /// Renders each the footer using a default template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderFooterTemplate()
        {
            Table table;
            TableRow tr;
            TableCell td;

            table = new Table();
            tr = new TableRow();

            // Single column
            td = new TableCell();
            td.CssClass = "forumHeaderBackgroundAlternate";
            td.ColumnSpan = 8;
            tr.Controls.Add(td);

            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  HandleDataBindingForUserCount
        //
        /// <summary>
        /// Handles the databinding for the user count
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForUserCount(Object sender, EventArgs e)
        {
            Label count = (Label)sender;
            DataListItem container = (DataListItem)count.NamingContainer;

            if (UserCountIsAscending)
                count.Text = (UserCount++).ToString("n0");
            else
                count.Text = (UserCount--).ToString("n0");
        }

        // *********************************************************************
        //  HandleDatabindingForUsername
        //
        /// <summary>
        /// Handles the databinding for the user count
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForUsername(Object sender, EventArgs e)
        {
            HyperLink username = (HyperLink)sender;
            DataListItem container = (DataListItem)username.NamingContainer;

            User userToDisplay = (User)container.DataItem;

            username.Text = userToDisplay.DisplayName;
            username.NavigateUrl = UrlUserNameRedirect + userToDisplay.Username;
        }


        // *********************************************************************
        //  HandleDatabindingForID
        //
        /// <summary>
        /// Handles the databinding for the user's website
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForID(Object sender, EventArgs e)
        {
            HyperLink website = (HyperLink)sender;
            DataListItem container = (DataListItem)website.NamingContainer;

            User userToDisplay = (User)container.DataItem;

            // Only display the web site if we have it.
            website.Text = userToDisplay.Username;


        }

        
        // *********************************************************************
        //  BuildHeaderTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        override protected void BuildHeaderTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderHeaderTemplate());

        }

        // *********************************************************************
        //  BuildItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        override protected void BuildItemTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderItemTemplate());

        }

        // *********************************************************************
        //  BuildFooterTemplate
        //
        /// <summary>
        /// This function is called to create the template for the footer
        /// </summary>
        /// 
        // ********************************************************************/
        override protected void BuildFooterTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderFooterTemplate());

        }

        // *********************************************************************
        //  UserCount
        //
        /// <summary>
        /// Sets/Gets the total number of users
        /// </summary>
        /// 
        // ********************************************************************/
        public int UserCount
        {
            get { return userCount; }
            set { userCount = value; }
        }

        // *********************************************************************
        //  UserCountIsAscending
        //
        /// <summary>
        /// Controls direction of user counting
        /// </summary>
        /// 
        // ********************************************************************/
        public bool UserCountIsAscending
        {
            get { return userCountIsAscending; }
            set { userCountIsAscending = value; }
        }

        // *********************************************************************
        //  UrlUserNameRedirect
        //
        /// <summary>
        /// Controls the default redirect URL 
        /// </summary>
        /// 
        // ********************************************************************/
        public string UrlUserNameRedirect
        {
            get
            {
                if (ViewState["urlUserNameRedirect"] == null)
                    return Globals.UrlUserProfile;

                return ViewState["urlUserNameRedirect"].ToString();
            }
            set
            {
                ViewState["urlUserNameRedirect"] = value;
            }
        }
    }
}
