using System;
using System.Configuration;
using EfDemo.Application.Services.NotificationService;
using EfDemo.Application.Services.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace EfDemo.Presentation.Web.DefaultSite.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void ConfigureAppIdentityContext(this IAppBuilder app)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString;
            app.CreatePerOwinContext(() => ApplicationSecurityDbContext.Create(connectionString));
            app.CreatePerOwinContext<ApplicationUserManager>((a, b) => ApplicationUserManager.Create(a, b, manager =>
            {
                // Configure validation logic for usernames
                manager.UserValidator = new UserValidator<ApplicationUser, long>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                // Configure validation logic for passwords
                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true
                };

                // Configure user lockout defaults
                manager.UserLockoutEnabledByDefault = true;
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                manager.MaxFailedAccessAttemptsBeforeLockout = 5;

                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug it in here.
                manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser, long>
                {
                    MessageFormat = "Your security code is {0}"
                });
                manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser, long>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                });
                manager.EmailService = new EmailService();
                manager.SmsService = new SmsService();
            }, "Test App "));
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }

        public static void ConfigureCookieAuthentication(this IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = Application.Services.Security.SecurityStampValidator.OnValidateIdentity(
                      TimeSpan.FromMinutes(30),
                      (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
        }

        public static void ConfigureSignIn(this IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}