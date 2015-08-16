using GeekQuiz.TokenServer.Models.Entity;
using GeekQuiz.TokenServer.Models.POCO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GeekQuiz.TokenServer.Models.BusinessLogic
{
    public class AppUserManager : UserManager<ApplicationUser>
    {
        private const int UsedPasswordLimit = 3;

        public AppUserManager()
            : base(new AppUserStore(new AuthContext()))
        {
            PasswordValidator = new CustomizePasswordValidation(8);
        }

        public override async Task<IdentityResult> ChangePasswordAsync(string UserID, string CurrentPassword, string NewPassword)
        {
            if (await IsPreviousPassword(UserID, NewPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("You Cannot Reuse Previous Password"));
            }

            var Result = await base.ChangePasswordAsync(UserID, CurrentPassword, NewPassword);

            if (Result.Succeeded)
            {
                var appStore = Store as AppUserStore;
                await appStore.AddToUsedPasswordAsync(await FindByIdAsync(UserID), PasswordHasher.HashPassword(NewPassword));
            }

            return Result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(string UserID, string UsedToken, string NewPassword)
        {
            if (await IsPreviousPassword(UserID, NewPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("You Cannot Reuse Previous Password"));
            }

            var Result = await base.ResetPasswordAsync(UserID, UsedToken, NewPassword);

            if (Result.Succeeded)
            {
                var appStore = Store as AppUserStore;
                await appStore.AddToUsedPasswordAsync(await FindByIdAsync(UserID), PasswordHasher.HashPassword(NewPassword));
            }

            return Result;
        }

        private async Task<bool> IsPreviousPassword(string UserID, string NewPassword)
        {
            var User = await FindByIdAsync(UserID);

            if (User.UserUsedPassword.OrderByDescending(up => up.CreatedDate).Select(up => up.HashPassword).Take(UsedPasswordLimit).Where(up => PasswordHasher.VerifyHashedPassword(up, NewPassword) != PasswordVerificationResult.Failed).Any())
            {
                return true;
            }

            return false;
        }

    }
}