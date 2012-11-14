using System;

namespace AspNetForums.Components {
    /// <summary>
    /// This class defines the properties that makeup a forum.
    /// </summary>
    public class Forum {

        // Member Variables
        int forumID = 0;				// Unique forum identifier
        int totalPosts = -1;			// Total posts in the forum
        int totalThreads = -1;			// Total threads in the forum
        String name = "";				// Name of the forum
        String description = "";		// Description of the forum
        DateTime mostRecentPostDate = DateTime.MinValue.AddMonths(1);	        // The date of the most recent post to the forum
        String mostRecentUser = "";		// The author of the most recent post to the forum
        int mostRecentPostId = 0;       // the most recent post id
        int mostRecentThreadId = 0;     // Post ID of the most recent thread
        DateTime dateCreated;			// The date/time the forum was created
        DateTime lastUserActivity;      // Last time the user was isActive in the forum





        // *********************************************************************
        //  IsAnnouncement
        //
        /// <summary>
        /// If post is locked and post date > 2 years
        /// </summary>
        // ********************************************************************/
        public virtual bool IsAnnouncement {
            get { 
                if (MostRecentPostDate > DateTime.Now.AddYears(2))
                    return true;
                else
                    return false;
            }
        }

        /*************************** PROPERTY STATEMENTS *****************************/
        /// <summary>
        /// Specifies the unique identifier for the each forum.
        /// </summary>
        public int ForumID {
            get { return forumID; }
            set {
                if (value < 0)
                    forumID = 0;
                else
                    forumID = value;
            }
        }


        public DateTime LastUserActivity {
            get { return lastUserActivity; }
            set {
                    lastUserActivity = value;
            }
        }
        /// <summary>
        /// Indicates how many total posts the forum has received.
        /// </summary>
        public int TotalPosts {
            get { return totalPosts; }
            set {
                if (value < 0)
                    totalPosts = -1;
                else
                    totalPosts = value;
            }
        }


        /// <summary>
        /// Specifies the date/time of the most recent post to the forum.
        /// </summary>
        public DateTime MostRecentPostDate {
            get { return mostRecentPostDate; }
            set {
                mostRecentPostDate = value;
            }
        }

        /// <summary>
        /// Specifies the most recent post to the forum.
        /// </summary>
        public int MostRecentPostId {
            get { return mostRecentPostId; }
            set {
                mostRecentPostId = value;
            }
        }

        /// <summary>
        /// Specifies the most recent thread id to the forum.
        /// </summary>
        public int MostRecentThreadId {
            get { return mostRecentThreadId; }
            set {
                mostRecentThreadId = value;
            }
        }

        /// <summary>
        /// Specifies the author of the most recent post to the forum.
        /// </summary>
        public String MostRecentPostAuthorID {
            get { return mostRecentUser; }
            set {
                mostRecentUser = value;
            }
        }
        /// <summary>
        /// Specifies the display name for the author of the most recent post to the forum.
        /// </summary>
        public String MostRecentPostAuthorDisplayName
        {
            get {
                if (MostRecentPostAuthorID == "")
                    return "-";
                else
                    return Users.GetUserInfo(MostRecentPostAuthorID).DisplayName; 
            }
        }

        /// <summary>
        /// Indicates how many total threads are in the forum.  A thread is a top-level post.
        /// </summary>
        public int TotalThreads {
            get { return totalThreads; }
            set {
                if (value < 0)
                    totalThreads = -1;
                else
                    totalThreads = value;
            }
        }

        /// <summary>
        /// Specifies the name of the forum.
        /// </summary>
        public String Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Specifies the description of the forum.
        /// </summary>
        public String Description {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Returns the date/time the forum was created.
        /// </summary>
        public DateTime DateCreated {
            get { return dateCreated; }
            set { dateCreated = value; }
        }
        /****************************************************************************/
    }
}
