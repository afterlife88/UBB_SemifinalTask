using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using UBB_SemifinalTask.Data;

namespace UBB_SemifinalTask.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
        
            using (AuthRepository repo = new AuthRepository())
            {
                var email = await repo.FindByEmailAsync(context.UserName);
                if (email == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    context.OwinContext.Response.Headers["error"] = $"{"Invalid UserName or Password"}";
                    return;
                }
                IdentityUser user = await repo.FindUser(email.Name, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    context.OwinContext.Response.Headers["error"] = $"{"Invalid UserName or Password"}";
                    return;
                }
                context.OwinContext.Response.Headers["Keys"] = $"{user.Id}";
            }
     
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}