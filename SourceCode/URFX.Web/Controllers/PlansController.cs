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
using URFX.Data.Entities;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [RoutePrefix("api/Plans")]
    public class PlansController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        PlanService planService = new PlanService();
        // GET: api/Plans
        public List<Plan> GetPlans()
        {
            List<Plan> planList = new List<Plan>();
            planList= planService.GetPlans().Select(x=>new Plan {
            Description= CommonFunctions.ReadResourceValue(x.Description),
            ApplicationFee=x.ApplicationFee,
            PlanId=x.PlanId,
            CreatedDate=x.CreatedDate,
            Detail=CommonFunctions.ReadResourceValue(x.Detail),
            IsActive=x.IsActive,
            PerVisitPercentage=x.PerVisitPercentage,
            TeamRegistrationFee=x.TeamRegistrationFee,
            TeamRegistrationType=x.TeamRegistrationType
            
            }).ToList();
            return planList;
        }

        // GET: api/Plans/5
        [ResponseType(typeof(Plan))]
        public IHttpActionResult GetPlan(int id)
        {
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        // PUT: api/Plans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlan(int id, Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plan.PlanId)
            {
                return BadRequest();
            }

            db.Entry(plan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
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

        // POST: api/Plans
        [ResponseType(typeof(Plan))]
        public IHttpActionResult PostPlan(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Plans.Add(plan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = plan.PlanId }, plan);
        }

        // DELETE: api/Plans/5
        [ResponseType(typeof(Plan))]
        public IHttpActionResult DeletePlan(int id)
        {
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }

            db.Plans.Remove(plan);
            db.SaveChanges();

            return Ok(plan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlanExists(int id)
        {
            return db.Plans.Count(e => e.PlanId == id) > 0;
        }
    }
}