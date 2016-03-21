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
    public class DistrictsController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        DistrictService districtService = new DistrictService();

        // GET: api/Districts
        public IHttpActionResult GetDistrict()
        {
            List<DistrictModel> districtList = new List<DistrictModel>();
            districtList = districtService.GetAllDistrict().Select(x=>new DistrictModel {
                CityId=x.CityId,
                Description= CommonFunctions.ReadResourceValue(x.Description),
                DistrictId=x.DistrictId
            }).ToList();
            return Json(districtList);
        }

        // GET: api/Districts/5
        [ResponseType(typeof(District))]
        public IHttpActionResult GetDistrict(int id)
        {
            List<DistrictModel> districtList = new List<DistrictModel>();
            districtList = districtService.GetDistrictByCityId(id);
           // District district = db.District.Find(id);
            if (districtList == null)
            {
                return NotFound();
            }

            return Ok(districtList);
        }

        // PUT: api/Districts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDistrict(int id, District district)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != district.DistrictId)
            {
                return BadRequest();
            }

            db.Entry(district).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
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

        // POST: api/Districts
        [ResponseType(typeof(District))]
        public IHttpActionResult PostDistrict(District district)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.District.Add(district);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = district.DistrictId }, district);
        }

        // DELETE: api/Districts/5
        [ResponseType(typeof(District))]
        public IHttpActionResult DeleteDistrict(int id)
        {
            District district = db.District.Find(id);
            if (district == null)
            {
                return NotFound();
            }

            db.District.Remove(district);
            db.SaveChanges();

            return Ok(district);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistrictExists(int id)
        {
            return db.District.Count(e => e.DistrictId == id) > 0;
        }
    }
}