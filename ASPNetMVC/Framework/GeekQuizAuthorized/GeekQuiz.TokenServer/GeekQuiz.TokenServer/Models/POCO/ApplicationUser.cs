using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekQuiz.TokenServer.Models.POCO
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public virtual IList<UsedPassword> UserUsedPassword { get; set; }

        public ApplicationUser() : base()
        {
            UserUsedPassword = new List<UsedPassword>();
        }
    }
}