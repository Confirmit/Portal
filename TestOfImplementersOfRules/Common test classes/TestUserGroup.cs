using System.Collections.Generic;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;

namespace TestOfImplementersOfRules
{
    public class TestUserGroup : IUserGroup
    {
        private List<int> _usersId = new List<int>(); 
        public List<int> GetUsersId()
        {
            return _usersId;
        }

        public void AddUserId(int id)
        {
            _usersId.Add(id);
        }

        public void RemoveUserId(int id)
        {
            _usersId.Remove(id);
        }

        public string Description { get; set; }
        public string TableAccordName { get; private set; }
    }
}