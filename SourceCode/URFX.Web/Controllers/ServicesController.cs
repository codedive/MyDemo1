using Microsoft.AspNet.Identity;
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
using URFX.Web.Infrastructure;
using URFX.Web.Models;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        SubServicesService subServicesService = new SubServicesService();
        ServiceCategoryService servicesCategoryService = new ServiceCategoryService();
        //GET: api/Services


       [HttpGet]
        public List<Service> GetServices()
        {
            return db.Services.ToList();
        }


        #region Get All Services
        [HttpGet]
        [Route("ActualServicesByServiceId")]
        public List<SubServiceModel> ActualServicesByServiceId(int serviceId)
        {
            List<SubServiceModel> serviceCategoryList = new List<SubServiceModel>();
            serviceCategoryList = subServicesService.GetAllActualServiceByServiceId(serviceId).ToList();

            foreach (var x in serviceCategoryList)
            {
                ServiceCategoryModel serviceCategoryModel = null;
                if (x.ServiceCategoryId != null)
                {
                    serviceCategoryModel = servicesCategoryService.GetCategoryServiceById(Convert.ToInt32(x.ServiceCategoryId));
                }
                else
                {
                    var service = subServicesService.GetSubServiceById(x.ParentServiceId);
                    if (service != null)
                    {
                        serviceCategoryModel = servicesCategoryService.GetCategoryServiceById(Convert.ToInt32(service.ServiceCategoryId));
                    }
                }

                if (serviceCategoryModel != null)
                {
                    x.ServiceCategory = new ServiceCategory();
                    x.ServiceCategory.ServiceCategoryId = serviceCategoryModel.ServiceCategoryId;
                    x.ServiceCategory.CategoryPicturePath = serviceCategoryModel.CategoryPicturePath;
                    x.ServiceCategory.Description = serviceCategoryModel.Description;
                }
                var subServices = subServicesService.GetAllSubServicesByServiceId(x.ServiceId);
                x.IsActualService = subServices.Count > 0 ? false : true;
            }

            //serviceCategoryList = subServicesService.GetAllSubServices().Select(x => new SubServiceModel()
            //{
            //    Description = CommonFunctions.ReadResourceValue(x.Description),
            //    ServiceCategoryId = x.ServiceCategoryId,

            //}).ToList();
            return serviceCategoryList;
        }
        #endregion

        #region Get Services By Category Id
        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        [HttpGet]
        public List<SubServiceModel> GetService(int id)
        {

            List<SubServiceModel> subServiceList = new List<SubServiceModel>();

            List<SubServiceModel> tempSubServiceList = new List<SubServiceModel>();
            tempSubServiceList = subServicesService.GetAllSubServicesByCategoryServiceId(id).Select(x => new SubServiceModel()
            {
                Description = x.Description,
                ServiceCategoryId = x.ServiceCategoryId,
                HourlyRate = x.HourlyRate,
                IsActive = x.IsActive,
                ParentServiceId = x.ParentServiceId,
                CreatedDate = x.CreatedDate,
                Price = x.Price,
                ServiceId = x.ServiceId,
                VisitRate = x.VisitRate,
                ServiceCategory = x.ServiceCategory,
                SubServices = x.SubServices,
            }).ToList();
            foreach (var x in tempSubServiceList)
            {
                var subServices = subServicesService.GetAllSubServicesByServiceId(x.ServiceId);
                x.IsActualService = subServices.Count > 0 ? false : true;
            }

            subServiceList.AddRange(tempSubServiceList);



            return subServiceList;


        }
        #endregion


        #region Get Sub Service By ServiceId
        // GET: api/Services/SubServicesByServiceIds
        [ResponseType(typeof(Service))]
        [Route("SubServicesByServiceIds")]
        [HttpGet]
        public List<SubServiceModel> SubServicesByServiceIds(int id)
        {

            List<SubServiceModel> subServiceList = new List<SubServiceModel>();

            List<SubServiceModel> tempSubServiceList = new List<SubServiceModel>();
            tempSubServiceList = subServicesService.GetAllSubServicesByServiceId(Convert.ToInt32(id)).Select(x => new SubServiceModel()
            {
                Description = CommonFunctions.ReadResourceValue(x.Description),
                ServiceCategoryId = x.ServiceCategoryId,
                HourlyRate = x.HourlyRate,
                IsActive = x.IsActive,
                ParentServiceId = x.ParentServiceId,
                CreatedDate = x.CreatedDate,
                Price = x.Price,
                ServiceId = x.ServiceId,
                VisitRate = x.VisitRate,
                ServiceCategory = x.ServiceCategory,
                SubServices = x.SubServices
            }).ToList();
            subServiceList.AddRange(tempSubServiceList);



            return subServiceList;

            //Service service = db.Services.Find(id);
            //if (service == null)
            //{
            //    return NotFound();
            //}

            //return Ok(service);
        }
        #endregion


        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        [Route("ServicesByCategoryIds")]
        [HttpGet]
        public List<SubServiceModel> ServicesByCategoryIds()
        {
            IEnumerable<string> Ids;
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            if (Request.Headers.TryGetValues("CategoryIds", out Ids))
            {
                string[] categoryIds = Ids.ToList()[0].Split(',').ToArray();
                foreach (var Id in categoryIds)
                {
                    List<SubServiceModel> tempSubServiceList = new List<SubServiceModel>();
                    tempSubServiceList = subServicesService.GetAllSubServicesByCategoryServiceId(Convert.ToInt32(Id)).Select(x => new SubServiceModel()
                    {
                        Description = CommonFunctions.ReadResourceValue(x.Description),
                        ServiceCategoryId = x.ServiceCategoryId,
                        HourlyRate = x.HourlyRate,
                        IsActive = x.IsActive,
                        ParentServiceId = x.ParentServiceId,
                        CreatedDate = x.CreatedDate,
                        Price = x.Price,
                        ServiceId = x.ServiceId,
                        VisitRate = x.VisitRate,
                        ServiceCategory = x.ServiceCategory,
                        SubServices = x.SubServices
                    }).ToList();
                    subServiceList.AddRange(tempSubServiceList);
                }
            }

            return subServiceList;

            //Service service = db.Services.Find(id);
            //if (service == null)
            //{
            //    return NotFound();
            //}

            //return Ok(service);
        }



        [ResponseType(typeof(Service))]
        [Route("ServicesByCategoryId")]
        [HttpGet]
        public List<SubServiceModel> ServicesByCategoryId(int categoryId)
        {
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            var subServicesByCategoryId = subServicesService.GetServicesByCategoryId(categoryId);

            SubServiceModel subService = new SubServiceModel();
            subService.ServiceId = 0;
            subService.Description = "------Root Service------";
            subServiceList.Add(subService);
            if (subServicesByCategoryId != null && subServicesByCategoryId.Count() > 0)
            {

                foreach (var service in subServicesByCategoryId)
                {

                    if (service.ServiceCategoryId != null)
                    {
                        subService = new SubServiceModel();
                        subService.ServiceId = service.ServiceId;
                        subService.Description = service.Description;
                        subServiceList.Add(subService);

                        //var parentServiceIds = subServicesByCategoryId.Where(x => x.ServiceCategoryId == null && x.ParentServiceId == service.ServiceId).Select(x => x.ServiceId).ToArray();

                        //foreach (var parentServiceId in parentServiceIds)
                        //{
                        //    var parentService = subServicesService.GetSubServiceById(parentServiceId);
                        //    subService = new SubServiceModel();
                        //    subService.ServiceId = parentService.ServiceId;
                        //    subService.Description = service.Description + " >> " + parentService.Description;
                        //    subServiceList.Add(subService);
                        //}
                    }
                }
            }
            return subServiceList;
        }


        //// GET: api/Services/5
        //[ResponseType(typeof(Service))]
        //[Route("SubServicesByServiceIds")]
        //[HttpGet]
        //public List<SubServiceModel> SubServicesByServiceIds()
        //{
        //    IEnumerable<string> Ids;
        //    List<SubServiceModel> subServiceList = new List<SubServiceModel>();
        //    if (Request.Headers.TryGetValues("ServiceIds", out Ids))
        //    {
        //        string[] serviceIds = Ids.ToList()[0].Split(',').ToArray();
        //        foreach (var Id in serviceIds)
        //        {
        //            List<SubServiceModel> tempSubServiceList = new List<SubServiceModel>();
        //            tempSubServiceList = subServicesService.GetAllSubServicesByServiceId(Convert.ToInt32(Id)).Select(x => new SubServiceModel()
        //            {
        //                Description = CommonFunctions.ReadResourceValue(x.Description),
        //                ServiceCategoryId = x.ServiceCategoryId,
        //                HourlyRate = x.HourlyRate,
        //                IsActive = x.IsActive,
        //                ParentServiceId = x.ParentServiceId,
        //                CreatedDate = x.CreatedDate,
        //                Price = x.Price,
        //                ServiceId = x.ServiceId,
        //                VisitRate = x.VisitRate,
        //                ServiceCategory = x.ServiceCategory,
        //                SubServices = x.SubServices
        //            }).ToList();
        //            subServiceList.AddRange(tempSubServiceList);
        //        }
        //    }

        //    return subServiceList;

        //    //Service service = db.Services.Find(id);
        //    //if (service == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return Ok(service);
        //}

        // PUT: api/Services/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutService(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SubServiceBindingModel modal = new SubServiceBindingModel();
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
                    modal = JsonConvert.DeserializeObject<SubServiceBindingModel>(resultModel.FormData["model"]);
                    modal.CreatedDate = DateTime.Now;
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
                                modal.ServicePicturePath = fileName;
                            }
                        }
                    }
                }

                SubServiceModel serviceModel = new SubServiceModel();
                SubServiceModel checkserviceModel = new SubServiceModel();
                if (modal.ServiceCategoryId == 0)
                {
                    modal.ServiceCategoryId = null;
                }
                AutoMapper.Mapper.Map(modal, serviceModel);
                serviceModel = subServicesService.UpadteSubService(serviceModel);
                //checkserviceModel = subServicesService.GetSubServiceById(id);
                //if(checkserviceModel.ParentServiceId == modal.ParentServiceId)
                //{

                //}
                //else
                //{
                //    subServicesService.DeleteSubService(checkserviceModel.ServiceId);
                //    serviceModel = subServicesService.GetSubServiceById(serviceModel.ServiceId);
                //    modal.ServiceId = 0;
                //    AutoMapper.Mapper.Map(modal, serviceModel);
                //    serviceModel = subServicesService.SaveSubService(serviceModel);
                //}




                return Ok(serviceModel);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> PostService()
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                SubServiceBindingModel modal = new SubServiceBindingModel();
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
                    modal = JsonConvert.DeserializeObject<SubServiceBindingModel>(resultModel.FormData["model"]);
                    modal.CreatedDate = DateTime.Now;
                    modal.ServiceCategoryId = modal.ServiceCategoryId > 0 ? modal.ServiceCategoryId : null;
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
                                modal.ServicePicturePath = fileName;
                            }
                        }
                    }
                }

                SubServiceModel serviceModel = new SubServiceModel();
                AutoMapper.Mapper.Map(modal, serviceModel);
                bool isParentActualService = true;
                var parentServiceOfServiceToInsert = subServicesService.GetSubServiceById(modal.ParentServiceId);
                var parentServiceOfServiceToInsertChildren = subServicesService.GetAllSubServicesByServiceId(parentServiceOfServiceToInsert.ServiceId).ToList();

                isParentActualService = parentServiceOfServiceToInsertChildren.Count() > 0 ? false : true;

                if (isParentActualService)
                {
                    serviceModel.ServiceCategoryId = null;
                }

                serviceModel = subServicesService.SaveSubService(serviceModel);
                return Ok(serviceModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {

            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceId == id) > 0;
        }

        #region Post service
        [Route("SaveService")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveService()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest();
                }
                var root = HttpContext.Current.Server.MapPath(Utility.Constants.BASE_FILE_UPLOAD_PATH);
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var resultModel = await Request.Content.ReadAsMultipartAsync(provider);
                if (resultModel.FormData["model"] == null)
                {
                    return BadRequest();
                }

                var model = resultModel.FormData["model"];
                Service serviceModal = new Service();
                serviceModal = JsonConvert.DeserializeObject<Service>(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
    }
}