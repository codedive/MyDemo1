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
using URFX.Web.Infrastructure;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        SubServicesService subServicesService = new SubServicesService();
        // GET: api/Services
        public IQueryable<Service> GetServices()
        {
            return db.Services;
        }

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


        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        [Route("SubServicesByServiceIds")]
        [HttpGet]
        public List<SubServiceModel> SubServicesByServiceIds()
        {
            IEnumerable<string> Ids;
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            if (Request.Headers.TryGetValues("ServiceIds", out Ids))
            {
                string[] serviceIds = Ids.ToList()[0].Split(',').ToArray();
                foreach (var Id in serviceIds)
                {
                    List<SubServiceModel> tempSubServiceList = new List<SubServiceModel>();
                    tempSubServiceList = subServicesService.GetAllSubServicesByServiceId(Convert.ToInt32(Id)).Select(x => new SubServiceModel()
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

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceId }, service);
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
    }
}