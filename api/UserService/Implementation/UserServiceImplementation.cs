using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Implementation
{
    public class UserServiceImplementation : IUserService
    {
        #region Class fields
        private string InstanceId;
        private IReliableStateManager StateManager;

        public UserServiceImplementation() { }

        public UserServiceImplementation(IReliableStateManager stateManager, string id)
        {
            this.InstanceId = id;
            this.StateManager = stateManager;
        }
        #endregion Class fields

        public bool CreateNewUser()
        {
            return false;
        }

        public async Task<User> GetUserInfo()
        {
            return new User() { Username = "testing" };
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        // TODO remove
        public async Task<bool> Test()
        {
            bool retval = true;
            return retval;
        }
    }
}
