
using FluentScheduler;
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
using System.Net.Mail;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Web.Models;
using URFX.Web.Utility;


namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Jobs")]
    public class JobsController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        JobService jobService = new JobService();
        CityService cityService = new CityService();
        ClientService clientService = new ClientService();
        SubServicesService subServicesService = new SubServicesService();
        UserLocationService userLocationService = new UserLocationService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        JobServiceMappingService jobServiceMappingService = new JobServiceMappingService();
        JobServicePictruesMappingService jobServicePictruesMappingService = new JobServicePictruesMappingService();
        SendNotificationService sendNotificationService = new SendNotificationService();
        TransactionHistoryService transactionHistoryService = new TransactionHistoryService();
        EmployeeScheduleService employeeScheduleService = new EmployeeScheduleService();
        EmployeeService employeeServices = new EmployeeService();
        RatingService ratingService = new RatingService();

        private ApplicationUserManager _userManager;

        public JobsController()
        {

        }
        public JobsController(ApplicationUserManager userManager)
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




        // GET: api/Jobs
        public List<JobBindingModel> GetJob()
        {
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            List<JobModel> jobModel = new List<JobModel>();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetAllJobs();
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            jobBindingModel.ForEach(x =>
            {
                //Get Client Name
                ClientModel clientModel = new ClientModel();
                clientModel = clientService.GetClientById(x.ClientId);
                x.ClientName = clientModel.FirstName;
                //Get service City
                UserLocationModel userLocationModel = new UserLocationModel();
                userLocationModel = userLocationService.FindLocationById(x.ClientId);
                if (x.JobAddress == null || x.JobAddress == "")
                {
                    x.JobAddress = userLocationModel.Address;
                }
                //Get Employee Name 
                if (x.EmployeeId != null)
                {
                    x.EmployeeModel = employeeServices.GetEmployeeById(x.EmployeeId);
                }
            });
            //Get serviceMapping
            List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
            string[] jobIds = jobBindingModel.Select(u => u.JobId.ToString()).ToArray();
            jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingByJobIds(jobIds);
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
            return jobBindingModel;
        }

        // GET: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult GetJob(int id)
        {
            JobBindingModel jobBindingModel = new JobBindingModel();
            JobModel jobModel = new JobModel();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetJobById(id);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            jobBindingModel.EmployeeModel = employeeServices.GetEmployeeById(jobBindingModel.EmployeeId);
            return Ok(jobBindingModel);
        }


        // PUT: api/Jobs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJob(JobBindingModel model)
        {
            JobModel job = new JobModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AutoMapper.Mapper.Map(model, job);
            job = jobService.UpadteJob(job);
            AutoMapper.Mapper.Map(job, model);
            return Ok(model);
        }

        // POST: api/Jobs
        [ResponseType(typeof(Job))]
        public IHttpActionResult PostJob(JobBindingModel model)
        {
            JobModel job = new JobModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AutoMapper.Mapper.Map(model, job);
            job = jobService.SaveJob(job);
            AutoMapper.Mapper.Map(job, model);
            return Ok(model);
        }

        // DELETE: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult DeleteJob(int id)
        {
            jobService.DeleteJob(id);
            return Ok();
        }

        #region Paging For Jobs
        [HttpPost]
        [AllowAnonymous]
        [Route("Paging")]
        public ResponseMessage Paging(PagingModel model)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<JobModel> jobModel = new List<JobModel>();
            jobModel = jobService.Paging(model);
            responseMessage.totalRecords = jobModel.Count();
            jobModel = jobModel.OrderBy(x => x.Description).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            responseMessage.data = jobModel;

            return responseMessage;
        }
        #endregion

        #region Get Job By ServiceProvider Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetJobByServiceProviderId")]
        public IHttpActionResult GetJobByServiceProviderId(string serviceProviderId)
        {
            //Get job
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            List<JobModel> jobModel = new List<JobModel>();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetJobListByServiceProviderId(serviceProviderId);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);

            jobBindingModel.ForEach(x =>
            {
                //Get Client Name
                ClientModel clientModel = new ClientModel();
                clientModel = clientService.GetClientById(x.ClientId);
                x.ClientName = clientModel.FirstName;
                //Get service City
                UserLocationModel userLocationModel = new UserLocationModel();
                userLocationModel = userLocationService.FindLocationById(x.ClientId);
                if (x.JobAddress == null || x.JobAddress=="")
                {
                    x.JobAddress = userLocationModel.Address;
                }
               
            });
            //Get serviceMapping
            List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
            string[] jobIds = jobBindingModel.Select(u => u.JobId.ToString()).ToArray();
            jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingByJobIds(jobIds);
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
            return Ok(jobBindingModel);
        }
        #endregion

        #region Get Job By Job Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetJobByJobId")]
        public IHttpActionResult GetJobByJobId(Int32 jobId)
        {
            JobBindingModel jobBindingModel = new JobBindingModel();
            JobModel jobModel = new JobModel();
            AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            jobModel = jobService.GetJobById(jobId);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            //Get Client Name
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(jobBindingModel.ClientId);
            jobBindingModel.ClientName = clientModel.FirstName;
            //Get service City
            UserLocationModel userLocationModel = new UserLocationModel();
            userLocationModel = userLocationService.FindLocationById(jobBindingModel.ClientId);
            if (jobBindingModel.JobAddress == null || jobBindingModel.JobAddress == "")
            {
                jobBindingModel.JobAddress = userLocationModel.Address;
            }
            
            //Get serviceMapping
            JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
            jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingByJobId(jobId);
            //get service name
            SubServiceModel subServiceModel = new SubServiceModel();
            subServiceModel = subServicesService.GetSubServiceById(jobServiceMappingModel.ServiceId);
            jobBindingModel.ServiceName = subServiceModel.Description;
            //get service picture mapping
            List<JobServicePictureMappingModel> jobServicePictureMappingModel = new List<JobServicePictureMappingModel>();
            jobServicePictureMappingModel = jobServicePictruesMappingService.GetJobServicePictureMappingByJobServiceMappingId(jobServiceMappingModel.JobServiceMappingId);
            jobServiceMappingModel.JobServicePictureMappings = jobServicePictureMappingModel;
            jobBindingModel.JobServiceMapping = jobServiceMappingModel;
            jobBindingModel.EmployeeModel = employeeServices.GetEmployeeById(jobBindingModel.EmployeeId);
            jobBindingModel.RatingModel = ratingService.GetRatingListByJobId(jobBindingModel.JobId);
            return Ok(jobBindingModel);
        }
        #endregion

        #region Create Job
        // POST: api/Jobs/CreateJob
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Route("CreateJob")]
        public async Task<IHttpActionResult> CreateJob()
        {
            //using (var dataContext = new URFXDbContext())
            //{
            //    TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //    {
            TaskModel taskModel = new TaskModel();
            JobModel jobModel = new JobModel();
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
                var result = resultModel.FormData["model"];
                var model = result.Substring(1, result.Length - 2);
                List<JobBindingModel> JobBindingModelList = new List<JobBindingModel>();

                taskModel = JsonConvert.DeserializeObject<TaskModel>(model);
                List<JobModel> jobModelList = new List<JobModel>();
                AutoMapper.Mapper.Map(JobBindingModelList, jobModelList);
                string fileName;
                if (taskModel.StartDate >= DateTime.UtcNow)
                {
                    //Check for individual
                    ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
                    serviceProviderModel = serviceProviderService.GetServiceProviderById(taskModel.ServiceProviderId);
                    if (serviceProviderModel.ServiceProviderType == ServiceProviderType.Individual)
                    {
                        jobModel.EmployeeId = taskModel.ServiceProviderId;
                    }
                    try
                    {
                        //set status and details of job
                        jobModel.Status = JobStatus.New;
                        //jobModel.CreatedDate = DateTime.UtcNow;
                        jobModel.StartDate = taskModel.StartDate;
                        jobModel.EndDate = taskModel.EndDate;
                        jobModel.Description = taskModel.Description;
                        jobModel.ServiceProviderId = taskModel.ServiceProviderId;
                        jobModel.ClientId = taskModel.ClientId;
                        jobModel.Quantity = taskModel.Quantity;
                        jobModel.Comments = taskModel.Comments;
                        jobModel.JobAddress = taskModel.JobAddress;
                        jobModel.Latitude = taskModel.Latitude;
                        jobModel.Longitude = taskModel.Longitude;
                        jobModel.Cost = taskModel.Cost;
                        //save job
                        jobModel = jobService.SaveJob(jobModel);
                        //Get transcation history for job
                        //TransactionHistoryModel historyModel = new TransactionHistoryModel();
                        //historyModel = transactionHistoryService.FindTransactionHistoryByMerchantRegrence(taskModel.MerchantReference);
                        //historyModel.JobId = jobModel.JobId;
                        //transactionHistoryService.UpdateTransactionHistory(historyModel);
                        //Job service Mapping
                        JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
                        JobServicePictureMappingModel jobServicePictureMappingModel = new JobServicePictureMappingModel();
                        taskModel.SubTaskList.ForEach(x =>
                        {
                            jobServiceMappingModel.Quantity = x.Quantity;
                            jobServiceMappingModel.JobId = jobModel.JobId;
                            jobServiceMappingModel.Comments = x.Comments;
                            jobServiceMappingModel.ServiceId = x.ServiceId;
                            jobServiceMappingModel.Description = x.Description;
                            jobServiceMappingModel = jobServiceMappingService.SaveJobServiceMapping(jobServiceMappingModel);
                            jobServicePictureMappingModel.JobServiceMappingId = jobServiceMappingModel.JobServiceMappingId;
                            if (HttpContext.Current.Request.Files != null)
                            {
                                for (var i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                                {
                                    var file = HttpContext.Current.Request.Files[i];
                                    fileName = file.FileName;
                                    var splittedFileName = fileName.Split('$');
                                    if (splittedFileName[0] == x.Description)
                                    {
                                        file.SaveAs(Path.Combine(root, Utility.Constants.JOB_SERVICE_IMAGES_PATH, splittedFileName[1]));
                                        jobServicePictureMappingModel.ImagePath = splittedFileName[1];
                                        jobServicePictureMappingModel = jobServicePictruesMappingService.SaveJobServicePictureMapping(jobServicePictureMappingModel);
                                    }
                                }
                            }

                        });
                        //  transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        //   transaction.Dispose();
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return BadRequest("Job date is not valid");
                }
            }
            
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        
            //    }
            //}
            return Ok(jobModel);
        }
        #endregion

        #region Accept Job
        // POST: api/Jobs/AcceptJob
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Route("AcceptJob")]
        public IHttpActionResult AcceptJob(string userId, int jobId)
        {
            JobBindingModel jobBindingModel = new JobBindingModel();
            try
            {
                //Get job by jobId
                JobModel jobModel = new JobModel();
                AutoMapper.Mapper.Map(jobBindingModel, jobModel);
                jobModel = jobService.GetJobById(jobId);
                jobModel.EmployeeId = userId;
                jobModel.Status = JobStatus.Current;
                jobModel = jobService.UpadteJob(jobModel);
                AutoMapper.Mapper.Map(jobModel, jobBindingModel);
                //get job service mapping
                List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
                jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingListByJobId(jobBindingModel.JobId);
                foreach (var item in jobServiceMappingModel)
                {
                    SubServiceModel subServiceModel = new SubServiceModel();
                    subServiceModel = subServicesService.GetSubServiceById(item.ServiceId);
                    item.ServiceName = subServiceModel.Description;
                }
                //get job service picture mapping
                List<JobServicePictureMappingModel> jobServicePictureMappingModel = new List<JobServicePictureMappingModel>();
                jobServicePictureMappingModel = jobServicePictruesMappingService.GetJobServicePictureMapping();
                jobBindingModel.JobServiceMappings = jobServiceMappingModel.Where(j => j.JobId == jobBindingModel.JobId).ToList();

                jobServiceMappingModel.ForEach(x =>
                {
                    x.JobServicePictureMappings = jobServicePictureMappingModel.Where(j => j.JobServiceMappingId == x.JobServiceMappingId).ToList();
                });
                //get client inforamtion

                //Get Client Name
                ClientModel clientModel = new ClientModel();
                clientModel = clientService.GetClientById(jobBindingModel.ClientId);
                jobBindingModel.ClientModel = clientModel;
                ApplicationUser user = UserManager.FindById(jobModel.ClientId);
                if (user != null)
                {
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + userId + "&data.jobId=" + jobId + "&data.type=" + Utility.Constants.JOB_ACCEPT_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_JOB_ACCEPT + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_JOB_ACCEPT + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + jobId + ",\"userId\":\"" + userId + "\",\"type\":\"" + Utility.Constants.JOB_ACCEPT_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }





            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }

            return Json(jobBindingModel);
        }
        #endregion

        #region Reject Job
        // POST: api/Jobs/RejectJob
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Route("RejectJob")]
        public IHttpActionResult RejectJob(string userId, int jobId, string comment)
        {
            JobBindingModel jobBindingModel = new JobBindingModel();
            try
            {

                //Get job by jobId

                JobModel jobModel = new JobModel();
                AutoMapper.Mapper.Map(jobBindingModel, jobModel);
                jobModel = jobService.GetJobById(jobId);
                jobModel.Status = JobStatus.Rejected;
                jobModel.Comment = comment;
                //Check for individual
                ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
                serviceProviderModel = serviceProviderService.GetServiceProviderById(userId);
                if (serviceProviderModel.ServiceProviderType == ServiceProviderType.Individual)
                {
                    jobModel.EmployeeId = null;
                }
                jobModel = jobService.UpadteJob(jobModel);
                AutoMapper.Mapper.Map(jobModel, jobBindingModel);
                //remove job from schedule table
                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByJobId(jobId);
                employeeScheduleService.DeleteEmployeeSchedule(employeeScheduleModel.Id);
                //Get client for job
                ApplicationUser user = UserManager.FindById(jobModel.ClientId);
                if (user != null)
                {
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + userId + "&data.jobId=" + jobId + "&data.type=" + Utility.Constants.JOB_REJECT_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_JOB_REJECT + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_JOB_REJECT + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + jobId + ",\"userId\":\"" + userId + "\",\"type\":\"" + Utility.Constants.JOB_REJECT_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(jobBindingModel);
        }
        #endregion

        #region Complete Task
        // POST: api/Jobs/CompleteTask
        [ResponseType(typeof(Job))]
        [HttpPost]
        [AllowAnonymous]
        [Route("CompleteTask")]
        public IHttpActionResult CompleteTask(string userId, int jobId)
        {
            JobBindingModel jobBindingModel = new JobBindingModel();
            try
            {
                
                //Get job by jobId
                JobModel jobModel = new JobModel();
                AutoMapper.Mapper.Map(jobBindingModel, jobModel);
                jobModel = jobService.GetJobById(jobId);
                jobModel.Status = JobStatus.Completed;
                jobModel = jobService.UpadteJob(jobModel);
                AutoMapper.Mapper.Map(jobModel, jobBindingModel);
                //Get employee schedule by jobid and delete
                EmployeeScheduleModel employeeScheduleModel = new EmployeeScheduleModel();
                employeeScheduleModel = employeeScheduleService.GetEmployeesScheduleByJobId(jobId);
                employeeScheduleService.DeleteEmployeeSchedule(employeeScheduleModel.Id);
                ApplicationUser user = UserManager.FindById(jobModel.ClientId);
                if (user != null)
                {
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + userId + "&data.jobId=" + jobId + "&data.type=" + Utility.Constants.JOB_COMPLETED_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_JOB_COMPLETED + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_JOB_COMPLETED + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + jobId + ",\"userId\":\"" + userId + "\",\"type\":\"" + Utility.Constants.JOB_COMPLETED_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
            return Json(jobBindingModel);
        }
        #endregion

        #region Additional Task
        // POST: api/Jobs/AdditionalTask
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Route("AdditionalTask")]
        public IHttpActionResult AdditionalTask(AdditionalTaskModel model)
        {
            JobModel jobModel = new JobModel();
            jobModel = jobService.GetJobById(model.JobId);
            jobModel.AdditionalTaskDescription = model.Description;
            jobModel = jobService.UpadteJob(jobModel);
            return Ok(model);
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

        private bool JobExists(int id)
        {
            return db.Job.Count(e => e.JobId == id) > 0;
        }


       

    }
    #region JobScheduling
    
    #endregion
}