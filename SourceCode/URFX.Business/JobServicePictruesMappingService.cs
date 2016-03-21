using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Infrastructure;

namespace URFX.Business
{
  public class JobServicePictruesMappingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get Job Service Picture Mapping RelationShip
        public List<JobServicePictureMappingModel> GetJobServicePictureMapping()
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            List<JobServicePictureMappingModel> jobServicePictureMappingModelList = new List<JobServicePictureMappingModel>();
            List<JobServicePicturesMapping> jobServicePicturesMapping = new List<JobServicePicturesMapping>();
            AutoMapper.Mapper.Map(jobServicePictureMappingModelList, jobServicePicturesMapping);
            jobServicePicturesMapping = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServicePicturesMapping, jobServicePictureMappingModelList);
            return jobServicePictureMappingModelList;
        }
        #endregion

        #region Get Job Service Picture Mapping RelationShip By Id
        public JobServicePictureMappingModel GetJobServicePictureMappingById(int id)
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            JobServicePictureMappingModel jobServicePictureMappingModel = new JobServicePictureMappingModel();
            JobServicePicturesMapping jobServicePicturesMapping = new JobServicePicturesMapping();
            AutoMapper.Mapper.Map(jobServicePictureMappingModel, jobServicePicturesMapping);
            jobServicePicturesMapping = repo.GetAll().Where(x => x.Id == id).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServicePicturesMapping, jobServicePictureMappingModel);
            return jobServicePictureMappingModel;
        }
        #endregion

        #region Get Job Service Picture Mapping RelationShip By JobServiceMappingId
        public List<JobServicePictureMappingModel> GetJobServicePictureMappingByJobServiceMappingId(int JobServiceMappingId)
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            List<JobServicePictureMappingModel> jobServicePictureMappingModelList = new List<JobServicePictureMappingModel>();
            List<JobServicePicturesMapping> jobServicePicturesMappingList = new List<JobServicePicturesMapping>();
            AutoMapper.Mapper.Map(jobServicePictureMappingModelList, jobServicePicturesMappingList);
            jobServicePicturesMappingList = repo.GetAll().Where(x => x.JobServiceMappingId == JobServiceMappingId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServicePicturesMappingList, jobServicePictureMappingModelList);
            return jobServicePictureMappingModelList;
        }
        #endregion

        #region Save Job Service Picture Mapping RelationShip
        public JobServicePictureMappingModel SaveJobServicePictureMapping(JobServicePictureMappingModel model)
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            JobServicePicturesMapping jobServicePicturesMapping = new JobServicePicturesMapping();
            AutoMapper.Mapper.Map(model, jobServicePicturesMapping);
            repo.Insert(jobServicePicturesMapping);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServicePicturesMapping, model);
            return model;
        }

        #endregion

        #region Update Job Service Picture Mapping RelationShip
        public JobServicePictureMappingModel UpadteJobServiceMapping(JobServicePictureMappingModel model)
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            JobServicePicturesMapping jobServicePicturesMapping = new JobServicePicturesMapping();
            jobServicePicturesMapping = repo.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
            AutoMapper.Mapper.Map(model, jobServicePicturesMapping);
            repo.Update(jobServicePicturesMapping);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServicePicturesMapping, model);
            return model;
        }
        #endregion

        #region Delete Job Service Picture Mapping RelationShip
        public void DeleteJobServiceMapping(int id)
        {
            //unitOfWork.StartTransaction();
            JobServicePicturesMappingRepository repo = new JobServicePicturesMappingRepository(unitOfWork);
            repo.Delete(x => x.Id == id);
            //unitOfWork.Commit();

        }
        #endregion
    }
}
