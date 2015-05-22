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
using System.Globalization;

namespace AspNetForums.Controls
{

    /// <summary>
    /// This control is a specialized datalist that is capable of rendering a list
    /// of threads that are provided through a Threads property.
    /// </summary>
    [
    ParseChildren(true)
    ]
    public class ThreadList : DataList
    {

        User user;
        String skinName;

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
            if (Page.Request.IsAuthenticated)
            {
                user = Users.GetLoggedOnUser();
            }
            // Set the siteStyle for the page
            skinName = Globals.Skin;

            // Viewstate is disabled
            this.EnableViewState = false;

            // Apply Template
            ApplyTemplates();

        }


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

            // Column 1
            th = new TableHeaderCell();
            th.ColumnSpan = 2;
            th.Height = 25;
            th.CssClass = "tableHeaderText";
            th.HorizontalAlign = HorizontalAlign.Center;
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Topic").ToString() + "&nbsp;";
            th.Width = Unit.Percentage(50);

            tr.Controls.Add(th);

            // Column 3
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Author").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Center;
            th.Width = Unit.Pixel(100);
            th.Wrap = false;
            tr.Controls.Add(th);


            // Replies Column 
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Replies").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Center;
            th.Width = Unit.Percentage(10);
            tr.Controls.Add(th);


            // Views Column
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Views").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Center;
            th.Width = Unit.Percentage(10);
            tr.Controls.Add(th);

            // Last Post
            th = new TableHeaderCell();
            th.CssClass = "tableHeaderText";
            th.Text = "&nbsp;" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Last Post").ToString() + "&nbsp;";
            th.HorizontalAlign = HorizontalAlign.Center;
            th.Wrap = false;
            th.Width = Unit.Percentage(15);
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
            Label link;
            PlaceHolder placeHolder;
            Label label;

            // Create a new table
            table = new Table();

            // Create a new table row
            tr = new TableRow();

            // Column 1
            td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Left;
            td.VerticalAlign = VerticalAlign.Middle;
            td.CssClass = "forumRow";
            td.Width = Unit.Pixel(25);
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.DataBinding += new System.EventHandler(HandleDataBindingForImage);
            td.Controls.Add(img);
            tr.Controls.Add(td);

            // Column 2
            td = new TableCell();
            td.Height = 25;
            td.CssClass = "forumRow";
            td.Width = Unit.Percentage(70);
            placeHolder = new PlaceHolder();
            placeHolder.DataBinding += new System.EventHandler(HandleDatabindingForPostSubject);
            td.Controls.Add(placeHolder);
            tr.Controls.Add(td);

            // Author Column
            td = new TableCell();
            td.CssClass = "forumRow";
            td.Width = 100;
            td.HorizontalAlign = HorizontalAlign.Center;
            link = new Label();
            link.DataBinding += new System.EventHandler(HandleDatabindingForAuthor);
            link.CssClass = "normalTextSmall";
            td.Controls.Add(link);
            tr.Controls.Add(td);

            // Replies Column
            td = new TableCell();
            td.CssClass = "forumRow";
            td.Width = 50;
            td.HorizontalAlign = HorizontalAlign.Center;
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.DataBinding += new System.EventHandler(HandleDatabindingForTotalReplies);
            td.Controls.Add(label);
            tr.Controls.Add(td);

            // Views Column
            td = new TableCell();
            td.CssClass = "forumRow";
            td.Width = 50;
            td.HorizontalAlign = HorizontalAlign.Center;
            label = new Label();
            label.CssClass = "normalTextSmall";
            label.DataBinding += new System.EventHandler(HandleDatabindingForTotalViews);
            td.Controls.Add(label);
            tr.Controls.Add(td);

            // Post Date Column
            td = new TableCell();
            td.CssClass = "forumRow";
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Width = 140;
            placeHolder = new PlaceHolder();
            placeHolder.DataBinding += new System.EventHandler(HandleDatabindingForPostDetails);
            td.Controls.Add(placeHolder);
            td.Wrap = false;
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
            table = new Table();

