using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatHub.Models
{
    public class ApplicationDbModel
    {
        public class ApplicationUser : IdentityUser
        {
        }
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {

        }
    }
}