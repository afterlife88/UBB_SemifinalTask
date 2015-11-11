using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using UBB_SemifinalTask.Providers;

namespace UBB_SemifinalTask
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            ConfigureWebApi(httpConfig);
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new SimpleAuthorizationServerProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
   
        //private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        //{
        //    // Configure the db context and user manager to use a single instance per request
        //    app.CreatePerOwinContext(ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

        //    // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here
        //    OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
        //    {
        //        //For Dev enviroment only (on production should be AllowInsecureHttp = false)
        //        AllowInsecureHttp = true,
        //        TokenEndpointPath = new PathString("/oauth/token"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        //        Provider = new CustomOAuthProvider(),
        //        AccessTokenFormat = new CustomJwtFormat("http://localhost:1711"),
        //    };

        //    // OAuth 2.0 Bearer Access Token Generation
        //    app.UseOAuthAuthorizationServer(oAuthServerOptions);
        //}

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}