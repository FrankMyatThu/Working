namespace GeekQuiz.TokenServer.Migrations
{
    using GeekQuiz.TokenServer.Models.POCO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Helper;
    using System.Data.Entity.Validation;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<GeekQuiz.TokenServer.Models.Entity.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GeekQuiz.TokenServer.Models.Entity.AuthContext context)
        {
            string UserName = "Myat";
            string Password = "Pass@1word";

            try
            {
                if (context.Users.Count() <= 0)
                {
                    var _IdentityRole_SuperAdmin = new IdentityRole { Name = "SuperAdmin", Id = Guid.NewGuid().ToString() };
                    var _IdentityRole_Admin = new IdentityRole { Name = "Admin", Id = Guid.NewGuid().ToString() };
                    var _IdentityRole_User = new IdentityRole { Name = "User", Id = Guid.NewGuid().ToString() };
                    context.Roles.Add(_IdentityRole_SuperAdmin);
                    context.Roles.Add(_IdentityRole_Admin);
                    context.Roles.Add(_IdentityRole_User);

                    var _IdentityUser = new ApplicationUser
                    {
                        UserName = UserName,                        
                        PasswordHash = Password,
                        Email = "myat@mdlmail.com",
                        EmailConfirmed = true,                        
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    _IdentityUser.Roles.Add(new IdentityUserRole { RoleId = _IdentityRole_SuperAdmin.Id, UserId = _IdentityUser.Id });
                    _IdentityUser.Roles.Add(new IdentityUserRole { RoleId = _IdentityRole_Admin.Id, UserId = _IdentityUser.Id });
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    UserManager.Create(_IdentityUser, Password);
                }

                if (context.Clients.Count() <= 0)
                    context.Clients.AddRange(BuildClientsList());

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private static List<Client> BuildClientsList()
        {
            List<Client> ClientsList = new List<Client> 
            {
                new Client
                { Id = "GeekQuizClientApp", 
                    Secret= Helper.GetHash ("GeekQuizClientApp@GeekQuizAdmin"), 
                    Name="Geek Quiz Client Application", 
                    ApplicationType = ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200,                    
                    AllowedOrigin = "http://localhost:32150"
                },
                new Client
                { Id = "OtherClientApp", 
                    Secret=Helper.GetHash("OtherClientApp@OtherClientAdmin"), 
                    Name="Other Client Application", 
                    ApplicationType = ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
    }
}
