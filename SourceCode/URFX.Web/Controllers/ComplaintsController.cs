using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Enums;
using URFX.Web.Models;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Complaints")]
    public class ComplaintsController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        ComplaintService complaintService = new ComplaintService();
        EmployeeService employeeService = new EmployeeService();
        ClientService clientService = new ClientService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        JobService jobService = new JobService();
        private ApplicationUserManager _userManager;
        public ComplaintsController()
        {
        }

        public ComplaintsController(ApplicationUserManager userManager)
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






        #region Get All Complaints
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllComplaints")]
        public IHttpActionResult GetAllComplaints()
        {
            try
            {
            List<ComplaintBindingModel> complaintBindingModel = new List<ComplaintBindingModel>();
            List<ComplaintModel> complaintModel = new List<ComplaintModel>();
            // AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            complaintModel = complaintService.GetComplaints();
            AutoMapper.Mapper.Map(complaintModel, complaintBindingModel);
            EmployeeModel employeeModel = new EmployeeModel();
            ClientModel clientModel = new ClientModel();
            ServiceProviderModel ServiceProviderModel = new ServiceProviderModel();
                JobModel jobModel = new JobModel();
            complaintBindingModel.ForEach(x => {
                jobModel = jobService.GetJobById(x.JobId);
                employeeModel = employeeService.GetEmployeeById(x.EmployeeId);
                x.EmployeeName = employeeModel.FirstName;
                clientModel = clientService.GetClientById(x.ClientId);
                x.ClientName = clientModel.FirstName;
                ServiceProviderModel = serviceProviderService.GetServiceProviderById(x.ServiceProviderId);
                x.ServiceProviderName = ServiceProviderModel.CompanyName;
                x.JobAddress = jobModel.JobAddress;
                x.JobDescription = jobModel.Description;
                x.Description = x.Description;
                ApplicationUser user = UserManager.FindById(x.ClientId);
                if (user != null)
                {
                    x.ClientPhoneNumber = user.PhoneNumber;
                }

            });
                return Ok(complaintBindingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        #endregion

        #region Get complaint By ServiceProvider Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetComplaintByServiceProviderId")]
        public IHttpActionResult GetComplaintByServiceProviderId(string serviceProviderId)
        {
            List<ComplaintBindingModel> complaintBindingModel = new List<ComplaintBindingModel>();
            List<ComplaintModel> complaintModel = new List<ComplaintModel>();
           // AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            complaintModel = complaintService.GetComplaintListByServiceProviderId(serviceProviderId);
            AutoMapper.Mapper.Map(complaintModel, complaintBindingModel);
            EmployeeModel employeeModel = new EmployeeModel();
            ClientModel clientModel = new ClientModel();
            ServiceProviderModel ServiceProviderModel = new ServiceProviderModel();
            JobModel jobModel = new JobModel();
            complaintBindingModel.ForEach(x => {
                jobModel = jobService.GetJobById(x.JobId);
                employeeModel = employeeService.GetEmployeeById(x.EmployeeId);
                x.EmployeeName = employeeModel.FirstName;
                clientModel = clientService.GetClientById(x.ClientId);
                x.ClientName = clientModel.FirstName;
                ServiceProviderModel = serviceProviderService.GetServiceProviderById(x.ServiceProviderId);
                x.ServiceProviderName = ServiceProviderModel.CompanyName;
                x.JobAddress = jobModel.JobAddress;
                x.JobDescription = jobModel.Description;
                x.Description = x.Description;
                ApplicationUser user = UserManager.FindById(x.ClientId);
                if (user != null)
                {
                    x.ClientPhoneNumber = user.PhoneNumber;
                }

            });
            return Ok(complaintBindingModel);
        }
        #endregion

        #region Get complaint By Complaint Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetComplaintByComplaintId")]
        public IHttpActionResult GetComplaintByComplaintId(int complaintId)
        {
            ComplaintBindingModel complaintBindingModel = new ComplaintBindingModel();
            ComplaintModel complaintModel = new ComplaintModel();
            // AutoMapper.Mapper.Map(jobBindingModel, jobModel);
            complaintModel = complaintService.GetComplaintDetailByComplaintId(complaintId);
            AutoMapper.Mapper.Map(complaintModel, complaintBindingModel);
            return Ok(complaintBindingModel);
        }
        #endregion

        #region Update complaint
        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateComplaint")]
        public IHttpActionResult UpdateComplaint(int complaintId)
        {
            try {
                ComplaintBindingModel complaintBindingModel = new ComplaintBindingModel();
                ComplaintModel complaintModel = new ComplaintModel();
                complaintModel = complaintService.GetComplaintByComplaintId(complaintId);
                complaintModel.Status = ComplaintStatus.Close;
                complaintModel = complaintService.UpdateComplaint(complaintModel);
                AutoMapper.Mapper.Map(complaintModel, complaintBindingModel);
                return Ok(complaintBindingModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add complaint
        [HttpPost]
        [AllowAnonymous]
        [Route("AddComplaint")]
        public IHttpActionResult AddComplaint(ComplaintBindingModel model)
        {
            try
            {
                ComplaintBindingModel complaintBindingModel = new ComplaintBindingModel();
                ComplaintModel complaintModel = new ComplaintModel();
                AutoMapper.Mapper.Map(model,complaintModel);
                complaintModel = complaintService.InsertComplaint(complaintModel);
                AutoMapper.Mapper.Map(complaintModel, complaintBindingModel);
                return Ok(complaintBindingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete Selected Complaints
        // DELETE: api/Complaints/DeleteSelectedComplaints
        [HttpDelete]
        [Route("DeleteSelectedComplaints")]
        public ResponseMessage DeleteSelectedComplaints(List<string> complaints)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<ComplaintModel> complaintModelList = new List<ComplaintModel>();
            ComplaintModel complaintModel = new ComplaintModel();
            try
            {

                foreach (var i in complaints)
                {
                    var id =Convert.ToInt32(i);
                    if (id >0)
                    {
                        //Delete Complaint
                        complaintService.DeleteComplaint(Convert.ToInt32(id));
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                responseMessage.Message = ex.Message.ToString();
                return responseMessage;
            }
            
            responseMessage.Message = "Selected complaints deleted successfully.";
            return responseMessage;
        }
        #endregion

       
        // GET: api/Complaints/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Complaints
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Complaints/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Complaints/5
        public void Delete(int id)
        {
            complaintService.DeleteComplaint(id);
        }

        
    }
}
