using GeekQuiz.TokenServer.Models.POCO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GeekQuiz.TokenServer.Models.BusinessLogic
{
    public class AppUserStore : UserStore<ApplicationUser>
    {
        public AppUserStore(DbContext MyDbContext)
            : base(MyDbContext)
        {
        }

        public override async Task CreateAsync(ApplicationUser appuser)
        {
            await base.CreateAsync(appuser);
            await AddToUsedPasswordAsync(appuser, appuser.PasswordHash);
        }

        public Task AddToUsedPasswordAsync(ApplicationUser appuser, string userpassword)
        {
            appuser.UserUsedPassword.Add(new UsedPassword() { UserID = appuser.Id, HashPassword = userpassword });
            return UpdateAsync(appuser);
        }
    }
}