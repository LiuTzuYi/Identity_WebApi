using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Identity_WebApi.Models;

namespace Identity_WebApi.Controllers
{

        [RoutePrefix("api/account")]
        //為了以下的 Route 必須加 RoutePrefix

    public class AccountController : ApiController
        {
            private static class RandomOAuthStateGenerator
            {
                private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

                public static string Generate(int strengthInBits)
                {
                    const int bitsPerByte = 8;

                    if (strengthInBits % bitsPerByte != 0)
                    {
                        throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                    }

                    int strengthInBytes = strengthInBits / bitsPerByte;

                    byte[] data = new byte[strengthInBytes];
                    _random.GetBytes(data);
                    return HttpServerUtility.UrlTokenEncode(data);
                }
            }
            private ApplicationUserManager _userManager;
            public ApplicationUserManager UserManager
            {
                get
                {
                    return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                private set
                {
                    _userManager = value;
                }
            }
            private IAuthenticationManager Authentication
            {
                get { return Request.GetOwinContext().Authentication; }
            }
            [AllowAnonymous]
            [Route("ExternalLogins")]
            public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
            {
                IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
                List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

                string state;

                if (generateState)
                {
                    const int strengthInBits = 256;
                    state = RandomOAuthStateGenerator.Generate(strengthInBits);
                }
                else
                {
                    state = null;
                }

                foreach (AuthenticationDescription description in descriptions)
                {
                    ExternalLoginViewModel login = new ExternalLoginViewModel
                    {
                        Name = description.Caption,
                        Url = Url.Route("ExternalLogins", new
                        {
                            provider = description.AuthenticationType,
                            response_type = "token",
                            client_id = Startup.PublicClientId,
                            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                            state = state
                        }),
                        State = state
                    };
                    logins.Add(login);
                }

                return logins;
            }
            [AllowAnonymous]
            [Route("Register89")]
            public async Task<IHttpActionResult> Register(RegisterBindingModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }
            private IHttpActionResult GetErrorResult(IdentityResult result)
            {
                if (result == null)
                {
                    return InternalServerError();
                }

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        // No ModelState errors are available to send, so just return an empty BadRequest.
                        return BadRequest();
                    }

                    return BadRequest(ModelState);
                }

                return null;
            }
        }
    
}
