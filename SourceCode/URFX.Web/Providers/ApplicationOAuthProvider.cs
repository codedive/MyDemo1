using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using URFX.Data.DataEntity;
using System.Web;
using URFX.Data.DataEntity.DomainModel;
using URFX.Business;
using URFX.Data.Enums;

namespace URFX.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        URFXDbContext db = new URFXDbContext();
        private ApplicationUserManager _userManager;
        UserPlanService userPlanService = new UserPlanService();
        EmployeeService employeeService = new EmployeeService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            try
            {
                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user.Id); // Get all roles of user from server
                    var role = roles.FirstOrDefault(); // Get user role

                    var IsEmployee = HttpContext.Current.Request.Headers["IsEmployee"];
                    var IsClient = HttpContext.Current.Request.Headers["IsClient"];
                    if (IsEmployee == null && role == URFXRoles.Employee.ToString())
                    {
                        context.SetError("invalid_grant", "You are not authorised to login.Please contact at support@urfxco.com");
                        return;
                    }
                    if (IsClient == null && role == URFXRoles.Client.ToString())
                    {
                        context.SetError("invalid_grant", "You are not authorised to login.Please contact at support@urfxco.com");
                        return;
                    }
                    if (role != null && role != URFXRoles.Employee.ToString())
                    {
                        if (user.RegistrationType == RegistrationType.Simple)
                        {
                            if (!user.EmailConfirmed)
                            {
                                context.SetError("invalid_grant", "Please confirm your email first.");
                                return;
                            }
                        }
                    }
                    if (role != URFXRoles.Client.ToString())
                    {
                        var deviceType = HttpContext.Current.Request.Headers["deviceType"];
                        var deviceToken = HttpContext.Current.Request.Headers["deviceToken"];
                        user.DeviceToken = deviceToken;
                        user.DeviceType = deviceType;
                    }



                    IdentityResult result = await UserManager.UpdateAsync(user);

                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                       OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                        CookieAuthenticationDefaults.AuthenticationType);
                    UserPlanModel userPlanModel = new UserPlanModel();
                    ServiceProviderModel serviceproviderModel = new ServiceProviderModel();
                    userPlanModel = userPlanService.GetUserPlanByUserId(user.Id);
                    if (role == URFXRoles.ServiceProvider.ToString())
                    {
                        serviceproviderModel = serviceProviderService.GetServiceProviderById(user.Id);
                    }
                    
                    AuthenticationProperties properties = null;
                    if (user.DeviceType != null && user.DeviceToken != null)
                    {
                        properties = CreateProperties(user.UserName, role, user.Id, user.DeviceType, user.DeviceToken, userPlanModel.PlanId, user.IsRegister, user.IsLogin, serviceproviderModel.ServiceProviderType);
                    }
                    else
                    {
                        properties = CreateProperties(user.UserName, role, user.Id, userPlanModel.PlanId, user.IsRegister, user.IsLogin, serviceproviderModel.ServiceProviderType);
                    }
                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
        public static AuthenticationProperties CreateProperties(string userName, string Roles, string userId, int planId, bool IsRegister, bool IsLogin, ServiceProviderType type)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
        {
            { "userName", userName },
            {"roles",Roles},
            {"userId",userId},
            {"planId",planId.ToString() } ,
            {"IsRegister",IsRegister.ToString() },
            {"IsLogin",IsLogin.ToString() },
            {"type",type.ToString() }

        };
            return new AuthenticationProperties(data);
        }
        public static AuthenticationProperties CreateProperties(string userName, string Roles, string userId, string deviceType, string deviceToken, int planId, bool IsRegister, bool IsLogin, ServiceProviderType type)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
        {
            { "userName", userName },
            {"roles",Roles},
            {"userId",userId},
            {"deviceType", deviceType},
            {"deviceToken", deviceToken},
            {"planId",planId.ToString() },
            {"IsRegister",IsRegister.ToString() },
            {"IsLogin",IsLogin.ToString() },
             {"type",type.ToString() }

        };
            return new AuthenticationProperties(data);
        }



    }

}