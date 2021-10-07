using DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManager.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> UpdateUser(UserRole document);
        Task<bool> DeleteUser(int id);
    }
}
