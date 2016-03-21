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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Web.Models;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/ServiceCategories")]
    public class ServiceCategoriesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
       
        ServiceCategoryService serviceCategoryService = new ServiceCategoryService();
        SubServicesService subServicesService = new SubServicesService();
        ServiceProviderServiceMappingService serviceProviderServiceMappingService = new ServiceProviderServiceMappingService();
        // GET: api/ServiceCategories
        public List<ServiceCategoryModel> GetServiceCategories()
        {
            List<ServiceCategoryModel> serviceCategoryList = new List<ServiceCategoryModel>();
            serviceCategoryList = serviceCategoryService.GetAllServiceCategories().Select(x => new ServiceCategoryModel()
            {
                Description = CommonFunctions.ReadResourceValue(x.Description),
                ServiceCategoryId = x.ServiceCategoryId,
                CategoryPicturePath=x.CategoryPicturePath,
                Services = x.Services
            }).ToList();
            return serviceCategoryList;
        }

        // GET: api/ServiceCategories/5
        [ResponseType(typeof(ServiceCategory))]
        public IHttpActionResult GetServiceCategory(int id)
        {
            ServiceCategory serviceCategory = db.ServiceCategories.Find(id);
            if (serviceCategory == null)
            {
                return NotFound();
            }

            return Ok(serviceCategory);
        }

        // PUT: api/ServiceCategories/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServiceCategory(int id)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ServiceCategory modal = new ServiceCategory();
                var root = HttpContext.Current.Server.MapPath(Utility.Constants.BASE_FILE_UPLOAD_PATH);
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var resultModel = await Request.Content.ReadAsMultipartAsync(provider);
                if (resultModel.FormData["model"] == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                else
                {
                    modal = JsonConvert.DeserializeObject<ServiceCategory>(resultModel.FormData["model"]);
                    //modal.Description = resultModel.FormData["Description"];
                }
                if (resultModel.FileData.Count > 0)
                {
                    string fileName;

                    if (HttpContext.Current.Request.Files != null)
                    {
                        for (var i = 0; i < resultModel.FileData.Count; i++)
                        {
                            var file = HttpContext.Current.Request.Files[i];
                            fileName = file.FileName;
                            file.SaveAs(Path.Combine(root, Utility.Constants.SERVICES_LOGO, fileName));
                            modal.CategoryPicturePath = fileName;
                        }
                    }
                }

                ServiceCategoryModel serviceModel = new ServiceCategoryModel();
                AutoMapper.Mapper.Map(modal, serviceModel);
                if (id == modal.ServiceCategoryId)
                {
                    serviceModel = serviceCategoryService.UpadteCategoryService(serviceModel);
                    return Ok(serviceModel);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/ServiceCategories
        [ResponseType(typeof(ServiceCategory))]
        public async Task<IHttpActionResult> PostServiceCategory()
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ServiceCategory modal = new ServiceCategory();
                var root = HttpContext.Current.Server.MapPath(Utility.Constants.BASE_FILE_UPLOAD_PATH);
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var resultModel = await Request.Content.ReadAsMultipartAsync(provider);
                if (resultModel.FormData["model"] == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                else {
                    modal = JsonConvert.DeserializeObject<ServiceCategory>(resultModel.FormData["model"]);
                    //modal.Description = resultModel.FormData["Description"];
                }
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

                                file.SaveAs(Path.Combine(root, Utility.Constants.SERVICES_LOGO, fileName));
                                modal.CategoryPicturePath = fileName;
                            }
                        }
                    }
                }

                ServiceCategoryModel serviceModel = new ServiceCategoryModel();
                AutoMapper.Mapper.Map(modal, serviceModel);
                serviceModel = serviceCategoryService.SaveCategoryService(serviceModel);
               

                return Ok(serviceModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ServiceCategories/5
        [ResponseType(typeof(ServiceCategory))]
        public IHttpActionResult DeleteServiceCategory(int id)
        {
            try {
                ServiceCategory serviceCategory = db.ServiceCategories.Find(id);
                if (serviceCategory == null)
                {
                    return NotFound();
                }

                serviceCategoryService.DeleteCategoryService(id);
                

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceCategoryExists(int id)
        {
            return db.ServiceCategories.Count(e => e.ServiceCategoryId == id) > 0;
        }



        #region Get CategoriesWithPrice

        // GET: api/ServiceCategories/GetCategoriesWithPrice
        [HttpGet]
        [Route("GetCategoriesWithPrice")]
        [AllowAnonymous]
        public List<CategoryWithPriceModel> GetCategoriesWithPrice()
        {
            List<CategoryWithPriceModel> serviceCategoryWithPrice = new List<CategoryWithPriceModel>();
            List<SubServiceModel> subServiceModel = new List<SubServiceModel>();
            List<SubServiceModel> allSubServiceModelList = new List<SubServiceModel>();
            allSubServiceModelList = subServicesService.GetAllSubServices();
            serviceCategoryWithPrice = db.CategoryWithPriceModel();
            serviceCategoryWithPrice.ForEach(x => {
                subServiceModel = subServicesService.GetAllSubServicesByCategoryServiceId(x.CategoryId);
                subServiceModel.ForEach(y => {
                    var count = allSubServiceModelList.Where(z => z.ParentServiceId == y.ServiceId).Count();
                    if (count > 0)
                    {
                        x.CategoryDescription =Utility.Constants.SUB_SERVICE_TEXT;
                    }
                    else
                    {
                        x.CategoryDescription = Utility.Constants.SERVICE_TEXT;
                    }
                });

            });
            
            return serviceCategoryWithPrice;
        }
        #endregion
    }
}