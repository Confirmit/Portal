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
using System.Web.Security;

namespace AspNetForums.Controls
{

    // *********************************************************************
    //  MyForums
    //
    /// <summary>
    /// A helpful control that simply displays common threads the user has 
    /// participated in or threads that the user is tracking.
    /// </summary>
    // ***********************************************************************/
    public class MyForums : SkinnedForumWebControl
    {

        string skinFilename = "Skin-MyForums.ascx";
        ThreadList threadTracking;
        ThreadList participatedThreads;
        DropDownList LastCount;

        // *********************************************************************
        //  Login
        //
        /// <summary>
        /// Constructor
        /// </summary>
        // ***********************************************************************/
        public MyForums()
            : base()
        {

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
            IDataProviderBase dp = DataProvider.Instance();
            int LC;
            LC = dp.GetLastCount((UlterSystems.PortalLib.BusinessObjects.Person.RequestUser().ID).ToString());

            // Find the thread tracking thread list control
            threadTracking = (ThreadList)skin.FindControl("ThreadTracking");
            if (null != threadTracking)
            {
                ThreadCollection trackedThreads = Threads.GetThreadsUserIsTracking(ForumUser.Username);

                if (trackedThreads.Count > 0)
                {
                    threadTracking.DataSource = trackedThreads;
                    threadTracking.DataBind();
                }
                else
                {
                    threadTracking.Visible = false;
                    Label label = (Label)skin.FindControl("NoTrackedThreads");
                    if (null != label)
                    {
                        label.Visible = true;
                    }

                }
            }

            // Find the participated threads control
            participatedThreads = (ThreadList)skin.FindControl("ParticipatedThreads");
            LastCount = (DropDownList)skin.FindControl("LastCount");
            LastCount.SelectedIndex = (LC - 5) / 5;
            LastCount.SelectedIndexChanged += new System.EventHandler(LastCount_SelectedIndexChanged);
            LastCount_SelectedIndexChanged(null, null);

            if (null != participatedThreads)
            {
                ThreadCollection participatedInThreads = Threads.GetThreadsUserMostRecentlyParticipatedIn(ForumUser.Username, LC);

                if (participatedInThreads.Count > 0)
                {
                    participatedThreads.DataSource = participatedInThreads;
                    participatedThreads.DataBind();
                }
                else
                {
                    participatedThreads.Visible = false;
                    Label label = (Label)skin.FindControl("NoParticipatedThreads");
                    if (null != label)
                    {
                        label.Visible = true;
                    }
                }
            }


        }
        protected void LastCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            IDataProviderBase dp = DataProvider.Instance();
            dp.SetLastCount((UlterSystems.PortalLib.BusinessObjects.Person.RequestUser().ID).ToString(), Int16.Parse(this.LastCount.SelectedValue));

            if (null != participatedThreads)
            {
                ThreadCollection participatedInThreads = Threads.GetThreadsUserMostRecentlyParticipatedIn(ForumUser.Username, Int16.Parse(this.LastCount.SelectedValue));

                if (participatedInThreads.Count > 0)
                {
                    participatedThreads.DataSource = participatedInThreads;
                    participatedThreads.DataBind();
                }
                else
                {
                    Control skin = LoadSkin();
                    participatedThreads.Visible = false;
                    Label label = (Label)skin.FindControl("NoParticipatedThreads");
                    if (null != label)
                    {
                        label.Visible = true;
                    }
                }
            }

        }

    }
}