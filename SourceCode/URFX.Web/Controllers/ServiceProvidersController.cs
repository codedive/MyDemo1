using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Resources;
using URFX.Web.Models;

namespace URFX.Web.Controllers
{
    [RoutePrefix("api/ServiceProviders")]
    public class ServiceProvidersController : ApiController
    {
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        ServiceCategoryService serviceCategoryService = new ServiceCategoryService();
    
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
        public List<ServiceProviderModel> GetServiceProvider()
        {
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            serviceProviderList = serviceProviderService.GetAllServiceProviders().ToList();
            //serviceProviderList = serviceProviderService.GetAllServiceProviders().Select(x => new ServiceProviderModel()
            //{
            //    CompanyName = Resource.gf,
            //    Description = Resource.gf,
            //    GeneralManagerName = Resource.gfdg
            //}).ToList();
            return serviceProviderList;
        }
        #endregion

        #region Get Service Provider BY Id
        // GET: api/ServiceProviders/5
        [ResponseType(typeof(ServiceProvider))]
        public IHttpActionResult GetServiceProvider(string id)
        {
            ServiceProviderModel serviceProvider = new ServiceProviderModel();
            if (serviceProvider == null)
            {
                return NotFound();
            }
            serviceProvider = serviceProviderService.GetServiceProviderById(id);
            return Ok(serviceProvider);
        }
        #endregion

        #region Update Service Provider
        // PUT: api/ServiceProviders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServiceProvider(string id, RegisterServiceProviderBindingModel serviceProviderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != serviceProviderModel.ServiceProviderId)
            //{
            //    return BadRequest();
            //}
            try
            {
                ApplicationUser user = UserManager.FindById(id);
                user.Email = serviceProviderModel.Email;
                IdentityResult result =  UserManager.Update(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    serviceProviderModel.ServiceProviderId = id;
                    ServiceProviderModel serviceProvider = new ServiceProviderModel();
                    AutoMapper.Mapper.Map(serviceProviderModel, serviceProvider);
                    serviceProviderService.UpadteServiceProvider(serviceProvider);
                    AutoMapper.Mapper.Map(serviceProvider, serviceProviderModel);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!serviceProviderService.CheckExistance(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion

        #region Delete Service Provider
        [ResponseType(typeof(ServiceProvider))]
        public IHttpActionResult DeleteServiceProvider(string id)
        {
            ServiceProviderModel serviceProvider = new ServiceProviderModel();
            if (serviceProvider == null)
            {
                return NotFound();
            }
            serviceProviderService.DeleteServiceProvider(id);
            ApplicationUser user = UserManager.FindById(id);
          
            IdentityResult result = UserManager.Delete(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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
    }
}