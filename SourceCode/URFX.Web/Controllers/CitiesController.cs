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
    [Authorize]
    public class CitiesController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        CityService cityService = new CityService();
        // GET: api/Cities
        [AllowAnonymous]
       public IHttpActionResult GetCity()
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
           // return Ok("{\"response\":" + Json(cityBindingModel) + "}");
            return Json(cityBindingModel);
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
        public IHttpActionResult PutCity(CityBindingModel model)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                CityModel cityModel = new CityModel();
                AutoMapper.Mapper.Map(model, cityModel);
                cityModel = cityService.UpadteCity(cityModel);
                AutoMapper.Mapper.Map(cityModel, model);
                return Ok(model);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Cities
        [ResponseType(typeof(City))]
        public IHttpActionResult PostCity(CityBindingModel model)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                CityModel cityModel = new CityModel();
                AutoMapper.Mapper.Map(model, cityModel);
                cityModel = cityService.SaveCity(cityModel);
                AutoMapper.Mapper.Map(cityModel, model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }
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