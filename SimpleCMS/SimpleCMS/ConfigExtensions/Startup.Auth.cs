using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using SimpleCMS.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.OpenIdConnect;
using BootstrapControllers;

namespace SimpleCMS
{
    public partial class SimpleCMSStartupConfig
    {
        public static void ConfigureAuth2(IAppBuilder app)
        {
#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
#endif

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "simplecsmclient",
                Authority = Constants.BaseAddress,
                RedirectUri = "http://localhost:44308/",
                ResponseType = "id_token",
                Scope = "openid email phone profile",
                PostLogoutRedirectUri = "http://localhost:44308/",
                SignInAsAuthenticationType = "Cookies",

                // sample how to access token on form (when adding the token response type)
                //Notifications = new OpenIdConnectAuthenticationNotifications
                //{
                //    SecurityTokenValidated = async n =>
                //        {
                //            var token = n.ProtocolMessage.AccessToken;

                //            if (!string.IsNullOrEmpty(token))
                //            {
                //                n.AuthenticationTicket.Identity.AddClaim(
                //                    new Claim("access_token", token));
                //            }
                //        }
                //}
            });
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public static void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            
            

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            // Release auth info
            //var googleAuth = new
            //{
            //    Id = "",
            //    Secret = ""
            //};
            //var fbAuth = new
            //{
            //    Id = "",
            //    Secret = ""
            //};

            // Debug auth info
            //if (General.Helpers.Debug.IsDebugMode)
            //{
            //    googleAuth = new
            //    {
            //        Id = "",
            //        Secret = ""
            //    };
            //    fbAuth = new
            //    {
            //        Id = "",
            //        Secret = ""
            //    };
            //}

            //// https://console.developers.google.com/project/apps~valid-banner-664
            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = googleAuth.Id,
            //    ClientSecret = googleAuth.Secret
            //});

            //// https://developers.facebook.com/apps/261016004099052/dashboard/
            //app.UseFacebookAuthentication(appId: fbAuth.Id, appSecret: fbAuth.Secret);
        }
    }
}