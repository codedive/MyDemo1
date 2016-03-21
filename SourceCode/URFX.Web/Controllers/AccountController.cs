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
using System.Linq;
using System.Web.Http.Description;
using Microsoft.Owin;
using Owin;
using System.Transactions;
using System.Drawing;
//using System.Web.Http.Cors;
//using System.Web.Mvc;
namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]

    public class AccountController : ApiController
    {
        ClientService clientService = new ClientService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        ServiceProviderServiceMappingService serviceProviderServiceMappingService = new ServiceProviderServiceMappingService();
        ServiceProviderEmployeeMappingService serviceProviderEmployeeMappingService = new ServiceProviderEmployeeMappingService();
        UserLocationService userLocationService = new UserLocationService();
        EmployeeService employeeService = new EmployeeService();
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        //  private ApplicationRole _roleManager;

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
        //public ApplicationRole RoleManager
        //{
        //    get
        //    {
        //        return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRole>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}


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
            if (role[0].ToString() == URFXRoles.Client.ToString())
            {
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
                ServiceProvider = serviceProviderModel


            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout(string userId)
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            ApplicationUser user = UserManager.FindById(userId);
            if (user != null)
            {
                user.DeviceToken = null;
                user.DeviceType = null;
            }
           
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
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;
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
            //redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
            //                                redirectUri,
            //                                externalLogin.ExternalAccessToken,
            //                                externalLogin.LoginProvider,
            //                                hasRegistered.ToString(),
            //                                externalLogin.UserName);

            //return Redirect(redirectUri);
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

            return Ok(externalLogin);
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
            //using (var dataContext = new URFXDbContext())
            //{
            //    TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //    {
            try
            {
                if (model.RegistrationType == RegistrationType.Simple)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DeviceType = model.DeviceType,
                        DeviceToken = model.DeviceToken,
                        RegistrationType = model.RegistrationType,
                        FacebookId = model.FacebookId,
                        GoogleId = model.GoogleId,
                        TwitterId = model.TwitterId
                    };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        // transaction.Dispose();
                        return GetErrorResult(result);
                    }

                    else
                    {
                        model.ClientId = user.Id;
                        IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.Client.ToString());
                        if (!resultRoleCreated.Succeeded)
                        {
                            // transaction.Dispose();
                            return GetErrorResult(resultRoleCreated);
                        }
                        else
                        {
                           try
                            {
                                //generate OTP 
                                Random r = new Random();
                                int randNum = r.Next(1000000);
                                string sixDigitNumber = randNum.ToString("D6");
                                model.OTP = sixDigitNumber;
                                //Save Client
                                ClientModel clientModel = new ClientModel();
                                AutoMapper.Mapper.Map(model, clientModel);
                                clientModel = clientService.SaveClient(clientModel);
                                AutoMapper.Mapper.Map(clientModel, model);
                                //save location for client
                                UserLocationModel locationModel = new UserLocationModel();
                                locationModel.UserId = user.Id;
                                locationModel.CityId = model.CityId;
                                locationModel.DistrictId = 1;
                                locationModel.Latitude = model.Latitude;
                                locationModel.Longitude = model.Longitude;
                                locationModel.Address = model.Address;
                                if (locationModel.Address != null)
                                {
                                    locationModel = userLocationService.InsertUserLocation(locationModel);
                                }

                                //Send Email
                                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                                var scheme = HttpContext.Current.Request.Url.Scheme;
                                var host = HttpContext.Current.Request.Url.Host;
                                var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
                                string language = "en";
                                var cookie = HttpContext.Current.Request.Cookies.Get("APPLICATION_LANGUAGE");
                                if (cookie != null)
                                    language = cookie.Value;
                                string exactPath;
                                if (language == "en")
                                {
#if DEBUG
                                    exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
#else
                                    exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#endif
                                }
                                else
                                {
#if DEBUG
                                    exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/arabic-logo.png";
#else
                                    exactPath = scheme + "://" + host + "/Content/URFXTheme/images/arabic-logo.png";
#endif

                                }
                                // var exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
                                //var exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
                                var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
                                string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.CONFIRMATION_OTP_PATH));
                                String Body = "";
                                Body = String.Format(text, user.UserName, sixDigitNumber, exactPath);
                                try
                                {
                                    await UserManager.SendEmailAsync(user.Id, Subject, Body);
                                }
                                catch (Exception ex)
                                {
                                    // transaction.Dispose();
                                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                    return BadRequest(ex.Message);
                                }
                                //transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                // transaction.Dispose();
                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                return BadRequest(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    if (model.Password == null && model.RegistrationType == RegistrationType.Facebook)
                    {
                        model.Password = model.FacebookId;
                    }
                    else if (model.Password == null && model.RegistrationType == RegistrationType.Google)
                    {
                        model.Password = model.GoogleId;
                    }
                    else if (model.Password == null && model.RegistrationType == RegistrationType.Twitter)
                    {
                        model.Password = model.TwitterId;
                    }
                    var checkUser = UserManager.FindByEmail(model.Email);
                    if (checkUser == null)
                    {
                        var user = new ApplicationUser()
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            DeviceType = model.DeviceType,
                            DeviceToken = model.DeviceToken,
                            RegistrationType = model.RegistrationType,
                            FacebookId = model.FacebookId,
                            GoogleId = model.GoogleId,
                            TwitterId = model.TwitterId,
                            IsRegister = true,
                            IsLogin = false

                        };
                        IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                        if (!result.Succeeded)
                        {
                            //  transaction.Dispose();
                            return GetErrorResult(result);
                        }

                        else
                        {
                            model.ClientId = user.Id;
                            IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.Client.ToString());
                            //Save Client
                            try
                            {
                                ClientModel clientModel = new ClientModel();
                                AutoMapper.Mapper.Map(model, clientModel);
                                clientModel = clientService.SaveClient(clientModel);
                                AutoMapper.Mapper.Map(clientModel, model);

                                //save location for client
                                UserLocationModel locationModel = new UserLocationModel();
                                locationModel.UserId = user.Id;
                                locationModel.CityId = model.CityId;
                                locationModel.DistrictId = 1;
                                locationModel.Latitude = model.Latitude;
                                locationModel.Longitude = model.Longitude;
                                locationModel.Address = model.Address;
                                if (locationModel.Address != null)
                                {
                                    locationModel = userLocationService.InsertUserLocation(locationModel);
                                }
                                //check if register using facebook,google,twitter
                                string token = GetToken(user.UserName, model.Password);
                                var json = JsonConvert.DeserializeObject(token);
                                //    transaction.Complete();
                                return Json(json);

                            }
                            catch (Exception ex)
                            {
                                //  transaction.Dispose();
                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                return BadRequest(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (checkUser.RegistrationType == model.RegistrationType)
                            {

                                checkUser.DeviceType = model.DeviceType;
                                checkUser.DeviceToken = model.DeviceToken;
                                checkUser.IsLogin = true;
                                //UserManager.Update(checkUser);
                                IdentityResult result = await UserManager.UpdateAsync(checkUser);
                                //update client
                                ClientModel clientModel = new ClientModel();
                                clientModel = clientService.GetClientById(checkUser.Id);
                                //if (clientModel.ClientId != null)
                                //{
                                //    clientModel.QuickBloxId = model.QuickBloxId;
                                //    clientModel = clientService.UpadteClient(clientModel);
                                //}

                                //update location for client
                                UserLocationModel locationModel = new UserLocationModel();
                                locationModel = userLocationService.FindLocationById(checkUser.Id);
                                locationModel.UserId = checkUser.Id;
                                locationModel.CityId = model.CityId;
                                locationModel.DistrictId = 1;
                                locationModel.Latitude = model.Latitude;
                                locationModel.Longitude = model.Longitude;
                                locationModel.Address = model.Address;
                                if (locationModel.UserLocationId > 0)
                                {
                                    locationModel = userLocationService.UpadteUserLocation(locationModel);
                                }
                                else
                                {
                                    locationModel = userLocationService.InsertUserLocation(locationModel);
                                }
                                var resetToken = await UserManager.GeneratePasswordResetTokenAsync(checkUser.Id);
                                await UserManager.ResetPasswordAsync(checkUser.Id, resetToken, model.Password);
                                string token = GetToken(checkUser.UserName, model.Password);
                                var json = JsonConvert.DeserializeObject(token);
                                //   transaction.Complete();
                                return Json(json);
                            }

                            else
                            {
                                if (model.RegistrationType == RegistrationType.Google)
                                {
                                    checkUser.RegistrationType = model.RegistrationType;
                                    checkUser.GoogleId = model.GoogleId;
                                    checkUser.DeviceType = model.DeviceType;
                                    checkUser.DeviceToken = model.DeviceToken;
                                    checkUser.IsLogin = true;
                                    //UserManager.Update(checkUser);
                                    IdentityResult result = await UserManager.UpdateAsync(checkUser);
                                    //update client
                                    ClientModel clientModel = new ClientModel();
                                    clientModel = clientService.GetClientById(checkUser.Id);
                                    //if (clientModel.ClientId != null)
                                    //{
                                    //    clientModel.QuickBloxId = model.QuickBloxId;
                                    //    clientModel = clientService.UpadteClient(clientModel);
                                    //}
                                    
                                    //update location for client
                                    UserLocationModel locationModel = new UserLocationModel();
                                    locationModel = userLocationService.FindLocationById(checkUser.Id);
                                    locationModel.UserId = checkUser.Id;
                                    locationModel.CityId = model.CityId;
                                    locationModel.DistrictId = 1;
                                    locationModel.Latitude = model.Latitude;
                                    locationModel.Longitude = model.Longitude;
                                    locationModel.Address = model.Address;
                                    if (locationModel.UserLocationId > 0)
                                    {
                                        locationModel = userLocationService.UpadteUserLocation(locationModel);
                                    }
                                    else
                                    {
                                        locationModel = userLocationService.InsertUserLocation(locationModel);
                                    }
                                    var resetToken = await UserManager.GeneratePasswordResetTokenAsync(checkUser.Id);
                                    await UserManager.ResetPasswordAsync(checkUser.Id, resetToken, model.Password);
                                    string token = GetToken(checkUser.UserName, model.Password);
                                    var json = JsonConvert.DeserializeObject(token);
                                    //   transaction.Complete();
                                    return Json(json);
                                }
                                else
                                {
                                    checkUser.RegistrationType = model.RegistrationType;
                                    checkUser.TwitterId = model.TwitterId;
                                    checkUser.DeviceType = model.DeviceType;
                                    checkUser.DeviceToken = model.DeviceToken;
                                    checkUser.IsLogin = true;
                                    //UserManager.Update(checkUser);
                                    IdentityResult result = await UserManager.UpdateAsync(checkUser);
                                    //update client
                                    ClientModel clientModel = new ClientModel();
                                    clientModel = clientService.GetClientById(checkUser.Id);
                                    //if (clientModel.ClientId != null)
                                    //{
                                    //    clientModel.QuickBloxId = model.QuickBloxId;
                                    //    clientModel = clientService.UpadteClient(clientModel);
                                    //}
                                    
                                    //update location for client
                                    UserLocationModel locationModel = new UserLocationModel();
                                    locationModel = userLocationService.FindLocationById(checkUser.Id);
                                    locationModel.UserId = checkUser.Id;
                                    locationModel.CityId = model.CityId;
                                    locationModel.DistrictId = 1;
                                    locationModel.Latitude = model.Latitude;
                                    locationModel.Longitude = model.Longitude;
                                    locationModel.Address = model.Address;
                                    if (locationModel.UserLocationId > 0)
                                    {
                                        locationModel = userLocationService.UpadteUserLocation(locationModel);
                                    }
                                    else
                                    {
                                        locationModel = userLocationService.InsertUserLocation(locationModel);
                                    }
                                    var resetToken = await UserManager.GeneratePasswordResetTokenAsync(checkUser.Id);
                                    await UserManager.ResetPasswordAsync(checkUser.Id, resetToken, model.Password);
                                    string token = GetToken(checkUser.UserName, model.Password);
                                    var json = JsonConvert.DeserializeObject(token);
                                    //   transaction.Complete();
                                    return Json(json);
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            //  transaction.Dispose();
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                            return BadRequest(ex.Message);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                // transaction.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            //    }
            //}
            return Ok(model);
        }

        static string GetToken(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                                {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", userName ),
                            new KeyValuePair<string, string> ( "Password", password )
                                };
            var content = new FormUrlEncodedContent(pairs);
            content.Headers.Add("IsClient", "client");
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
#if DEBUG
            var link = scheme + "://" + host + port + "/Token";
#else
             var link = scheme + "://" + host + "/Token";
#endif
            using (var client = new HttpClient())
            {
                var response =
            client.PostAsync(link, content).Result;
                return response.Content.ReadAsStringAsync().Result;
                //var splittedFileName = fileName.Split('_');

            }
        }



        // POST api/Account/RegisterServiceProvider
        [AllowAnonymous]
        [Route("RegisterServiceProvider")]
        public async Task<IHttpActionResult> RegisterServiceProvider()
        {
            //using (var dataContext = new URFXDbContext())
            //{
            //    TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //    {
            try
            {
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
                UserLocationModel locationModel = new UserLocationModel();
                locationModel = JsonConvert.DeserializeObject<UserLocationModel>(location);
                //var services = resultModel.FormData["services"];
                //string[] serviceIds = { string.Empty };
                //List<string> serviceIdList = new List<string>();
                //if (!string.IsNullOrEmpty(services))
                //{
                //    //serviceIds = services.Substring(1, services.Length - 1).Split(',');
                //    serviceIdList = new List<string>(services.Substring(1, services.Length - 2).Split(','));
                //    //serviceIdList.AddRange(serviceIds);
                //}
                var model = resultModel.FormData["model"];

                RegisterServiceProviderBindingModel serviceProviderModel = new RegisterServiceProviderBindingModel();
                serviceProviderModel = JsonConvert.DeserializeObject<RegisterServiceProviderBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = serviceProviderModel.Email, Email = serviceProviderModel.Email, PhoneNumber = serviceProviderModel.PhoneNumber };

                IdentityResult result = await UserManager.CreateAsync(user, serviceProviderModel.Password);

                if (!result.Succeeded)
                {
                    // transaction.Dispose();
                    //UserManager.Delete(user);
                    return BadRequest("{\"status\" : false, \"message\" : \"Email id is already used\"}");
                }

                else
                {
                    serviceProviderModel.ServiceProviderId = user.Id;
                    serviceProviderModel.StartDate = DateTime.UtcNow;
                    IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.ServiceProvider.ToString());
                    if (!resultRoleCreated.Succeeded)
                    {
                        //  transaction.Dispose();
                        //UserManager.Delete(user);
                        return GetErrorResult(resultRoleCreated);
                    }
                    else
                    {
                        try
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
                                        //// Load image.
                                        //Image image = Image.FromStream(file.InputStream);
                                        
                                        //// Compute thumbnail size.
                                        //Size thumbnailSize = GetThumbnailSize(image);
                                         

                                        if (i == 0)
                                        {
                                            file.SaveAs(Path.Combine(root, Utility.Constants.REGISTRATION_CERTIFICATE_PATH, fileName));
                                            serviceProviderModel.RegistrationCertificatePath = fileName;
                                            //var thumbnails= Guid.NewGuid() + ".jpg";
                                            // Get thumbnail.
                                            //Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width,
                                            //    thumbnailSize.Height, null, IntPtr.Zero);
                                            //thumbnail.Save(Path.Combine(root, Utility.Constants.THUMBNAILSIMAGES, thumbnails));
                                        }
                                        else
                                        {
                                            file.SaveAs(Path.Combine(root, Utility.Constants.GOSI_CERTIFICATE_PATH, fileName));
                                            serviceProviderModel.GosiCertificatePath = fileName;
                                            //var thumbnails = Guid.NewGuid() + ".jpg";
                                            // Get thumbnail.
                                            //Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width,
                                            //    thumbnailSize.Height, null, IntPtr.Zero);
                                            //thumbnail.Save(Path.Combine(root, Utility.Constants.THUMBNAILSIMAGES, thumbnails));
                                        }

                                    }

                                }

                            }
                            if (locationModel.CityId != 0)
                            {
                                locationModel.UserId = user.Id;
                                locationModel.DistrictId = 1;
                                userLocationService.InsertUserLocation(locationModel);
                            }
                            ServiceProviderModel serviceProviderModelEntity = new ServiceProviderModel();
                            AutoMapper.Mapper.Map(serviceProviderModel, serviceProviderModelEntity);
                            serviceProviderModelEntity = serviceProviderService.SaveServiceProvider(serviceProviderModelEntity);

                            AutoMapper.Mapper.Map(serviceProviderModelEntity, serviceProviderModel);
                            var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
                            var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                            var scheme = HttpContext.Current.Request.Url.Scheme;
                            var host = HttpContext.Current.Request.Url.Host;
                            var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
                            string language = "en";
                            var cookie = HttpContext.Current.Request.Cookies.Get("APPLICATION_LANGUAGE");
                            if (cookie != null)
                                language = cookie.Value;
                            string exactPath;
                            if (language == "en")
                            {
#if DEBUG
                                exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
#else
                                exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#endif
                            }
                            else
                            {
#if DEBUG
                                exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/arabic-logo.png";
#else
                                exactPath = scheme + "://" + host + "/Content/URFXTheme/images/arabic-logo.png";
#endif
                            }
                            //var exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#if DEBUG
                            var link = scheme + "://" + host + port + "/App/#/confirmemail/" + user.Id + "";
#else
                             var link = scheme + "://" + host + "/App/#/confirmemail/" + user.Id + "";
#endif
                            var Link = "<a href='" + link + "'>" + link + "</a>";

                            string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.CONFIRMATION_EMAIL_PATH));
                            String Body = "";
                            Body = String.Format(text, user.UserName, Link, exactPath);
                            try
                            {
                                // await service.SendAsync(message);
                                await UserManager.SendEmailAsync(user.Id, Subject, Body);
                            }
                            catch (Exception ex)
                            {

                                // transaction.Dispose();
                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                return BadRequest(ex.Message);
                            }
                            // transaction.Complete();

                        }
                        catch (Exception ex)
                        {
                            //  transaction.Dispose();
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                            return BadRequest(ex.Message);
                        }                        
                    }

                }

                return Ok();
            }
            catch (Exception ex)
            {
                //  transaction.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            //    }
            //}
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

        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string language = "en";
                var cookie = HttpContext.Current.Request.Cookies.Get("APPLICATION_LANGUAGE");
                if (cookie != null)
                    language = cookie.Value;

                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("{\"status\" : false, \"message\" : \"Email id does not exist\"}");
                    // Don't reveal that the user does not exist or is not confirmed
                    // return View("ForgotPasswordConfirmation");
                }
                if (user.EmailConfirmed)
                {
                    var Subject = Utility.Constants.RESET_PASSWORD_SUBJECT;
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var scheme = HttpContext.Current.Request.Url.Scheme;
                    var host = HttpContext.Current.Request.Url.Host;
                    var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
                    string exactPath;
                    if (language == "en")
                    {
#if DEBUG
                        exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
#else
                        exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#endif
                    }
                    else
                    {
#if DEBUG
                        exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/arabic-logo.png";
#else
                        exactPath = scheme + "://" + host + "/Content/URFXTheme/images/arabic-logo.png";
#endif
                    }
                    string emial = Utility.CommonFunctions.Encrypt(user.Email);
                    emial = emial.Replace('/', '$');
                    code = code.Replace('/', '$');
#if DEBUG
                    var link = scheme + "://" + host + port + "/App/#/resetpwd/" + emial + "/" + code + "";
#else
                    var link = scheme + "://" + host + "/App/#/resetpwd/" + emial + "/" + code + "";
#endif
                    var Link = "<a href='" + link + "'>" + link + "</a>";
                    string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.FORGOT_PASSWORD_PATH));
                    String Body = "";
                    Body = String.Format(text, user.UserName, Link, exactPath);
                    try
                    {
                        // await service.SendAsync(message);
                        await UserManager.SendEmailAsync(user.Id, Subject, Body);
                    }

                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        return BadRequest(ex.Message);
                    }
                }
                else
                {   
                    return BadRequest("{\"status\" : false, \"message\" : \"Email is not confirmed yet please confirm your email first\"}");                    
                }

            }
            return Ok();

            // If we got this far, something failed, redisplay form

        }

        // POST api/Account/ResetPassword
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await UserManager.FindByEmailAsync(model.Email);
                var token= model.Code.Replace('$', '/');
                //var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
               if (!result.Succeeded)
                {
                    return BadRequest("{\"status\" : false, \"message\" : \"Your link has been expired please try again.\"}");
                }
               
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        // POST api/Account/ConfirmEmail
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ConfirmEmail(string UserId)
        {
            try
            {
                if (UserId == null)
                {
                    return BadRequest("UserId is not provided");
                }
                var code = UserManager.GenerateEmailConfirmationToken(UserId);
                bool IsConfirmed = await UserManager.IsEmailConfirmedAsync(UserId);

                var result = await UserManager.ConfirmEmailAsync(UserId, code);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }


                //ServiceProviderModel serviceProviderModelEntity = new ServiceProviderModel();
                //serviceProviderModelEntity = serviceProviderService.GetServiceProviderById(UserId);
                //serviceProviderModelEntity.IsActive = true;
                //serviceProviderModelEntity = serviceProviderService.UpadteServiceProvider(serviceProviderModelEntity);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok();

        }
        // POST api/Account/DecreptEmail
        [Route("DecreptEmail")]
        [AllowAnonymous]
        public IHttpActionResult DecreptEmail(string email)
        {
            string decreptEmail;
            try
            {
                if (email == null)
                {
                    return BadRequest("UserId is not provided");
                }

                email = email.Replace(' ', '+');
                email = email.Replace('$', '/');
                
                decreptEmail = Utility.CommonFunctions.Decrypt(email);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok(decreptEmail);

        }

        // POST api/Account/ResendConfirmation
        [Route("ResendConfirmation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResendConfirmation(string Email)
        {
            if (ModelState.IsValid)
            {
                string language = "en";
                var cookie = HttpContext.Current.Request.Cookies.Get("APPLICATION_LANGUAGE");
                if (cookie != null)
                    language = cookie.Value;
                string exactPath;
                var user = await UserManager.FindByNameAsync(Email);
                if (user == null)
                {
                    return BadRequest("{\"status\" : false, \"message\" : \"Email id does not exist\"}");
                    // Don't reveal that the user does not exist or is not confirmed
                    // return View("ForgotPasswordConfirmation");
                }
                var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                var scheme = HttpContext.Current.Request.Url.Scheme;
                var host = HttpContext.Current.Request.Url.Host;
                var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
                if (language == "en")
                {
#if DEBUG
                                exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
#else
                    exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#endif
                }
                else
                {
#if DEBUG
                                exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/arabic-logo.png";
#else
                    exactPath = scheme + "://" + host + "/Content/URFXTheme/images/arabic-logo.png";
#endif
                }
                //var exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
#if DEBUG
                            var link = scheme + "://" + host + port + "/App/#/confirmemail/" + user.Id + "";
#else
                var link = scheme + "://" + host + "/App/#/confirmemail/" + user.Id + "";
#endif
                var Link = "<a href='" + link + "'>" + link + "</a>";

                string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.CONFIRMATION_EMAIL_PATH));
                String Body = "";
                Body = String.Format(text, user.UserName, Link, exactPath);
                try
                {
                    // await service.SendAsync(message);
                    await UserManager.SendEmailAsync(user.Id, Subject, Body);
                }
                catch (Exception ex)
                {

                    // transaction.Dispose();
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    return BadRequest(ex.Message);
                }

            }
            return Ok();

            // If we got this far, something failed, redisplay form

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
            public string ExternalAccessToken { get; set; }
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

        // GET api/Account/Users
        [Route("Users")]
        public IQueryable GetAllUser()
        {
            var context = new URFXDbContext();
            var role = context.Roles.Where(x => x.Name == URFXRoles.Admin.ToString()).SingleOrDefault();
            IQueryable users = UserManager.Users.Where(x => (x.Roles.Where(r => r.RoleId != role.Id).Count() > 0));
            return users;
        }

        // GET api/Account/Roles
        [Route("Roles")]
        [ResponseType(typeof(RoleBindingModel))]
        public IHttpActionResult GetAllRole()
        {
            URFXDbContext db = new URFXDbContext();
            var roles = db.Roles.Select(x => new RoleBindingModel() { Id = x.Id, Name = x.Name }).ToList();
            return Ok(roles);
        }


        // Post /api/Account/Paging
        [Route("Paging")]
        public ResponseMessage Paging(PagingModel model)
        {
            
            ResponseMessage responseMessage = new ResponseMessage();
            var context = new URFXDbContext();
            var role = context.Roles.Where(x => x.Name == URFXRoles.Admin.ToString()).SingleOrDefault();
            string searchparam = model.SearchText == null ? "" : model.SearchText;
            List<ApplicationUser> users = UserManager.Users.Where(x => (x.Roles.Where(r => r.RoleId != role.Id).Count() > 0)).ToList();
            users.ForEach(x => {
                var roles = UserManager.GetRoles(x.Id);
                var userRole = roles.FirstOrDefault();
                x.Role = userRole;
            });
           
            if (searchparam != "")
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchparam.ToLower())).ToList();
            }


            responseMessage.totalRecords = users.Count();
            users = users.OrderBy(x => x.UserName).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            responseMessage.data = users;

            return responseMessage;
        }



        // Post /api/Account/RolePaging
        [Route("RolePaging")]
        public ResponseMessage RolesPaging(PagingModel model)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            string searchparam = model.SearchText == null ? "" : model.SearchText;
            List<RoleBindingModel> roles;
            URFXDbContext db = new URFXDbContext();
            if (searchparam == "")
            {

                roles = db.Roles.Select(x => new RoleBindingModel() { Id = x.Id, Name = x.Name }).ToList();
            }
            else
            {
                roles = db.Roles.Select(x => new RoleBindingModel() { Id = x.Id, Name = x.Name }).Where(y => y.Name.ToLower().Contains(searchparam.ToLower())).ToList();
            }
            responseMessage.totalRecords = roles.Count();
            roles = roles.OrderBy(x => x.Name).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            responseMessage.data = roles;

            return responseMessage;
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [HttpGet]
        [Route("RegisterExternalLogin", Name = "RegisterExternalLogin")]
        public async Task<IHttpActionResult> RegisterExternalLogin()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
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

            return Ok(externalLogin);
        }

        static Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 40;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }
}


