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
using URFX.Web.Models;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    public class CitiesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        CityService cityService = new CityService();
        // GET: api/Cities
       public List<CityModel> GetCity()
        {
            List<CityModel> cityList = new List<CityModel>();
            List<CityBindingModel> cityBindingModel = new List<CityBindingModel>();
            AutoMapper.Mapper.Map(cityBindingModel, cityList);
            cityList = cityService.GetAllCiites().Select(x => new CityModel
            {
                CityId = x.CityId,
                Description = CommonFunctions.ReadResourceValue(x.Description)
            }).ToList();
            AutoMapper.Mapper.Map(cityList, cityBindingModel);
            return cityList;
        }
        // GET: api/Cities/5
        [ResponseType(typeof(City))]
        public IHttpActionResult GetCity(int id)
        {
            City city = db.City.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // PUT: api/Cities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCity(int id, City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.CityId)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
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

        // POST: api/Cities
        [ResponseType(typeof(City))]
        public IHttpActionResult PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.City.Add(city);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        [ResponseType(typeof(City))]
        public IHttpActionResult DeleteCity(int id)
        {
            City city = db.City.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            db.City.Remove(city);
            db.SaveChanges();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(int id)
        {
            return db.City.Count(e => e.CityId == id) > 0;
        }
    }
}