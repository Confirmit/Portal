using System;
using System.Data;
using System.Data.SqlClient;
using AspNetForums.Components;
using System.Web;
using System.Web.Mail;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace AspNetForums.Data {


    /// <summary>
    /// Summary description for WebForumsDataProvider.
    /// </summary>
    public class SqlDataProvider : IDataProviderBase {

        /****************************************************************/
        // GetThreadsUserMostRecentlyParticipatedIn
        //
        /// <summary>
        /// Returns a collection of threads that the user has recently partipated in.
        /// </summary>
        //
        /****************************************************************/
        public ThreadCollection GetThreadsUserMostRecentlyParticipatedIn(string username, int count) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTopicsUserMostRecentlyParticipatedIn", myConnection);
            ThreadCollection threads;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
            myCommand.Parameters.Add("@Count", SqlDbType.Int).Value = count;
            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            threads = new ThreadCollection();

            while (dr.Read()) {
                threads.Add(PopulateThreadFromSqlDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            // Only return the posts specified through paging

            return threads;
        }


        /****************************************************************/
        // GetThreadsUserIsTracking
        //
        /// <summary>
        /// Returns a collection of threads that the user is tracking
        /// </summary>
        //
        /****************************************************************/
        public ThreadCollection GetThreadsUserIsTracking(string username) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTopicsUserIsTracking", myConnection);

            ThreadCollection threads;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);


            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            threads = new ThreadCollection();

            while (dr.Read()) {
                threads.Add(PopulateThreadFromSqlDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            // Only return the posts specified through paging

            return threads;
        }


               
        /****************************************************************/
        // MarkPostAsRead
        //
        /// <summary>
        /// Flags a post a 'read' in the database
        /// </summary>
        //
        /****************************************************************/
        public void MarkPostAsRead(int postID, string username) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_MarkPostAsRead", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Pass sproc parameters
            myCommand.Parameters.Add("@PostID", SqlDbType.Int).Value = postID;
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
            //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
 
        }

        /****************************************************************/
        // GetTotalPostsForThread
        //
        /// <summary>
        /// Returns the total number of posts in a given thread. This call
        /// is used mainly by paging when viewing posts.
        /// </summary>
        //
        /****************************************************************/
        public int GetTotalPostsForThread(int postID) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTotalPostsForThread", myConnection);
            int postCount = 0;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Pass sproc parameters
            myCommand.Parameters.Add("@PostID", SqlDbType.Int).Value = postID;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            // Populate the colleciton of users
            while (dr.Read())
                postCount = Convert.ToInt32(dr["TotalPostsForThread"]);

            dr.Close();
            myConnection.Close();
 
            return postCount;
        }

        /****************************************************************/
        // GetTotalPostsForThread
        //
        /// <summary>
        /// Returns the total number of posts in a given thread. This call
        /// is used mainly by paging when viewing posts.
        /// </summary>
        //
        /****************************************************************/
        public int GetTotalPinnedPostsForThread(int postID)
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTotalPinnedPostsForThread", myConnection);
            int postCount = 0;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Pass sproc parameters
            myCommand.Parameters.Add("@PostID", SqlDbType.Int).Value = postID;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            // Populate the colleciton of users
            while (dr.Read())
                postCount = Convert.ToInt32(dr["TotalPinnedPostsForThread"]);

            dr.Close();
            myConnection.Close();

            return postCount;
        }


        /****************************************************************/
        // GetTotalThreadsInForum
        //
        /// <summary>
        /// Returns the total number of threads in a given forum
        /// </summary>
        /// <param name="username">forum id to look up</param>
        //
        /****************************************************************/
        public int GetTotalThreadsInForum(int ForumID, DateTime maxDateTime, DateTime minDateTime) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_TopicCountForForum", myConnection);
            int totalThreads = 0;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@ForumID", SqlDbType.Int).Value = ForumID;

            // Control the returned values by DateTime
            myCommand.Parameters.Add("@MaxDate", SqlDbType.DateTime).Value = maxDateTime;
            myCommand.Parameters.Add("@MinDate", SqlDbType.DateTime).Value = minDateTime;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            while (dr.Read())
                totalThreads = Convert.ToInt32(dr["TotalTopics"]);

            dr.Close();
            myConnection.Close();

            return totalThreads;
        }

        /****************************************************************/
        // MarkAllThreadsRead
        //
        /// <summary>
        /// Marks all threads from Forum ID and below as read
        /// </summary>
        //
        /*****************************************************************/
        public void MarkAllThreadsRead(int forumID, string username) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_MarkAllThreadsRead", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = new SqlParameter("@ForumID", SqlDbType.Int);
            param.Value = forumID;
            myCommand.Parameters.Add(param);

            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);

            //param = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
            //param.Value = username;
            //myCommand.Parameters.Add(param);


            // Open the connection
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        /// <summary>
        /// Builds and returns an instance of the Post class based on the current row of an
        /// aptly populated SqlDataReader object.
        /// </summary>
        /// <param name="dr">The SqlDataReader object that contains, at minimum, the following
        /// columns: PostID, ParentID, Body, ForumID, PostDate, PostLevel, SortOrder, Subject,
        /// ThreadDate, ThreadID, Replies, Username, and Approved.</param>
        /// <returns>An instance of the Post class that represents the current row of the passed 
        /// in SqlDataReader, dr.</returns>
        private Post PopulatePostFromSqlDataReader(SqlDataReader dr) {

            Post post = new Post();
            post.PostID = Convert.ToInt32(dr["PostID"]);
            post.ParentID = Convert.ToInt32(dr["ParentID"]);
            post.Body = Convert.ToString(dr["Body"]);
            post.ForumName = Convert.ToString(dr["ForumName"]);
            post.ForumID = Convert.ToInt32(dr["ForumID"]);
            post.PostDate = Convert.ToDateTime(dr["PostDate"]);
            post.Subject = Convert.ToString(dr["Subject"]);
            post.ThreadDate = Convert.ToDateTime(dr["ThreadDate"]);
            post.ThreadID = Convert.ToInt32(dr["ThreadID"]);
            post.Replies = Convert.ToInt32(dr["Replies"]);
            post.Username = Convert.ToString(dr["UserID"]);
            post.IsLocked = Convert.ToBoolean(dr["IsLocked"]);
            post.Views = Convert.ToInt32(dr["TotalViews"]);
            post.HasRead = Convert.ToBoolean(dr["HasRead"]);
            post.PinnedPost = Convert.ToBoolean(dr["IsPostPinned"]);
			
            return post;
        }

        
        // *********************************************************************
        //
        //  PopulateThreadFromSqlDataReader
        //
        /// <summary>
        /// This private method accepts a datareader and attempts to create and
        /// populate a thread class instance which is returned to the caller. For
        /// all practical purposes, a thread is simply a lightweigh version of a 
        /// post - no details, such as the body, are provided though and a thread is
        /// always considered the first post in a thread.
        /// </summary>
        //
        // ********************************************************************/
        private Thread PopulateThreadFromSqlDataReader(SqlDataReader reader) {
            Thread thread = new Thread();

            thread.PostID = Convert.ToInt32(reader["PostID"]);
            thread.PostDate = Convert.ToDateTime(reader["PostDate"]);
            thread.Subject = Convert.ToString(reader["Subject"]);
            thread.Body = Convert.ToString(reader["Body"]);
            thread.ThreadDate = Convert.ToDateTime(reader["ThreadDate"]);
            thread.PinnedDate = Convert.ToDateTime(reader["PinnedDate"]);
            thread.Replies = Convert.ToInt32(reader["Replies"]);
            thread.Username = Convert.ToString(reader["UserID"]);
            thread.IsLocked = Convert.ToBoolean(reader["IsLocked"]);
            thread.IsPinned = Convert.ToBoolean(reader["IsPinned"]);
            thread.Views = Convert.ToInt32(reader["TotalViews"]);
            thread.HasRead = Convert.ToBoolean(reader["HasRead"]);
            thread.MostRecentPostAuthorID = Convert.ToString(reader["MostRecentPostAuthorID"]);
            thread.MostRecentPostID = Convert.ToInt32(reader["MostRecentPostID"]);
            thread.ThreadID = Convert.ToInt32(reader["ThreadID"]);

         return thread;
        }


        /// <summary>
        /// Builds and returns an instance of the Forum class based on the current row of an
        /// aptly populated SqlDataReader object.
        /// </summary>
        /// <param name="dr">The SqlDataReader object that contains, at minimum, the following
        /// columns: ForumID, DateCreated, Description, Name, Moderated, and DaysToView.</param>
        /// <returns>An instance of the Forum class that represents the current row of the passed 
        /// in SqlDataReader, dr.</returns>
        private  Forum PopulateForumFromSqlDataReader(SqlDataReader dr) {
            Forum forum = new Forum();
            forum.ForumID = Convert.ToInt32(dr["ForumID"]);
            forum.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
            forum.Description = Convert.ToString(dr["Description"]);
            forum.Name = Convert.ToString(dr["Name"]);

            return forum;
        }

        /// <summary>
        /// Builds and returns an instance of the User class based on the current row of an
        /// aptly populated SqlDataReader object.
        /// </summary>
        /// <param name="dr">The SqlDataReader object that contains, at minimum, the following
        /// columns: Signature, Email, FakeEmail, Url, Password, Username, Administrator,
        /// Trusted, Timezone, DateCreated, LastLogin, and ForumView.</param>
        /// <returns>An instance of the User class that represents the current row of the passed 
        /// in SqlDataReader, dr.</returns>
        private User PopulateUserFromSqlDataReader(SqlDataReader dr) {
            User user = new User();
            user.Username = Convert.ToString(dr["UserID"]);
            
            UlterSystems.PortalLib.BusinessObjects.Person uss = new UlterSystems.PortalLib.BusinessObjects.Person();
            //uss.LoadByDomainName(user.Username);
            uss.Load(Int32.Parse(user.Username));
            user.DisplayName = uss.FullName;
				user.IsApproved = !uss.IsInRole( "ForumBannedUser" );
				user.IsAdministrator = uss.IsInRole( "ForumAdministrator" );
            try
            {
                
                //user.IsApproved = !Convert.ToBoolean(dr["IsBanned"]); ;

                //user.IsModerator = Convert.ToBoolean(dr["IsForumAdmin"]);
            }
            catch (IndexOutOfRangeException)
            {
                return user;
            }
         
            return user;
        }

        /************************ POST FUNCTIONS ***********************
                 * These functions return information about a post or posts.  They
                 * are called from the WebForums.Posts class.
                 * *************************************************************/
        /// <summary>
        /// Get all threads with default order and page size
        /// </summary>

        public ThreadCollection GetAllThreads(int forumID, string username, bool unreadThreadsOnly) {
       
            return GetAllThreads(forumID, 9999, 0, DateTime.Now.AddYears(-20), username, unreadThreadsOnly, 4, 1);

        }

        /// <summary>
        /// Get all threads
        /// </summary>
        /// <param name="SortBy">Colomn number to sort by</param>
        /// <param name="SortOrder">0 - ASC, 1 - DESC</param>

        public ThreadCollection GetAllThreads(int forumID, int pageSize, int pageIndex, DateTime endDate, string username, bool unreadThreadsOnly, int SortBy, int SortOrder) {
            // Create Instance of Connection and Command Object
        
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetAllTopicsPaged", myConnection);
            ThreadCollection threads;

            // Ensure DateTime is min value for SQL
            if (endDate == DateTime.MinValue)
                endDate = (DateTime) System.Data.SqlTypes.SqlDateTime.MinValue;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@ForumId", SqlDbType.Int, 4).Value = forumID;
            myCommand.Parameters.Add("@PageSize", SqlDbType.Int, 4).Value = pageSize;
            myCommand.Parameters.Add("@PageIndex", SqlDbType.Int, 4).Value = pageIndex;
            myCommand.Parameters.Add("@DateFilter", SqlDbType.DateTime).Value = endDate;
            myCommand.Parameters.Add("@SortBy", SqlDbType.Int).Value = SortBy;
            myCommand.Parameters.Add("@SortOrder", SqlDbType.Int).Value = SortOrder;
            
            // Only pass username if it's not null
            if (username == null)
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = System.DBNull.Value;
            else
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
                //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            myCommand.Parameters.Add("@UnReadTopicsOnly", SqlDbType.Bit).Value = unreadThreadsOnly;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            threads = new ThreadCollection();

            while (dr.Read()) {
                threads.Add(PopulateThreadFromSqlDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            // Only return the posts specified through paging

            return threads;

        }
        /// <summary>
        /// Is the user tracking this thread?
        /// </summary>
        
        public bool IsUserTrackingThread(int threadID, string username) {

            bool userIsTrackingPost = false; 

            // If username is null, don't continue
            if (username == null)
                return userIsTrackingPost;

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_IsUserTrackingPost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@ThreadID", SqlDbType.Int).Value = threadID;
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
            //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read())
                return userIsTrackingPost;

            userIsTrackingPost = Convert.ToBoolean(dr["IsUserTrackingPost"]);

            dr.Close();
            myConnection.Close();

            return userIsTrackingPost;
        }


        /// <summary>
        /// Returns count of all posts in system
        /// </summary>
        public int GetTotalPostCount() {
            int totalPostCount;

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTotalPostCount", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            dr.Read();
            
            totalPostCount = (int) dr[0];

            dr.Close();
            myConnection.Close();

            return totalPostCount;
        }

        /// <summary>
        /// Get basic information about a single post.  This method returns an instance of the Post class,
        /// which contains less information than the PostDeails class, which is what is returned by the
        /// GetPostDetails method.
        /// </summary>
        /// <param name="postID">The ID of the post whose information we are interested in.</param>
        /// <returns>An instance of the Post class.</returns>
        /// <remarks>If a PostID is passed in that is NOT found in the database, a PostNotFoundException
        /// exception is thrown.</remarks>
        public  Post GetPost(int postID, string username, bool trackViews) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetPostInfo", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@PostID", SqlDbType.Int).Value = postID;
            myCommand.Parameters.Add("@TrackViews", SqlDbType.Bit).Value = trackViews;

            if (username == null)
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = System.DBNull.Value;
            else
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
                //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();
                // we did not get back a post
                throw new Components.PostNotFoundException("Did not get back a post for PostID " + postID.ToString());
            }

            Post p = PopulatePostFromSqlDataReader(dr);
            dr.Close();
            myConnection.Close();


            // we have a post to work with	
            return p;
        }


	
        /// <summary>
        /// Reverses a particular user's email thread tracking options for the thread that contains
        /// the post specified by PostID. 
        /// </summary>
        /// <param name="Username">The User whose email thread tracking options we wish to reverse.</param>
        public  void ReverseThreadTracking(String Username, int PostID) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_ReverseTrackingOption", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //SqlParameter parameterUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
            //parameterUsername.Value = Username;
            //myCommand.Parameters.Add(parameterUsername);
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(Username);

            SqlParameter parameterPostId = new SqlParameter("@PostID", SqlDbType.Int, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }



        /// <summary>
        /// Returns a collection of Posts that make up a particular thread.
        /// </summary>
        /// <param name="ThreadID">The ID of the Thread to retrieve the posts of.</param>
        /// <returns>A PostCollection object that contains the posts in the thread specified by
        /// ThreadID.</returns>
        public  PostCollection GetThread(int ThreadID) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetThread", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterThreadId = new SqlParameter("@ThreadID", SqlDbType.Int, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            // loop through the results
            PostCollection posts = new PostCollection();
            while (dr.Read()) {
                posts.Add(PopulatePostFromSqlDataReader(dr));
            }
            dr.Close();
            myConnection.Close();

            return posts;
        }


        /// <summary>
        /// Returns a collection of Posts that make up a particular thread with paging
        /// </summary>
        /// <param name="postID">The ID of a Post in the thread that you are interested in retrieving.</param>
        /// <returns>A PostCollection object that contains the posts in the thread.</returns>
        public PostCollection GetThreadByPostID(int postID, int currentPageIndex, int pageSize, string username) {
            SqlParameter param;

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetThreadByPostIDPaged", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // PostID Parameter
            param = new SqlParameter("@PostID", SqlDbType.Int, 4);
            param.Value = postID;
            myCommand.Parameters.Add(param);

            // CurrentPage Parameter
            param = new SqlParameter("@PageIndex", SqlDbType.Int);
            param.Value = currentPageIndex;
            myCommand.Parameters.Add(param);

            // PageSize Parameter
            param = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            param.Value = pageSize;
            myCommand.Parameters.Add(param);

            // Username
            if ( (username == null) || (username == String.Empty))
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = System.DBNull.Value;
            else
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
                //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            // loop through the results
            PostCollection posts = new PostCollection();
            while (dr.Read()) {
                Post p = PopulatePostFromSqlDataReader(dr);
                //p.PostType = (Posts.PostType) dr["PostType"];
                posts.Add(p);
            }

            dr.Close();
            myConnection.Close();

            return posts;
        }



        /// <summary>
        /// Adds a new Post.  This method checks the allowDuplicatePosts settings to determine whether
        /// or not to allow for duplicate posts.  If allowDuplicatePosts is set to false and the user
        /// attempts to enter a duplicate post, a PostDuplicateException exception is thrown.
        /// </summary>
        /// <param name="postToAdd">A Post object containing the information needed to add a new
        /// post.  The essential fields of the Post class that must be set are: the Subject, the
        /// Body, the Username, and a ForumID or a ParentID (depending on whether the post to add is
        /// a new post or a reply to an existing post, respectively).</param>
        /// <returns>A Post object with information on the newly inserted post.  This returned Post
        /// object includes the ID of the newly added Post (PostID) as well as if the Post is
        /// Approved or not.</returns>
        public Post AddPost(Post postToAdd, string username) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlParameter param;
            myConnection.Open();

            SqlParameter parameterUserName = new SqlParameter("@UserID", SqlDbType.Int);
            parameterUserName.Value = int.Parse(postToAdd.Username);

            SqlParameter parameterBody = new SqlParameter("@Body", SqlDbType.NText);
            parameterBody.Value = postToAdd.Body;

            if (!Globals.AllowDuplicatePosts) {
                SqlCommand checkForDupsCommand = new SqlCommand("dbo.forums_IsDuplicatePost", myConnection);
                checkForDupsCommand.CommandType = CommandType.StoredProcedure;	// Mark the Command as a SPROC
                checkForDupsCommand.Parameters.Add(parameterUserName);
                checkForDupsCommand.Parameters.Add(parameterBody);

                if (((int) checkForDupsCommand.ExecuteScalar()) > 0) {
                    myConnection.Close();
                    throw new PostDuplicateException("Attempting to insert a duplicate post.");
                }

                checkForDupsCommand.Parameters.Clear();			// clear the parameters
            }

            SqlCommand myCommand = new SqlCommand("dbo.forums_AddPost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterForumId = new SqlParameter("@ForumID", SqlDbType.Int, 4);
            parameterForumId.Value = postToAdd.ForumID;
            myCommand.Parameters.Add(parameterForumId);

            SqlParameter parameterPostId = new SqlParameter("@ReplyToPostID", SqlDbType.Int, 4);
            parameterPostId.Value = postToAdd.ParentID;
            myCommand.Parameters.Add(parameterPostId);

            SqlParameter parameterSubject = new SqlParameter("@Subject", SqlDbType.NVarChar, 256);
            parameterSubject.Value = postToAdd.Subject;
            myCommand.Parameters.Add(parameterSubject);

            param = new SqlParameter("@IsLocked", SqlDbType.Bit);
            param.Value = postToAdd.IsLocked;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@IsPinnedPost", SqlDbType.Bit);
            param.Value = postToAdd.PinnedPost;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@Pinned", SqlDbType.DateTime);
            if (postToAdd.PostDate > DateTime.Now)
                param.Value = postToAdd.PostDate;
            else
                param.Value = System.DBNull.Value;
            myCommand.Parameters.Add(param);

            myCommand.Parameters.Add(parameterUserName);
            myCommand.Parameters.Add(parameterBody);

            // Execute the command
            int iNewPostID = 0;
			
            try {
                // Get the new PostID
                iNewPostID = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
            } 
            catch (Exception e) {
                myConnection.Close();
                // if an exception occurred, throw it back
                throw new Exception(e.Message);
            }

            myConnection.Close();
            
            // Return a Post instance with info from the newly inserted post.
            return GetPost(iNewPostID, username, false);
        }

		

        /// <summary>
        /// Updates a post.
        /// </summary>
        /// <param name="post">The Post data used to update the Post.  The ID of the UpdatedPost
        /// Post object corresponds to what post is to be updated.  The only other fields used to update
        /// the Post are the Subject and Body.</param>
        public void UpdatePost(Post post, bool ChangePin)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_UpdatePost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@PostID", SqlDbType.Int, 4).Value = post.PostID;
            myCommand.Parameters.Add("@Subject", SqlDbType.NVarChar, 256).Value = post.Subject;
            myCommand.Parameters.Add("@Body", SqlDbType.NText).Value = post.Body;
            myCommand.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = post.IsLocked;
            myCommand.Parameters.Add("@IsPinnedPost", SqlDbType.Bit).Value = post.PinnedPost;

            SqlParameter param = new SqlParameter("@Pinned", SqlDbType.DateTime);
            if (post.PostDate > DateTime.Now)
                param.Value = post.PostDate;
            else
                param.Value = System.DBNull.Value;
            myCommand.Parameters.Add(param);

            myCommand.Parameters.Add("@ChangePin", SqlDbType.Bit, 50).Value = ChangePin;

            // Execute the command
            myConnection.Open();
            try {
                myCommand.ExecuteNonQuery();
            } 
            catch (Exception e) {
                myConnection.Close();
                // oops, something went wrong
                throw new Exception(e.Message);
            }
            myConnection.Close();
        }


		
        /// <summary>
        /// This method deletes a particular post and all of its replies.
        /// </summary>
        /// <param name="postID">The PostID that you wish to delete.</param>
        public void DeletePost(int postID) {
            // Delete the post
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_DeleteModeratedPost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@PostID", SqlDbType.Int, 4).Value = postID;

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }




        /*********************************************************************************/

        /************************ FORUM FUNCTIONS ***********************
         * These functions return information about a forum.
         * are called from the WebForums.Forums class.
         * **************************************************************/
	
        /// <summary>
        /// Returns information about a particular forum that contains a particular thread.
        /// </summary>
        /// <param name="ThreadID">The ID of the thread that is contained in the Forum you wish to
        /// retrieve information about.</param>
        /// <returns>A Forum object instance containing the information about the Forum that the
        /// specified thread exists in.</returns>
        /// <remarks>If a ThreadID is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfoByThreadID(int ThreadID) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetForumByThreadID", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterThreadId = new SqlParameter("@ThreadID", SqlDbType.Int, 4);
            parameterThreadId.Value = ThreadID;
            myCommand.Parameters.Add(parameterThreadId);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();
                // we didn't get a forum, handle it
                throw new Components.ForumNotFoundException("Did not get back a forum for ThreadID " + ThreadID.ToString());
            } 

            Forum f = PopulateForumFromSqlDataReader(dr);
            dr.Close();
            myConnection.Close();

            return f;
        }



        /// <summary>
        /// Returns a Forum object with information on a particular forum.
        /// </summary>
        /// <param name="forumID">The ID of the Forum you are interested in.</param>
        /// <returns>A Forum object.</returns>
        /// <remarks>If a ForumID is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfo(int forumID, string username) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetForumInfo", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@ForumID", SqlDbType.Int).Value = forumID;
            if (( username == null) || (username == String.Empty))
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = System.DBNull.Value;
            else
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
                //myCommand.Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();
                // we didn't get a forum, handle it
                throw new Components.ForumNotFoundException("Did not get back a forum for ForumID " + forumID.ToString());
            }

            Forum f = PopulateForumFromSqlDataReader(dr);
            f.TotalThreads = Convert.ToInt32(dr["TotalTopics"]);

            dr.Close();
            myConnection.Close();

            return f;
        }




        /// <summary>
        /// Returns information about a particular forum that contains a particular post.
        /// </summary>
        /// <param name="PostID">The ID of the post that is contained in the Forum you wish to
        /// retrieve information about.</param>
        /// <returns>A Forum object instance containing the information about the Forum that the
        /// specified thread exists in.</returns>
        /// <remarks>If a Post is passed in that is NOT found in the database, a ForumNotFoundException
        /// exception is thrown.</remarks>
        public  Forum GetForumInfoByPostID(int PostID) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetForumByPostID", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPostId = new SqlParameter("@PostID", SqlDbType.Int, 4);
            parameterPostId.Value = PostID;
            myCommand.Parameters.Add(parameterPostId);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();
                // we didn't get a forum, handle it
                throw new Components.ForumNotFoundException("Did not get back a forum for PostID " + PostID.ToString());
            }

            Forum f = PopulateForumFromSqlDataReader(dr);
            dr.Close();
            myConnection.Close();
            return f;
        }

        /// <summary>
        /// Returns a list of all Forums.
        /// </summary>
        /// <returns>A ForumCollection object.</returns>
        public  ForumCollection GetAllForums(string username) {
            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetAllForums", myConnection);
            SqlParameter param;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;
            
            // Add Parameters to SPROC
            param = new SqlParameter("@UserID", SqlDbType.NVarChar, 50);
            if (username == null)
                param.Value = System.DBNull.Value;
            else
                param.Value = int.Parse(username);

            myCommand.Parameters.Add(param);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            // populate a ForumCollection object
            ForumCollection forums = new ForumCollection();
            Forum forum;
            while (dr.Read()) {
                forum = PopulateForumFromSqlDataReader(dr);

                forum.TotalPosts = Convert.ToInt32(dr["TotalPosts"]);
                forum.TotalThreads = Convert.ToInt32(dr["TotalTopics"]);

                // Handle Nulls
                if (Convert.IsDBNull(dr["MostRecentPostAuthorID"]))
                    forum.MostRecentPostAuthorID = null;
                else
                    forum.MostRecentPostAuthorID = Convert.ToString(dr["MostRecentPostAuthorID"]);

                if (Convert.IsDBNull(dr["MostRecentPostId"])) {
                    forum.MostRecentPostId = 0;
                    forum.MostRecentThreadId = 0;
                } else {
                    forum.MostRecentPostId = Convert.ToInt32(dr["MostRecentPostId"]);
                    forum.MostRecentThreadId = Convert.ToInt32(dr["MostRecentThreadId"]);
                }

                if (Convert.IsDBNull(dr["MostRecentPostDate"]))
                    forum.MostRecentPostDate = DateTime.MinValue.AddMonths(1);
                else
                    forum.MostRecentPostDate = Convert.ToDateTime(dr["MostRecentPostDate"]);

                // Last time the user was active in the forum
                if (username != null) {
                    if (Convert.IsDBNull(dr["LastUserActivity"]))
                        forum.LastUserActivity = DateTime.MinValue.AddMonths(1);
                    else
                        forum.LastUserActivity = Convert.ToDateTime(dr["LastUserActivity"]);
                } else {
                    forum.LastUserActivity = DateTime.MinValue;
                }

                forums.Add(forum);
            }
            dr.Close();
            myConnection.Close();

            return forums;
        }


	

        /// <summary>
        /// Deletes a forum and all of its posts.
        /// </summary>
        /// <param name="ForumID">The ID of the forum to delete.</param>
        /// <remarks>Be very careful when using this method.  It will permanently delete ALL of the
        /// posts associated with the forum.</remarks>
        public  void DeleteForum(int ForumID) {
            // return all of the forums and their total and daily posts
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_DeleteForum", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterForumID = new SqlParameter("@ForumID", SqlDbType.Int, 4);
            parameterForumID.Value = ForumID;
            myCommand.Parameters.Add(parameterForumID);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Adds a new forum.
        /// </summary>
        /// <param name="forum">A Forum object instance that defines the variables for the new forum to
        /// be added.  The Forum object properties used to create the new forum are: Name, Description,
        /// Moderated, and DaysToView.</param>
        public void AddForum(Forum forum) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_AddForum", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterForumName = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
            parameterForumName.Value = forum.Name;
            myCommand.Parameters.Add(parameterForumName);

            SqlParameter parameterForumDesc = new SqlParameter("@Description", SqlDbType.NVarChar, 3000);
            parameterForumDesc.Value = forum.Description;
            myCommand.Parameters.Add(parameterForumDesc);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }



        /// <summary>
        /// Updates an existing forum.
        /// </summary>
        /// <param name="forum">A Forum object with the new, updated properties.  The ForumID property
        /// specifies what forum to update, while hte Name, Description, Moderated, and DaysToView
        /// properties indicate the new values.</param>
        public  void UpdateForum(Forum forum) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_UpdateForum", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterForumId = new SqlParameter("@ForumID", SqlDbType.Int, 4);
            parameterForumId.Value = forum.ForumID;
            myCommand.Parameters.Add(parameterForumId);

            SqlParameter parameterForumName = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
            parameterForumName.Value = forum.Name;
            myCommand.Parameters.Add(parameterForumName);

            SqlParameter parameterForumDesc = new SqlParameter("@Description", SqlDbType.NVarChar, 3000);
            parameterForumDesc.Value = forum.Description;
            myCommand.Parameters.Add(parameterForumDesc);



            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Returns the total number of forums.
        /// </summary>
        /// <returns>The total number of forums.</returns>
        public int TotalNumberOfForums() {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetTotalNumberOfForums", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            int totalForums = Convert.ToInt32(myCommand.ExecuteScalar());
            myConnection.Close();

            return totalForums;

        }
        /*********************************************************************************/




        /*********************************************************************************/

        /************************ USER FUNCTIONS ***********************
                 * These functions return information about a user.
                 * are called from the WebForums.Users class.
                 * *************************************************************/


        

        /// <summary>
        /// Сохраняет число последних тем с участием пользователя для отображения
        /// </summary>
        /// <param name="username">The name of the User whose information you storing.</param>
        
        public void SetLastCount(String username, int count)
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_SetLastCount", myConnection);
            SqlParameter param;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //param = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
            //param.Value = username;
            //myCommand.Parameters.Add(param);
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);

            // Add Parameters to SPROC
            param = new SqlParameter("@Count", SqlDbType.Int);
            param.Value = count;
            myCommand.Parameters.Add(param);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
         }


        /// <summary>
        /// Получает число последних тем с участием пользователя для отображения
        /// </summary>
        /// <param name="username">The name of the User whose information you storing.</param>
        public int GetLastCount(String username)
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetLastCount", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //param = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
            //param.Value = username;
            //myCommand.Parameters.Add(param);
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);

            // Execute the command
            myConnection.Open();
            int i = Convert.ToInt32(myCommand.ExecuteScalar());
            myConnection.Close();
            return i;
         }








        /// <summary>
        /// Retrieves information about a particular user.
        /// </summary>
        /// <param name="Username">The name of the User whose information you are interested in.</param>
        /// <returns>A User object.</returns>
        /// <remarks>If a Username is passed in that is NOT found in the database, a UserNotFoundException
        /// exception is thrown.</remarks>
        public  User GetUserInfo(String username, bool updateIsOnline) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetUserInfo", myConnection);
            SqlParameter param;

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            //param = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
            //param.Value = username;
            //myCommand.Parameters.Add(param);
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);

            // Add Parameters to SPROC
            param = new SqlParameter("@UpdateIsOnline", SqlDbType.Bit);
            param.Value = updateIsOnline;
            myCommand.Parameters.Add(param);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            User us = new User();
            if (!dr.Read())
            {

                // we didn't get a user, handle it
                throw new Components.UserNotFoundException("User not found for Username " + username);
            }
            else
            {

                us = PopulateUserFromSqlDataReader(dr);

                //locGreetings.Text = String.Format(locGreetings.Text, curUser.FullName);


                dr.Close();
                myConnection.Close();
            }
            
            return us;
              
        }


        /*********************************************************************************/



        /************************ SEARCH FUNCTIONS ***********************
                 * These functions are used to perform searching.
                 * ***************************************************************/
	
        /// <summary>
        /// Performs a search, returning a PostCollection object with appropriate posts.
        /// </summary>
        /// <param name="ToSearch">Specifies what to search, specifically.  Must be set to a valid
        /// ToSearchEnum value, which supports two possible values: PostsSearch and PostsBySearch.</param>
        /// <param name="SearchWhat">A SearchWhatEnum value, this parameter specifies what to search. 
        /// Acceptable values are: SearchAllWords, SearchAnyWord, and SearchExactPhrase.</param>
        /// <param name="ForumToSearch">Specifies what forum to search.  To search all forums, pass in a
        /// value of 0.</param>
        /// <param name="SearchTerms">Specifies the terms to search on.</param>
        /// <param name="Page">Specifies what page of the search results to display.</param>
        /// <param name="RecsPerPage">Specifies how many records per page to show on the search
        /// results.</param>
        /// <returns>A PostCollection object, containing the posts to display for the particular page
        /// of the search results.</returns>
        public  PostCollection GetSearchResults(ToSearchEnum ToSearch, SearchWhatEnum SearchWhat, int ForumToSearch, String SearchTerms, int Page, int RecsPerPage, string username) {

            // return all of the forums and their total and daily posts
            // first, though, we've got to put our search phrase in the right order
            String strColumnName = "";
            String strWhereClause = " WHERE (";
            String [] aTerms = null;
			
            
            // Are we searching for a particular user?
            if (ToSearch == ToSearchEnum .PostsSearch) {
                strColumnName = "Body";

                // depending on the search style, our WHERE clause will differ
                switch(SearchWhat) {
                    case SearchWhatEnum.SearchExactPhrase:
                        // easy, we want to search for the exact search term
                        strWhereClause += strColumnName + " LIKE '%" + SearchTerms + "%' ";
                        break;
					
                    case SearchWhatEnum.SearchAllWords:
                        // allrighty, we want to find rows where each word is found
                        // split up the search term string into an array
                        aTerms = SearchTerms.Split(new char[]{' '});
					
                        // now, loop through the aTerms array
                        strWhereClause += strColumnName + " LIKE '%" + String.Join("%' AND " + strColumnName + " LIKE '%", aTerms) + "%'";
                        break;

                    case SearchWhatEnum.SearchAnyWord:
                        // allrighty, we want to find rows where each word is found
                        // split up the search term string into an array
                        aTerms = SearchTerms.Split(new char[]{' '});
					
                        // now, loop through the aTerms array
                        strWhereClause += strColumnName + " LIKE '%" + String.Join("%' OR " + strColumnName + " LIKE '%", aTerms) + "%'";
                        break;
                }
			
                strWhereClause += ")";


            }
            else if (ToSearch == ToSearchEnum.PostsBySearch) {
                strColumnName = "UserName";

                strWhereClause += strColumnName + " = '" + SearchTerms + "') ";
            }
			
            // see if we need to add a restriction on the ForumID
            if (ForumToSearch > 0)
                strWhereClause += " AND P.ForumID = " + ForumToSearch.ToString() + " ";
				
			
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_GetSearchResults", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;
            
            SqlParameter parameterPage = new SqlParameter("@Page", SqlDbType.Int, 4);
            parameterPage.Value = Page;
            myCommand.Parameters.Add(parameterPage);

            SqlParameter parameterRecsPerPage = new SqlParameter("@RecsPerPage", SqlDbType.Int, 4);
            parameterRecsPerPage.Value = RecsPerPage;
            myCommand.Parameters.Add(parameterRecsPerPage);

            SqlParameter parameterSearchTerms = new SqlParameter("@SearchTerms", SqlDbType.NVarChar, 500);
            parameterSearchTerms.Value = strWhereClause;
            myCommand.Parameters.Add(parameterSearchTerms);
            
            if ( (username == null) || (username == String.Empty))
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = System.DBNull.Value;
            else
                myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(username);
                //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();

            // populate the Posts collection
            PostCollection posts = new PostCollection();
            if (!dr.Read()) {
                dr.Close();
                myConnection.Close();
                // we have an empty result, return the empty post collection
                return posts;
            } else {
                // we have to populate our postcollection
                posts.TotalRecordCount = Convert.ToInt32(dr["MoreRecords"]);

                do {
                    posts.Add(PopulatePostFromSqlDataReader(dr));
                    ((Post) posts[posts.Count - 1]).ForumName = Convert.ToString(dr["ForumName"]);
                } while (dr.Read());

                dr.Close();
                myConnection.Close();

                return posts;
            }
        }
        /*********************************************************************************/


        /********************* MODERATION FUNCTIONS *********************
         * These functions are used to perform moderation.  They are called
         * from the WebForums.Moderate class.
         * **************************************************************/

        /// <summary>
        /// Determines if a user can edit a particular post.
        /// </summary>
        /// <param name="Username">The name of the User.</param>
        /// <param name="PostID">The Post the User wants to edit.</param>
        /// <returns>A boolean value - True if the user can edit the Post, False otherwise.</returns>
        /// <remarks>An Administrator can edit any post.  Moderators may edit posts from forums that they
        /// have moderation rights to and that are awaiting approval.</remarks>
        public  bool CanEditPost(String Username, int PostID) {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_CanEditPost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPostID = new SqlParameter("@PostID", SqlDbType.Int, 4);
            parameterPostID.Value = PostID;
            myCommand.Parameters.Add(parameterPostID);

            //SqlParameter parameterUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 50);
            //parameterUsername.Value = Username;
            //myCommand.Parameters.Add(parameterUsername);
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(Username);

            // Execute the command
            myConnection.Open();
            int iResponse = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
            myConnection.Close();
            
            return iResponse == 1;
        }


        /// <summary>
        /// Moves a from one Forum to another.
        /// </summary>
        /// <param name="PostID">The ID of the Post to move.</param>
        /// <param name="MoveToForumID">The ID of the forum to move the post to.</param>
        /// <param name="Username">The name of the User who is attempting to move the post.</param>
        /// <returns>A MovedPostStatus enumeration value that indicates the status of the attempted move.
        /// This enumeration has three values: NotMoved, MovedButNotApproved, and MovedAndApproved.</returns>
        /// <remarks>A value of NotMoved means the post was not moved (either it has been approved already
        /// or deleted); a value of MovedButNotApproved indicates that the post has been moved to a new
        /// forum, but the user moving the post was NOT a moderator for the forum it was moved to, hence
        /// the moved post is still waiting to be approved; a value of MovedAndApproved indicates that the
        /// moderator moved to post to a forum he moderates, hence the post is automatically approved.</remarks>
        public  MovedPostStatus MovePost(int postID, int moveToForumID, String approvedBy) {

            // moves a post to a specified forum
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(Globals.DatabaseConnectionString);
            SqlCommand myCommand = new SqlCommand("dbo.forums_MovePost", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            myCommand.Parameters.Add("@PostID", SqlDbType.Int, 4).Value = postID;
            myCommand.Parameters.Add("@MoveToForumID", SqlDbType.Int, 4).Value = moveToForumID;
            myCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = int.Parse(approvedBy);
            //myCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = approvedBy;

            // Execute the command
            myConnection.Open();
            int iStatus = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
            myConnection.Close();

            // Determine the status of the moved post
            switch (iStatus) {
                case 0:
                    return MovedPostStatus.NotMoved;
					
                case 1:
                    return MovedPostStatus.MovedButNotApproved;
					
                default:
                    return MovedPostStatus.MovedAndApproved;
            }
        }


    }
}
