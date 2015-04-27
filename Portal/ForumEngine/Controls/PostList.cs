using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AspNetForums;
using AspNetForums.Controls.Moderation;
using AspNetForums.Components;

namespace AspNetForums.Controls
{

    /// <summary>
    /// This Web control displays a thread in a flat display.  The developer must pass in
    /// either a PostID.  If a PostID is passed in, the thread that that Post belongs
    /// to is constructed.
    /// </summary>
    [
        ParseChildren(true)
    ]
    public class PostList : DataList
    {

        User user;
        String skinName;
        int postID;

        // *********************************************************************
        //  CreateChildControls
        //
        /// <summary>
        /// This event handler adds the children controls and is resonsible
        /// for determining the template type used for the control.
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void CreateChildControls()
        {

            // Do we have a user?
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                user = Users.GetLoggedOnUser();
            }

            skinName = Globals.Skin;

            // Apply Template
            ApplyTemplates();

            // Viewstate is disabled
            EnableViewState = false;

        }

        // *********************************************************************
        //  HandleDataBindingForPostCell
        //
        /// <summary>
        /// Databinds the post cell
        /// </summary>
        /// <remarks>
        /// Used only if a user defined template is not provided.
        /// </remarks>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForPostCell(Object sender, EventArgs e)
        {
            Table table;
            TableRow tr;
            TableCell td;
            Label label;
            string dateFormat;
            DateTime postDateTime;
            User postUser;

            // Get the sender
            TableCell postInfo = (TableCell)sender;
            DataListItem container = (DataListItem)postInfo.NamingContainer;
            Post post = (Post)container.DataItem;

            // Get the user that created the post
            postUser = Users.GetUserInfo(post.Username);

            // Create the table
            table = new Table();
            table.CellPadding = 3;
            table.CellSpacing = 1;
            table.Width = Unit.Percentage(100);

            // Row 1
            tr = new TableRow();
            td = new TableCell();


            // Add in Subject
            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.Text = post.Subject + "<a name=\"" + post.PostID + "\"/>";
            td.Controls.Add(label);

            td.Controls.Add(new LiteralControl("<br>"));

            // Add in 'Posted: '
            label = new Label();
            label.CssClass = "normalTextSmaller";
            label.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "PostList", "Posted").ToString();
            td.Controls.Add(label);

            // Get the postDateTime
            postDateTime = post.PostDate;
            dateFormat = Globals.DateFormat;


            // Add in PostDateTime
            label = new Label();
            label.CssClass = "normalTextSmaller";
            label.Text = postDateTime.ToString(dateFormat + " " + Globals.TimeFormat);
            td.Controls.Add(label);
            td.Width = Unit.Percentage(99);

            // Add column 1
            tr.Controls.Add(td);

            // Add the reply button
            if (!post.IsLocked && user != null && user.IsApproved)
            {
                // Reply button
                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Right;
                td.VerticalAlign = VerticalAlign.Middle;

                HyperLink replyButton = new HyperLink();
                replyButton.Text = "<img border=0 src=" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + Globals.Skin + "/images" + Globals.LangDir + "reply.gif" + ">";
                replyButton.NavigateUrl = Globals.UrlReplyToPost + post.PostID;
                td.Controls.Add(replyButton);
                tr.Controls.Add(td);
            }

            table.Controls.Add(tr);

            tr.Controls.Add(td);
            table.Controls.Add(tr);
            
            


            // Row 2 (body)
            tr = new TableRow();
            td = new TableCell();
            td.ColumnSpan = 2;
            td.Controls.Add(new LiteralControl("<hr color=\"black\" noshade size=\"1px\" width=\"100%\" />\n"));
            
            // Add Body
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.Text = Globals.FormatPostBody(post.Body);
            td.Controls.Add(label);
            
            // Add row 2
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Row 3 (Signature)
            tr = new TableRow();
            td = new TableCell();
            td.ColumnSpan = 2;

            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add whitespace
            tr = new TableRow();
            td = new TableCell();
            td.Height = 2;
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add buttons for user options
            tr = new TableRow();
            td = new TableCell();
            td.ColumnSpan = 2;



