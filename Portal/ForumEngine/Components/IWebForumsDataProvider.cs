using System;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;
using System.Web;

namespace AspNetForums.Components {

    /// <summary>
    /// The DataProvider class contains a single method, Instance(), which returns an instance of the
    /// user-specified data provider class.
    /// </summary>
    /// <remarks>  The data provider class must inherit the IDataProviderBase
    /// interface.</remarks>
    public class DataProvider {
        
        /// <summary>
        /// Returns an instance of the user-specified data provider class.
        /// </summary>
        /// <returns>An instance of the user-specified data provider class.  This class must inherit the
        /// IDataProviderBase interface.</returns>
        public static IDataProviderBase Instance() {
            //use the cache because the reflection used later is expensive
            Cache cache = System.Web.HttpContext.Current.Cache;

            if ( cache["IDataProviderBase"] == null ) {
                //get the assembly path and class name from web.config
                String prefix = "";
                NameValueCollection context = (NameValueCollection)ConfigurationManager.GetSection("AspNetForumsSettings");
                
                if (context == null) {
                    // get the appSettings context
                    prefix = Globals._appSettingsPrefix;
                    context = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
                }

                String assemblyPath = context[prefix + "DataProviderAssemblyPath"];
                String className = context[prefix + "DataProviderClassName"];

                // assemblyPath presented in virtual form, must convert to physical path
                assemblyPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + "/bin/" + assemblyPath);					

                // Uuse reflection to store the constructor of the class that implements IWebForumDataProvider
                try {
                    cache.Insert( "IDataProviderBase", Assembly.LoadFrom( assemblyPath).GetType( className ).GetConstructor(new Type[0]), new CacheDependency( assemblyPath ) );
                }
                catch (Exception) {

                    // could not locate DLL file
                    HttpContext.Current.Response.Write("<b>ERROR:</b> Could not locate file: <code>" + assemblyPath + "</code> or could not locate class <code>" + className + "</code> in file.");
                    HttpContext.Current.Response.End();
                }
            }
            return (IDataProviderBase)(  ((ConstructorInfo)cache["IDataProviderBase"]).Invoke(null) );
        }
    }




    public interface IDataProviderBase {
        /*************************** POST METHODS ******************************/
        Post GetPost(int postID, string username, bool trackViews);		
        void ReverseThreadTracking(String Username, int PostID);
        PostCollection GetThread(int ThreadID);
        PostCollection GetThreadByPostID(int postID, int currentPageIndex, int pageSize, string username);
        Post AddPost(Post postToAdd, string username);
        void UpdatePost(Post post, bool ChangePin);
        void DeletePost(int postID);
        int GetTotalPostCount();
        void MarkPostAsRead(int postID, string username);
        bool IsUserTrackingThread(int threadID, string username);
        /***********************************************************************/

        /*************************** Thread METHODS ******************************/
        ThreadCollection GetAllThreads(int forumID, string username, bool unreadThreadsOnly);
        ThreadCollection GetAllThreads(int forumID, int pageSize, int pageIndex, DateTime endDate, string username, bool unreadThreadsOnly, int SortBy, int SortOrder);
        int GetTotalPostsForThread(int postID);
        int GetTotalPinnedPostsForThread(int postID);
        ThreadCollection GetThreadsUserIsTracking(string username);
        ThreadCollection GetThreadsUserMostRecentlyParticipatedIn(string username, int count);
        /***********************************************************************/

        /*************************** FORUM METHODS ******************************/
        Forum GetForumInfoByThreadID(int ThreadID);
        void MarkAllThreadsRead(int forumID, string username);
        Forum GetForumInfo(int ForumID, string username);
        Forum GetForumInfoByPostID(int PostID);
        ForumCollection GetAllForums(string username);
        void DeleteForum(int ForumID);
        void AddForum(Forum forum);
        void UpdateForum(Forum forum);
        int TotalNumberOfForums();
        int GetTotalThreadsInForum(int ForumID, DateTime maxDateTime, DateTime minDateTime);
        /***********************************************************************/

        /*************************** USER METHODS *****************************/
        void SetLastCount(string username, int count);
        int GetLastCount(string username);

        /**********************************************************************/

        /************************** SEARCH METHODS *****************************/
        PostCollection GetSearchResults(ToSearchEnum ToSearch, SearchWhatEnum SearchWhat, int ForumToSearch, String SearchTerms, int Page, int RecsPerPage, string username);
        /***********************************************************************/
	
	
        /*********************** MODERATION METHODS ***************************/
        bool CanEditPost(String Username, int PostID);
        MovedPostStatus MovePost(int postID, int moveToForumID, String approvedBy);
        /**********************************************************************/


        
    }
}
