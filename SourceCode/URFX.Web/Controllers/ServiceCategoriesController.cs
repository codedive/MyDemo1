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
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [RoutePrefix("api/ServiceCategories")]
    public class ServiceCategoriesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        ServiceCategoryService serviceCategoryService = new ServiceCategoryService();

        // GET: api/ServiceCategories
        public List<ServiceCategoryModel> GetServiceCategories()
        {
            List<ServiceCategoryModel> serviceCategoryList = new List<ServiceCategoryModel>();
            serviceCategoryList = serviceCategoryService.GetAllServiceCategories().Select(x => new ServiceCategoryModel()
            {
                Description = CommonFunctions.ReadResourceValue(x.Description),
                ServiceCategoryId = x.ServiceCategoryId,
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServiceCategory(int id, ServiceCategory serviceCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceCategory.ServiceCategoryId)
            {
                return BadRequest();
            }

            db.Entry(serviceCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceCategoryExists(id))
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

        // POST: api/ServiceCategories
        [ResponseType(typeof(ServiceCategory))]
        public IHttpActionResult PostServiceCategory(ServiceCategory serviceCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServiceCategories.Add(serviceCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = serviceCategory.ServiceCategoryId }, serviceCategory);
        }

        // DELETE: api/ServiceCategories/5
        [ResponseType(typeof(ServiceCategory))]
        public IHttpActionResult DeleteServiceCategory(int id)
        {
            ServiceCategory serviceCategory = db.ServiceCategories.Find(id);
            if (serviceCategory == null)
            {
                return NotFound();
            }

            db.ServiceCategories.Remove(serviceCategory);
            db.SaveChanges();

            return Ok(serviceCategory);
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
    }
}