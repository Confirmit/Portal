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

namespace AspNetForums.Controls
{

    /// <summary>
    /// This server control is used to display all the members of the current forum.
    /// </summary>
    [
        ParseChildren(true)
    ]
    public class ShowAllUsers : SkinnedForumWebControl
    {
        // Define the default skin for this control
        private const string skinFilename = "Skin-ShowAllUsers.ascx";

        UserList userList;
        Paging pager;
        bool isSearchMode = false;

        // *********************************************************************
        //  ShowAllUsers
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        // ********************************************************************/
        public ShowAllUsers()
        {
            // Set the default skin
            if (SkinFilename == null)
            {
                SkinFilename = skinFilename;
            }
        }


        // *********************************************************************
        //  InitializeControlTemplate
        //
        /// <summary>
        /// Initializes the user control loaded in CreateChildControls. Initialization
        /// consists of finding well known control names and wiring up any necessary events.
        /// </summary>
        /// 
        // ********************************************************************/ 
        override protected void InitializeSkin(Control skin)
        {

            // Find the UserList server control in our template
            userList = (UserList)skin.FindControl("UserList");
            userList.EnableViewState = false;

            // Find the pager control
            pager = (Paging)skin.FindControl("Pager");
            // Get the total records used in the pager
            pager.PageSize = 50;
            pager.TotalRecords = Users.TotalNumberOfUserAccounts();
            pager.PageIndex_Changed += new System.EventHandler(PageIndex_Changed);

            if (!Page.IsPostBack)
                SetDataSource();

        }


        // *********************************************************************
        //  Search_Click
        //
        /// <summary>
        /// Event raised when the user opt's to perform a search
        /// </summary>
        /// 
        // ********************************************************************/
        private void Search_Click(Object sender, EventArgs e)
        {
            pager.PageIndex = 0;
            IsSearchMode = true;
            SetDataSource();
        }

        // *********************************************************************
        //  SetDataSource
        //
        /// <summary>
        /// Private event used to set the datasource based on options selected by the user.
        /// </summary>
        /// 
        // ********************************************************************/
        public void SetDataSource()
        {
            //Users.SortUsersBy enumSortBy = 0;
            userList.DataSource = Users.GetAllUsers(pager.PageIndex, pager.PageSize);

            // Get the total records used in the pager
            pager.TotalRecords = Users.TotalNumberOfUserAccounts();

            userList.UserCountIsAscending = true;
            userList.UserCount = (pager.PageIndex * pager.PageSize) + 1;



        }

        // *********************************************************************
        //  SortByList_Changed
        //
        /// <summary>
        /// Event raised when the sort by dropdownlist value changes
        /// </summary>
        /// 
        // ********************************************************************/
        public void SortByList_Changed(Object sender, EventArgs e)
        {
            pager.PageIndex = 0;
            SetDataSource();
        }

        // *********************************************************************
        //  SortDirection_Changed
        //
        /// <summary>
        /// Event raised when the sort direction dropdownlist value changes
        /// </summary>
        /// 
        // ********************************************************************/
        public void SortDirection_Changed(Object sender, EventArgs e)
        {
            pager.PageIndex = 0;
            SetDataSource();
        }

        // *********************************************************************
        //  PageIndex_Changed
        //
        /// <summary>
        /// Event raised when the selected index of the page has changed.
        /// </summary>
        /// 
        // ********************************************************************/
        private void PageIndex_Changed(Object sender, EventArgs e)
        {
            SetDataSource();
        }

        // *********************************************************************
        //  OnPreRender
        //
        /// <summary>
        /// Override OnPreRender and databind
        /// </summary>
        /// 
        // ********************************************************************/ 
        protected override void OnPreRender(EventArgs e)
        {
            DataBind();
        }

        // *********************************************************************
        //  IsSearchMode
        //
        /// <summary>
        /// Private property to determine if we're in search mode or doing a linear
        /// walkthrough of users
        /// </summary>
        /// 
        // ********************************************************************/ 
        private bool IsSearchMode
        {
            get
            {
                if (ViewState["IsSearchMode"] == null)
                    return isSearchMode;

                return (bool)ViewState["IsSearchMode"];
            }
            set
            {
                ViewState["IsSearchMode"] = value;
            }
        }
    }
}