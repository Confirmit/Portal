using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using AspNetForums;
using AspNetForums.Components;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace AspNetForums.Controls
{

    // *********************************************************************
    //  NavigationMenu
    //
    /// <summary>
    /// This control renders a navigation menu used to navigate the hierarchy
    /// of the Discussion Board. It's links are generated from paths named
    /// in the web.config file.
    /// </summary>
    // ********************************************************************/ 
    public class NavigationMenu : SkinnedForumWebControl
    {
        static string skinFilename = "Skin-Navigation.ascx";

        string menuItemTextAdmin;
        string menuItemTextMyForums;
        string menuItemTextSearch;
        string menuItemTextHome;
        bool displayTitle = true; // by default we display the title

        // *********************************************************************
        //  NavigationMenu
        //
        /// <summary>
        /// Constructor
        /// </summary>
        // ***********************************************************************/
        public NavigationMenu()
            : base()
        {
            
            if (SkinFilename == null)
                SkinFilename = skinFilename;
        }

        // *********************************************************************
        //  InitializeSkin
        //
        /// <summary>
        /// Initialize the control template and populate the control with values
        /// </summary>
        // ***********************************************************************/
        override protected void InitializeSkin(Control skin)
        {
            HyperLink link;
            HyperLink searchMenu;
            HyperLink homeMenu;
            HyperLink adminMenu;
            HyperLink myForumsMenu;
            Image image;

            menuItemTextAdmin = HttpContext.GetLocalResourceObject(Globals.SkinsDir + skinFilename, "menuItemTextAdmin").ToString() + "&nbsp;";
            menuItemTextMyForums = HttpContext.GetLocalResourceObject(Globals.SkinsDir + skinFilename, "menuItemTextMyForums").ToString() + "&nbsp;";
            menuItemTextSearch = HttpContext.GetLocalResourceObject(Globals.SkinsDir + skinFilename, "menuItemTextSearch").ToString() + "&nbsp;";
            menuItemTextHome = HttpContext.GetLocalResourceObject(Globals.SkinsDir + skinFilename, "menuItemTextHome").ToString() + "&nbsp;";


            // Find the Home Hyperlink
            link = (HyperLink)skin.FindControl("Home");
            if (DisplayTitle)
            {
                if (link != null)
                {
                    link.NavigateUrl = Globals.UrlHome;

                    // Find the home image
                    image = (Image)skin.FindControl("HomeImage");
                    if (image != null)
                        image.ImageUrl = Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/title.gif";
                }
            }
            else
            {
                link.Visible = false;
            }


            // Find the controls we need
            searchMenu = (HyperLink)skin.FindControl("SearchMenu");
            homeMenu = (HyperLink)skin.FindControl("HomeMenu");
            adminMenu = (HyperLink)skin.FindControl("AdminMenu");
            myForumsMenu = (HyperLink)skin.FindControl("MyForumsMenu");

            // Search
            searchMenu.Visible = true;
            searchMenu.NavigateUrl = Globals.UrlSearch;
            if (UseIcons)
            {
                searchMenu.Text = "<img src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/icon_mini_search.gif" + "\" border=\"0\">";
            }
            searchMenu.Text += MenuTextForSearch;

            // Display My Forums
            if (Context.Request.IsAuthenticated)
            {
                myForumsMenu.Visible = true;
                myForumsMenu.NavigateUrl = Globals.UrlMyForums;
                if (UseIcons)
                {
                    myForumsMenu.Text = "<img src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/icon_mini_myforums.gif" + "\" border=\"0\">";
                }
                myForumsMenu.Text += MenuTextForMyForums;
            }


            // Home
            homeMenu.Visible = true;
            homeMenu.NavigateUrl = Globals.UrlHome;
            if (UseIcons)
            {
                homeMenu.Text = "<img src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/icon_mini_home.gif" + "\" border=\"0\">";
            }
            homeMenu.Text += MenuTextForHome;


            // Admin?
            if ((null != ForumUser) && (ForumUser.IsAdministrator))
            {
                adminMenu.Visible = true;
                if (UseIcons)
                {
                    adminMenu.Text = "<img src=\"" + Globals.ApplicationVRoot + Globals.ForumsDirectory + "/Skins/" + SkinName + "/images/icon_mini_moderate.gif" + "\" border=\"0\">";
                }

                // Admin
                adminMenu.Text += MenuTextForAdmin;
                adminMenu.NavigateUrl = Globals.UrlAdmin;
            }
        }

        // *********************************************************************
        //  Controls
        //
        /// <summary>
        /// Design time hack to get the control to render.
        /// </summary>
        /// 
        // ********************************************************************/ 
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public string MenuTextForSearch
        {
            get { return menuItemTextSearch; }
            set { menuItemTextSearch = value; }
        }

        public string MenuTextForHome
        {
            get { return menuItemTextHome; }
            set { menuItemTextHome = value; }
        }

        public string MenuTextForAdmin
        {
            get { return menuItemTextAdmin; }
            set { menuItemTextAdmin = value; }
        }

        public string MenuTextForMyForums
        {
            get { return menuItemTextMyForums; }
            set { menuItemTextMyForums = value; }
        }

        public bool UseIcons
        {
            get
            {
                if (ViewState["useIcons"] == null)
                    return true;

                return (bool)ViewState["useIcons"];
            }

            set
            {
                ViewState["useIcons"] = value;
            }
        }

        public bool DisplayTitle
        {
            get
            {
                return displayTitle;
            }
            set
            {
                displayTitle = value;
            }
        }
    }
}