using System;

namespace AspNetForums.Components
{

    // *********************************************************************
    //
    //  Post
    //
    /// <summary>
    /// This class contains the properties that describe a Post.
    /// </summary>
    //
    // ********************************************************************/
    public class Post
    {

        int postID = 0;				// the unique ID for the post
        int threadID = 0;				// the ID indicating what thread the post belongs to
        int parentID = 0;				// indicates the thread's parent - 0 if the post is an original post
        int forumID = 0;				// indicates the forum the thread was posted to
        int replies = 0;				// how many replies this posting has received
        int views = 0;                  // Total times the post has been viewed

        String username = "";			// uniquely identifies the user who posted the post
        String subject = "";			// the subject of the post
        String body = "";				// the body of the post, in raw format
        String forumName = "";			// the name of the forum that the post appears in

        DateTime postDate;				// the date the post was made
        DateTime threadDate;			// the date of the thread (the postDate of the most recent post to the thread)

        bool islocked = false;          // whether or not the post allows replies
        bool hasRead = false;           // whether or not the post has been read by the user
        bool pinnedPost = false;        // whether or not the post is pinned inside the thread



        // *********************************************************************
        //
        //  PostID
        //
        /// <summary>
        /// Specifies the ID of the Post, the unique identifier.
        /// </summary>
        //
        // ********************************************************************/
        public int PostID
        {
            get
            {
                return postID;
            }

            set
            {
                if (value < 0)
                    postID = 0;
                else
                    postID = value;
            }
        }


        // *********************************************************************
        //
        //  IsLocked
        //
        /// <summary>
        /// Whether or not this post allows replies
        /// </summary>
        //
        // ********************************************************************/
        public bool IsLocked
        {
            get { return this.islocked; }

            set
            {
                islocked = value;
            }
        }

        // *********************************************************************
        //
        //  HasRead
        //
        /// <summary>
        /// Whether or not this post allows replies
        /// </summary>
        //
        // ********************************************************************/
        public bool HasRead
        {
            get { return hasRead; }

            set
            {
                hasRead = value;
            }
        }

        // *********************************************************************
        //
        //  IsAnnouncement
        //
        /// <summary>
        /// If post is locked and post date > 2 years
        /// </summary>
        //
        // ********************************************************************/
        public bool IsAnnouncement
        {
            get
            {
                if ((PostDate > DateTime.Now.AddYears(2)) && (IsLocked))
                    return true;
                else
                    return false;
            }
        }


        // *********************************************************************
        //
        //  PinnedPost
        //
        /// <summary>
        /// If post is pinned inside the thread
        /// </summary>
        //
        // ********************************************************************/
        public bool PinnedPost
        {
            set
            {
                pinnedPost = value;
            }
            get
            {
                return pinnedPost;
            }
        }


        // *********************************************************************
        //
        //  Views
        //
        /// <summary>
        /// Total number of views for a given post
        /// </summary>
        //
        // ********************************************************************/
        public int Views
        {
            get { return this.views; }

            set
            {
                views = value;
            }
        }

        // *********************************************************************
        //
        //  ThreadID
        //
        /// <summary>
        /// Indicates what thread the Post belongs to.
        /// </summary>
        //
        // ********************************************************************/
        public int ThreadID
        {
            get { return threadID; }
            set
            {
                if (value < 0)
                    threadID = 0;
                else
                    threadID = value;
            }
        }


        // *********************************************************************
        //
        //  ParentID
        //
        /// <summary>
        /// Specifies the thread's parent ID.  (That is, the PostID of the replied-to post.)
        /// </summary>
        //
        // ********************************************************************/
        public int ParentID
        {
            get { return parentID; }
            set
            {
                if (value < 0)
                    parentID = 0;
                else
                    parentID = value;
            }
        }



        // *********************************************************************
        //
        //  ForumID
        //
        /// <summary>
        /// Specifies the ID of the Forumt the post belongs to.
        /// </summary>
        //
        // ********************************************************************/
        public int ForumID
        {
            get { return forumID; }
            set
            {
                if (value < 0)
                    forumID = 0;
                else
                    forumID = value;
            }
        }



        // *********************************************************************
        //
        //  Replies
        //
        /// <summary>
        /// Specifies how many replies have been made to this post.
        /// </summary>
        /// <remarks>
        /// This property is only populated when viewing all of the posts for a particular
        /// forum.
        /// </remarks>
        //
        // ********************************************************************/
        public int Replies
        {
            get { return replies; }
            set
            {
                if (value < 0)
                    replies = 0;
                else
                    replies = value;
            }
        }


        // *********************************************************************
        //
        //  Username
        //
        /// <summary>
        /// Returns the Username of the user who made the post.
        /// </summary>
        //
        // ********************************************************************/
        public String Username
        {
            get { return username; }
            set { username = value; }
        }


        // *********************************************************************
        //
        //  ForumName
        //
        /// <summary>
        /// Returns the name of the Forum that the Post belongs to.
        /// </summary>
        //
        // ********************************************************************/
        public String ForumName
        {
            get
            {
                return forumName;
            }
            set
            {
                forumName = value;
            }
        }

        // *********************************************************************
        //
        //  Subject
        //
        /// <summary>
        /// Returns the subject of the post.
        /// </summary>
        //
        // ********************************************************************/
        public String Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        // *********************************************************************
        //
        //  Body
        //
        /// <summary>
        /// Returns the body of the post.
        /// </summary>
        /// <remarks>
        /// The body of the post is stored in a raw format in the database.
        /// </remarks>
        //
        // ********************************************************************/
        public String Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }


        // *********************************************************************
        //
        //  PostDate
        //
        /// <summary>
        /// Specifies the date/time the post was made, relative to the database's timezone.
        /// </summary>
        //
        // ********************************************************************/
        public DateTime PostDate
        {
            get
            {
                return postDate;
            }
            set
            {
                postDate = value;
            }
        }

        // *********************************************************************
        //
        //  ThreadDate
        //
        /// <summary>
        /// Specifies the date/time of the most recent post in the thread, relative to the database's
        /// time zone.
        /// </summary>
        //
        // ********************************************************************/
        public DateTime ThreadDate
        {
            get
            {
                return threadDate;
            }
            set
            {
                threadDate = value;
            }
        }


    }
}
