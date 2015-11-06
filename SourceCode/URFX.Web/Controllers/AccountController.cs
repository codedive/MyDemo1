using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using URFX.Business;
using URFX.Web.Models;
using URFX.Data.DataEntity.DomainModel;
using URFX.Web.Results;
using URFX.Web.Providers;
using URFX.Data.Enums;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using URFX.Data.DataEntity;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Collections;
using URFX.Data.Resources;
using URFX.Data.Entities;
using System.Net.Mail;
using System.Configuration;
//using System.Web.Mvc;
namespace URFX.Web.Controllers
{

    [RoutePrefix("api/Account")]
    
    public class AccountController : ApiController
    {
        ClientService clientService = new ClientService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        ServiceProviderServiceMappingService serviceProviderServiceMappingService = new ServiceProviderServiceMappingService();

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
       

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

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
       

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            
            var id = User.Identity.GetUserId();
            var role = UserManager.GetRoles(id);
           // UserManager.FindByIdAsync(id);
            ClientModel clientDetail = new ClientModel();
            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
            if (role[0].ToString() ==URFXRoles.Client.ToString()) {
                clientDetail = clientService.GetClientById(id);
            }
            else
            {
                serviceProviderModel = serviceProviderService.GetServiceProviderById(id);
            }
           return new UserInfoViewModel
            {
                Email = UserManager.FindByName(User.Identity.GetUserName()).Email,
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null,
                UserName = User.Identity.GetUserName(),
                Client = clientDetail,
                ServiceProvider= serviceProviderModel


           };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
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
                    Url = Url.Route("ExternalLogin", new
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

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
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

        // POST api/Account/RegisterClient
        [AllowAnonymous]
        [Route("RegisterClient")]
        public async Task<IHttpActionResult> RegisterClient(RegisterClientBindingModel model)
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
            else
            {
                model.ClientId = user.Id;
                IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.Client.ToString());
                if (!resultRoleCreated.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    ClientModel clientModel = new ClientModel();
                    AutoMapper.Mapper.Map(model, clientModel);
                    clientModel= clientService.SaveClient(clientModel);
                    AutoMapper.Mapper.Map(clientModel, model);
                }
            }
            return Ok();
        }

