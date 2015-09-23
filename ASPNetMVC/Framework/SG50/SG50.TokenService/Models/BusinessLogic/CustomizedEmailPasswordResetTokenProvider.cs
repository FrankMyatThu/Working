using Microsoft.AspNet.Identity;
using SG50.TokenService.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class CustomizedEmailPasswordResetTokenProvider<TUser> : IUserTokenProvider<ApplicationUser, string> where TUser : class, IUser
    {
        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            Guid resetToken = Guid.NewGuid();            
            user.PasswordResetToken = resetToken.ToString();
            manager.UpdateAsync(user);
            return Task.FromResult<string>(resetToken.ToString());
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            if (manager == null) throw new ArgumentNullException();
            else {
                return Task.FromResult<bool>(manager.SupportsUserPassword);
            }
        }

        public Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult<int>(0);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult<bool>(user.PasswordResetToken.ToString() == token);            
        }
    }
}