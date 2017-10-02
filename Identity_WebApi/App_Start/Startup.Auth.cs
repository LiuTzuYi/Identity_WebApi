using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Identity_WebApi.Models;
using Microsoft.Owin.Security.Google;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Facebook;

namespace Identity_WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/account/login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(validateInterval: TimeSpan.FromMinutes(30)
            ////regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});


            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(10),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = false
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            var facebookOptions = new FacebookAuthenticationOptions() {
                AppId= "1886025231409161",
                AppSecret= "c682843e3ad80c4ba0555905ee532e79",
                BackchannelHttpHandler=new FacebookBackChannelHandler(),
                UserInformationEndpoint= "https://graph.facebook.com/v2.8/me?fields=id,email"
            };
            facebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(facebookOptions);
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "236960702636-ddvedktu0ctkmncutefhq33jqeoirhnj.apps.googleusercontent.com",
                ClientSecret = "aBbzZbVHoCSw5kd3QXXKRu9g"
            });
        }
    }
}