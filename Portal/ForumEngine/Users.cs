using System;
using System.Web;

using AspNetForums.Components;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.DB;

namespace AspNetForums
{

	// *********************************************************************
	//  Users
	//
	/// <summary>
	/// Класс для манипулирования пользователями форума.
	/// </summary>
	// ***********************************************************************/
	public class Users
	{

		// *********************************************************************
		//  GetUserInfo
		//
		/// <summary>
		/// Вернуть информацию о конкретном форуме.
		/// </summary>
		/// <param name="username">The user whose information you are interested in.</param>
		/// <param name="updateIsOnline">Updates user's online datetime stamp.</param>
		/// <returns>Instance of User with details about a given forum user.</returns>
		/// <remarks>
		/// If the specified user is not found, a UserNotFoundException exception is thrown. Feel
		/// free to call this multiple times during the request as the value is stored in Context once
		/// read from the data source.
		/// </remarks>
		// ***********************************************************************/
		public static User GetUserInfo( String username )
		{

			User user = new User();
			UlterSystems.PortalLib.BusinessObjects.Person UlterUser = new UlterSystems.PortalLib.BusinessObjects.Person();
			UlterUser.Load( Int32.Parse( username ) );
			user.Username = username;
			user.DisplayName = UlterUser.FirstName.ToString() + "&nbsp;" + UlterUser.LastName.ToString();
			user.IsApproved = !UlterUser.IsInRole( "ForumBannedUser" );
			user.IsAdministrator = UlterUser.IsInRole( "ForumAdministrator" );

			return user;

		}




		// *********************************************************************
		//  GetAllUsers
		//
		/// <summary>
		/// Returns all the users currently in the system.
		/// </summary>
		/// <param name="pageIndex">Page position in which to return user's for, e.g. position of result set</param>
		/// <param name="pageSize">Size of a given page, e.g. size of result set.</param>
		/// <param name="sortBy">How the returned user's are to be sorted.</param>
		/// <param name="sortOrder">Direction in which to sort</param>
		/// <returns>A collection of user.</returns>
		/// 
		// ********************************************************************/
		public static UserCollection GetAllUsers( int pageIndex, int pageSize )
		{
			UserCollection users, users1;
			users = new UserCollection();
			users1 = new UserCollection();
			Person[] us = UserList.GetUserList();
			string[] un = new string[ us.Length ];
			for( int i = 0; i < us.Length; i++ )
			{
				un[ i ] = us[ i ].LastName.ToString();
			}

			Array.Sort( un, us );

			foreach( Person u in us )
			{
				User usr = new User();
				usr.DisplayName = u.FullName;
				usr.Username = u.ID.ToString();
				users.Add( usr );
			}
			users1.AddRange( users.GetRange( pageIndex * pageSize, Math.Min( pageSize, users.Count - pageIndex * pageSize ) ) );

			return users1;
		}

		// *********************************************************************
		//  GetLoggedOnUser
		//
		/// <summary>
		/// Short-cut for getting back a user instance
		/// </summary>
		/// <returns>A User instance based off the value of User.Identity.Name</returns>
		/// 
		// ********************************************************************/
		public static User GetLoggedOnUser()
		{

			if( !( HttpContext.Current.Request.IsAuthenticated && HttpContext.Current.Session[ "UserID" ] != null ) )
				return null;

			return Users.GetUserInfo( ( (int) HttpContext.Current.Session[ "UserID" ] ).ToString() );
		}



		// *********************************************************************
		//  UpdateUserInfoFromAdminPage
		//
		/// <summary>
		/// Updates a user's system-level information.
		/// </summary>
		/// <param name="user">A user object containing information to be updated.  The Username
		/// property specifies what user should be updated.</param>
		/// <remarks>This method updates a user's system-level information: their approved status, their
		/// trusted status, etc.  To update a user's personal information (such as their password,
		/// signature, homepage Url, etc.), use the UpdateUserInfo method.  <seealso cref="UpdateUserInfo"/></remarks>
		/// 
		// ********************************************************************/
		public static void UpdateUserInfoFromAdminPage( User user )
		{
			Role bannedUsersRole = Role.GetRole( "ForumBannedUser" );
			if( bannedUsersRole == null )
				throw new InvalidOperationException( string.Format( "Can't get ForumBannedUser role." ) );

			if( !( user.IsApproved ) && GetUserInfo( user.Username ).IsApproved )
			{
				//banning
				bannedUsersRole.AddUser( Int32.Parse( user.Username ) );
			}
			else if( ( user.IsApproved ) && !GetUserInfo( user.Username ).IsApproved )
			{
				//unbanning
				bannedUsersRole.RemoveUser( Int32.Parse( user.Username ) );
			}

		}


		// *********************************************************************
		//  ValidUser
		//
		/// <summary>
		/// Determines if the user is a valid user.
		/// </summary>
		/// <param name="user">The user to check.  Note that the Username and Password properties of the
		/// User object must be set.</param>
		/// <returns>A boolean: true if the user's Username/password are valid; false if they are not,
		/// or if the user has been banned.</returns>
		/// 
		// ********************************************************************/
		public static bool ValidUser( User user )
		{
			// Create Instance of the IDataProviderBase
			if( DBManager.GetInternetUsersCount( user.Username, user.Password ) == 1 )
				return true;
			else
				return false;

		}

		// *********************************************************************
		//  TotalNumberOfUserAccounts
		//
		/// <summary>
		/// Calculates and returns the total number of user accounts.
		/// </summary>
		/// <returns>The total number of user accounts created.</returns>
		/// 
		// ********************************************************************/
		public static int TotalNumberOfUserAccounts()
		{
			return UserList.GetUserList().Length;
		}

	}
}
