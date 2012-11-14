using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.IO;

namespace AspNetForums.Controls {
    
    [ParseChildren(true)]
    public class WhereAmI : SkinnedForumWebControl {
        private const string skinFilename = "Skin-WhereAmI.ascx";
		
        private bool showHome = false;
        private bool enableLinks = true;
        
        private Forum forum = null;
        private Post post = null;

        // *********************************************************************
        //  WhereAmI
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        // ***********************************************************************/
        public WhereAmI() : base() {
            // Set-up our skin
            if (SkinFilename == null ) {
                SkinFilename = skinFilename;
            }

        }
			
        // *********************************************************************
        //  Initializeskin
        //
        /// <summary>
        /// This method initializes the control with the named skin.
        /// </summary>
        /// <param name="control">Instance of the user control to populate</param>
        /// 
        // ***********************************************************************/
        protected override void InitializeSkin(Control skin) {
            // Get the navigation links
            HyperLink linkHome = skin.FindControl("LinkHome") as HyperLink;
            HyperLink linkForum = skin.FindControl("LinkForum") as HyperLink;
            HyperLink linkPost = skin.FindControl("LinkPost") as HyperLink;
            // Get the separators
            Control forumGroupSep = skin.FindControl("ForumGroupSeparator") as Control;
            if ( forumGroupSep != null ) {
                forumGroupSep.Visible = false;
            }
            Control forumSep = skin.FindControl("ForumSeparator") as Control;
            if ( forumSep != null ) {
                forumSep.Visible = false;
            }
            Control postSep = skin.FindControl("PostSeparator") as Control;
            if ( postSep != null ) {
                postSep.Visible = false;
            }
			
            // Get the Menu Control areas
            HtmlControl forumGroupMenu = skin.FindControl("ForumGroupMenu") as HtmlControl;
            HtmlControl forumMenu = skin.FindControl("ForumMenu") as HtmlControl;
            HtmlControl postMenu = skin.FindControl("PostMenu") as HtmlControl;
			
            if ( showHome && linkHome != null ) {
                linkHome.Visible = true;
                if ( enableLinks )
                    linkHome.NavigateUrl = UrlHome;
                linkHome.Text = RootLabel;
            }
            else {
                linkHome.Visible = false;
            }
			
            if ( Forum != null && linkForum != null ) {
                if ( linkHome != null && ShowHome && forumSep != null ) {
                    forumSep.Visible = true;
                }

                linkForum.Visible = true;
                if ( enableLinks )
                    linkForum.NavigateUrl = UrlShowForum + Forum.ForumID;
                linkForum.Text = Forum.Name;

            }
            else {
                linkForum.Visible = false;
            }
			
            if ( Post != null && linkPost != null ) {
                if ( linkForum != null && postSep != null ) {
                    postSep.Visible = true;
                }
                
                linkPost.Visible = true;
                if ( enableLinks )
                    linkPost.NavigateUrl = UrlShowPost + Post.ThreadID;
                linkPost.Text = Post.Subject;

            }
            else {
                linkPost.Visible = false;
            }

        }
		

        private Forum Forum {
            get {
                if ( forum == null ) {
                    if ( ForumID > -1 ) {
                        forum = Forums.GetForumInfo(ForumID);
                    }
                    else {
                        if ( PostID > -1 ) {
                            forum = Forums.GetForumInfo(Post.ForumID);
                        }
                    }
                }
				
                return forum;
            }
        }
		
        private Post Post {
            get {
                if ( post == null ) {
                    if ( PostID > -1 ) {
                        post = Posts.GetPost(PostID, ((int)Session["UserID"]).ToString());
                    }
                }
				
                return post;
            }
        }

        public string UrlHome {
            get {
                if (ViewState["urlHome"] == null)
                    return Globals.UrlHome;
					
                return ViewState["urlHome"].ToString();
            }
            set {
                // set the viewstate
                ViewState["urlHome"] = value;
            }
        }

        public string RootLabel {
            get {
                if (ViewState["rootLabel"] == null)
                    return Globals.SiteName;
					
                return ViewState["rootLabel"].ToString();
            }
            set {
                // set the viewstate
                ViewState["rootLabel"] = value;
            }
        }


        public string UrlShowForum {
            get {
                if (ViewState["urlShowForum"] == null)
                    return Globals.UrlShowForum;
					
                return ViewState["urlShowForum"].ToString();
            }
            set {
                // set the viewstate
                ViewState["urlShowForum"] = value;
            }
        }

        public string UrlShowPost {
            get {
                if (ViewState["urlShowPost"] == null)
                    return Globals.UrlShowPost;
					
                return ViewState["urlShowPost"].ToString();
            }
            set {
                // set the viewstate
                ViewState["urlShowPost"] = value;
            }
        }

        /****************************************************************
        // ShowHome
        //
        /// <summary>
        /// Controls whether or not the root element for the home is shown
        /// </summary>
        //
        ****************************************************************/
        public bool ShowHome {
            get {return showHome; }
            set {showHome = value; }
        }

        /****************************************************************
        // EnableLinks
        //
        /// <summary>
        ///  Determines whether or not links are hook-ed up.
        /// </summary>
        //
        ****************************************************************/
        public bool EnableLinks {
            get {return enableLinks; }
            set {enableLinks = value; }
        }
    }
}