            // Add the edit button
            if ((user != null) && (user.Username.ToLower() == post.Username.ToLower()) && user.IsApproved)
            {
                // Edit button
                HyperLink editButton = new HyperLink();
                editButton.Text = "<img border=0 src=" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + Globals.Skin + "/images" + Globals.LangDir + "icon_edit.gif" + ">";
                editButton.NavigateUrl = Globals.UrlUserEditPost + post.PostID + "&RedirectUrl=" + HttpContext.Current.Server.UrlEncode(Globals.UrlShowPost + postID);
                td.Controls.Add(editButton);
            }

            // Anything to add to the table control?
            if (td.Controls.Count > 0)
            {
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }

            // Is the current user a moderator?
            if ((user != null) && (user.IsAdministrator))
            {
                tr = new TableRow();
                td = new TableCell();
                td.ColumnSpan = 2;

                // Find the moderation menu
                ModerationMenu moderationMenu = new ModerationMenu();
                moderationMenu.PostID = post.PostID;
                moderationMenu.ThreadID = post.ThreadID;
                moderationMenu.UsernamePostedBy = Users.GetUserInfo(post.Username).DisplayName;
                moderationMenu.SkinFilename = "Moderation/Skin-ModeratePost.ascx";

                td.Controls.Add(moderationMenu);
                tr.Controls.Add(td);
                table.Controls.Add(tr);

            }

