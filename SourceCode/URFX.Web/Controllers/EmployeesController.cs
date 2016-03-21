using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Web.Models;
using URFX.Web.Scheduler;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        CarTypeService carTypeService = new CarTypeService();
        ClientService clientService = new ClientService();
        EmployeeService employeeService = new EmployeeService();
        UserLocationService userLocationService = new UserLocationService();
        ServiceProviderServiceMappingService serviceProviderServiceMappingService = new ServiceProviderServiceMappingService();
        UserLocationModel userLocationModel = new UserLocationModel();
        JobService jobService = new JobService();
        JobServicePictruesMappingService jobServicePictruesMappingService = new JobServicePictruesMappingService();
        JobServiceMappingService jobServiceMappingService = new JobServiceMappingService();
        CarEmployeeMappingService carEmployeeMappingService = new CarEmployeeMappingService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        ServiceProviderEmployeeMappingService serviceProviderEmployeeMappingService = new ServiceProviderEmployeeMappingService();
        SubServicesService subServicesService = new SubServicesService();
        EmployeeScheduleService employeeScheduleService = new EmployeeScheduleService();
        SendNotificationService sendNotificationService = new SendNotificationService();
        RatingService ratingService = new RatingService();
        JobTask jobTask = new JobTask();
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;


        public EmployeesController()
        {
        }

        public EmployeesController(ApplicationUserManager userManager)
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


        #region Get All Employees
        // GET: api/Employees
        [ResponseType(typeof(Employee))]
        public List<EmployeeBindingModel> GetEmployee()
        {
            List<EmployeeBindingModel> employeeBindingModel = new List<EmployeeBindingModel>();
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList = employeeService.GetAllEmployees().ToList();
            AutoMapper.Mapper.Map(employeeList, employeeBindingModel);

            //serviceProviderBindingModel = serviceProviderService.GetAllServiceProviders().Select(x => new RegisterServiceProviderBindingModel()
            //{
            //    CompanyName = CommonFunctions.ReadResourceValue(x.CompanyName),
            //    Description = CommonFunctions.ReadResourceValue(x.Description),
            //    GeneralManagerName = CommonFunctions.ReadResourceValue(x.GeneralManagerName),
            //    IsActive=x.IsActive,
            //    ServiceProviderId=x.ServiceProviderId,

            //}).ToList();
            return employeeBindingModel;
        }
        #endregion


        #region Get Employee BY Id
        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(string id)
        {
            //Get employee
            EmployeeModel employeeModel = new EmployeeModel();
            EmployeeBindingModel employeeBindingModel = new EmployeeBindingModel();
            List<JobModel> jobModel = new List<JobModel>();
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            employeeModel = employeeService.GetEmployeeById(id);
            AutoMapper.Mapper.Map(employeeModel, employeeBindingModel);
            //Get Details of employee from user table
            var Email = UserManager.FindById(id) != null ? UserManager.FindById(id).Email : "";
            var PhoneNumber = UserManager.FindById(id) != null ? UserManager.FindById(id).PhoneNumber : "";
            //Get Car Type of employee
            CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
            carEmployeeMappingModel = carEmployeeMappingService.GetCarEmployeeMappingByEmployeeId(id);
            //Get Location of employee
            userLocationModel = userLocationService.FindLocationById(id);
            employeeBindingModel.UserLocationModel = userLocationModel;
            employeeBindingModel.CarTypeId = carEmployeeMappingModel.CarTypeId;
            employeeBindingModel.Email = Email;
            employeeBindingModel.PhoneNumber = PhoneNumber;
            //Get rating for employee
            jobModel = jobService.GetJobListByEmployeeId(id);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            List<RatingModel> ratingModel = new List<RatingModel>();
            ratingModel = ratingService.GetAllRatings();
            jobBindingModel.ForEach(x =>
            {
                x.RatingModel = ratingModel.Where(j => j.JobId == x.JobId).ToList();
            });
            employeeBindingModel.JobBindingModel = jobBindingModel;
            var serviceProvider = serviceProviderService.GetServiceProviderById(id);
            if (serviceProvider != null)
            {
                employeeBindingModel.model = serviceProvider;
            }
            return Json(employeeBindingModel);
        }
        #endregion

        #region Get Employee BY ServiceProviderId
        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        [Route("GetEmployeeByServiceProviderId")]
        public IHttpActionResult GetEmployeeByServiceProviderId(string id, int jobId)
        {
            //get service provider employee mapping 
            List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
            serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.GetEmployeeByServiceProviderId(id);
            //Get employee
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            List<EmployeeBindingModel> employeeBindingModel = new List<EmployeeBindingModel>();
            string[] employeeId = serviceProviderEmployeeMappingModel.Select(u => u.EmployeeId.ToString()).ToArray();
            employeeModel = employeeService.GetEmployeeListByServiceProviderId(employeeId);
            AutoMapper.Mapper.Map(employeeModel, employeeBindingModel);
            //get job according to employee
            JobModel jobModel = new JobModel();
            foreach (var item in employeeBindingModel)
            {
                jobModel = jobService.GetJobById(jobId);
                //int count = jobModel.Where(x => JobStatus.Current.ToString().Contains(x.Status.ToString())).Count(); //jobService.GetStatusOfEmployee();
                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByEmployeeId(item.EmployeeId).FirstOrDefault();
                JobModel assignedJobModel = new JobModel();
                if (employeeScheduleModel != null)
                {
                    assignedJobModel = jobService.GetJobById(employeeScheduleModel.JobId);
                }

                if (employeeScheduleModel != null)
                {
                    if (jobModel.StartDate > employeeScheduleModel.Start)
                    {
                        if (employeeScheduleModel.Start < jobModel.StartDate && employeeScheduleModel.End > jobModel.StartDate)
                        {
                            //if (assignedJobModel.Status == JobStatus.Current)
                            //{
                            //    item.EmployeeStatus = EmployeeStatus.Working;
                            //}
                            //else
                            //{
                            //    item.EmployeeStatus = EmployeeStatus.Available;
                            //}
                            item.EmployeeStatus = EmployeeStatus.Working;
                        }
                        else
                        {
                            item.EmployeeStatus = EmployeeStatus.Available;
                        }
                    }
                    else
                    {
                        TimeSpan duration = Convert.ToDateTime(employeeScheduleModel.Start) - Convert.ToDateTime(jobModel.StartDate);
                        jobModel.StartDate = Convert.ToDateTime(jobModel.StartDate).AddHours(duration.Hours).AddMinutes(duration.Minutes);
                        if (employeeScheduleModel.Start <= jobModel.StartDate && employeeScheduleModel.Start <= jobModel.EndDate)
                        {
                            //if (assignedJobModel.Status == JobStatus.Current)
                            //{
                            //    item.EmployeeStatus = EmployeeStatus.Working;
                            //}
                            //else
                            //{
                            //    item.EmployeeStatus = EmployeeStatus.Available;
                            //}
                            item.EmployeeStatus = EmployeeStatus.Working;
                        }
                        else
                        {
                            item.EmployeeStatus = EmployeeStatus.Available;
                        }
                    }
                    //if (jobModel.StartDate != employeeScheduleModel.Start && jobModel.EndDate != employeeScheduleModel.End)
                    //{
                    //    item.EmployeeStatus = EmployeeStatus.Available;
                    //}
                    //else
                    //{
                    //    item.EmployeeStatus = EmployeeStatus.Working;
                    //}
                }
                else
                {
                    item.EmployeeStatus = EmployeeStatus.Available;
                }



            }


            return Ok(employeeBindingModel);
        }
        #endregion

        #region Update Employee
        // PUT: api/Employees
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutEmployee()
        {

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
                var model = resultModel.FormData["model"];

                EmployeeBindingModel employeeBindingModel = new EmployeeBindingModel();
                employeeBindingModel = JsonConvert.DeserializeObject<EmployeeBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                else
                {

                    EmployeeModel employeeModelForCheckRecord = new EmployeeModel();
                    employeeModelForCheckRecord = employeeService.GetEmployeeById(employeeBindingModel.EmployeeId);

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

                                    file.SaveAs(Path.Combine(root, Utility.Constants.EMPLOYEE_PROFILE_PATH, fileName));
                                    employeeBindingModel.ProfileImage = fileName;
                                }
                                else
                                {
                                    file.SaveAs(Path.Combine(root, Utility.Constants.NATIONAL_ID_PATH, fileName));
                                    employeeBindingModel.NationalIdAndIqamaNumber = fileName;
                                }

                            }

                        }


                    }
                    if (employeeBindingModel.ProfileImage == null)
                    {
                        employeeBindingModel.ProfileImage = employeeModelForCheckRecord.ProfileImage;
                    }
                    if (employeeBindingModel.NationalIdAndIqamaNumber == null)
                    {
                        employeeBindingModel.NationalIdAndIqamaNumber = employeeModelForCheckRecord.NationalIdAndIqamaNumber;
                    }
                    //if (employeeBindingModel.IqamaNumber == null)
                    //{
                    //    employeeBindingModel.IqamaNumber = employeeModelForCheckRecord.IqamaNumber;
                    //}
                    try
                    {
                        //Update employee
                        EmployeeModel employeeModelEntity = new EmployeeModel();
                        AutoMapper.Mapper.Map(employeeBindingModel, employeeModelEntity);
                        employeeModelEntity = employeeService.UpadteEmployee(employeeModelEntity);
                        AutoMapper.Mapper.Map(employeeModelEntity, employeeBindingModel);
                        //Add Car type for Employee
                        if (employeeBindingModel.CarTypeId != 0)
                        {
                            CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
                            carEmployeeMappingModel.CarTypeId = employeeBindingModel.CarTypeId;
                            carEmployeeMappingModel.EmployeeId = employeeBindingModel.EmployeeId;
                            CarEmployeeMappingModel carEmployeeMappingModelCheck = new CarEmployeeMappingModel();
                            carEmployeeMappingModelCheck = carEmployeeMappingService.GetCarEmployeeMappingByEmployeeId(carEmployeeMappingModel.EmployeeId);
                            carEmployeeMappingModelCheck.CarTypeId = employeeBindingModel.CarTypeId;
                            carEmployeeMappingModelCheck.EmployeeId = employeeBindingModel.EmployeeId;
                            if (carEmployeeMappingModelCheck.CarEmployeeMappingId == 0)
                            {
                                carEmployeeMappingModel = carEmployeeMappingService.SaveCarEmployeeMapping(carEmployeeMappingModelCheck);
                            }
                            else
                            {
                                carEmployeeMappingModel = carEmployeeMappingService.UpadteCarEmployeeMapping(carEmployeeMappingModelCheck);
                            }
                        }


                        //save location for employee
                        var location = resultModel.FormData["LocationModel"];
                        UserLocationModel locationModel = new UserLocationModel();
                        UserLocationModel userlocationModel = new UserLocationModel();
                        locationModel = JsonConvert.DeserializeObject<UserLocationModel>(location);
                        locationModel.UserId = employeeBindingModel.EmployeeId;
                        locationModel.DistrictId = 1;
                        bool Exists;
                        Exists = userLocationService.CheckExistance(locationModel.UserId);
                        if (!Exists && locationModel.CityId != 0)
                        {
                            userLocationService.InsertUserLocation(locationModel);
                        }
                        else if (Exists)
                        {
                            userlocationModel = userLocationService.FindLocationById(locationModel.UserId);
                            locationModel.UserLocationId = userlocationModel.UserLocationId;
                            userLocationService.UpadteUserLocation(locationModel);
                        }
                        //check service provider as individual and update service provider
                        IndividualBindingModel individualBindingModel = new IndividualBindingModel();
                        RegisterServiceProviderBindingModel serviceProviderBindingModel = new RegisterServiceProviderBindingModel();
                        ServiceProviderModel serviceProvider = new ServiceProviderModel();
                        serviceProvider = serviceProviderService.GetServiceProviderById(employeeBindingModel.EmployeeId);
                        if (serviceProvider != null && serviceProvider.ServiceProviderType == ServiceProviderType.Individual)
                        {

                            serviceProvider.CompanyLogoPath = employeeBindingModel.ProfileImage;
                            serviceProvider.RegistrationCertificatePath = employeeBindingModel.NationalIdAndIqamaNumber;
                            serviceProvider.Description = employeeBindingModel.Description;

                            serviceProvider = serviceProviderService.UpadteServiceProvider(serviceProvider);
                        }
                        var user = await UserManager.FindByIdAsync(employeeBindingModel.EmployeeId);
                        if (user != null)
                        {
                            if (user.Email != employeeBindingModel.Email || user.PhoneNumber != employeeBindingModel.PhoneNumber)
                            {
                                user.Email = employeeBindingModel.Email;
                                user.PhoneNumber = employeeBindingModel.PhoneNumber;
                                IdentityResult result = await UserManager.UpdateAsync(user);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        return BadRequest(ex.Message);
                    }

                }

                return Ok();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Save Employee
        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> PostEmployee()
        {
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

                var model = resultModel.FormData["model"];
                EmployeeBindingModel employeeModel = new EmployeeBindingModel();
                employeeModel = JsonConvert.DeserializeObject<EmployeeBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = employeeModel.Email, Email = employeeModel.Email, PhoneNumber = employeeModel.PhoneNumber, DeviceType = employeeModel.DeviceType, DeviceToken = employeeModel.DeviceToken };

                IdentityResult result = await UserManager.CreateAsync(user, employeeModel.Password);

                if (!result.Succeeded)
                {
                    //UserManager.Delete(user);
                    return GetErrorResult(result);
                }

                else
                {

                    IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.Employee.ToString());
                    if (!resultRoleCreated.Succeeded)
                    {
                        //UserManager.Delete(user);
                        return GetErrorResult(resultRoleCreated);
                    }
                    else
                    {
                        try
                        {
                            //Save employee
                            employeeModel.EmployeeId = user.Id;
                            EmployeeModel employeeModelEntity = new EmployeeModel();
                            AutoMapper.Mapper.Map(employeeModel, employeeModelEntity);
                            employeeModelEntity = employeeService.SaveEmployee(employeeModelEntity);
                            AutoMapper.Mapper.Map(employeeModelEntity, employeeModel);
                            //ServiceProviderEmployee Mapping
                            ServiceProviderEmployeeMappingModel serviceProviderEmployeeMappingModel = new ServiceProviderEmployeeMappingModel();
                            serviceProviderEmployeeMappingModel.EmployeeId = employeeModel.EmployeeId;
                            if (employeeModel.ServiceProviderId != null)
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = employeeModel.ServiceProviderId;
                            }
                            else
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = User.Identity.GetUserId();
                            }
                            ServiceProviderModel serviceProviderModel = serviceProviderService.GetServiceProviderById(serviceProviderEmployeeMappingModel.ServiceProviderId);
                            if (serviceProviderModel.CompanyName != null)
                                serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.SaveEmployeeAccordingToServiceProvider(serviceProviderEmployeeMappingModel);
                            var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
                            var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                            var scheme = HttpContext.Current.Request.Url.Scheme;
                            var host = HttpContext.Current.Request.Url.Host;
                            var port = HttpContext.Current.Request.Url.Port;
                            //var link = scheme + "//" + host + port + "/#/confirmemail";
                            //var Link = "<a href='" + link + "/" + user.Id + "'></a>";
                            string language = "en";
                            var cookie = HttpContext.Current.Request.Cookies.Get("APPLICATION_LANGUAGE");
                            if (cookie != null)
                                language = cookie.Value;
                            string exactPath;
                            if (language == "en")
                            {
                                // exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/logo.png";
                                exactPath = scheme + "://" + host + "/Content/URFXTheme/images/logo.png";
                            }
                            else
                            {
                                //exactPath = scheme + "://" + host + port + "/Content/URFXTheme/images/arabic-logo.png";
                                exactPath = scheme + "://" + host + "/Content/URFXTheme/images/arabic-logo.png";
                            }
                            string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.EMPLOYEE_USERNAME_PASSWORD_PATH));
                            String Body = "";
                            Body = String.Format(text, user.UserName, employeeModel.Password, exactPath);
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
                        catch (Exception ex)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                            return BadRequest(ex.Message);
                        }


                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }



        }

        #endregion

        #region Paging For Employee
        [HttpPost]
        [Route("Paging")]
        public ResponseMessage Paging(PagingModel model)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            employeeModel = employeeService.Paging(model);
            //var ids = employeeModel.Select(x => x.EmployeeId).ToList();
            ServiceProviderEmployeeMappingService service = new ServiceProviderEmployeeMappingService();

            responseMessage.totalRecords = employeeModel.Count();
            employeeModel = employeeModel.OrderBy(x => x.FirstName).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            employeeModel = service.GetServiceProviderByEmployeeId(employeeModel);
            responseMessage.data = employeeModel;

            return responseMessage;
        }
        #endregion


        #region Delete Employees
        // DELETE: api/Employees/5
        [HttpDelete]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(string id)
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                if (employeeModel == null)
                {
                    return NotFound();
                }
                List<UserLocationModel> model = new List<UserLocationModel>();
                model = userLocationService.FindLocationListById(id);
                foreach (var item in model)
                {
                    userLocationService.DeleteUserLocation(item.UserLocationId);
                }

                serviceProviderEmployeeMappingService.DeleteServiceProviderEmployeeMapping(id);
                //delete Car Type of employee
                List<CarEmployeeMappingModel> carEmployeeMappingModel = new List<CarEmployeeMappingModel>();
                carEmployeeMappingModel = carEmployeeMappingService.GetCarEmployeeMappingListByEmployeeId(id);
                carEmployeeMappingModel.ForEach(x =>
                {
                    carEmployeeMappingService.DeleteCarEmployeeMapping(x.CarEmployeeMappingId);
                });

                employeeService.DeleteEmployee(id);
                ServiceProviderModel serviceProvider = new ServiceProviderModel();
                serviceProvider = serviceProviderService.GetServiceProviderById(id);
                if (serviceProvider.ServiceProviderId != null)
                {

                    //delete service provider service mapping
                    List<ServiceProviderServiceMappingModel> serviceProviderMappingModel = new List<ServiceProviderServiceMappingModel>();
                    serviceProviderMappingModel = serviceProviderServiceMappingService.FindServiceByUserId(id);
                    foreach (var item in serviceProviderMappingModel)
                    {
                        serviceProviderServiceMappingService.DeleteServiceProviderServiceMapping(item.Id);
                    }
                    //Delete service provider employee mapping
                    List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
                    serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.GetEmployeeByServiceProviderId(id);
                    foreach (var item in serviceProviderEmployeeMappingModel)
                    {
                        serviceProviderEmployeeMappingService.DeleteServiceProviderEmployeeMapping(item.EmployeeId);
                    }
                    //Delete service provider
                    serviceProviderService.DeleteServiceProvider(id);

                }
                ApplicationUser user = UserManager.FindById(id);

                IdentityResult result = UserManager.Delete(user);
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

        #region Delete Selected Employees 
        // DELETE: api/Employees/DeleteSelectedServiceProvider

        [ResponseType(typeof(Employee))]
        [Route("DeleteSelectedEmployees")]
        public ResponseMessage DeleteSelectedEmployees(List<string> employees)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            try
            {

                foreach (var i in employees)
                {
                    var id = i;
                    if (id != null)
                    {

                        List<UserLocationModel> model = new List<UserLocationModel>();
                        model = userLocationService.FindLocationListById(id);
                        foreach (var item in model)
                        {
                            userLocationService.DeleteUserLocation(item.UserLocationId);
                        }

                        serviceProviderEmployeeMappingService.DeleteServiceProviderEmployeeMapping(id);
                        //var job = jobService.GetJobListByEmployeeId(id);
                        //job.ForEach(x =>
                        //{
                        //    jobService.DeleteJob(x.JobId);
                        //});
                        employeeService.DeleteEmployee(id);
                        var serviceProvider = serviceProviderService.GetServiceProviderById(id);
                        if (serviceProvider.CompanyName != null)
                            serviceProviderService.DeleteServiceProvider(id);

                        ApplicationUser user = UserManager.FindById(id);

                        IdentityResult result = UserManager.Delete(user);

                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                responseMessage.Message = ex.Message.ToString();
                return responseMessage;
            }
            employeeModel = employeeService.GetAllEmployees().ToList();
            responseMessage.totalRecords = employeeModel.Count();
            responseMessage.Message = "Employees has been deleted successfully.";
            return responseMessage;
        }
        #endregion

        #region My Jobs
        // POST: api/Employees/MyJobs
        [ResponseType(typeof(Employee))]
        [HttpGet]
        [Route("MyJobs")]
        public IHttpActionResult MyJobs(string userId, string type)
        {
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            try
            {
                //get job according to user id
                List<JobModel> jobModel = new List<JobModel>();
                JobStatus jobStatus = (JobStatus)Enum.Parse(typeof(JobStatus), type);
                jobModel = jobService.GetJobsByEmployeeId(userId, jobStatus);
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
                //get client inforamtion
                jobBindingModel.ForEach(x =>
                {
                    //Get Client Name
                    ClientModel clientModel = new ClientModel();
                    clientModel = clientService.GetClientById(x.ClientId);
                    ApplicationUser user = UserManager.FindById(clientModel.ClientId);
                    if (user != null)
                    {
                        clientModel.PhoneNumber = user.PhoneNumber;
                    }
                    x.ClientModel = clientModel;
                    userLocationModel = userLocationService.FindLocationById(x.ClientId);
                    if (x.JobAddress == null || x.JobAddress == "")
                    {
                        x.JobAddress = userLocationModel.Address;
                        x.Latitude = userLocationModel.Latitude;
                        x.Longitude = userLocationModel.Latitude;
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


        #region Register Service Provider As Individual for app
        // POST: api/Employees/RegisterServiceProviderAsIndividual
        [ResponseType(typeof(Employee))]
        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterServiceProviderAsIndividual")]
        public async Task<IHttpActionResult> RegisterServiceProviderAsIndividual(EmployeeBindingModel employeeModel)
        {
            //using (var dataContext = new URFXDbContext())
            //{
            //    TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //    {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = employeeModel.Email, Email = employeeModel.Email, PhoneNumber = employeeModel.PhoneNumber, DeviceType = employeeModel.DeviceType, DeviceToken = employeeModel.DeviceToken };

                IdentityResult result = await UserManager.CreateAsync(user, employeeModel.Password);

                if (!result.Succeeded)
                {
                    //  transaction.Dispose();
                    ////UserManager.Delete(user);
                    return GetErrorResult(result);
                }

                else
                {
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
                            //save service provider
                            RegisterServiceProviderBindingModel serviceProviderBindingModel = new RegisterServiceProviderBindingModel();
                            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();

                            serviceProviderBindingModel.ServiceProviderId = user.Id;
                            serviceProviderBindingModel.StartDate = DateTime.UtcNow;
                            serviceProviderBindingModel.CompanyName = employeeModel.FirstName + " " + employeeModel.LastName;
                            serviceProviderBindingModel.Description = employeeModel.Description;
                            serviceProviderBindingModel.GeneralManagerName = employeeModel.FirstName + " " + employeeModel.LastName;
                            serviceProviderBindingModel.ServiceProviderType = ServiceProviderType.Individual;
                            AutoMapper.Mapper.Map(serviceProviderBindingModel, serviceProviderModel);
                            serviceProviderModel = serviceProviderService.SaveServiceProvider(serviceProviderModel);

                            employeeModel.EmployeeId = user.Id;
                            EmployeeModel employeeModelEntity = new EmployeeModel();
                            AutoMapper.Mapper.Map(employeeModel, employeeModelEntity);
                            employeeModelEntity = employeeService.SaveEmployee(employeeModelEntity);
                            AutoMapper.Mapper.Map(employeeModelEntity, employeeModel);



                            //ServiceProviderEmployee Mapping
                            ServiceProviderEmployeeMappingModel serviceProviderEmployeeMappingModel = new ServiceProviderEmployeeMappingModel();
                            serviceProviderEmployeeMappingModel.EmployeeId = employeeModel.EmployeeId;
                            if (employeeModel.ServiceProviderId != null)
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = employeeModel.ServiceProviderId;
                            }
                            else
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = employeeModel.EmployeeId;
                            }
                            serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.SaveEmployeeAccordingToServiceProvider(serviceProviderEmployeeMappingModel);
                            //send email
                            var Subject = Utility.Constants.CONFIRMATION_SUBJECT;
                            var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                            var scheme = HttpContext.Current.Request.Url.Scheme;
                            var host = HttpContext.Current.Request.Url.Host;
                            var port = HttpContext.Current.Request.Url.Port;
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
#if DEBUG
                            var link = scheme + "://" + host + port + "/App/#/confirmemail/" + user.Id + "";
#else
                            var link = scheme + "://" + host + "/#/confirmemail/" + user.Id + "";
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
                                //  transaction.Dispose();
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
                return Json(employeeModel);
                // return Ok("{\"response\":" + Json(employeeModel) + "}");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }

            //    }
            //}

        }

        #endregion

        #region Update Service Provider As Individual for App
        // PUT: api/Employees/UpdateServiceProviderAsIndividual
        [ResponseType(typeof(void))]
        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateServiceProviderAsIndividual")]
        public async Task<IHttpActionResult> UpdateServiceProviderAsIndividual()
        {

            try
            {
                //var userAgent = HttpContext.Current.Request.UserAgent;
                //var deviceType = HttpContext.Current.Request.Headers.GetValues("device_type");
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
                EmployeeBindingModel employeeBindingModel = new EmployeeBindingModel();
                //var model = result.Substring(1, result.Length - 2);
                employeeBindingModel = JsonConvert.DeserializeObject<EmployeeBindingModel>(result);
                //AutoMapper.Mapper.Map(employeeBindingModel, clientModel);
                //var count = resultModel.FormData.Count;

                //employeeBindingModel.EmployeeId = resultModel.FormData[0];
                //employeeBindingModel.FirstName = resultModel.FormData[1];
                //employeeBindingModel.LastName = resultModel.FormData[2];
                //employeeBindingModel.Description = resultModel.FormData[3];
                //employeeBindingModel.CityId = Convert.ToInt32(resultModel.FormData[4]);
                //employeeBindingModel.DistrictId = Convert.ToInt32(resultModel.FormData[5]);
                //employeeBindingModel.CarTypeId = Convert.ToInt32(resultModel.FormData[6]);
                //employeeBindingModel.LicensePlateNumber = resultModel.FormData[7];

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                else
                {
                    try
                    {
                        EmployeeModel employeeModelForCheckRecord = new EmployeeModel();
                        employeeModelForCheckRecord = employeeService.GetEmployeeById(employeeBindingModel.EmployeeId);

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

                                        file.SaveAs(Path.Combine(root, Utility.Constants.EMPLOYEE_PROFILE_PATH, fileName));
                                        employeeBindingModel.ProfileImage = fileName;
                                    }
                                    else
                                    {
                                        file.SaveAs(Path.Combine(root, Utility.Constants.NATIONAL_ID_PATH, fileName));
                                        employeeBindingModel.NationalIdAndIqamaNumber = fileName;
                                    }

                                }

                            }


                        }
                        if (employeeBindingModel.ProfileImage == null)
                        {
                            employeeBindingModel.ProfileImage = employeeModelForCheckRecord.ProfileImage;
                        }
                        if (employeeBindingModel.NationalIdAndIqamaNumber == null)
                        {
                            employeeBindingModel.NationalIdAndIqamaNumber = employeeModelForCheckRecord.NationalIdAndIqamaNumber;
                        }

                        //Update employee
                        EmployeeModel employeeModelEntity = new EmployeeModel();
                        AutoMapper.Mapper.Map(employeeBindingModel, employeeModelEntity);
                        employeeModelEntity = employeeService.UpadteEmployee(employeeModelEntity);
                        AutoMapper.Mapper.Map(employeeModelEntity, employeeBindingModel);
                        //Add Car type for Employee
                        CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
                        carEmployeeMappingModel.CarTypeId = employeeBindingModel.CarTypeId;
                        carEmployeeMappingModel.EmployeeId = employeeBindingModel.EmployeeId;
                        carEmployeeMappingModel = carEmployeeMappingService.SaveCarEmployeeMapping(carEmployeeMappingModel);
                        //save location for employee

                        UserLocationModel locationModel = new UserLocationModel();
                        locationModel.UserId = employeeBindingModel.EmployeeId;
                        locationModel.CityId = employeeBindingModel.CityId;
                        locationModel.DistrictId = 1;
                        userLocationService.InsertUserLocation(locationModel);
                        //locationModel = userLocationService.FindLocationById(employeeBindingModel.EmployeeId);
                        //if (locationModel.UserLocationId != 0)
                        //{
                        //    userLocationService.UpadteUserLocation(locationModel);
                        //}


                        var user = await UserManager.FindByIdAsync(employeeBindingModel.EmployeeId);
                        if (user.Email != employeeBindingModel.Email)
                        {
                            IdentityResult userResult = await UserManager.UpdateAsync(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        return BadRequest(ex.Message);
                    }

                }

                return Json(employeeBindingModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Register Service Provider As Individual for web
        // POST: api/Employees/RegisterServiceProviderAsIndividual
        [ResponseType(typeof(Employee))]
        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterServiceProviderAsAnIndividual")]
        public async Task<IHttpActionResult> RegisterServiceProviderAsAnIndividual()
        {
            //using (var dataContext = new URFXDbContext())
            //{
            //    var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
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

                var model = resultModel.FormData["model"];
                EmployeeBindingModel employeeModel = new EmployeeBindingModel();
                employeeModel = JsonConvert.DeserializeObject<EmployeeBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser() { UserName = employeeModel.Email, Email = employeeModel.Email, PhoneNumber = employeeModel.PhoneNumber, DeviceType = employeeModel.DeviceType, DeviceToken = employeeModel.DeviceToken };

                IdentityResult result = await UserManager.CreateAsync(user, employeeModel.Password);

                if (!result.Succeeded)
                {

                    // transaction.Dispose();
                    return BadRequest("{\"status\" : false, \"message\" : \"Email is already taken\"}");
                }

                else
                {
                    //transaction.Complete();
                    IdentityResult resultRoleCreated = await UserManager.AddToRoleAsync(user.Id, URFXRoles.ServiceProvider.ToString());
                    if (!resultRoleCreated.Succeeded)
                    {
                        // transaction.Dispose();

                        return GetErrorResult(resultRoleCreated);
                    }
                    else
                    {
                        try
                        {

                            //save service provider
                            RegisterServiceProviderBindingModel serviceProviderBindingModel = new RegisterServiceProviderBindingModel();
                            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();

                            serviceProviderBindingModel.ServiceProviderId = user.Id;
                            serviceProviderBindingModel.StartDate = DateTime.UtcNow;
                            serviceProviderBindingModel.CompanyName = employeeModel.FirstName + " " + employeeModel.LastName;
                            serviceProviderBindingModel.Description = employeeModel.Description;
                            serviceProviderBindingModel.GeneralManagerName = employeeModel.FirstName + " " + employeeModel.LastName;
                            serviceProviderBindingModel.ServiceProviderType = ServiceProviderType.Individual;
                            AutoMapper.Mapper.Map(serviceProviderBindingModel, serviceProviderModel);
                            serviceProviderModel = serviceProviderService.SaveServiceProvider(serviceProviderModel);


                            //Save employee

                            //IdentityResult resultRoleCreatedForServiceProvider = await UserManager.AddToRoleAsync(user.Id, URFXRoles.ServiceProvider.ToString());
                            //if (!resultRoleCreatedForServiceProvider.Succeeded)
                            //{
                            //    return GetErrorResult(resultRoleCreatedForServiceProvider);
                            //}
                            employeeModel.EmployeeId = user.Id;
                            EmployeeModel employeeModelEntity = new EmployeeModel();
                            AutoMapper.Mapper.Map(employeeModel, employeeModelEntity);
                            employeeModelEntity = employeeService.SaveEmployee(employeeModelEntity);

                            AutoMapper.Mapper.Map(employeeModelEntity, employeeModel);
                            ServiceProviderEmployeeMappingModel serviceProviderEmployeeMappingModel = new ServiceProviderEmployeeMappingModel();
                            serviceProviderEmployeeMappingModel.EmployeeId = employeeModel.EmployeeId;
                            if (employeeModel.ServiceProviderId != null)
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = employeeModel.ServiceProviderId;
                            }
                            else
                            {
                                serviceProviderEmployeeMappingModel.ServiceProviderId = employeeModel.EmployeeId;
                            }
                            serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.SaveEmployeeAccordingToServiceProvider(serviceProviderEmployeeMappingModel);
                            //send email
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
#if DEBUG
                            var link = scheme + "://" + host + port + "/App/#/confirmemail/" + user.Id + "";
#else
                            var link = scheme + "://" + host + "/App/#/confirmemail/" + user.Id + "";
#endif
                            var Link = "<a href='" + link + "'>" + link + "</a>";
                            string text = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(Utility.Constants.CONFIRMATION_EMAIL_PATH));
                            String Body = "";
                            Body = String.Format(text, user.UserName, Link, user.Id, code, exactPath);
                            try
                            {

                                await UserManager.SendEmailAsync(user.Id, Subject, Body);
                            }
                            catch (Exception ex)
                            {
                                //  transaction.Dispose();
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

                return Ok(employeeModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                //transaction.Dispose();
                return BadRequest(ex.Message);
            }

            //}
            // }

        }

        #endregion

        #region Update Service Provider As Individual for web
        // PUT: api/Employees/UpdateServiceProviderAsIndividual
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("UpdateServiceProviderAsAnIndividual")]
        public async Task<IHttpActionResult> UpdateServiceProviderAsAnIndividual()
        {

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
                var model = resultModel.FormData["model"];

                EmployeeBindingModel employeeBindingModel = new EmployeeBindingModel();
                employeeBindingModel = JsonConvert.DeserializeObject<EmployeeBindingModel>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                else
                {
                    try
                    {
                        EmployeeModel employeeModelForCheckRecord = new EmployeeModel();
                        employeeModelForCheckRecord = employeeService.GetEmployeeById(employeeBindingModel.EmployeeId);

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

                                        file.SaveAs(Path.Combine(root, Utility.Constants.EMPLOYEE_PROFILE_PATH, fileName));
                                        employeeBindingModel.ProfileImage = fileName;
                                    }
                                    else
                                    {
                                        file.SaveAs(Path.Combine(root, Utility.Constants.NATIONAL_ID_PATH, fileName));
                                        employeeBindingModel.NationalIdAndIqamaNumber = fileName;
                                    }

                                }

                            }


                        }
                        if (employeeBindingModel.ProfileImage == null)
                        {
                            employeeBindingModel.ProfileImage = employeeModelForCheckRecord.ProfileImage;
                        }
                        if (employeeBindingModel.NationalIdAndIqamaNumber == null)
                        {
                            employeeBindingModel.NationalIdAndIqamaNumber = employeeModelForCheckRecord.NationalIdAndIqamaNumber;
                        }

                        //Update employee
                        EmployeeModel employeeModelEntity = new EmployeeModel();
                        AutoMapper.Mapper.Map(employeeBindingModel, employeeModelEntity);
                        employeeModelEntity = employeeService.UpadteEmployee(employeeModelEntity);
                        AutoMapper.Mapper.Map(employeeModelEntity, employeeBindingModel);
                        //Add Car type for Employee
                        CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
                        carEmployeeMappingModel.CarTypeId = employeeBindingModel.CarTypeId;
                        carEmployeeMappingModel.EmployeeId = employeeBindingModel.EmployeeId;
                        carEmployeeMappingModel = carEmployeeMappingService.SaveCarEmployeeMapping(carEmployeeMappingModel);
                        //save location for employee
                        var location = resultModel.FormData["LocationModel"];
                        UserLocationModel locationModel = new UserLocationModel();
                        locationModel = JsonConvert.DeserializeObject<UserLocationModel>(location);
                        locationModel.UserId = employeeBindingModel.EmployeeId;
                        userLocationService.InsertUserLocation(locationModel);
                        //locationModel = userLocationService.FindLocationById(employeeBindingModel.EmployeeId);
                        //if (locationModel.UserLocationId != 0)
                        //{
                        //    userLocationService.UpadteUserLocation(locationModel);
                        //}


                        var user = await UserManager.FindByIdAsync(employeeBindingModel.EmployeeId);
                        if (user.Email != employeeBindingModel.Email)
                        {
                            IdentityResult result = await UserManager.UpdateAsync(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        return BadRequest(ex.Message);
                    }

                }

                return Ok();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get All Car Types
        // POST: api/Employees/GetAllCarTypes
        [ResponseType(typeof(CarType))]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllCarTypes")]
        public IHttpActionResult GetAllCarTypes()
        {
            List<CarTypeBindingModel> carTypeBindingModelList = new List<CarTypeBindingModel>();
            List<CarTypeModel> carTypeModelList = new List<CarTypeModel>();
            try
            {

                carTypeModelList = carTypeService.GetAllCarTypes();
                AutoMapper.Mapper.Map(carTypeModelList, carTypeBindingModelList);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(carTypeBindingModelList);
        }
        #endregion

        #region Add Car Type
        // POST: api/Employees/AddCarType
        [ResponseType(typeof(CarType))]
        [HttpPost]
        [Route("AddCarType")]
        public IHttpActionResult AddCarType(CarTypeBindingModel model)
        {

            CarTypeModel carTypeModel = new CarTypeModel();
            AutoMapper.Mapper.Map(model, carTypeModel);
            try
            {

                carTypeModel = carTypeService.SaveCarType(carTypeModel);
                AutoMapper.Mapper.Map(carTypeModel, model);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(model);
        }
        #endregion

        #region Update Car Type
        // PUT: api/Employees/UpdateCarType
        [ResponseType(typeof(CarType))]
        [HttpPut]
        [Route("UpdateCarType")]
        public IHttpActionResult UpdateCarType(CarTypeBindingModel model)
        {

            CarTypeModel carTypeModel = new CarTypeModel();
            AutoMapper.Mapper.Map(model, carTypeModel);
            try
            {

                carTypeModel = carTypeService.UpadteCarType(carTypeModel);
                AutoMapper.Mapper.Map(carTypeModel, model);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(model);
        }
        #endregion

        #region Delete Car Type
        // DELETE: api/Employees/DeleteCarType
        [HttpDelete]
        [Route("DeleteCarType")]
        public IHttpActionResult DeleteCarType(int carTypeId)
        {
            try
            {
                carTypeService.DeleteCarType(carTypeId);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        #endregion

        #region Get Employee Schedule By EmployeeId
        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        [Route("GetEmployeeSchedule")]
        public IHttpActionResult GetEmployeeSchedule(string employeeId)
        {
            List<EmployeeScheduleModel> employeeScheduleModel = new List<EmployeeScheduleModel>();
            employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByEmployeeId(employeeId);
            var result = employeeScheduleModel.Select(e => new JsonEvent()
            {
                start = e.Start.ToString(),
                end = e.End.ToString(),
                text = e.Description,
                id = e.Id.ToString()
            }).ToList();
            return Ok(result);
        }
        #endregion

        #region Get Employee Schedule By Start and end time
        // GET: api/Employees/GetEmployeeScheduleByDateAndTime
        [ResponseType(typeof(Employee))]
        [HttpGet]
        [Route("GetEmployeeScheduleByDateAndTime")]
        public IHttpActionResult GetEmployeeScheduleByDateAndTime(DateTime? start, DateTime? end, string employeeId)
        {
            List<EmployeeScheduleModel> employeeScheduleModel = new List<EmployeeScheduleModel>();
            employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByDateAndTime(start, end, employeeId);
            var result = employeeScheduleModel.Select(e => new JsonEvent()
            {
                start = e.Start.ToString(),
                end = e.End.ToString(),
                text = e.Description,
                id = e.Id.ToString()
            }).ToList();
            return Ok(result);
        }
        #endregion

        #region Add schedule For Employee
        // GET: api/Employees/addscheduleForEmployee
        [ResponseType(typeof(Employee))]
        [Route("addscheduleForEmployee")]
        public IHttpActionResult addscheduleForEmployee(EmployeeScheduleBindingModel model)
        {
            try
            {
                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                AutoMapper.Mapper.Map(model, employeeScheduleModel);
                employeeScheduleModel = employeeScheduleService.SaveEmployeeSchedule(employeeScheduleModel);
                AutoMapper.Mapper.Map(employeeScheduleModel, model);
                ////Get job by jobId
                //JobModel jobModel = new JobModel();
                //jobModel = jobService.GetJobById(model.JobId);
                //jobModel.EmployeeId = model.EmployeeId;
                //jobModel = jobService.UpadteJob(jobModel);
                //ApplicationUser user = UserManager.FindById(model.EmployeeId);
                //if (user != null)
                //{
                //    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                //    {
                //        var response = sendNotificationService.SendNotificationForAndroid(user.DeviceToken, "This is your new task");
                //    }
                //    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                //    {
                //        sendNotificationService.SendNotificationForIOS(user.DeviceToken, "This is your new task");
                //    }
                //}
                return Ok(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Job Assigning Confirm
        // GET: api/Employees/Confirm
        [ResponseType(typeof(Employee))]
        [Route("Confirm")]
        public IHttpActionResult Confirm(string employeeId, int jobId)
        {
            try
            {

                //Get job by jobId
                JobModel jobModel = new JobModel();
                jobModel = jobService.GetJobById(jobId);
                jobModel.EmployeeId = employeeId;
                jobModel = jobService.UpadteJob(jobModel);
                ApplicationUser user = UserManager.FindById(employeeId);
                if (user != null)
                {
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + employeeId + "&data.jobId=" + jobId + "&data.type=" + Utility.Constants.JOB_ASSING_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_JOBASSIGN + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_JOBASSIGN + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + jobId + ",\"userId\":\"" + employeeId + "\",\"type\":\"" + Utility.Constants.JOB_ASSING_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }
                return Ok("Confirm");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Job Assigning Cancel
        // GET: api/Employees/Cancel
        [ResponseType(typeof(Employee))]
        [Route("Cancel")]
        public IHttpActionResult Cancel(string employeeId, int jobId)
        {
            try
            {

                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByEmployeeId(employeeId).FirstOrDefault();
                employeeScheduleService.DeleteEmployeeSchedule(employeeScheduleModel.Id);
                return Ok("Cancel");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update schedule For Employee
        // GET: api/Employees/addscheduleForEmployee
        [ResponseType(typeof(Employee))]
        [Route("UpdatescheduleForEmployee")]
        public IHttpActionResult UpdatescheduleForEmployee(EmployeeScheduleBindingModel model)
        {
            try
            {
                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                AutoMapper.Mapper.Map(model, employeeScheduleModel);
                employeeScheduleModel = employeeScheduleService.UpdateEmployeeSchedule(employeeScheduleModel);
                AutoMapper.Mapper.Map(employeeScheduleModel, model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete schedule For Employee
        // GET: api/Employees/DeleteEmployeeSchedule
        [ResponseType(typeof(Employee))]
        [Route("DeleteEmployeeSchedule")]
        public IHttpActionResult DeleteEmployeeSchedule(int id)
        {
            try
            {
                employeeScheduleService.DeleteEmployeeSchedule(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update Employee Location
        //Post:api/Employees/UpdateEmployeeLocation
        [ResponseType(typeof(UserLocationModel))]
        [HttpPost]
        [Route("UpdateEmployeeLocation")]
        public IHttpActionResult UpdateEmployeeLocation(UserLocationBindingModel model)
        {
            try
            {
                UserLocationModel userLocationModel = new UserLocationModel();
                userLocationModel = userLocationService.FindUserLocationById(model.UserId);
                model.UserLocationId = userLocationModel.UserLocationId;
                model.CityId = userLocationModel.CityId;
                model.DistrictId = userLocationModel.DistrictId;
                AutoMapper.Mapper.Map(model, userLocationModel);
                userLocationModel = userLocationService.UpadteUserLocation(userLocationModel);
                AutoMapper.Mapper.Map(userLocationModel, model);
                ApplicationUser user = UserManager.FindById(model.UserId);
                if (user != null)
                {
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.Latitude=" + model.Latitude + "&data.Longitude=" + model.Longitude + "&data.type=" + Utility.Constants.TRACKING_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_TRACKING + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_TRACKING + "" + "\",\"badge\":1,\"sound\":\"default\"},\"Latitude\":" + model.Latitude + ",\"Longitude\":\"" + model.Longitude + "\",\"type\":\"" + Utility.Constants.TRACKING_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update Employee For QuickBlox Id
        //Post:api/Employees/UpdateQuickBlox
        [ResponseType(typeof(UserLocationModel))]
        [HttpPost]
        [Route("UpdateQuickBlox")]
        public IHttpActionResult UpdateQuickBlox(string userId, string quickBloxId)
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                employeeModel = employeeService.GetEmployeeById(userId);
                employeeModel.QuickBloxID = quickBloxId;
                employeeModel.Registred = true;
                employeeModel = employeeService.UpadteEmployee(employeeModel);
                return Json(employeeModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Save Feedback For Employee
        // POST: api/Employees/SaveFeedback
        [ResponseType(typeof(Client))]
        [HttpPost]
        [Route("SaveFeedback")]
        public IHttpActionResult SaveFeedback(RatingBindingModel model)
        {
            try
            {
                RatingModel ratingModel = new RatingModel();
                AutoMapper.Mapper.Map(model, ratingModel);
                ratingModel = ratingService.SaveRating(ratingModel);
                AutoMapper.Mapper.Map(ratingModel, model);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(model);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
            return BadRequest();

        }
        private bool EmployeeExists(string id)
        {
            return db.Employee.Count(e => e.EmployeeId == id) > 0;
        }

        private IEnumerable<HttpPostedFileBase> EnumerateFiles()
        {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var f = new HttpPostedFileWrapper(HttpContext.Current.Request.Files[i]) as HttpPostedFileBase;
                if (f != null && f.ContentLength > 0)
                {
                    yield return f;
                }
            }
        }

    }
}