        // POST api/Account/RegisterServiceProvider
        [AllowAnonymous]
        [Route("RegisterServiceProvider")]
        public async Task<IHttpActionResult> RegisterServiceProvider()
        {
            try {

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var root = HttpContext.Current.Server.MapPath(Utility.Constants.BASE_FILE_UPLOAD_PATH);
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var resultModel = await Request.Content.ReadAsMultipartAsync(provider);
                if (resultModel.FormData["model"] == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                var plan = resultModel.FormData["planModel"];
                PlanServiceProviderModel planModel = new PlanServiceProviderModel();
                planModel = JsonConvert.DeserializeObject<PlanServiceProviderModel>(plan);
                var location = resultModel.FormData["LocationModel"];
                UserLocation locationModel = new UserLocation();
                locationModel = JsonConvert.DeserializeObject<UserLocation>(location);
                var services = resultModel.FormData["services"];
                string[] serviceIds = { string.Empty};
                List<string> serviceIdList = new List<string>();
                if (!string.IsNullOrEmpty(services))
                {
                    //serviceIds = services.Substring(1, services.Length - 1).Split(',');
                    serviceIdList = new List<string>(services.Substring(1, services.Length - 2).Split(','));
                    //serviceIdList.AddRange(serviceIds);
                }
                var model = resultModel.FormData["model"];
              
                RegisterServiceProviderBindingModel serviceProviderModel = new RegisterServiceProviderBindingModel();
                serviceProviderModel = JsonConvert.DeserializeObject<RegisterServiceProviderBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = serviceProviderModel.Email, Email = serviceProviderModel.Email };

                IdentityResult result = await UserManager.CreateAsync(user, serviceProviderModel.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account",
                    //   new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account",
                    //   "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");



                    serviceProviderModel.ServiceProviderId = user.Id;
                    IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.ServiceProvider.ToString());
                    if (!resultRoleCreated.Succeeded)
                    {
                        return GetErrorResult(resultRoleCreated);
                    }
                    else
                    {
                        if (resultModel.FileData.Count > 0)
                        {
                            string fileName;

                            if (HttpContext.Current.Request.Files != null)
                            {
                                for (var i = 0; i < resultModel.FileData.Count; i++)
                                {
                                    var file = HttpContext.Current.Request.Files[i];
                                    fileName = file.FileName;


                                    if (i == 0)
                                    {

                                        file.SaveAs(Path.Combine(root, Utility.Constants.COMPANY_LOGO_PATH, fileName));
                                        serviceProviderModel.CompanyLogoPath = fileName;
                                    }
                                    else if (i == 1)
                                    {
                                        file.SaveAs(Path.Combine(root, Utility.Constants.REGISTRATION_CERTIFICATE_PATH, fileName));
                                        serviceProviderModel.RegistrationCertificatePath = fileName;
                                    }
                                    else
                                    {
                                        file.SaveAs(Path.Combine(root, Utility.Constants.GOSI_CERTIFICATE_PATH, fileName));
                                        serviceProviderModel.GosiCertificatePath = fileName;
                                    }
                                }

                            }
                            ServiceProviderModel serviceProviderModelEntity = new ServiceProviderModel();
                            AutoMapper.Mapper.Map(serviceProviderModel, serviceProviderModelEntity);
                            serviceProviderModelEntity = serviceProviderService.SaveServiceProvider(serviceProviderModelEntity);
                            AutoMapper.Mapper.Map(serviceProviderModelEntity, serviceProviderModel);
                            ServiceProviderServiceMappingBindingModel serviceProviderServiceBindingModel = new ServiceProviderServiceMappingBindingModel();
                            ServiceProviderServiceMappingModel serviceProviderServiceModel = new ServiceProviderServiceMappingModel();
                            serviceProviderServiceBindingModel.ServiceProviderId = serviceProviderModelEntity.ServiceProviderId;
                            for(var i=0; i < serviceIdList.Count; i++)
                            {
                                serviceProviderServiceBindingModel.ServiceId =Convert.ToInt32(serviceIdList[i]);
                                AutoMapper.Mapper.Map(serviceProviderServiceBindingModel, serviceProviderServiceModel);
                                serviceProviderServiceModel = serviceProviderServiceMappingService.InsertServicesForServiceProvider(serviceProviderServiceModel);
                                AutoMapper.Mapper.Map(serviceProviderServiceModel, serviceProviderServiceBindingModel);
                            }
                        }

                    }
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }


        public IHttpActionResult Get(string lang)
        {
            var resourceObject = new JObject();

            var resourceSet = Resources.ResourceManager.GetResourceSet(new CultureInfo(lang), true, true);
            IDictionaryEnumerator enumerator = resourceSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                resourceObject.Add(enumerator.Key.ToString(), enumerator.Value.ToString());
            }
            return Ok(resourceObject);
        }

        // POST api/Account/ForgotPassword
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("user not exist");
                    // Don't reveal that the user does not exist or is not confirmed
                    // return View("ForgotPasswordConfirmation");
                }
                var Subject = Utility.Constants.RESET_PASSWORD_SUBJECT;
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                String Body = "";
                var result = await UserManager.FindByNameAsync(model.Email); //WebSecurity.GetUserId(model.UserName);
                Body += Utility.Constants.LINK_FOR_RESETPASSWORD;
                Body += Utility.Constants.TEXT_FOR_RESETPASSWORD;
                Body += "Your Email Id is : " + model.Email + " Your code is:" + code + "</br></p>";
                
                try
                {
                    // await service.SendAsync(message);
                    await UserManager.SendEmailAsync(user.Id, Subject, Body);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
               
            }
                return Ok();

            // If we got this far, something failed, redisplay form

        }

        // POST api/Account/ResetPassword
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await UserManager.FindByEmailAsync(model.Email);
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
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

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

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

        #endregion
    }
}
