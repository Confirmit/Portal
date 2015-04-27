using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace AspNetForums.Controls
{

    [
    ParseChildren(true),
    Designer(typeof(AspNetForums.Controls.Design.ForumRepeaterDesigner))
    ]
    public class ForumRepeater : ForumRepeaterControl
    {
        bool showAllForums = true;


        // *********************************************************************
        //  HandleDataBindingForForumTitle
        //
        /// <summary>
        /// DataBinding event for the forum title
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForForumTitle(Object sender, EventArgs e)
        {
            HyperLink forumTitle = (HyperLink)sender;
            RepeaterItem container = (RepeaterItem)forumTitle.NamingContainer;

            Forum forum = (Forum)container.DataItem;


            forumTitle.Text = forum.Name;
            forumTitle.NavigateUrl = Globals.UrlShowForum + forum.ForumID;
        }

        // *********************************************************************
        //  HandleDataBindingForForumDescription
        //
        /// <summary>
        /// DataBinding event for the forum description
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForForumDescription(Object sender, EventArgs e)
        {
            Label forumDescription = (Label)sender;
            RepeaterItem container = (RepeaterItem)forumDescription.NamingContainer;

            Forum forum = (Forum)container.DataItem;

            forumDescription.Text = "<br>" + forum.Description;
        }



        // *********************************************************************
        //  HandleDataBindingForTotalThreads
        //
        /// <summary>
        /// DataBinding event for the forum total threads
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForTotalThreads(Object sender, EventArgs e)
        {
            TableCell totalThreads = (TableCell)sender;
            RepeaterItem container = (RepeaterItem)totalThreads.NamingContainer;
            Label label;

            // Create label and set style
            label = new Label();
            label.CssClass = "normalTextSmall";

            Forum forum = (Forum)container.DataItem;

            if (forum.TotalThreads > 0)
                label.Text = forum.TotalThreads.ToString();
            else
                label.Text = "-";

            totalThreads.Controls.Add(label);

        }

        // *********************************************************************
        //  HandleDataBindingForTotalPosts
        //
        /// <summary>
        /// DataBinding event for the forum total posts
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForTotalPosts(Object sender, EventArgs e)
        {
            TableCell totalPosts = (TableCell)sender;
            RepeaterItem container = (RepeaterItem)totalPosts.NamingContainer;
            Label label;

            Forum forum = (Forum)container.DataItem;

            label = new Label();
            label.CssClass = "normalTextSmall";

            if (forum.TotalPosts > 0)
                label.Text = forum.TotalPosts.ToString();
            else
                label.Text = "-";

            totalPosts.Controls.Add(label);
        }

        // *********************************************************************
        //  HandleDataBindingForMostRecentPost
        //
        /// <summary>
        /// DataBinding event for the forum most recent post
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForMostRecentPost(Object sender, EventArgs e)
        {
            Label postDetails = (Label)sender;
            RepeaterItem container = (RepeaterItem)postDetails.NamingContainer;
            DateTime postDateTime;
            Label postDate = new Label();
            HyperLink newPost = new HyperLink();
            HyperLink author = new HyperLink();
            String dateFormat;

            // Do we have a signed in user?
            dateFormat = Globals.DateFormat;

            // Get the forum
            Forum forum = (Forum)container.DataItem;

            // Get the post date
            postDateTime = forum.MostRecentPostDate;

            // Did the post occur today?
            if ((postDateTime.DayOfYear == DateTime.Now.DayOfYear) && (postDateTime.Year == DateTime.Now.Year))
                postDate.Text = "<b>" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "Today").ToString() + postDateTime.ToString(Globals.TimeFormat) + "</b>";
            else if (postDateTime.Year > 0050)
                postDate.Text = postDateTime.ToString(dateFormat + " " + Globals.TimeFormat);
            else
                postDate.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "NoPosts").ToString();

            // Add the post
            postDetails.Controls.Add(postDate);
        }

        // *********************************************************************
        //  HandleDataBindingForPostedBy
        //
        /// <summary>
        /// DataBinding event for the forum most recent post by
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForPostedBy(Object sender, EventArgs e)
        {
            Label postedBy = new Label();
            HyperLink newPost = new HyperLink();
            Label label = (Label)sender;
            RepeaterItem container = (RepeaterItem)label.NamingContainer;

            Forum forum = (Forum)container.DataItem;

            // No threads just return
            if (forum.TotalThreads == 0)
                return;
            postedBy.CssClass = "normalTextSmall";
            // Hyper link for author
            postedBy.Text = Users.GetUserInfo(forum.MostRecentPostAuthorID).DisplayName;

            int c = Threads.GetTotalPostsForThread(forum.MostRecentThreadId);

            // Link to new post
            newPost.Text = "<img border=\"0\" src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + Globals.Skin + "/images/icon_mini_topic.gif\">";

            if ((c + 1) > Globals.PageSize)
            {
                int totalPages = Paging.CalculateTotalPages(c, Globals.PageSize,Threads.GetTotalPinnedPostsForThread(forum.MostRecentThreadId));

                // Newest post will be on the last page
                newPost.NavigateUrl = Globals.UrlShowPost + forum.MostRecentThreadId + "&PageIndex=" + totalPages + "#" + forum.MostRecentPostId;

            }
            else
            {
                newPost.NavigateUrl = Globals.UrlShowPost + forum.MostRecentThreadId + "#" + forum.MostRecentPostId;
            }

            newPost.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "newPost").ToString();

            label.Controls.Add(new LiteralControl(" "));
            label.Controls.Add(postedBy);
            label.Controls.Add(newPost);

        }


        // *********************************************************************
        //  HandleDataBindingForStatusImage
        //
        /// <summary>
        /// DataBinding event for the image used to indicate if there are new posts
        /// </summary>
        /// 
        // ********************************************************************/   
        private void HandleDataBindingForStatusImage(Object sender, EventArgs e)
        {
            Image img = (Image)sender;
            RepeaterItem container = (RepeaterItem)img.NamingContainer;

            Forum forum = (Forum)container.DataItem;

            if (ForumUser == null)
            {
                img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/forum_status.gif";
                img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "NoNewPosts").ToString();
            }
            else
            {

                if (forum.LastUserActivity < forum.MostRecentPostDate)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/forum_status_new.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "NewPosts").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/forum_status.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "NoNewPosts").ToString();
                }
            }

        }

        // *********************************************************************
        //  BeginBuildItemTemplate
        //
        /// <summary>
        /// Builds a default Item template if the user does not specify one
        /// </summary>
        /// 
        // ********************************************************************/   
        public virtual Control BeginBuildItemTemplate()
        {

            //PlaceHolder subForums = new PlaceHolder();
            Label label;
            PlaceHolder placeHolder = new PlaceHolder();
            TableCell td;
            TableRow tr = new TableRow();

            // Column 1
            td = new TableCell();
            td.CssClass = "forumRow";
            td.HorizontalAlign = HorizontalAlign.Center;
            td.VerticalAlign = VerticalAlign.Top;
            td.Width = 34;
            td.Wrap = false;
            Image img = new Image();
            img.Width = 34;
            img.DataBinding += new System.EventHandler(HandleDataBindingForStatusImage);
            td.Controls.Add(img);
            tr.Controls.Add(td);

            // Column 2
            td = new TableCell();
            td.CssClass = "forumRow";
            td.Width = Unit.Percentage(80);
            HyperLink link = new HyperLink();
            link.CssClass = "forumTitle";
            link.DataBinding += new System.EventHandler(HandleDataBindingForForumTitle);

            // Description and sub forums
            Label forumDescription = new Label();
            forumDescription.CssClass = "normalTextSmall";
            forumDescription.DataBinding += new System.EventHandler(HandleDataBindingForForumDescription);
            td.Controls.Add(link);
            td.Controls.Add(forumDescription);
            //td.Controls.Add(subForums);
            tr.Controls.Add(td);

            // Column 3
            td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Center;
            td.CssClass = "forumRow";
            td.DataBinding += new System.EventHandler(HandleDataBindingForTotalThreads);
            tr.Controls.Add(td);

            // Column 4
            td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Center;
            td.CssClass = "forumRow";
            td.DataBinding += new System.EventHandler(HandleDataBindingForTotalPosts);
            tr.Controls.Add(td);

            // Column 5
            td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Center;
            td.CssClass = "forumRow";
            Label mostRecentPostDate = new Label();
            mostRecentPostDate.CssClass = "normalTextSmaller";
            mostRecentPostDate.DataBinding += new System.EventHandler(HandleDataBindingForMostRecentPost);
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.DataBinding += new System.EventHandler(HandleDataBindingForPostedBy);
            td.Controls.Add(mostRecentPostDate);
            td.Controls.Add(new LiteralControl("<BR>"));
            td.Controls.Add(label);
            tr.Controls.Add(td);

            // Add the Table Row
            placeHolder.Controls.Add(tr);

            return placeHolder;
        }

        // *********************************************************************
        //  BuildItemTemplate
        //
        /// <summary>
        /// Template builder for the ItemTemplate
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
        //  CreateChildControls
        //
        /// <summary>
        /// Override create child controls
        /// </summary>
        /// 
        // ********************************************************************/   
        protected override void CreateChildControls()
        {
            EnableViewState = false;
            string username = null;

            // determine if we want to bind to the default template or not
            ApplyTemplates();

            if (ForumUser != null)
                username = ForumUser.Username;

            // bind the datalist to the SqlDataReader returned by the GetAllForums() method
            ForumCollection forums;
            Forums f = new Forums();
            try
            {
                forums = Forums.GetAllForums(username);
                DataSource = forums;

            }
            catch (Components.ForumNotFoundException)
            {
                Page.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnknownForum));
                Page.Response.End();
            }

            this.DataBind();
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
        public virtual void ApplyTemplates()
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
                pathToHeaderTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/Templates/ForumRepeater-Header.ascx";
                pathToItemTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/Templates/ForumRepeater-Item.ascx";
                pathToAlternatingItemTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/Templates/ForumRepeater-AlternatingItem.ascx";
                pathToFooterTemplate = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/Templates/ForumRepeater-Footer.ascx";

                // Set the file paths to where the templates should be found
                keyForHeaderTemplate = SkinName + "/Templates/ForumRepeater-Header.ascx";
                keyForItemTemplate = SkinName + "/Templates/ForumRepeater-Item.ascx";
                keyForAlternatingItemTemplate = SkinName + "/Templates/ForumRepeater-AlternatingItem.ascx";
                keyForFooterTemplate = SkinName + "/Templates/ForumRepeater-Footer.ascx";

                // Attempt to get the skinned header template
                if (HeaderTemplate == null)
                    HeaderTemplate = Globals.LoadSkinnedTemplate(pathToHeaderTemplate, keyForHeaderTemplate, Page);

                // Attempt to get the skinned item template
                if (ItemTemplate == null)
                    ItemTemplate = Globals.LoadSkinnedTemplate(pathToItemTemplate, keyForItemTemplate, Page);

                // Attempt to get the skinned item template
                if (AlternatingItemTemplate == null)
                    AlternatingItemTemplate = Globals.LoadSkinnedTemplate(pathToAlternatingItemTemplate, keyForAlternatingItemTemplate, Page);

                // Attempt to get the footer template
                if (FooterTemplate == null)
                    FooterTemplate = Globals.LoadSkinnedTemplate(pathToFooterTemplate, keyForFooterTemplate, Page);
            }

            // No skinned or user defined template, load the default
            if (ItemTemplate == null)
                ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildItemTemplate));



            // No skinned or user defined template, load the default
            if (HeaderTemplate == null)
                HeaderTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildHeaderTemplate));

            // No skinned or user defined template, load the default
            if (FooterTemplate == null)
                FooterTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildFooterTemplate));


        }

        // *********************************************************************
        //  ShowAllForums
        //
        /// <summary>
        /// Returns forums that are not active - however, will not return forums that
        /// the user's credentials do not have permissions to.
        /// </summary>
        /// 
        // ********************************************************************/   
        public bool ShowAllForums
        {
            get { return showAllForums; }
            set { showAllForums = value; }
        }


        // *********************************************************************
        //  BuildHeaderTemplate
        //
        /// <summary>
        /// Builds the default header template if the user does not specify one
        /// </summary>
        /// 
        // ********************************************************************/
        public virtual void BuildHeaderTemplate(Control _ctrl)
        {
            TableRow tr = new TableRow();
            TableHeaderCell th;

            // Build table headers
            th = new TableHeaderCell();
            th.ColumnSpan = 2;
            th.Height = 20;
            th.CssClass = "tableHeaderText";
            th.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "Forum").ToString();
            tr.Controls.Add(th);

            th = new TableHeaderCell();
            th.Width = 50;
            th.Wrap = false;
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "Threads").ToString() + "&nbsp;&nbsp;";
            tr.Controls.Add(th);

            th = new TableHeaderCell();
            th.Width = 50;
            th.Wrap = false;
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "Posts").ToString() + "&nbsp;&nbsp;";
            tr.Controls.Add(th);

            th = new TableHeaderCell();
            th.Width = Unit.Pixel(140);
            th.Wrap = false;
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ForumRepeater", "Last posts").ToString() + "&nbsp;&nbsp;";
            tr.Controls.Add(th);

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(new LiteralControl("<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\" width=\"100%\" class=\"tableBorder\">"));
            __parser.AddParsedSubObject(tr);

        }

        public virtual void BuildFooterTemplate(Control _ctrl)
        {
            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));
            __parser.AddParsedSubObject(new LiteralControl("\n</table>"));
        }


    }
}

