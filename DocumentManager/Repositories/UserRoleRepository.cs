using DocumentManager.Repositories;
using DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DocumentDBContext _db;
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var userrole = _db.UserRole.Find(id);
                if (userrole != null) _db.UserRole.Remove(userrole);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<UserRole> UpdateUser(UserRole document)
        {
            throw new NotImplementedException();
        }
    }
}