            return table;

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
            string pathToFooterTemplate;
            string pathToItemTemplate;
            string pathToAlternatingItemTemplate;
            string keyForHeaderTemplate;
            string keyForItemTemplate;
            string keyForAlternatingItemTemplate;
            string keyForFooterTemplate;

            // Are we using skinned template?
            if (Page != null)
            {

                // Set the file paths to where the templates should be found
                keyForHeaderTemplate = skinName + "/Templates/ThreadList-Header.ascx";
                keyForItemTemplate = skinName + "/Templates/ThreadList-Item.ascx";
                keyForAlternatingItemTemplate = skinName + "/Templates/ThreadList-AlternatingItem.ascx";
                keyForFooterTemplate = skinName + "/Templates/ThreadList-Footer.ascx";

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

            // If the item template is null we force our view
            if (ItemTemplate == null)
            {
                ExtractTemplateRows = true;
                HeaderTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildHeaderTemplate));
                ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildItemTemplate));
                FooterTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(BuildFooterTemplate));
            }
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
        /// This function is called to create the template for the header
        /// </summary>
        /// 
        // ********************************************************************/
        private void BuildItemTemplate(Control _ctrl)
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
        private void BuildFooterTemplate(Control _ctrl)
        {

            System.Web.UI.IParserAccessor __parser = ((System.Web.UI.IParserAccessor)(_ctrl));

            __parser.AddParsedSubObject(RenderFooterTemplate());

        }


        // *********************************************************************
        //  HandleDataBindingForImage
        //
        /// <summary>
        /// Handle data binding for the image rendered next to the post
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDataBindingForImage(Object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)sender;
            DataListItem container = (DataListItem)img.NamingContainer;

            Thread thread = (Thread)container.DataItem;

            if (thread.IsPopular)
            {
                img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-popular.gif";
                img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Popular").ToString();
            }
            else if (thread.IsAnnouncement)
            {
                if (thread.HasRead)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-announce.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "AnnouncementRead").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-announce_notread.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "AnnouncementNotRead").ToString();
                }
            }
            else if ((thread.IsPinned) && (thread.IsLocked))
            {
                if (thread.HasRead)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-pinned_locked.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "PinnedLocked").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-pinned_locked_notread.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "PinnedLockedNotRead").ToString();
                }
            }
            else if (thread.IsPinned)
            {
                if (thread.HasRead)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-pinned.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Pinned").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-pinned_notread.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "PinnedNotRead").ToString();
                }
            }
            else if (thread.IsLocked)
            {
                if (thread.HasRead)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-locked.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Locked").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic-locked_notread.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "LockedNotRead").ToString();
                }
            }
            else
            {
                if (thread.HasRead)
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Post").ToString();
                }
                else
                {
                    img.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/topic_notread.gif";
                    img.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "PostNotRead").ToString();
                }
            }

        }

        // *********************************************************************
        //  HandleDatabindingForPostSubject
        //
        /// <summary>
        /// Handle data binding for the post subject
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForPostSubject(Object sender, EventArgs e)
        {
            HyperLink forumTitle = new HyperLink();
            Label label;
            String subject;
            bool subjectTooLong = false;
            PlaceHolder placeHolder = (PlaceHolder)sender;
            DataListItem container = (DataListItem)placeHolder.NamingContainer;

            Thread thread = (Thread)container.DataItem;

            if (thread.IsAnnouncement)
            {
                label = new Label();
                label.CssClass = "normalTextSmallBold";
                label.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Announcement").ToString();
                placeHolder.Controls.Add(label);
            }

            // Get the subject
            subject = thread.Subject;

            // If the subject is > 30 cut it down and there is not whitespace
            if ((subject.Length > 30) && (subject.IndexOf(" ") == -1))
            {
                subject = subject.Substring(0, 30);
                subjectTooLong = true;
            }
            else if (subject.Length > 60)
            {

                // We have whitespace, but the string is still exceptionally long
                if (subject.Substring(30, 30).IndexOf(" ") == -1)
                {
                    subject = subject.Substring(0, 30);
                    subjectTooLong = true;
                }
            }

            forumTitle.Text = subject;
            forumTitle.NavigateUrl = Globals.UrlShowPost + thread.PostID;
            placeHolder.Controls.Add(forumTitle);

            // Add elipses
            if (subjectTooLong)
            {
                label = new Label();
                label.CssClass = "normalTextSmallBold";
                label.Text = " ...";
                placeHolder.Controls.Add(label);
            }

            // Do we have more than 
            if (thread.Replies >= Globals.PageSize)
            {
                HyperLink link;

                // Add the opening paren
                label = new Label();
                label.CssClass = "normalTextSmall";
                label.Text = " (" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "page").ToString() + ": ";
                placeHolder.Controls.Add(label);

                // Get the total number of pages available
                int totalPages = Paging.CalculateTotalPages((thread.Replies + 1), Globals.PageSize,Threads.GetTotalPinnedPostsForThread(thread.ThreadID));

                // Display the first 3
                if (totalPages < 4)
                {
                    for (int i = 0; i < totalPages; i++)
                    {

                        link = new HyperLink();
                        link.Text = (i + 1).ToString();
                        link.NavigateUrl = Globals.UrlShowPost + thread.PostID + "&PageIndex=" + (i + 1).ToString();
                        placeHolder.Controls.Add(link);

                        if ((i + 1) != totalPages)
                        {
                            label = new Label();
                            label.CssClass = "normalTextSmall";
                            label.Text = ", ";
                            placeHolder.Controls.Add(label);
                        }
                    }
                }

                // Do we need elipses?
                if (totalPages >= 4)
                {
                    if (totalPages < 6)
                    {
                        for (int i = 0; i < totalPages; i++)
                        {
                            link = new HyperLink();
                            link.Text = (i + 1).ToString();
                            link.NavigateUrl = Globals.UrlShowPost + thread.PostID + "&PageIndex=" + (i + 1).ToString();
                            placeHolder.Controls.Add(link);

                            if ((i + 1) != totalPages)
                            {
                                label = new Label();
                                label.CssClass = "normalTextSmall";
                                label.Text = ", ";
                                placeHolder.Controls.Add(label);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            link = new HyperLink();
                            link.Text = (i + 1).ToString();
                            link.NavigateUrl = Globals.UrlShowPost + thread.PostID + "&PageIndex=" + (i + 1).ToString();
                            placeHolder.Controls.Add(link);

                            if ((i + 1) < 3)
                            {
                                label = new Label();
                                label.CssClass = "normalTextSmall";
                                label.Text = ", ";
                                placeHolder.Controls.Add(label);
                            }
                        }

                        label = new Label();
                        label.CssClass = "normalTextSmall";
                        label.Text = " ... ";
                        placeHolder.Controls.Add(label);

                        for (int i = totalPages - 2; i < totalPages; i++)
                        {
                            link = new HyperLink();
                            link.Text = (i + 1).ToString();
                            link.NavigateUrl = Globals.UrlShowPost + thread.PostID + "&PageIndex=" + (i + 1).ToString();
                            placeHolder.Controls.Add(link);

                            if ((i + 1) != totalPages)
                            {
                                label = new Label();
                                label.CssClass = "normalTextSmall";
                                label.Text = ", ";
                                placeHolder.Controls.Add(label);
                            }
                        }
                    }
                }

                // Add the closing paren
                label = new Label();
                label.CssClass = "normalTextSmall";
                label.Text = ")";
                placeHolder.Controls.Add(label);
            }
        }

        // *********************************************************************
        //  HandleDatabindingForTotalReplies
        //
        /// <summary>
        /// Handle data binding for the total replies to the post
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForTotalReplies(Object sender, EventArgs e)
        {
            Label totalReplies = (Label)sender;
            DataListItem container = (DataListItem)totalReplies.NamingContainer;

            Thread thread = (Thread)container.DataItem;

            if (thread.Replies == 0)
                totalReplies.Text = "-";
            else
                totalReplies.Text = thread.Replies.ToString("n0");
        }

        // *********************************************************************
        //  HandleDatabindingForTotalViews
        //
        /// <summary>
        /// Handle data binding for the total views for the post
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForTotalViews(Object sender, EventArgs e)
        {
            Label totalViews = (Label)sender;
            DataListItem container = (DataListItem)totalViews.NamingContainer;

            Thread thread = (Thread)container.DataItem;

            totalViews.Text = thread.Views.ToString("n0");
        }

        // *********************************************************************
        //  HandleDatabindingForAuthor
        //
        /// <summary>
        /// Handle data binding for the author of the post
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForAuthor(Object sender, EventArgs e)
        {
            Label author = (Label)sender;
            DataListItem container = (DataListItem)author.NamingContainer;

            Thread thread = (Thread)container.DataItem;

            author.Text = Users.GetUserInfo(thread.Username).DisplayName;
        }

        // *********************************************************************
        //  HandleDatabindingForPostDetails
        //
        /// <summary>
        /// Handle data binding for the details about the most recent post
        /// </summary>
        /// 
        // ********************************************************************/
        private void HandleDatabindingForPostDetails(Object sender, EventArgs e)
        {
            PlaceHolder postDetails = (PlaceHolder)sender;
            DataListItem container = (DataListItem)postDetails.NamingContainer;
            DateTime postDateTime;
            Label postDate = new Label();
            HyperLink newPost = new HyperLink();
            Label author = new Label();
            String dateFormat;


            // Set the style of the lablel
            postDate.CssClass = "normalTextSmaller";

            // Do we have a signed in user?
            dateFormat = Globals.DateFormat;

            // Get the post
            Thread thread = (Thread)container.DataItem;

            // Get the post date
            postDateTime = thread.ThreadDate;

            // Did the post occur today?
            if (thread.IsAnnouncement)
                postDate.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "AnnouncementDate").ToString();
            else if (thread.IsPinned)
                postDate.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "PinnedDate").ToString();
            else if ((postDateTime.DayOfYear == DateTime.Now.DayOfYear) && (postDateTime.Year == DateTime.Now.Year))
                postDate.Text = "<b>" + HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "Today").ToString() + postDateTime.ToString(Globals.TimeFormat) + "</b><br> ";
            else
                postDate.Text = postDateTime.ToString(dateFormat + " " + Globals.TimeFormat) + "<br> ";

            // Add the post
            postDetails.Controls.Add(postDate);

            // Add the post author
            author.Text = Users.GetUserInfo(thread.MostRecentPostAuthorID).DisplayName;
            author.CssClass = "normalTextSmall";
            postDetails.Controls.Add(author);

            // Link to new post - we need to figure out what page the post is on
            newPost.Text = "<img border=\"0\" src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + skinName + "/images/icon_mini_topic.gif\">";
            newPost.ToolTip = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "ThreadList", "GoToLast").ToString();
            if ((thread.Replies + 1) > Globals.PageSize)
            {
                int totalPages = Paging.CalculateTotalPages(thread.Replies + 1, Globals.PageSize,Threads.GetTotalPinnedPostsForThread(thread.ThreadID));

                // Newest post will be on the last page
                newPost.NavigateUrl = Globals.UrlShowPost + thread.PostID + "&PageIndex=" + totalPages + "#" + thread.MostRecentPostID;
            }
            else
            {
                newPost.NavigateUrl = Globals.UrlShowPost + thread.PostID + "#" + thread.MostRecentPostID;
            }
            postDetails.Controls.Add(newPost);
        }
    }
}