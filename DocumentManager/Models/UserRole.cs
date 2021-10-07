using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DocumentManager.Models
{
    public class UserRole: IdentityUserRole<string>
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public int Role { get; set; }
        
    }
}
