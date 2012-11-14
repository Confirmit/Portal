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
using System.Web.Security;

namespace AspNetForums.Controls.Moderation
{

    // *********************************************************************
    //  ModerationMenu
    //
    /// <summary>
    /// This server control provides moderators with options for managing posts.
    /// </summary>
    // ********************************************************************/ 
    public class ModerationMenu : SkinnedForumWebControl
    {

        string skinFilename = "Moderation/Skin-ModerationMenu.ascx";
        LinkButton turnOffModerationForUser;
        HyperLink movePost;
        HyperLink moderateThread;
        HyperLink deletePost;
        HyperLink editPost;
        Label usernameLabel;
        Label postIDLabel;
        string usernamePostedBy;

        // *********************************************************************
        //  ModerationMenu
        //
        /// <summary>
        /// Constructor for moderation menu.
        /// </summary>
        // ***********************************************************************/
        public ModerationMenu()
            : base()
        {
            // Is this user isn't an administrator?
            if (!Users.GetLoggedOnUser().IsAdministrator)
                Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));

            // Assign a default template name
            if (SkinFilename == null)
                SkinFilename = skinFilename;


        }

        // *********************************************************************
        //  Initializeskin
        //
        /// <summary>
        /// Initialize the control template and populate the control with values
        /// </summary>
        // ***********************************************************************/
        override protected void InitializeSkin(Control skin)
        {

            // We require a post id value
            if (PostID == -1)
                throw new AspNetForums.Components.PostNotFoundException("You must set the PostID property of this control to the post being moderated.");

            // Find the controls in our control template.

            // Turn off moderation for this user
            turnOffModerationForUser = (LinkButton)skin.FindControl("TurnOffModeration");
            if (null != turnOffModerationForUser)
            {
                turnOffModerationForUser.CommandArgument = UsernamePostedBy;
                turnOffModerationForUser.Command += new CommandEventHandler(TurnOffModerationForUser_Command);
            }

            // Delete the post/thread
            deletePost = (HyperLink)skin.FindControl("DeletePost");
            if (null != deletePost)
            {
                // Go to the delete view
                deletePost.NavigateUrl = Globals.UrlDeletePost + PostID + "&ReturnURL=" + Page.Server.UrlEncode(Page.Request.RawUrl);
            }

            // Move the post/thread
            movePost = (HyperLink)skin.FindControl("MovePost");
            if (null != movePost)
            {

                if (ThreadID == PostID)
                    movePost.NavigateUrl = Globals.UrlMovePost + PostID + "&ReturnURL=" + Page.Server.UrlEncode(HttpContext.Current.Request.RawUrl);
                else
                {
                    movePost.Visible= false;
                    Label Sep = (Label)skin.FindControl("Sep");
                    Sep.Visible = false;
                }
            }

            // Edit the post
            editPost = (HyperLink)skin.FindControl("EditPost");
            if (null != editPost)
            {
                editPost.NavigateUrl = Globals.UrlEditPost + PostID;
            }

            // Moderate Thread
            moderateThread = (HyperLink)skin.FindControl("ModerateThread");
            if (null != moderateThread)
            {
                moderateThread.NavigateUrl = Globals.UrlModerateThread + PostID;
            }

            // Username of user that created the post
            usernameLabel = (Label)skin.FindControl("Username");
            if (null != usernameLabel)
                usernameLabel.Text = UsernamePostedBy;

            // Display the PostID for the moderator
            postIDLabel = (Label)skin.FindControl("PostID");
            if (null != postIDLabel)
                postIDLabel.Text = PostID.ToString();
        }


        // *********************************************************************
        //  TurnOffModerationForUser_Command
        //
        /// <summary>
        /// Turn off moderation for a given user
        /// </summary>
        // ***********************************************************************/
        public void TurnOffModerationForUser_Command(Object sender, CommandEventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
        }

        // *********************************************************************
        //  Username
        //
        /// <summary>
        /// Name of the user that the post this moderation menu is applied to.
        /// </summary>
        // ***********************************************************************/
        public string UsernamePostedBy
        {
            get { return usernamePostedBy; }
            set { usernamePostedBy = value; }
        }

    }
}
