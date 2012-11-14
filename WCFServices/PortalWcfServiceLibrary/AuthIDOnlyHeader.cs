using System.Runtime.Serialization;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
    /// <summary>
    /// Class of authentication header.
    /// </summary>
    [DataContract]
    public class AuthIDOnlyHeader
    {
        #region Properties
        /// <summary>
        /// User ID.
        /// </summary>
        [DataMember]
        public int? UserID { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public AuthIDOnlyHeader()
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userID">User ID.</param>
        public AuthIDOnlyHeader(int? userID)
        {
            UserID = userID;
        }
        #endregion
    }
}
