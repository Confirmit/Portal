using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.DAL
{
    public class UserListDAL
    {
        public static UserStatusInfo[] GetCustomerList(string sortExpression, bool isAscendingSortDirection)
        {
            var temp = UserList.GetUserList(sortExpression, isAscendingSortDirection);
            return null;
        }
    }
}