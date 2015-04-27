using System;
using System.Web;
using AspNetForums.Components;
using System.Web.UI.WebControls;

namespace AspNetForums {

    // *********************************************************************
    //  Forums
    //
    /// <summary>
    /// This class contains methods for working with the Forums.
    /// </summary>
    /// 
    // ********************************************************************/ 
    public class Forums {

        // *********************************************************************
        //  MarkAllThreadsRead
        //
        /// <summary>
        /// Marks all threads in the current forum as read
        /// </summary>
        /// <param name="forumID">Forum to mark threads read for</param>
        /// <param name="username">Username to mark threads read for</param>
        /// 
        // ********************************************************************/ 
        public static void MarkAllThreadsRead(int forumID, string username) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.MarkAllThreadsRead(forumID, username);
        }

        // *********************************************************************
        //  GetAllForums
        //
        /// <summary>
        /// Returns all of the active forums in the database.
        /// </summary>
        /// 
        // ********************************************************************/ 
        public static ForumCollection GetAllForums() {
            return GetAllForums(null);
        }

        // *********************************************************************
        //  ForumListItemCollection
        //
        /// <summary>
        /// A listing of forums.
        /// </summary>
        /// <param name="username">Username making the request</param>
        /// <param name="listStyle">How the list is to be formatted</param>
        /// 
        // ***********************************************************************/
        public ListItemCollection ForumListItemCollection(string username) {
            return ForumListItemCollection(username, null);
        }

        // *********************************************************************
        //  ForumListItemCollection
        //
        /// <summary>
        /// A listing of forums.
        /// </summary>
        /// <param name="username">Username making the request</param>
        /// <param name="listStyle">How the list is to be formatted</param>
        /// 
        // ***********************************************************************/
        public ListItemCollection ForumListItemCollection(string username, ListItemCollection listItems) {

            // Only do this once per request
            if (HttpContext.Current.Items["Moderation-ForumList"] == null) {
                if (listItems == null)
                    listItems = new ListItemCollection();

                ForumCollection forumCollection;
                Forums forums = new Forums();

                forumCollection = Forums.GetAllForums();
                
                foreach (Forum forum in forumCollection) {
                    listItems.Add(new ListItem("---" + forum.Name, "f-" + forum.ForumID.ToString()));
                }

                HttpContext.Current.Items["Moderation-ForumList"] = listItems;

                return listItems;
            } else {
                return (ListItemCollection) HttpContext.Current.Items["Moderation-ForumList"];
            }
        }

