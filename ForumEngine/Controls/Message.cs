using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums.Components;
using System.IO;

namespace AspNetForums.Controls {

    // *********************************************************************
    //  Message
    //
    /// <summary>
    /// Renders the appropriate error message passed in by a query string id value.
    /// </summary>
    // ********************************************************************/ 
    public class Message : SkinnedForumWebControl {
        string skinFilename = "Skin-Message.ascx";
        int messageId = -1;
        string mtitle;
        string mbody;
        //ForumMessage message;

        // *********************************************************************
        //  Message
        //
        /// <summary>
        /// Constructor
        /// </summary>
        //
        // ********************************************************************/
        public Message() : base() {

            // Assign a default template name
            if (SkinFilename == null)
                SkinFilename = skinFilename;

            // Get the error message id
            if (null != Context) {

                // Get the message id
                if (null != Context.Request.QueryString["MessageId"])
                    MessageID = Convert.ToInt32(Context.Request.QueryString["MessageId"]);
                else if (null != Context.Request.Form["MessageId"])
                    MessageID = Convert.ToInt32(Context.Request.Form["MessageId"]);

            }
            mtitle = HttpContext.GetGlobalResourceObject("ForumMessages","mess" + MessageID.ToString()+"_title").ToString();
            mbody = HttpContext.GetGlobalResourceObject("ForumMessages", "mess" + MessageID.ToString() + "_body").ToString();
        }

        // *********************************************************************
        //  Initializeskin
        //
        /// <summary>
        /// Initialize the control template and populate the control with values
        /// </summary>
        // ***********************************************************************/
        override protected void InitializeSkin(Control skin) {
            Label title;
            Label body;

            // Do some processing on the messages
            mbody = mbody.Replace("<UrlHome>", "<a href=\"" + Globals.UrlHome + "\">" + Globals.SiteName + " Первая страница</a>");
            mbody = mbody.Replace("<UrlLogin>", "<a href=\"" + Globals.UrlLogin + "\">" + Globals.SiteName + " Вход</a>");
            mbody = mbody.Replace("<UrlProfile>", "<a href=\"" + Globals.UrlEditUserProfile + "\">профиль пользователя</a>");
            
            // Handle duplicate post messages or moderation messages
            if ((mbody.IndexOf("<DuplicatePost>") > 0) || (mbody.IndexOf("<PendingModeration>") > 0))
            {

                if (ForumID > 0) {
                    mbody = mbody.Replace("<DuplicatePost>", "<a href=\"" + Globals.UrlShowForum + ForumID.ToString() + "\">" + "Вернуться в форум</a>");
                    mbody = mbody.Replace("<PendingModeration>", "<a href=\"" + Globals.UrlShowForum + ForumID.ToString() + "\">" + "Вернуться в форум</a>");
                } else if (PostID > 0) {
                    mbody = mbody.Replace("<DuplicatePost>", "<a href=\"" + Globals.UrlShowPost + PostID.ToString() + "\">" + "Вернуться к сообщению</a>");
                    mbody = mbody.Replace("<PendingModeration>", "<a href=\"" + Globals.UrlShowPost + PostID.ToString() + "\">" + "Вернуться к сообщению</a>");
                }
            }

            // Find the title
            title = (Label) skin.FindControl("MessageTitle");
            if (title != null) {
                title.Text = mtitle;
            }

            // Find the title
            body = (Label) skin.FindControl("MessageBody");
            if (body != null) {
                body.Text = mbody;
            }

        }

        // *********************************************************************
        //  MessageID
        //
        /// <summary>
        /// Property to control the message id
        /// </summary>
        // ***********************************************************************/
        public int MessageID {
            get { return messageId; }
            set { messageId = value; }
        }

    }
}