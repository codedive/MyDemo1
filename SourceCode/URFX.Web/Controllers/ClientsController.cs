using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Data.Infrastructure;
using URFX.Web.Models;
using URFX.Web.Scheduler;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Clients")]
    public class ClientsController : ApiController
    {
        ClientService clientService = new ClientService();
        JobService jobService = new JobService();
        UserLocationService locationService = new UserLocationService();
        ClientRatingService clientRatingService = new ClientRatingService();
        EmployeeService employeeService = new EmployeeService();
        JobServiceMappingService jobServiceMappingService = new JobServiceMappingService();
        SubServicesService subServicesService = new SubServicesService();
        JobServicePictruesMappingService jobServicePictruesMappingService = new JobServicePictruesMappingService();
        TransactionHistoryService transactionHistoryService = new TransactionHistoryService();
        UserLocationService userLocationService = new UserLocationService();
        
        private ApplicationUserManager _userManager;

        public ClientsController()
        {

        }
        public ClientsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

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

        #region Get All Clients
        // GET: api/Clients
        public List<ClientModel> GetClient()
        {
            List<ClientModel> clientList = new List<ClientModel>();
            clientList = clientService.GetAllClients().Select(x=>new ClientModel {
             ClientId=x.ClientId,
             FirstName= CommonFunctions.ReadResourceValue(x.FirstName),
             LastName=CommonFunctions.ReadResourceValue(x.LastName),
             NationalIdNumber=CommonFunctions.ReadResourceValue(x.NationalIdNumber),
             NationaltIDType=x.NationaltIDType,
             IsActive=x.IsActive,
             IsDeleted=x.IsDeleted
            }).ToList();
            return clientList;
        }
        #endregion

        #region Get Client By Id
        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(string id)
        {
            //Get Client Details
            RegisterClientBindingModel registerClientBindingModel = new RegisterClientBindingModel();
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(id);
            AutoMapper.Mapper.Map(clientModel, registerClientBindingModel);
            var Email = UserManager.FindById(id) != null ? UserManager.FindById(id).Email : "";
            var PhoneNumber = UserManager.FindById(id) != null ? UserManager.FindById(id).PhoneNumber : "";
            registerClientBindingModel.Email = Email;
            registerClientBindingModel.PhoneNumber = PhoneNumber;
            //clientModel.FirstName = CommonFunctions.ReadResourceValue(clientModel.FirstName);
            //clientModel.LastName = CommonFunctions.ReadResourceValue(clientModel.LastName);
            //clientModel.NationalIdNumber = CommonFunctions.ReadResourceValue(clientModel.NationalIdNumber);


            //get rating for client
            List<ClientRatingBindingModel> clientRatingBindingModel = new List<ClientRatingBindingModel>();
            List<ClientRatingModel> clientRatingModel = new List<ClientRatingModel>();
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            List<JobModel> jobModel = new List<JobModel>();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetJobListByClientId(id);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            //Get employee Info for job

            ClientRatingModel ratingModel = new ClientRatingModel();

            string[] jobIds = jobBindingModel.Select(u => u.JobId.ToString()).ToArray();
            clientRatingModel = clientRatingService.GetClientRatingListByJobIds(jobIds);
            if (clientRatingModel.Count >0)
            {
                ratingModel.Behaivor = Convert.ToInt32(clientRatingModel.Select(c => c.Behaivor).Average());
                ratingModel.Communication = Convert.ToInt32(clientRatingModel.Select(c => c.Communication).Average());
                ratingModel.Corporation = Convert.ToInt32(clientRatingModel.Select(c => c.Corporation).Average());
                ratingModel.FriendLiness = Convert.ToInt32(clientRatingModel.Select(c => c.FriendLiness).Average());
                ratingModel.OverallSatisfaction = Convert.ToInt32(clientRatingModel.Select(c => c.OverallSatisfaction).Average());
                ratingModel.UnderStanding = Convert.ToInt32(clientRatingModel.Select(c => c.UnderStanding).Average());
            }
            ratingModel.TotalRating = CommonFunctions.GetTotalFeedbackForClient(ratingModel);

            //Get Location of client
            UserLocationModel model = locationService.FindLocationById(id);
            //
            registerClientBindingModel.ClientRatingModelList = clientRatingModel;
            registerClientBindingModel.AverageRating = ratingModel.TotalRating;
            registerClientBindingModel.UserLocationModel = model;




            return Ok(registerClientBindingModel);
        }
        #endregion

        #region Update Client
        // POST: api/Clients/UpdateClient
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("UpdateClient")]
        public async Task<IHttpActionResult> PutClient()
        {
            RegisterClientBindingModel clientBindingModel = new RegisterClientBindingModel();
            ClientModel clientModel = new ClientModel();
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
                var result = resultModel.FormData["model"];
                //var model = result.Substring(1, result.Length - 2);
                clientBindingModel = JsonConvert.DeserializeObject<RegisterClientBindingModel>(result);
                AutoMapper.Mapper.Map(clientBindingModel, clientModel);
                ClientModel clientCheckModel = new ClientModel();
                clientCheckModel = clientService.GetClientById(clientModel.ClientId);
                if (resultModel.FileData.Count > 0)
                {
                    string fileName;

                    if (HttpContext.Current.Request.Files != null)
                    {
                        for (var i = 0; i < resultModel.FileData.Count; i++)
                        {
                            var file = HttpContext.Current.Request.Files[i];
                            fileName = file.FileName;
                            file.SaveAs(Path.Combine(root, Utility.Constants.CLIENT_PROFILE_IMAGE_PATH, fileName));
                            clientModel.ProfilePicturePath = fileName;

                        }

                    }
             }
                if (clientModel.ProfilePicturePath == null)
                {
                    clientModel.ProfilePicturePath = clientCheckModel.ProfilePicturePath;
                }

                clientService.UpadteClient(clientModel);
                AutoMapper.Mapper.Map(clientModel, clientBindingModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }




            return Ok(clientBindingModel);
        }
        #endregion

        #region Delete Client
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(string id)
        {
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(id);
            if (clientModel == null)
            {
                return NotFound();
            }
            clientService.DeleteClient(id);
            ApplicationUser user = UserManager.FindById(id);

            IdentityResult result = UserManager.Delete(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
             return Ok();
        }
        #endregion

        #region ForgotPassword
        // POST api/Clients/ForgotPassword
        [AllowAnonymous]
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
                string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.FORGOT_PASSWORD_PATH_CLIENT));
                String Body = "";
                Body = String.Format(text, user.UserName);
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
            return Ok();

            // If we got this far, something failed, redisplay form

        }
        #endregion

        #region ResetPassword
        // POST api/Clients/ResetPassword
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
        {
            try
            {
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
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        #endregion

        #region OTPConfirmation
        // POST api/Clients/ConfirmOTP
        [HttpPost]
        [AllowAnonymous]
        [Route("ConfirmOTP")]
        public async Task<IHttpActionResult> ConfirmOTP(string email, string OTP)
        {
            try
            {
                if (email == null || OTP == null)
                {
                    return BadRequest("email and OTP is not provided");
                }
                var user = new ApplicationUser();
                user = await UserManager.FindByEmailAsync(email);
                ClientModel clientModel = new ClientModel();
                clientModel = clientService.GetClientById(user.Id);
                if (clientModel.OTP != OTP)
                {
                    return BadRequest("OTP you entered is not correct");
                }
                else
                {
                    user.EmailConfirmed = true;
                    IdentityResult result = await UserManager.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok();

        }
        #endregion

        #region Resend OTP
        // POST api/Clients/ResendOTP
        [HttpPost]
        [AllowAnonymous]
        [Route("ResendOTP")]
        public async Task<IHttpActionResult> ResendOTP(string clientId)
        {
            //get user
            var user = await UserManager.FindByIdAsync(clientId);
            //Get client Details
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(clientId);
            //generate OTP 
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            clientModel.OTP = sixDigitNumber;
            //Send Email
            var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
            var code = UserManager.GenerateEmailConfirmationToken(user.Id);
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
            var exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
            //var exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
            string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.CONFIRMATION_OTP_PATH));
            String Body = "";
            Body = String.Format(text, user.UserName, sixDigitNumber, exactPath);
            try
            {
                await UserManager.SendEmailAsync(user.Id, Subject, Body);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            //update Client
            clientModel = clientService.UpadteClient(clientModel);
            return Ok(clientModel);
        }
        #endregion

        #region Get Feedback
        // POST: api/Clients/GetFeedback
        [ResponseType(typeof(Client))]
        [HttpGet]
        [Route("GetFeedback")]
        public IHttpActionResult GetFeedback(string userId)
        {
            //Get Client Details
            RegisterClientBindingModel registerClientBindingModel = new RegisterClientBindingModel();
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(userId);
            AutoMapper.Mapper.Map(clientModel, registerClientBindingModel);
            var Email = UserManager.FindById(userId).Email;
            var PhoneNumber = UserManager.FindById(userId).PhoneNumber;
            
            //clientModel.FirstName = CommonFunctions.ReadResourceValue(clientModel.FirstName);
            //clientModel.LastName = CommonFunctions.ReadResourceValue(clientModel.LastName);
            //clientModel.NationalIdNumber = CommonFunctions.ReadResourceValue(clientModel.NationalIdNumber);


            //get rating for client
            List<ClientRatingBindingModel> clientRatingBindingModel = new List<ClientRatingBindingModel>();
            List<ClientRatingModel> clientRatingModel = new List<ClientRatingModel>();
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            List<JobModel> jobModel = new List<JobModel>();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetJobListByClientId(userId);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            ClientRatingModel ratingModel = new ClientRatingModel();
            string[] jobIds = jobBindingModel.Select(u => u.JobId.ToString()).ToArray();
            clientRatingModel = clientRatingService.GetClientRatingListByJobIds(jobIds);
            if (clientRatingModel.Count > 0)
            {
                ratingModel.Behaivor = Convert.ToInt32(clientRatingModel.Select(c => c.Behaivor).Average());
                ratingModel.Communication = Convert.ToInt32(clientRatingModel.Select(c => c.Communication).Average());
                ratingModel.Corporation = Convert.ToInt32(clientRatingModel.Select(c => c.Corporation).Average());
                ratingModel.FriendLiness = Convert.ToInt32(clientRatingModel.Select(c => c.FriendLiness).Average());
                ratingModel.OverallSatisfaction = Convert.ToInt32(clientRatingModel.Select(c => c.OverallSatisfaction).Average());
                ratingModel.UnderStanding = Convert.ToInt32(clientRatingModel.Select(c => c.UnderStanding).Average());
                ratingModel.TotalRating = CommonFunctions.GetTotalFeedbackForClient(ratingModel);
            }
            
           
            //Get Location of client
            UserLocationModel model = locationService.FindLocationById(userId);
            //bind info for client
            registerClientBindingModel.Email = Email;
            registerClientBindingModel.PhoneNumber = PhoneNumber;
            registerClientBindingModel.ClientRatingModelList = clientRatingModel;
            registerClientBindingModel.AverageRating = ratingModel.TotalRating;
            registerClientBindingModel.UserLocationModel = model;
           
            return Ok(registerClientBindingModel);
        }
        #endregion

        #region Save Feedback For Client
        // POST: api/Clients/SaveFeedback
        [ResponseType(typeof(Client))]
        [HttpPost]
        [Route("SaveFeedback")]
        public IHttpActionResult SaveFeedback(ClientRatingBindingModel model)
        {
            try
            {
                ClientRatingModel clientRatingModel = new ClientRatingModel();
                AutoMapper.Mapper.Map(model, clientRatingModel);
                clientRatingModel = clientRatingService.SaveClientRating(clientRatingModel);
                AutoMapper.Mapper.Map(clientRatingModel, model);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(model);
        }
        #endregion

        #region My Tasks
        // POST: api/Clients/MyTasks
        [ResponseType(typeof(Employee))]
        [HttpGet]
        [AllowAnonymous]
        [Route("MyTasks")]
        public IHttpActionResult MyTasks(string userId, string type)
        {
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            try
            {
                //get job according to user id
                List<JobModel> jobModel = new List<JobModel>();
                JobStatus jobStatus = (JobStatus)Enum.Parse(typeof(JobStatus), type);
                jobModel = jobService.GetJobListByClientId(userId, jobStatus);
                AutoMapper.Mapper.Map(jobModel, jobBindingModel);
                //get job service mapping
                List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
                string[] jobIds = jobBindingModel.Select(u => u.JobId.ToString()).ToArray();
                jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingByJobIds(jobIds);
                foreach (var item in jobServiceMappingModel)
                {
                    SubServiceModel subServiceModel = new SubServiceModel();
                    subServiceModel = subServicesService.GetSubServiceById(item.ServiceId);
                    item.ServiceName = subServiceModel.Description;
                }
                //get job service picture mapping
                List<JobServicePictureMappingModel> jobServicePictureMappingModel = new List<JobServicePictureMappingModel>();
                jobServicePictureMappingModel = jobServicePictruesMappingService.GetJobServicePictureMapping();
                jobBindingModel.ForEach(x =>
                {
                    x.JobServiceMappings = jobServiceMappingModel.Where(j => j.JobId == x.JobId).ToList();
                });
                jobServiceMappingModel.ForEach(x =>
                {
                    x.JobServicePictureMappings = jobServicePictureMappingModel.Where(j => j.JobServiceMappingId == x.JobServiceMappingId).ToList();
                });
                //get employee inforamtion
                jobBindingModel.ForEach(x =>
                {
                    //Get employee Name
                    EmployeeModel EmployeeModel = new EmployeeModel();
                    EmployeeModel = employeeService.GetEmployeeById(x.EmployeeId);
                    x.EmployeeModel = EmployeeModel;
                    UserLocationModel userLocationModel = new UserLocationModel();
                    userLocationModel = userLocationService.FindLocationById(x.ClientId);
                    if (x.JobAddress == null)
                    {
                        x.JobAddress = userLocationModel.Address;
                        x.Latitude = userLocationModel.Latitude;
                        x.Longitude = userLocationModel.Latitude;
                    }
                    if (x.StartDate != null && x.EndDate != null)
                    {
                        TimeSpan difference = Convert.ToDateTime(x.EndDate).AddDays(3) - Convert.ToDateTime(x.StartDate);
                        var days = difference.TotalDays;
                        if(days <= 3)
                        {
                            x.IsExpired = false;
                        }
                        else
                        {
                            x.IsExpired = true;
                        }
                    }

                });

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok(jobBindingModel);
        }
        #endregion

        #region Update Client For QuickBlox Id
        //Post:api/Clients/UpdateQuickBlox
        [ResponseType(typeof(UserLocationModel))]
        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateQuickBlox")]
        public IHttpActionResult UpdateQuickBlox(string userId, string quickBloxId)
        {
            try {
                ClientModel clientModel = new ClientModel();
                clientModel = clientService.GetClientById(userId);
                clientModel.QuickBloxId = quickBloxId;
                clientModel.Registred = true;
                clientModel = clientService.UpadteClient(clientModel);
                return Json(clientModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Receipt
        // GET: api/Clients/GetReceipt
        [ResponseType(typeof(Client))]
        [HttpGet]
        [AllowAnonymous]
        [Route("GetReceipt")]
        public IHttpActionResult GetReceipt(int jobId)
        {
            TransactionHistoryBindingModel transactionHistoryBindingModel = new TransactionHistoryBindingModel();
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            transactionHistoryModel = transactionHistoryService.GetTransactionHistoryByJobId(jobId);
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistoryBindingModel);
            return Ok(transactionHistoryBindingModel);
        }
        #endregion

        #region Guest Login
        // GET: api/Clients/GuestLogin
        [ResponseType(typeof(Client))]
        [AllowAnonymous]
        [HttpGet]
        [Route("GuestLogin")]
        public async Task<IHttpActionResult> GuestLogin()
        {
            try
            {
                var token = GetToken(Data.DataEntity.Constants.GUEST_USER_NAME, Data.DataEntity.Constants.GUEST_USER_PASSWORD);
                var json = JsonConvert.DeserializeObject(token);
                return Json(json);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
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
        static string GetToken(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", userName ),
                            new KeyValuePair<string, string> ( "Password", password )
                        };
            var content = new FormUrlEncodedContent(pairs);
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "";
#if DEBUG
            var link = scheme + "://" + host + port + "/Token";
#else
            var link = scheme + "://" + host +"/Token";
#endif
            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(link, content).Result;
                return response.Content.ReadAsStringAsync().Result;
                //var splittedFileName = fileName.Split('_');

            }
        }
    }
}