        // *********************************************************************
        //  GetAllForums
        //
        /// <summary>
        /// Returns all of the forums in the database.
        /// </summary>
        /// <returns>A ForumCollection with all of the active forums, or all of the active and nonactive
        /// forums, depending on the value of the ShowAllForums property.</returns>
        /// 
        // ***********************************************************************/
        public static ForumCollection GetAllForums(string username) {
            ForumCollection forums = null;

            // If the user is anonymous we'll take some load off the database
            if (username == null) {
                if (HttpContext.Current.Cache["ForumCollection-AllForums-Anonymous"] != null)
                    return (ForumCollection) HttpContext.Current.Cache["ForumCollection-AllForums-Anonymous"];
            }

            // Optimize this method to ensure we only ask for the forums once per request
            if (HttpContext.Current.Items["ForumCollection" + username] ==  null) {
                // Create Instance of the IDataProviderBase
                IDataProviderBase dp = DataProvider.Instance();

                forums = dp.GetAllForums(username);

                // If we have a user add the results to the items collection else add to cache
                if (username == null)
                    HttpContext.Current.Cache.Insert("ForumCollection-AllForums-Anonymous", forums, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                else
                    HttpContext.Current.Items.Add("ForumCollection" + username, forums);

                return forums;
            } else {
                forums = (ForumCollection) HttpContext.Current.Items["ForumCollection" + username];
            }

            return forums;
        }

      


        // *********************************************************************
        //  GetForumInfo
        //
        /// <summary>
        /// Returns information on a particular forum.
        /// </summary>
        /// <param name="ForumID">The ID of the Forum that you are interested in.</param>
        /// <returns>A Forum object with information about the specified forum.</returns>
        /// <remarks>If the passed in ForumID is not found, a ForumNotFoundException exception is
        /// thrown.</remarks>

        // ***********************************************************************/
        public static Forum GetForumInfo(int forumID) {

            if (null == HttpContext.Current.Items["ForumInfo" + forumID]) {
                // Create Instance of the IDataProviderBase
                IDataProviderBase dp = DataProvider.Instance();

                HttpContext.Current.Items["ForumInfo" + forumID] = dp.GetForumInfo(forumID, ((int)HttpContext.Current.Session["UserID"]).ToString());
            }

            return (Forum) HttpContext.Current.Items["ForumInfo" + forumID];
        }

        // *********************************************************************
        //  GetTotalThreadsInForum
        //
        /// <summary>
        /// Used in paging to return a count of the forums based on the query
        /// </summary>
        /// <param name="forumID">id of the forum</param>
        /// <param name="maxDateTime">Datetime filter</param>
        /// <param name="minDateTime">Datetime filter</param>
        // ***********************************************************************/
        public static int GetTotalThreadsInForum(int forumID, DateTime maxDateTime, DateTime minDateTime) {

            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.GetTotalThreadsInForum(forumID, maxDateTime, minDateTime);

        }
        
        // *********************************************************************
        //  GetForumInfoByPostID
        //
        /// <summary>
        /// Returns information about the forum that a particular post exists in.
        /// </summary>
        /// <param name="PostID">The ID of the Post that exists in the forum you're interested in.</param>
        /// <returns>A Forum object containing information about the forum the Post exists in.</returns>
        /// <remarks>If the post is not found or does not contain a valid ForumID, a 
        /// ForumNotFoundException is thrown.</remarks>

        // ***********************************************************************/
        public static Forum GetForumInfoByPostID(int PostID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.GetForumInfoByPostID(PostID);			
        }


        // *********************************************************************
        //  GetForumInfoByThreadID
        //
        /// <summary>
        /// Returns information about the forum that a particular thread exists in.
        /// </summary>
        /// <param name="ThreadID">The ID of the Thread that exists in the forum you're interested in.</param>
        /// <returns>A Forum object containing information about the forum the Thread exists in.</returns>
        /// <remarks>If the thread is not found or does not contain a valid ForumID, a 
 
        // ***********************************************************************/
        public static Forum GetForumInfoByThreadID(int ThreadID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.GetForumInfoByThreadID(ThreadID);
        }


        // *********************************************************************
        //  DeleteForum
        //
        /// <summary>
        /// Deletes a forum and all of the posts in the forum.
        /// </summary>
        /// <param name="ForumID">The ID of the forum to delete.</param>
        /// <remarks>Be very careful when using this method.  The specified forum and ALL of its posts
        /// will be deleted.</remarks>

        // ***********************************************************************/
        public static void DeleteForum(int forumID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.DeleteForum(forumID);
        }


        // *********************************************************************
        //  AddForum
        //
        /// <summary>
        /// Creates a new forum.
        /// </summary>
        /// <param name="forum">Specifies information about the forum to create.</param>

        // ***********************************************************************/
        public static void AddForum(Forum forum) {
            // turn the description into a formatted version
            forum.Description = ForumDescriptionFormattedToRaw(forum.Description);

            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.AddForum(forum);
        }


        // *********************************************************************
        //  UpdateForum
        //
        /// <summary>
        /// Updates a particular forum.
        /// </summary>
        /// <param name="forum">Specifies information about the forum to update.  The ForumID property
        /// indicates what forum it is that you wish to update.</param>

        // ***********************************************************************/
        public static void UpdateForum(Forum forum) {
            // turn the description into a formatted version
            forum.Description = ForumDescriptionFormattedToRaw(forum.Description);

            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.UpdateForum(forum);
        }

        // *********************************************************************
        //  ForumDescriptionFormattedToRaw
        //
        /// <summary>
        /// Converts the forum description from formatted text to raw text.
        /// </summary>
        /// <param name="Description">The formatted text forum description</param>
        /// <returns>A raw text version of the forum's description.</returns>
        /// <remarks>This function merely converts HTML newline tags to carraige returns.
        /// This method only needs to be called editing an existing forum.
        // ***********************************************************************/
        public static String ForumDescriptionFormattedToRaw(String Description) {
            // replace new line characters with \n
            return Description.Replace(Globals.HtmlNewLine, "\n");
        }

    }
}
