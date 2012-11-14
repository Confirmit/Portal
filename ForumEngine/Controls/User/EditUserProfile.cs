using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspNetForums;
using AspNetForums.Controls.Moderation;
using AspNetForums.Components;
using System.ComponentModel;
using System.IO;

namespace AspNetForums.Controls
{
    public class EditUserProfile : SkinnedForumWebControl
    {
        private UserInfoEditMode adminMode = UserInfoEditMode.Edit;
        private User user = null;
        string skinFilename = "Skin-EditUserInfo.ascx";
        bool requirePasswordForUpdate = false;
        //Control control;

        // *********************************************************************
        //  EditUserInfo
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        // ********************************************************************/
        public EditUserProfile()
        {
            // Is this user isn't an administrator?
            if (!Users.GetLoggedOnUser().IsAdministrator)
                Context.Response.Redirect(Globals.UrlMessage + Convert.ToInt32(Messages.UnableToAdminister));

            // Set the default skin
            if (SkinFilename == null)
                SkinFilename = skinFilename;
        }


        // *********************************************************************
        //  Initializeskin
        //
        /// <summary>
        /// This method populates the user control used to edit a user's information
        /// </summary>
        /// <param name="control">Instance of the user control to populate</param>
        /// 
        // ***********************************************************************/
        override protected void InitializeSkin(Control skin)
        {
            Label label;
            CheckBox checkbox;
            Button submit;

            // Set the name
            label = (Label)skin.FindControl("Username");
            label.Text = EditUser.DisplayName;
            // Is the user banned
            if (AdminMode == UserInfoEditMode.Admin)
            {
                checkbox = (CheckBox)skin.FindControl("Banned");
                checkbox.Checked = !EditUser.IsApproved;
            }

            // Wire-up the button
            submit = (Button)skin.FindControl("Submit");
            submit.Click += new System.EventHandler(UpdateUserInfo_ButtonClick);
        }


        // *********************************************************************
        //  UpdateUserInfo_ButtonClick
        //
        /// <summary>
        /// This event is raised when the user clicks the submit button in the user
        /// control loaded in the DisplayEditMode function. This event is responsible
        /// for processing the form values and writing them back to the database if
        /// necessary.
        /// </summary>
        /// 
        // ********************************************************************/
        private void UpdateUserInfo_ButtonClick(Object sender, EventArgs e)
        {
            // Ensure the page is valid
            if (!Page.IsValid)
                return;

            Control skin;
            CheckBox checkbox;

            // Find the EditUserInformation user control
            skin = ((Control)sender).Parent;

            checkbox = (CheckBox)skin.FindControl("Banned");
            EditUser.IsApproved = !checkbox.Checked;

            Users.UpdateUserInfoFromAdminPage(EditUser);

            // the user was updated successfully
            Context.Response.Redirect("UserAdmin.aspx");
            Context.Response.End();
            
        }

        /// <summary>
        ///  Allow for Administration mode
        /// </summary>
        public UserInfoEditMode AdminMode
        {
            get
            {
                return adminMode;
            }
            set
            {
                adminMode = value;
            }
        }

        /// <summary>
        ///  Dynamically update the editable user to enable admin vs normal
        ///  user mode.
        /// </summary>
        protected User EditUser
        {
            get
            {
                if (user == null)
                {
                    if (AdminMode == UserInfoEditMode.Admin)
                    {
                        if (Context != null)
                        {
                            string userName = Context.Request["Username"];

                            if (userName != null)
                            {
                                user = Users.GetUserInfo(userName);
                            }
                        }
                    }

                    if (user == null)
                    {
                        user = ForumUser;
                    }
                }

                return user;
            }
        }

        public bool RequirePasswordForUpdate
        {
            get { return requirePasswordForUpdate; }
            set { requirePasswordForUpdate = value; }
        }
    }
}