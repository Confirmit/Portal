using System;
using System.Web;
using AspNetForums;
using AspNetForums.Components;


namespace AspNetForums {

    // *********************************************************************
    //  Moderate
    //
    /// <summary>
    /// This class contains methods that are helpful for moderating posts.
    /// </summary>
    /// 
    // ********************************************************************/ 
    public class Moderate {

        // *********************************************************************
        //  CanEditPost
        //
        /// <summary>
        /// This method determines whether or not a user can edit a particular post.
        /// </summary>
        /// <param name="strUsername">The username of the user who you wish to know if he can edit
        /// the post.</param>
        /// <param name="iPostID">To post you wish to know if the user can edit.</param>
        /// <returns>A boolean value: true if the user can edit the post, false otherwise.</returns>
        /// <remarks>Moderators can edit posts that are still waiting for approval in the forum(s) they 
        /// are cleared to moderate.  Forum administrators may edit any post, awaiting approval or not,
        /// at any time.</remarks>
        /// 
        // ********************************************************************/ 
        public static bool CanEditPost(String Username, int PostID) {			
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            return dp.CanEditPost(Username, PostID);
        }

        
        // *********************************************************************
        //  MovePost
        //
        /// <summary>
        /// Moves a post from its current forum to another.
        /// </summary>
        /// <param name="iPostID">The post to move.</param>
        /// <param name="MoveToForumID">The forum to move the post to.</param>
        /// <param name="Username">The user attempting to move the post.</param>
        /// <returns>A MovedPostStatus enumeration member indicating the resulting status: 
        /// NotMoved, meaning the post was not moved; MovedButNotApproved, meaning the post was
        /// moved, but not approved in the new forum; or MovedAndApproved, meaning that the post
        /// was moved and approved in the new forum.</returns>
        /// <remarks>A post moved from one forum to another is automatically approved if the person
        /// moving the post is also a moderate in the forum that the post is being moved to.  Moving a
        /// post can fail if the user attempts to move a post that has already been approved.</remarks>
        /// 
        // ********************************************************************/ 
        public static MovedPostStatus MovePost(int postID, int moveToForumID, String approvedBy, bool sendEmail) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            MovedPostStatus status = dp.MovePost(postID, moveToForumID, approvedBy);

            return status;
        }


        // *********************************************************************
        //  DeletePost
        //
        /// <summary>
        /// This method delets a post and all of its replies.  It also includes a reason on why the
        /// post was deleted.
        /// </summary>
        /// <param name="PostID">The post to delete.</param>
        /// <param name="ReasonForDeleting">The reason why the post was deleted.</param>
        /// <returns>A boolean, indiciating if the post was successfully deleted.</returns>
        /// <remarks>The user of the post being deleted is automatically sent an email
        /// explaining why his or her post was removed.</remarks>
        /// 
        // ********************************************************************/ 
        public static void DeletePost(int postID) {
            // Create Instance of the IDataProviderBase
            IDataProviderBase dp = DataProvider.Instance();

            dp.DeletePost(postID);

        }
    }
}