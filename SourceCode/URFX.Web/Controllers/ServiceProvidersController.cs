using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Web.Models;
using URFX.Web.Utility;
using System.Device;
using System.Device.Location;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/ServiceProviders")]
    public class ServiceProvidersController : ApiController
    {
        EmployeeService employeeService = new EmployeeService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        ServiceCategoryService serviceCategoryService = new ServiceCategoryService();
        UserLocationService locationService = new UserLocationService();
        CarEmployeeMappingService carEmployeeMappingService = new CarEmployeeMappingService();
        SubServicesService subService = new SubServicesService();
        UserPlanService userPlanService = new UserPlanService();
        PlanService planService = new PlanService();
        RatingService ratingService = new RatingService();
        JobService jobService = new JobService();
        ClientService clientService = new ClientService();
        ServiceProviderEmployeeMappingService serviceProviderEmployeeMappingService = new ServiceProviderEmployeeMappingService();
        ServiceProviderServiceMappingService serviceProviderServiceMappingService = new ServiceProviderServiceMappingService();

        private ApplicationUserManager _userManager;

        public ServiceProvidersController()
        {

        }
        public ServiceProvidersController(ApplicationUserManager userManager)
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

        #region Get All Service Providers
        // GET: api/ServiceProviders
        [ResponseType(typeof(ServiceProvider))]
        public List<RegisterServiceProviderBindingModel> GetServiceProvider()
        {
            List<RegisterServiceProviderBindingModel> serviceProviderBindingModel = new List<RegisterServiceProviderBindingModel>();
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            serviceProviderList = serviceProviderService.GetAllServiceProviders().ToList();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProviderBindingModel);

            //serviceProviderBindingModel = serviceProviderService.GetAllServiceProviders().Select(x => new RegisterServiceProviderBindingModel()
            //{
            //    CompanyName = CommonFunctions.ReadResourceValue(x.CompanyName),
            //    Description = CommonFunctions.ReadResourceValue(x.Description),
            //    GeneralManagerName = CommonFunctions.ReadResourceValue(x.GeneralManagerName),
            //    IsActive=x.IsActive,
            //    ServiceProviderId=x.ServiceProviderId,
                
            //}).ToList();
            return serviceProviderBindingModel;
        }
        #endregion

        #region Get All Service Providers As individual
        // GET: api/ServiceProviders/GetServiceProvidersAsIndividual
        [ResponseType(typeof(ServiceProvider))]
        [Route("GetServiceProvidersAsIndividual")]
        public List<RegisterServiceProviderBindingModel> GetServiceProvidersAsIndividual()
        {
            List<RegisterServiceProviderBindingModel> serviceProviderBindingModel = new List<RegisterServiceProviderBindingModel>();
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            serviceProviderList = serviceProviderService.GetAllServiceProvidersAsIndividual();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProviderBindingModel);
            foreach (var item in serviceProviderBindingModel)
            {
                List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
                serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.GetEmployeeByServiceProviderId(item.ServiceProviderId);
                List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
                foreach (var employeeMapping in serviceProviderEmployeeMappingModel)
                {
                    EmployeeModel empmodel = new EmployeeModel();
                    var EmployeeEmail = UserManager.FindById(employeeMapping.EmployeeId).Email;
                    empmodel = employeeService.GetEmployeeById(employeeMapping.EmployeeId);
                    empmodel.Email = EmployeeEmail;
                    employeeModelList.Add(empmodel);
                    
                }
                item.EmployeesList = employeeModelList;
                List<ServiceProviderServiceMappingModel> servicesList = new List<ServiceProviderServiceMappingModel>();
                List<ServiceProviderServiceMappingBindingModel> serviceBindingModel = new List<ServiceProviderServiceMappingBindingModel>();
                RegisterServiceProviderBindingModel serviceProviderModel = new RegisterServiceProviderBindingModel();
                
                var Email = UserManager.FindById(item.ServiceProviderId).Email;
                var PhoneNumber = UserManager.FindById(item.ServiceProviderId).PhoneNumber;
                UserLocationModel model = locationService.FindLocationById(item.ServiceProviderId);
                item.UserLocationModel = model;
              
                servicesList = serviceProviderServiceMappingService.FindServiceByUserId(item.ServiceProviderId);
                AutoMapper.Mapper.Map(servicesList, serviceBindingModel);
                SubServiceModel serviceModelList = new SubServiceModel();
                foreach (var service in serviceBindingModel)
                {

                    serviceModelList = subService.GetAllCategoryServiceByServiceId(service.ServiceId);
                    //AutoMapper.Mapper.Map(serviceModelList, serviceBindingModel);
                    service.CategoryId = Convert.ToInt32(serviceModelList.ServiceCategoryId);
                }
                item.ServicesList = servicesList;
                List<JobModel> jobModelList = new List<JobModel>();
                jobModelList = jobService.GetJobListByServiceProviderId(item.ServiceProviderId);
                foreach(var job in jobModelList)
                {
                    List<RatingModel> ratingModelList = new List<RatingModel>();
                    ratingModelList = ratingService.GetRatingListByServiceProviderId(job.JobId);
                    item.RatingModelList = ratingModelList;
                }
                
                
                item.Email = Email;
                item.PhoneNumber = PhoneNumber;
                
                
            }
            
            
            //serviceProviderBindingModel = serviceProviderService.GetAllServiceProviders().Select(x => new RegisterServiceProviderBindingModel()
            //{
            //    CompanyName = CommonFunctions.ReadResourceValue(x.CompanyName),
            //    Description = CommonFunctions.ReadResourceValue(x.Description),
            //    GeneralManagerName = CommonFunctions.ReadResourceValue(x.GeneralManagerName),
            //    IsActive=x.IsActive,
            //    ServiceProviderId=x.ServiceProviderId,

            //}).ToList();
            

            return serviceProviderBindingModel;
        }
        #endregion

        #region Get Service Provider BY Id
        // GET: api/ServiceProviders/5
        [ResponseType(typeof(ServiceProvider))]
        public IHttpActionResult GetServiceProvider(string id)
        {

            //Get Deatils of service provider
            IndividualBindingModel individualBindingModel = new IndividualBindingModel();
            RegisterServiceProviderBindingModel serviceProviderBindingModel = new RegisterServiceProviderBindingModel();
            ServiceProviderModel serviceProvider = new ServiceProviderModel();
            var userName = UserManager.FindById(id) != null ? UserManager.FindById(id).UserName : "";
            var Email = UserManager.FindById(id) != null ? UserManager.FindById(id).Email : "";
            var PhoneNumber = UserManager.FindById(id) != null ? UserManager.FindById(id).PhoneNumber : "";
            serviceProvider = serviceProviderService.GetServiceProviderById(id);
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderBindingModel);

            //Get employee of service provider
            List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
            serviceProviderEmployeeMappingModel = serviceProviderEmployeeMappingService.GetEmployeeByServiceProviderId(id);
            List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
            foreach(var item in serviceProviderEmployeeMappingModel)
            {
                EmployeeModel empmodel = new EmployeeModel();
                var EmployeeEmail = UserManager.FindById(item.EmployeeId) != null ? UserManager.FindById(item.EmployeeId).Email : "";
                empmodel = employeeService.GetEmployeeById(item.EmployeeId);
                empmodel.Email = EmployeeEmail;
                employeeModelList.Add(empmodel);

            }
            //Get Location of service provider
            UserLocationModel model = locationService.FindLocationById(id);
            
            //Get services for service provider
            List<ServiceProviderServiceMappingModel> servicesList = new List<ServiceProviderServiceMappingModel>();
            List<ServiceProviderServiceMappingBindingModel> serviceBindingModel = new List<ServiceProviderServiceMappingBindingModel>();
            servicesList = serviceProviderServiceMappingService.FindServiceByUserId(id);
            AutoMapper.Mapper.Map(servicesList, serviceBindingModel);
            SubServiceModel serviceModelList = new SubServiceModel();
            foreach(var item in serviceBindingModel)
            {

                serviceModelList = subService.GetAllCategoryServiceByServiceId(item.ServiceId);
                //AutoMapper.Mapper.Map(serviceModelList, serviceBindingModel);
                item.CategoryId = Convert.ToInt32(serviceModelList.ServiceCategoryId);
            }
            //Get Rating of service provider according to Job
            List<JobModel> jobModelList = new List<JobModel>();
            List<RatingModel> ratingModelList = new List<RatingModel>();
            RatingModel  ratingModel = new RatingModel();
            jobModelList = jobService.GetJobListByServiceProviderId(id);
            string[] jobIds = jobModelList.Select(u => u.JobId.ToString()).ToArray();
            ratingModelList = ratingService.GetRatingListByjobIds(jobIds);
            if (ratingModelList.Count > 0)
            {
                ratingModel.Cleanliness = Convert.ToInt32(ratingModelList.Select(c => c.Cleanliness).Average());
                ratingModel.Communication = Convert.ToInt32(ratingModelList.Select(c => c.Communication).Average());
                ratingModel.Conduct = Convert.ToInt32(ratingModelList.Select(c => c.Conduct).Average());
                ratingModel.OnTime= Convert.ToInt32(ratingModelList.Select(c => c.OnTime).Average());
                ratingModel.Quality = Convert.ToInt32(ratingModelList.Select(c => c.Quality).Average());
                ratingModel.UnderStandingOfServiceRequired = Convert.ToInt32(ratingModelList.Select(c => c.UnderStandingOfServiceRequired).Average());
                ratingModel.TotalRating = CommonFunctions.GetTotalFeedback(ratingModel);
            }
            //Get plan for service provider
            UserPlanModel userPlanModel = new UserPlanModel();
            userPlanModel = userPlanService.GetUserPlanByUserId(id);
            PlanModel planModel = new PlanModel();
            planModel = planService.GetPlanById(userPlanModel.PlanId);
            //get car type
            CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
            carEmployeeMappingModel = carEmployeeMappingService.GetCarEmployeeMappingByEmployeeId(id);
            //bind details
            serviceProviderBindingModel.UserPlan = userPlanModel;
            serviceProviderBindingModel.PlanModel = planModel;
            //serviceProviderBindingModel.RatingModelList = ratingModelList;
            serviceProviderBindingModel.RatingModel = ratingModel;
            serviceProviderBindingModel.EmployeesList = employeeModelList;
            serviceProviderBindingModel.ServicesList = servicesList;
            serviceProviderBindingModel.Email = Email;
            serviceProviderBindingModel.PhoneNumber = PhoneNumber;
            serviceProviderBindingModel.UserLocationModel = model;
            serviceProviderBindingModel.CarTypeId = carEmployeeMappingModel.CarTypeId;
            serviceProviderBindingModel.UserName = userName;
            //if (serviceProvider.ServiceProviderType == ServiceProviderType.Individual)
            //{
            //    AutoMapper.Mapper.Map(serviceProviderBindingModel, individualBindingModel);
            //    return Ok(individualBindingModel);
            //}
            return Ok(serviceProviderBindingModel);
        }
        #endregion

        #region Update Service Provider
        // PUT: api/ServiceProviders
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutServiceProvider()
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
                //var plan = resultModel.FormData["planModel"];
                //PlanServiceProviderModel planModel = new PlanServiceProviderModel();
                //planModel = JsonConvert.DeserializeObject<PlanServiceProviderModel>(plan);
                var location = resultModel.FormData["LocationModel"];
                UserLocationModel locationModel = new UserLocationModel();
                locationModel = JsonConvert.DeserializeObject<UserLocationModel>(location);
                var services = resultModel.FormData["services"];
                string[] serviceIds = { string.Empty };
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

                else
                {
                    ServiceProviderModel serviceProviderModelForCheckRecord = new ServiceProviderModel();
                    serviceProviderModelForCheckRecord = serviceProviderService.GetServiceProviderById(serviceProviderModel.ServiceProviderId);

                    if (resultModel.FileData.Count > 0)
                    {
                        string fileName;

                        if (HttpContext.Current.Request.Files != null)
                        {
                            for (var i = 0; i < resultModel.FileData.Count; i++)
                            {
                                var file = HttpContext.Current.Request.Files[i];
                                var name= resultModel.FileData[i].Headers.ContentDisposition.Name;
                                fileName = file.FileName;


                                if (name.Contains("companyLogo"))
                                {

                                    file.SaveAs(Path.Combine(root, Utility.Constants.COMPANY_LOGO_PATH, fileName));
                                    serviceProviderModel.CompanyLogoPath = fileName;
                                }
                                else if (name.Contains("registrationCertificate"))
                                {
                                    file.SaveAs(Path.Combine(root, Utility.Constants.REGISTRATION_CERTIFICATE_PATH, fileName));
                                    serviceProviderModel.RegistrationCertificatePath = fileName;
                                }
                                else if(name.Contains("gosiCertificate"))
                                {
                                    file.SaveAs(Path.Combine(root, Utility.Constants.GOSI_CERTIFICATE_PATH, fileName));
                                    serviceProviderModel.GosiCertificatePath = fileName;
                                }
                            }

                        }


                    }
                    if (serviceProviderModel.CompanyLogoPath == null)
                    {
                        serviceProviderModel.CompanyLogoPath = serviceProviderModelForCheckRecord.CompanyLogoPath;
                    }
                    if (serviceProviderModel.GosiCertificatePath == null)
                    {
                        serviceProviderModel.GosiCertificatePath = serviceProviderModelForCheckRecord.GosiCertificatePath;
                    }
                    if (serviceProviderModel.RegistrationCertificatePath == null)
                    {
                        serviceProviderModel.RegistrationCertificatePath = serviceProviderModelForCheckRecord.RegistrationCertificatePath;
                    }
                    UserLocationModel UserlocationModel = new UserLocationModel();
                    UserlocationModel = locationService.FindLocationById(serviceProviderModel.ServiceProviderId);
                    UserlocationModel.CityId = locationModel.CityId;
                    UserlocationModel.DistrictId = locationModel.DistrictId;
                    UserlocationModel.Latitude = locationModel.Latitude;
                    UserlocationModel.Longitude = locationModel.Longitude;
                    if (UserlocationModel.UserLocationId != 0)
                    {
                        locationService.UpadteUserLocation(UserlocationModel);
                    }
                    ServiceProviderModel serviceProviderModelEntity = new ServiceProviderModel();
                    AutoMapper.Mapper.Map(serviceProviderModel, serviceProviderModelEntity);
                    serviceProviderModelEntity = serviceProviderService.UpadteServiceProvider(serviceProviderModelEntity);
                    AutoMapper.Mapper.Map(serviceProviderModelEntity, serviceProviderModel);
                    ServiceProviderServiceMappingBindingModel serviceProviderServiceBindingModel = new ServiceProviderServiceMappingBindingModel();
                    ServiceProviderServiceMappingModel serviceProviderServiceModel = new ServiceProviderServiceMappingModel();
                    serviceProviderServiceBindingModel.ServiceProviderId = serviceProviderModelEntity.ServiceProviderId;
                    if (serviceIdList.Count > 1)
                    {
                        for (var i = 0; i < serviceIdList.Count; i++)
                        {
                            serviceProviderServiceBindingModel.ServiceId = Convert.ToInt32(serviceIdList[i]);
                            AutoMapper.Mapper.Map(serviceProviderServiceBindingModel, serviceProviderServiceModel);
                            serviceProviderServiceModel = serviceProviderServiceMappingService.UpadteServicesForServiceProvider(serviceProviderServiceModel);
                            AutoMapper.Mapper.Map(serviceProviderServiceModel, serviceProviderServiceBindingModel);
                        }
                    }

                    var user = await UserManager.FindByIdAsync(serviceProviderModel.ServiceProviderId);
                    if (user.Email != serviceProviderModel.Email ||user.PhoneNumber!= serviceProviderModel.PhoneNumber)
                    {
                        user.Email = serviceProviderModel.Email;
                        user.PhoneNumber = serviceProviderModel.PhoneNumber;
                        IdentityResult result = await UserManager.UpdateAsync(user);
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

        #region Delete Service Provider
        // DELETE: api/ServiceProviders/2
        [HttpDelete]
        [ResponseType(typeof(ServiceProvider))]

        public IHttpActionResult DeleteServiceProvider(string id)
        {
            try
            {
                ServiceProviderModel serviceProvider = new ServiceProviderModel();
                if (serviceProvider == null)
                {
                    return NotFound();
                }
                //delete service provider location
                UserLocationModel model = new UserLocationModel();
                model = locationService.FindLocationById(id);
                locationService.DeleteUserLocation(model.UserLocationId);
                //delete service provider service mapping
                List<ServiceProviderServiceMappingModel> serviceProviderMappingModel = new List<ServiceProviderServiceMappingModel>();
                serviceProviderMappingModel = serviceProviderServiceMappingService.FindServiceByUserId(id);
                foreach(var item in serviceProviderMappingModel)
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
                //Delete employee if individual
                serviceProvider = serviceProviderService.GetServiceProviderById(id);
                if(serviceProvider.ServiceProviderType== ServiceProviderType.Individual)
                {
                    EmployeeModel employeeModel = new EmployeeModel();
                    //delete Car Type of employee
                    List<CarEmployeeMappingModel> carEmployeeMappingModel = new List<CarEmployeeMappingModel>();
                    carEmployeeMappingModel = carEmployeeMappingService.GetCarEmployeeMappingListByEmployeeId(id);
                    carEmployeeMappingModel.ForEach(x =>
                    {
                        carEmployeeMappingService.DeleteCarEmployeeMapping(x.CarEmployeeMappingId);
                    });

                    employeeService.DeleteEmployee(id);
                }
                //Delete service provider
                serviceProviderService.DeleteServiceProvider(id);
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

        #region Get All Service Categories
        // GET: api/ServiceProviders/ServiceCategries
        [HttpGet]
        [ResponseType(typeof(List<ServiceProvider>))]
        [Route("ServiceCategries")]
        public List<ServiceCategoryModel> ServiceCategries()
        {
            List<ServiceCategoryModel> serviceCategoryList = new List<ServiceCategoryModel>();
            serviceCategoryList = serviceCategoryService.GetAllServiceCategories().ToList();
            return serviceCategoryList;
        }
        #endregion

        #region Paging For Service Provider
        [HttpPost]
        [AllowAnonymous]
        [Route("Paging")]
        public ResponseMessage Paging(PagingModel model)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<ServiceProviderModel> serviceProviderModel = new List<ServiceProviderModel>();
            serviceProviderModel = serviceProviderService.Paging(model);
            responseMessage.totalRecords = serviceProviderModel.Count();
            serviceProviderModel = serviceProviderModel.OrderBy(x => x.CompanyName).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            responseMessage.data = serviceProviderModel;

            return responseMessage;
        }
        #endregion

        #region Delete Selected Service Provider
        // DELETE: api/ServiceProviders/DeleteSelectedServiceProvider
       
        [ResponseType(typeof(ServiceProvider))]
        [Route("DeleteSelectedServiceProviders")]
        public ResponseMessage DeleteSelectedServiceProviders(List<string> serviceProviders)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            ServiceProviderModel serviceProvider = new ServiceProviderModel();
            try {
                
                foreach (var i in serviceProviders)
                {
                    var id = i;
                    if (id != null)
                    {

                        //delete service provider location
                        UserLocationModel model = new UserLocationModel();
                        model = locationService.FindLocationById(id);
                        locationService.DeleteUserLocation(model.UserLocationId);
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
                        //Delete employee if individual
                        serviceProvider = serviceProviderService.GetServiceProviderById(id);
                        if (serviceProvider.ServiceProviderType == ServiceProviderType.Individual)
                        {
                            EmployeeModel employeeModel = new EmployeeModel();
                            //delete Car Type of employee
                            List<CarEmployeeMappingModel> carEmployeeMappingModel = new List<CarEmployeeMappingModel>();
                            carEmployeeMappingModel = carEmployeeMappingService.GetCarEmployeeMappingListByEmployeeId(id);
                            carEmployeeMappingModel.ForEach(x =>
                            {
                                carEmployeeMappingService.DeleteCarEmployeeMapping(x.CarEmployeeMappingId);
                            });
                            ComplaintService service = new ComplaintService();
                            service.DeleteComplaintbyEmployeeId(id);
                            employeeService.DeleteEmployee(id);
                        }
                        //Delete service provider
                        serviceProviderService.DeleteServiceProvider(id);
                        ApplicationUser user = UserManager.FindById(id);

                        IdentityResult result = UserManager.Delete(user);
                       

                    }
                }
                
            }
            catch(Exception ex)
            {
                responseMessage.Message = ex.Message.ToString();
                return responseMessage;
            }
            serviceProviderList = serviceProviderService.GetAllServiceProviders().ToList();
            responseMessage.totalRecords = serviceProviderList.Count();
            responseMessage.Message = "Selected service providers deleted successfully.";
            return responseMessage;
        }
        #endregion


        #region Add Services for Service Provider
        [HttpPost]
        [AllowAnonymous]
        [Route("SaveServices")]
        public IHttpActionResult SaveServices(ServiceProviderServiceMappingBindingModel model)
        {
            try {
               // model.ServiceProviderId = User.Identity.GetUserId();
                //ServiceProviderServiceMappingBindingModel serviceProviderServiceBindingModel = new ServiceProviderServiceMappingBindingModel();
                ServiceProviderServiceMappingModel serviceProviderServiceModel = new ServiceProviderServiceMappingModel();
                AutoMapper.Mapper.Map(model, serviceProviderServiceModel);
                bool Exist;
                Exist = serviceProviderServiceMappingService.FindServiceByServiceId(model.ServiceId, model.ServiceProviderId);
                if (!Exist)
                {
                    serviceProviderServiceModel = serviceProviderServiceMappingService.InsertServicesForServiceProvider(serviceProviderServiceModel);
                    AutoMapper.Mapper.Map(serviceProviderServiceModel, model);
                }
                else
                {
                    return BadRequest("Service already saved for service provider");
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Delete Services for Service Provider
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteServicesForServiceProvider")]
        public IHttpActionResult DeleteServicesForServiceProvider(int id)
        {
            try {
                List<ServiceProviderServiceMappingModel> serviceProviderServiceModel = new List<ServiceProviderServiceMappingModel>();
                serviceProviderServiceModel = serviceProviderServiceMappingService.FindServicesByServiceId(id);
                foreach (var item in serviceProviderServiceModel)
                {
                    serviceProviderServiceMappingService.DeleteServiceProviderServiceMapping(item.Id);
                }

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Delete Selected Services
        [HttpDelete]
        [Route("DeleteSelectedServices")]
        public ResponseMessage DeleteSelectedServices(List<string> services)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<ServiceProviderServiceMappingModel> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMappingModel>();
            try
            {

                foreach (var i in services)
                {
                    var id = Convert.ToInt32(i);
                    if (id > 0)
                    {

                        serviceProviderServiceMappingModel = serviceProviderServiceMappingService.FindServicesByServiceId(id);
                        foreach (var item in serviceProviderServiceMappingModel)
                        {
                            serviceProviderServiceMappingService.DeleteServiceProviderServiceMapping(item.Id);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                responseMessage.Message = ex.Message.ToString();
                return responseMessage;
            }
            //serviceProvider = serviceProviderService.GetAllServiceProviders().ToList();
            //responseMessage.totalRecords = serviceProvider.Count();
            responseMessage.Message = "Selected service providers deleted successfully.";
            return responseMessage;
        }

        #endregion

        #region Get Service Provider BY Service Id
        //  api/ServiceProviders/GetServiceProviderByServiceId
        [ResponseType(typeof(ServiceProvider))]
        [AllowAnonymous]
        [HttpPost]
        [Route("GetServiceProviderByServiceId")]
        public IHttpActionResult GetServiceProviderByServiceId(string ids, string filterType,string flag)
        {
            GeoCoordinate clientLocation = new GeoCoordinate();
            if (filterType ==Utility.Constants.LOCATION_FILTER)
            {
                var clientId = User.Identity.GetUserId();
                UserLocationModel userLocation = new UserLocationModel();
                userLocation = locationService.FindUserLocationById(clientId);
                clientLocation = new GeoCoordinate(Convert.ToDouble(userLocation.Latitude), Convert.ToDouble(userLocation.Longitude));
            }
            string[] serviceIds;
            serviceIds = JsonConvert.DeserializeObject<string[]>(ids);
            int[] myInts = Array.ConvertAll(serviceIds, int.Parse);
            //get service provider service mapping
            List<ServiceProviderServiceMappingModel> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMappingModel>();
            //ServiceProviderServiceMappingModel serviceProviderServiceMapping= new ServiceProviderServiceMappingModel();
            serviceProviderServiceMappingModel = serviceProviderServiceMappingService.FindServiceProviderByServiceId(myInts);
            //get service provider
            List<ServiceProviderModel> ServiceProviderModel = new List<ServiceProviderModel>();
            List<RegisterServiceProviderBindingModel> serviceProviderBindingModel = new List<RegisterServiceProviderBindingModel>();
            string[] serviceProviderId = serviceProviderServiceMappingModel.Select(x => x.ServiceProviderId).ToArray();
            ServiceProviderModel = serviceProviderService.GetServiceProviderListById(serviceProviderId);
            AutoMapper.Mapper.Map(ServiceProviderModel,serviceProviderBindingModel);
            List<int> idList = new List<int>();
            bool Match;
            serviceProviderBindingModel.ForEach(x => {
                x.ServicesList = serviceProviderServiceMappingService.FindServiceByUserId(x.ServiceProviderId);
                x.ServicesList.ForEach(y =>
                {
                    idList.Add(y.ServiceId);
                    
                });
                Match = ArraysEqual(myInts, idList.ToArray());
                x.Match = Match;
                idList.Clear();
            });
            //Get Rating of service provider according to Job
            List<JobModel> jobModelList = new List<JobModel>();
            List<RatingModel> ratingModelList = new List<RatingModel>();
            RatingModel ratingModel = new RatingModel();
            serviceProviderBindingModel = serviceProviderBindingModel.Where(x => x.Match).ToList();
            serviceProviderBindingModel.ForEach(x =>
            {
                jobModelList = jobService.GetJobListByServiceProviderId(x.ServiceProviderId);
                string[] jobIds = jobModelList.Select(u => u.JobId.ToString()).ToArray();
                ratingModelList = ratingService.GetRatingListByjobIds(jobIds);
                if (ratingModelList.Count > 0)
                {
                    ratingModel.Cleanliness = Convert.ToInt32(ratingModelList.Select(c => c.Cleanliness).Average());
                    ratingModel.Communication = Convert.ToInt32(ratingModelList.Select(c => c.Communication).Average());
                    ratingModel.Conduct = Convert.ToInt32(ratingModelList.Select(c => c.Conduct).Average());
                    ratingModel.Quality = Convert.ToInt32(ratingModelList.Select(c => c.Quality).Average());
                    ratingModel.OnTime = Convert.ToInt32(ratingModelList.Select(c => c.OnTime).Average());
                    ratingModel.UnderStandingOfServiceRequired = Convert.ToInt32(ratingModelList.Select(c => c.UnderStandingOfServiceRequired).Average());
                    ratingModel.TotalRating = CommonFunctions.GetTotalFeedback(ratingModel);
                }
                x.RatingModelList = ratingModelList;
                x.AverageRating = ratingModel.TotalRating;
                x.ServicesList = serviceProviderServiceMappingModel.Where(y => y.ServiceProviderId == x.ServiceProviderId).ToList();
                x.MinPrice = x.ServicesList.Select(y => y.Price).Min();
                x.MaxPrice = x.ServicesList.Select(y => y.Price).Max();
                x.UserLocationModel = locationService.FindUserLocationById(x.ServiceProviderId);
                var geoCordinate = new GeoCoordinate(Convert.ToDouble(x.UserLocationModel.Latitude), Convert.ToDouble(x.UserLocationModel.Longitude));
                x.Distance = clientLocation.GetDistanceTo(geoCordinate);
                x.TaskDone = jobService.GetCompletedJobListByServiceProviderId(x.ServiceProviderId).Count();
            });
            if (filterType == Utility.Constants.LOCATION_FILTER)
            {
                if(flag== Utility.Constants.ASCENDING)
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderBy(x => x.Distance).ToList();
                }
                else
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderByDescending(x => x.Distance).ToList();
                }


            }
            if (filterType == Utility.Constants.SERVICE_FILTER)
            {
                if (flag == Utility.Constants.ASCENDING)
                {
                    serviceProviderBindingModel= serviceProviderBindingModel.OrderBy(x => x.ServicesList.Count()).ToList();
                }
                else
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderByDescending(x => x.ServicesList.Count()).ToList();
                }

            }
            if (filterType == Utility.Constants.COST_FILTER)
            {
                if (flag == Utility.Constants.ASCENDING)
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderBy(x=>x.MinPrice).ToList();
                }
                else
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderBy(x => x.MaxPrice).ToList();
                }

            }
            if (filterType == Utility.Constants.RATING_FILTER )
            {
                if (flag == Utility.Constants.ASCENDING)
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderBy(x => x.AverageRating).ToList();
                }
                else
                {
                    serviceProviderBindingModel = serviceProviderBindingModel.OrderByDescending(x => x.AverageRating).ToList();
                }

            }
            return Ok(serviceProviderBindingModel);
        }
        #endregion

        #region Get Reviews For Service Provider by Id
        //  api/ServiceProviders/GetReviewsByServiceProviderId
       
      
        [Route("GetReviewsByServiceProviderId")]
        public IHttpActionResult GetReviewsByServiceProviderId(string id)
        {
           
                List<JobBindingModel> jobBindingModelList = new List<JobBindingModel>();
                List<JobModel> jobModelList = new List<JobModel>();
                jobModelList = jobService.GetJobListByServiceProviderId(id);
                AutoMapper.Mapper.Map(jobModelList, jobBindingModelList);
                jobBindingModelList.ForEach(x => {
                    x.ClientModel = clientService.GetClientById(x.ClientId);
                    x.RatingModel = ratingService.GetRatingListByJobId(x.JobId);
                });
                
           
            return Ok(jobBindingModelList);
        }
        #endregion

        #region Deactivate/ActivateService Provider
        [HttpPost]
       
        [Route("DeActivateServiceProvider")]
        public IHttpActionResult DeActivateServiceProvider(string Id,bool IsActive)
        {
            try
            {
                RegisterServiceProviderBindingModel serviceProviderBindingModel = new RegisterServiceProviderBindingModel();
                ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
                serviceProviderModel = serviceProviderService.GetServiceProviderById(Id);
                serviceProviderModel.IsActive = IsActive;
                serviceProviderModel = serviceProviderService.UpadteServiceProvider(serviceProviderModel);
                AutoMapper.Mapper.Map(serviceProviderModel, serviceProviderBindingModel);
                return Ok(serviceProviderBindingModel);
            }
            catch (Exception ex)
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

        static bool ArraysEqual<T>(T[] a, T[] b)
        {
            //int k = 0;
            //return a.All(x => x.Equals(b[k++]));
            var result = a.Except(b);
            if (result.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}