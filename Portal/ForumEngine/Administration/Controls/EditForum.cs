using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using AspNetForums.Controls.Moderation;
using System.ComponentModel;

namespace AspNetForums.Controls.Admin {

    /// <summary>
    /// This Web control displays a forum for the end user to edit.  The end user can edit the
    /// forum's properties (Title, Description, etc.) from here, as well as view the list of
    /// users who moderate the particular forum.  Additionally the users can edit or delete posts
    /// for this forum.
    /// </summary>
    [
        ParseChildren(true)
    ]
    public class EditForum : WebControl, INamingContainer {

        const String defaultSeparator = "<hr noshade size=\"1px\" />\n";		// the default content separator

        CreateEditForum editForum = new CreateEditForum();


        public EditForum() {
            // If we have an instance of context, let's attempt to
            // get the ForumID so we can save the user from writing
            // the code
            if (null != Context) {

                if (null != Context.Request.QueryString["ForumId"])
                    this.ForumID = Convert.ToInt32(Context.Request.QueryString["ForumId"]);
                else if (null != Context.Request.Form["ForumId"])
                    this.ForumID = Convert.ToInt32(Context.Request.Form["ForumId"]);

            }        
        }

        // *********************************************************************
        //  CreateChildControls
        //
        /// <summary>
        /// This event handler adds the children controls.
        /// </summary>
        //
        // ********************************************************************/
        protected override void CreateChildControls() {

            // Is this user isn't an administrator?
            if (!Users.GetLoggedOnUser().IsAdministrator)
                Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));

            // make sure we have a ForumID
            if (ForumID == -1)
                throw new Exception("You must pass in a valid ForumID in order to use the EditForum control.");

            // create an instance of the CreateEditForum Web control
            editForum.ForumID = ForumID;
            editForum.Mode = CreateEditForumMode.EditForum;
            Controls.Add(editForum);

        }



        // *********************************************************************
        //  ShowPostsInForum
        //
        /// <summary>
        /// This property indicates what HTML should appear between each section of the forum
        /// edit page.  The sections that are separated out are: the Forum properties (Title,
        /// Description, etc.); the list of users who moderate the particular forum; and (optionally)
        /// the forum's posts, which can be edited and deleted.
        /// </summary>
        /// 
        // ********************************************************************/
        public String Separator {

            get {
                if (ViewState["separator"] == null) 
                    return defaultSeparator;

                return (String) ViewState["separator"];
            }

            set {  
                ViewState["separator"] = value;  
            }

        }

        // *********************************************************************
        //  CreateEditForum
        //
        /// <summary>
        /// An instance of the CreateEditForum Web control, which the EditForum
        /// Web control uses internally to allow the end user to edit the forum's
        /// properties (forum Title, Description, etc.).  This exposed property allows
        /// end developers to customize the look and feel of these options.
        /// </summary>
        /// 
        // ********************************************************************/
        public CreateEditForum CreateEditForum {
            get { 
                return editForum; 
            }
        }



        // *********************************************************************
        //  ForumID
        //
        /// <summary>
        /// The ForumID of the Forum to edit.
        /// </summary>
        /// <remarks>
        /// If ForumID is not specified, an Exception is thrown.
        /// </remarks>
        /// 
        // ********************************************************************/
        public int ForumID {
            get {
                if (ViewState["forumID"] == null) 
                    return -1;

                return (int) ViewState["forumID"];
            }

            set { 
                ViewState["forumID"] = value;  
            }
        }

        // *********************************************************************
        //  CheckUserPermissions
        //
        /// <summary>
        /// Indicates if the Web control should check to verify that the user visiting the page
        /// is, indeed, a moderator.
        /// </summary>
        /// 
        // ********************************************************************/
        public bool CheckUserPermissions {
            get {
                if (ViewState["checkUserPerm"] == null) 
                    return true;

                return (bool) ViewState["checkUserPerm"];
            }

            set { 
                ViewState["checkUserPerm"] = value; 
            }

        }
    }
}
