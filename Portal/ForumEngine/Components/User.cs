using System;
using System.Web;

namespace AspNetForums.Components {

    // *********************************************************************
    //  User
    //
    /// <summary>
    /// This class contains the properties for a User.
    /// </summary>
    /// 
    // ********************************************************************/
    public class User {
        String username = "";				// the user's Username (unique identifier)
        String password = "";				// the user's password
        String displayName = "";            // the user's display name
        int totalPosts;                     // Total posts by this user
        bool approved;						// if the user is an approved user or not
        bool isAdministrator;                   // if the user is admin

        /// <summary>
        /// Returns the user's Username.
        /// </summary>
        /// <remarks>The Username is what uniquely identifies each user.</remarks>
        public String Username {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// Returns the user's DisplayName.
        /// </summary>
        public String DisplayName {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// Returns the user's password.
        /// </summary>
        public String Password {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Total posts by this user
        /// </summary>
        public int TotalPosts {
            get { return totalPosts; }
            set { totalPosts = value; }
        }
        
        /// <summary>
        /// Specifies whether a user is Approved or not.  Non-approved users cannot log into the system
        /// and, therefore, cannot post messages.
        /// </summary>
        public bool IsApproved {
            get { return approved; }
            set { approved = value; }
        }

        /// <summary>
        /// Specifies if a user in an administrator or not.
        /// </summary>
        public bool IsAdministrator {
            get {
                return isAdministrator;
            }
            set
            {
                isAdministrator = value;
            }
        }
    }
}
