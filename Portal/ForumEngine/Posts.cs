using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Web.Caching;
using AspNetForums.Components;

namespace AspNetForums {

    // *********************************************************************
    //  Posts
    //
    /// <summary>
    /// This class contains methods for working with an individual post.  There are methods to
    /// Add a New Post, Update an Existing Post, retrieve a single post, etc.
    /// </summary>
    // ***********************************************************************/
    public class Posts {

        // *********************************************************************
        //  GetPost
        //
        /// <summary>
        /// Returns information about a particular post.
        /// </summary>
        /// <param name="postID">The ID of the Post to return.</param>
        /// <returns>A Post object with the spcified Post's information.</returns>
        /// <remarks>This method returns information about a particular post.  If the post specified is
        /// not found, a PostNotFoundException exception is thrown.</remarks>
        // ***********************************************************************/
        public static Post GetPost(int postID, string username) {

            // We only want to call this code once per request
            if (HttpContext.Current.Items["Post" + postID] != null) {
                return (Post) HttpContext.Current.Items["Post" + postID];
            } else {
                Post post;

                // Create Instance of the IDataProviderBase
                IDataProviderBase dp = DataProvider.Instance();

                post = dp.GetPost(postID, username, true);

                // Store in context of current request
                HttpContext.Current.Items["Post" + postID] = post;

                return post;
            }
        }



        // *********************************************************************
        //  IsUserTrackingThread
        //
        /// <summary>
        /// Returns a boolean to indicate whether the user is tracking the thread.
        /// </summary>
        /// <param name="PostID">The ID of the Post to obtain information about.</param>
        /// <param name="Username">The Username of the user viewing the post.</param>
        /// 
        // ***********************************************************************/
        public static bool IsUserTrackingThread(int threadID, String username) {
            if (username == null)
                return false;

            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.IsUserTrackingThread(threadID, username);
        }


        // *********************************************************************
        //  ReverseThreadTrackingOptions
        //
        /// <summary>
        /// This method reverses a user's thread tracking options for the thread containing a
        /// particular Post.
        /// </summary>
        /// <param name="Username">The user whose thread tracking options you wish to change.</param>
        /// <param name="PostID">The post of the thread whose tracking option you wish to reverse for
        /// the specified user.</param>
        /// 
        // ***********************************************************************/
        public static void ReverseThreadTrackingOptions(String username, int postID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.ReverseThreadTracking(username, postID);
        }


        // *********************************************************************
        //  MarkPostAsRead
        //
        /// <summary>
        /// Given a post id, marks it as read in the database for a user.
        /// </summary>
        /// <param name="postID">Id of post to mark as read</param>
        /// <param name="username">Mark read for this user</param>
        /// 
        // ********************************************************************/ 
        public static void MarkPostAsRead(int postID, string username) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.MarkPostAsRead(postID, username);
        }

       
	
        // *********************************************************************
        //  GetThreadByPostID
        //
        /// <summary>
        /// This method returns a listing of the messages in a given thread using paging.
        /// </summary>
        /// <param name="PostID">Specifies the PostID of a post that belongs to the thread that we are 
        /// interested in grabbing the messages from.</param>
        /// <returns>A PostCollection containing the posts in the thread.</returns>
        /// 
        // ********************************************************************/ 
        public static PostCollection GetThreadByPostID(int postID, int currentPageIndex, int defaultPageSize) {
            PostCollection posts;
            string postCollectionKey = postID.ToString();

            // Attempt to retrieve from Cache
            posts = (PostCollection) HttpContext.Current.Cache[postCollectionKey];

            if (posts == null) {
                // Create Instance of the IDataProviderBase
                IDataProviderBase dp = DataProvider.Instance();

                posts = dp.GetThreadByPostID(postID, currentPageIndex, defaultPageSize, ((int)HttpContext.Current.Session["UserID"]).ToString());			
            }

            return posts;
        }

        // *********************************************************************
        //  GetThread
        //
        /// <summary>
        /// This method returns a listing of the messages in a given thread.
        /// </summary>
        /// <param name="ThreadID">Specifies the ThreadID that we are interested in grabbing the
        /// messages from.</param>
        /// <returns>A PostCollection containing the posts in the thread.</returns>
        /// 
        // ********************************************************************/ 
        public static PostCollection GetThread(int threadID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.GetThread(threadID);			
        }

	

        // *********************************************************************
        //  GetTotalPostCount
        //
        /// <summary>
        /// Returns the total count of all posts in the system
        /// </summary>
        /// <returns>A count of the total posts</returns>
        /// 
        // ********************************************************************/ 
        public static int GetTotalPostCount() {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.GetTotalPostCount();

        }


        // *********************************************************************
        //  AddPost
        //
        /// <summary>
        /// This method Adds a new post and returns a Post object containing information about the
        /// newly added post.
        /// </summary>
        /// <param name="PostToAdd">A Post object containing information about the post to add.
        /// This Post object need only have the following properties set: Subject, Body, Username,
        /// and ParentID or ForumID.  If the post is a new post, set ForumID; if it is a reply to
        /// an existing post, set the ParentID to the ID of the Post that is being replied to.</param>
        /// <returns>A Post object containing information about the newly added post.</returns>
        /// <remarks>The Post object being returned by the AddPost method indicates the PostID of the
        /// newly added post and specifies if the post is approved for viewing or not.</remarks>
        /// 
        // ********************************************************************/ 
        public static Post AddPost(Post postToAdd) {
            // convert the subject to the formatted version before adding the post
            postToAdd.Subject = PostSubjectRawToFormatted(postToAdd.Subject);
			
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            Post newPost = dp.AddPost(postToAdd, ((int)HttpContext.Current.Session["UserID"]).ToString());

            return newPost;
        }

        // *********************************************************************
        //  UpdatePost
        //
        /// <summary>
        /// This method updates a post (called from the admin/moderator editing the post).
        /// </summary>
        /// <param name="UpdatedPost">Changes needing to be made to a particular post.  The PostID
        /// represents to post to update.</param>
        /// 
        // ********************************************************************/ 
        public static void UpdatePost(Post post, bool ChangePin) {
            post.Subject = PostSubjectRawToFormatted(post.Subject);

            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.UpdatePost(post, ChangePin);
        }

	
        // *********************************************************************
        //  PostSubjectRawToFormatted
        //
        /// <summary>
        /// Converts the subject line from raw text to the proper display.
        /// </summary>
        /// <param name="RawMessageSubject">The raw text of the subject line.</param>
        /// <returns>A prepared subject line.</returns>
        /// <remarks>PostSubjectRawToFormatted simply strips out any HTML tags from the subject.  It is this
        /// prepared subject line that is stored in the database.</remarks>
        /// 
        // ********************************************************************/ 
        public static String PostSubjectRawToFormatted(String rawMessageSubject) {		
            String strSubject = rawMessageSubject;

            strSubject = HttpContext.Current.Server.HtmlEncode(strSubject);
			
            return strSubject;
        } 

    }
}