            postInfo.Controls.Add(table);
        }

        // *********************************************************************
        //  HandleDataBindingForAuthorCell
        //
        /// <summary>
        /// Databinds the name of the author.
        /// </summary>
        /// <remarks>
        /// Used only if a user defined template is not provided.
        /// </remarks>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForAuthorCell(Object sender, EventArgs e)
        {

            TableCell userInfo = (TableCell)sender;
            DataListItem container = (DataListItem)userInfo.NamingContainer;
            Post post = (Post)container.DataItem;
            Label link;
            User postUser;

            // Get the postUser object - note, we are using
            // the cache under the covers, so this doesn't
            // result in a db lookup for each request
            postUser = Users.GetUserInfo(post.Username);

            // Build postUser info table
            Table table = new Table();
            TableRow tr;
            TableCell td;

            // Author
            tr = new TableRow();
            td = new TableCell();
            link = new Label();
            link.CssClass = "normalTextSmallBold";
            link.Text = postUser.DisplayName;

            td.Controls.Add(link);

            // whitespace
            td.Controls.Add(new LiteralControl("<br>"));

            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Admin?
            if (postUser.IsAdministrator)
            {
                tr = new TableRow();
                td = new TableCell();
                Label l = new Label();
                l.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "PostList", "Admin").ToString();
                l.CssClass = "normalTextSmaller";
                l.ForeColor = System.Drawing.Color.DarkOrange;

                td.Controls.Add(l);
                tr.Controls.Add(td);
                table.Controls.Add(tr);
            }
            // Add a little whitespace
            tr = new TableRow();
            td = new TableCell();
            td.Text = "&nbsp;";
            tr.Controls.Add(td);
            table.Controls.Add(tr);

            // Add table to placeholder
            userInfo.Controls.Add(table);
        }


        // *********************************************************************
        //  HandleDataBindingForAuthorDetails
        //
        /// <summary>
        /// Databinds the name of the author.
        /// </summary>
        /// <remarks>
        /// Used only if a user defined template is not provided.
        /// </remarks>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForAuthorDetails(Object sender, EventArgs e)
        {
            TableCell userInfo = (TableCell)sender;
            DataListItem container = (DataListItem)userInfo.NamingContainer;
            Post post = (Post)container.DataItem;
            User user;
            Image image = new Image();

            // Get the user object - note, we are using
            // the cache under the covers, so this doesn't
            // result in a db lookup for each request
            user = Users.GetUserInfo(post.Username);

            // Build user info table
            Table table = new Table();

            // Add table to placeholder
            userInfo.Controls.Add(table);
        }

        // *********************************************************************
        //  RenderHeaderTemplate
        //
        /// <summary>
        /// This function renders the header template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderHeaderTemplate()
        {
            PostCollection posts;
            Post post;
            Table table = new Table();
            TableRow tr;
            TableHeaderCell th;

            // Set up the table
            table.Width = Unit.Percentage(100);

            // Get details about this post and ensure
            // we get the root post
            posts = (PostCollection)DataSource;
            post = (Post)posts[0];
            postID = post.PostID;

            // Header row to diplay Author and Title
            tr = new TableRow();

            // Authors
            th = new TableHeaderCell();
            th.Height = Unit.Pixel(25);
            th.Width = Unit.Pixel(100);
            th.CssClass = "tableHeaderText";
            th.Width = 100;

            th.HorizontalAlign = HorizontalAlign.Left;
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "PostList", "Author").ToString();
            tr.Controls.Add(th);

            // Messages
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Width = Unit.Percentage(85);
            th.HorizontalAlign = HorizontalAlign.Left;
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "PostList", "Thread").ToString();

            tr.Controls.Add(th);

            // Add header row to table
            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  HeaderDisplay
        //
        /// <summary>
        /// This function renders the display for items in the header
        /// </summary>
        /// 
        // ********************************************************************/
        private Table HeaderDisplay()
        {
            Table table = new Table();
            TableCell tdLeft = new TableCell();
            TableCell tdRight = new TableCell();
            TableRow tr = new TableRow();

            // Set up the table
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Width = Unit.Percentage(100);

            // Set up the cells
            tdLeft.HorizontalAlign = HorizontalAlign.Left;
            tdRight.HorizontalAlign = HorizontalAlign.Right;

            // Add table cells
            tr.Controls.Add(tdLeft);
            tr.Controls.Add(tdRight);
            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  ThreadTracking_Changed
        //
        /// <summary>
        /// Event raised when the user wants to enable or disable thread tracking
        /// </summary>
        /// 
        // ********************************************************************/
        public event System.EventHandler ThreadTracking_Changed;

        // *********************************************************************
        //  ThreadTracking_CheckChanged
        //
        /// <summary>
        /// Event raised when the thread tracking checkbox is clicked upon.
        /// </summary>
        /// 
        // ********************************************************************/
        private void ThreadTracking_CheckChanged(Object sender, EventArgs e)
        {

            if (null != ThreadTracking_Changed)
                ThreadTracking_Changed(sender, e);

        }

        // *********************************************************************
        //  RenderItemTemplate
        //
        /// <summary>
        /// This function renders the item template for flat display
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderItemTemplate()
        {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell authorCell = new TableCell();
            TableCell postCell = new TableCell();
            TableCell authorDetailsCell = new TableCell();
            TableCell authorContactCell = new TableCell();

            // Author Cell Details
            authorCell.VerticalAlign = VerticalAlign.Top;
            authorCell.CssClass = "forumRow";
            authorCell.Wrap = false;
            authorCell.Width = 150;
            authorCell.DataBinding += new System.EventHandler(HandleDataBindingForAuthorCell);

            // Post Cell Details
            postCell.VerticalAlign = VerticalAlign.Top;
            postCell.CssClass = "forumRow";
            postCell.DataBinding += new System.EventHandler(HandleDataBindingForPostCell);

            // Add controls to control tree
            tr.Controls.Add(authorCell);
            tr.Controls.Add(postCell);

            table.Controls.Add(tr);

            return table;
        }

        // *********************************************************************
        //  RenderAlternatingItemTemplate
        //
        /// <summary>
        /// This function renders the item template for flat display
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderAlternatingItemTemplate()
        {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell authorCell = new TableCell();
            TableCell postCell = new TableCell();
            TableCell authorDetailsCell = new TableCell();
            TableCell authorContactCell = new TableCell();

            // Author Cell Details
            authorCell.VerticalAlign = VerticalAlign.Top;
            authorCell.CssClass = "forumRowAlternate";
            authorCell.Wrap = false;
            authorCell.DataBinding += new System.EventHandler(HandleDataBindingForAuthorCell);

            // Post Cell Details
            postCell.VerticalAlign = VerticalAlign.Top;
            postCell.CssClass = "forumRowAlternate";
            postCell.DataBinding += new System.EventHandler(HandleDataBindingForPostCell);

            // Add controls to control tree
            tr.Controls.Add(authorCell);
            tr.Controls.Add(postCell);

            table.Controls.Add(tr);

            return table;

        }

        // *********************************************************************
        //  RenderFooterTemplate
        //
        /// <summary>
        /// This function renders the footer template
        /// </summary>
        /// 
        // ********************************************************************/
        private Control RenderFooterTemplate()
        {
            Table table = new Table();

            return table;
        }

        // *********************************************************************
        //  BuildHeaderTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildHeaderTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderHeaderTemplate());

        }


        // *********************************************************************
        //  BuildItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildItemTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(RenderItemTemplate());

        }

        // *********************************************************************
        //  BuildSeparatorItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildSeparatorTemplate(Control _ctrl)
        {
            Table table = new Table();
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            td.ColumnSpan = 2;
            td.Height = 2;
            td.CssClass = "flatViewSpacing";

            tr.Controls.Add(td);
            table.Controls.Add(tr);

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(table);
        }

        // *********************************************************************
        //  BuildAlternatingItemTemplate
        //
        /// <summary>
        /// This function is called to create the template for items
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildAlternatingItemTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(RenderAlternatingItemTemplate());

        }

        // *********************************************************************
        //  BuildFooterTemplate
        //
        /// <summary>
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildFooterTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderFooterTemplate());

        }

        // *********************************************************************
        //  ApplyTemplates
        //
        /// <summary>
        /// Applies templates to control the ui generated by the control. If no
        /// template is specified a custom template is used. If a template is found
        /// in the skins directory, that template is loaded and used. If a user defined
        /// template is found, that template takes priority.
        /// </summary>
        /// 
        // ********************************************************************/
        private void ApplyTemplates()
        {
            string pathToHeaderTemplate;
            string pathToItemTemplate;
            string pathToAlternatingItemTemplate;
            string pathToFooterTemplate;
            string keyForHeaderTemplate;
            string keyForItemTemplate;
            string keyForAlternatingItemTemplate;
            string keyForFooterTemplate;

            // Are we using skinned template?
            if (Page != null)
            {

                // Set the file paths to where the templates should be found
                keyForHeaderTemplate = Globals.Skin + "/Templates/PostList-Header.ascx";
                keyForItemTemplate = Globals.Skin + "/Templates/PostList-Item.ascx";
                keyForAlternatingItemTemplate = Globals.Skin + "/Templates/PostList-AlternatingItem.ascx";
                keyForFooterTemplate = Globals.Skin + "/Templates/PostList-Footer.ascx";

                // Set the file paths to where the templates should be found
                pathToHeaderTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + keyForHeaderTemplate;
                pathToItemTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + keyForItemTemplate;
                pathToAlternatingItemTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + keyForAlternatingItemTemplate;
                pathToFooterTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + keyForFooterTemplate;

                // Attempt to get the skinned header template
                if (HeaderTemplate == null)
                    HeaderTemplate = Globals.LoadSkinnedTemplate(pathToHeaderTemplate, keyForHeaderTemplate, Page);

                // Attempt to get the skinned item template
                if (ItemTemplate == null)
                    ItemTemplate = Globals.LoadSkinnedTemplate(pathToItemTemplate, keyForItemTemplate, Page);

                // Attempt to get the skinned alternating item template
                if (AlternatingItemTemplate == null)
                    AlternatingItemTemplate = Globals.LoadSkinnedTemplate(pathToAlternatingItemTemplate, keyForAlternatingItemTemplate, Page);

                // Attempt to get the footer template
                if (FooterTemplate == null)
                    FooterTemplate = Globals.LoadSkinnedTemplate(pathToFooterTemplate, keyForFooterTemplate, Page);
            }

            // Are any templates specified yet?
            if (ItemTemplate == null)
            {
                // Looks like we're using custom templates
                ExtractTemplateRows = true;

                HeaderTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildHeaderTemplate));
                ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildItemTemplate));
                AlternatingItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildAlternatingItemTemplate));
                FooterTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildFooterTemplate));
            }

        }


    }
}