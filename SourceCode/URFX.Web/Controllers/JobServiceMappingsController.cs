using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Web.Models;

namespace URFX.Web.Controllers
{
    [Authorize]
    public class JobServiceMappingsController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        JobServiceMappingService jobServiceMappingService = new JobServiceMappingService();
        JobServicePictruesMappingService jobServicePictruesMappingService = new JobServicePictruesMappingService();

        #region Get Job Service Mapping RelationShip
        // GET: api/JobServiceMappings
        public List<JobServiceMappingBindingModel> GetJobServiceMapping()
        {
            List<JobServiceMappingBindingModel> jobServiceMappingBindingModelList = new List<JobServiceMappingBindingModel>();
            List<JobServiceMappingModel> jobServiceMappingModelList = new List<JobServiceMappingModel>();
            AutoMapper.Mapper.Map(jobServiceMappingBindingModelList, jobServiceMappingModelList);
            jobServiceMappingModelList = jobServiceMappingService.GetJobServiceMapping();
            AutoMapper.Mapper.Map(jobServiceMappingModelList, jobServiceMappingBindingModelList);
            return jobServiceMappingBindingModelList;
            //return db.JobServiceMapping;
        }
        #endregion

        #region Get Job Service Picture RelationShip By Id
        // GET: api/JobServiceMappings/5
        [ResponseType(typeof(JobServiceMappingBindingModel))]
        public IHttpActionResult GetJobServiceMapping(int id)
        {
            JobServiceMappingBindingModel jobServiceMappingBindingModel = new JobServiceMappingBindingModel();
            JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
            AutoMapper.Mapper.Map(jobServiceMappingBindingModel, jobServiceMappingModel);
            jobServiceMappingModel = jobServiceMappingService.GetJobServiceMappingById(id);
            AutoMapper.Mapper.Map(jobServiceMappingModel, jobServiceMappingBindingModel);
            return Ok(jobServiceMappingBindingModel);
        }
        #endregion

        #region Update Job Service Mapping RelationShip
        // PUT: api/JobServiceMappings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobServiceMapping(JobServiceMappingBindingModel model)
        {
            try {
                JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                AutoMapper.Mapper.Map(model, jobServiceMappingModel);
                jobServiceMappingModel = jobServiceMappingService.UpadteJobServiceMapping(jobServiceMappingModel);
                AutoMapper.Mapper.Map(jobServiceMappingModel, model);
                return Ok(model);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        #endregion

        #region Save Job Service Mapping RelationShip
        // POST: api/JobServiceMappings
        [ResponseType(typeof(JobServiceMappingBindingModel))]
        public IHttpActionResult PostJobServiceMapping(JobServiceMappingBindingModel model)
        {

            JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
            JobServicePictureMappingModel jobServicePictureMappingModel = new JobServicePictureMappingModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var root = HttpContext.Current.Server.MapPath(Utility.Constants.BASE_FILE_UPLOAD_PATH);
            Directory.CreateDirectory(root);
            
                //if (ProfileImage != null)
                //{

                //    var filename = Guid.NewGuid() + ".jpg";
                //    ProfileImage.SaveAs(Path.Combine(root, Utility.Constants.JOB_SERVICE_IMAGES_PATH, filename));
                //    jobServicePictureMappingModel.ImagePath = filename;

                //}
                //else
                //{
                //    jobServicePictureMappingModel.ImagePath = "";
                //}
            AutoMapper.Mapper.Map(model, jobServiceMappingModel);
            jobServiceMappingModel = jobServiceMappingService.SaveJobServiceMapping(jobServiceMappingModel);
            AutoMapper.Mapper.Map(jobServiceMappingModel, model);
            jobServicePictureMappingModel.JobServiceMappingId = model.JobServiceMappingId;
            string fileName;
            if (HttpContext.Current.Request.Files != null)
            {
                for (var i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    var file = HttpContext.Current.Request.Files[i];
                    fileName = file.FileName;
                    file.SaveAs(Path.Combine(root, Utility.Constants.JOB_SERVICE_IMAGES_PATH, fileName));
                    jobServicePictureMappingModel.ImagePath = fileName;
                    jobServicePictureMappingModel = jobServicePictruesMappingService.SaveJobServicePictureMapping(jobServicePictureMappingModel);

                }
            }
            return Ok(model);
           
        }
        #endregion

        #region Delete Job Service Mapping RelationShip
        // DELETE: api/JobServiceMappings/5
        [ResponseType(typeof(JobServiceMapping))]
        public IHttpActionResult DeleteJobServiceMapping(int id)
        {
            try {
                jobServiceMappingService.DeleteJobServiceMapping(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobServiceMappingExists(int id)
        {
            return db.JobServiceMapping.Count(e => e.JobServiceMappingId == id) > 0;
        }
    }
}