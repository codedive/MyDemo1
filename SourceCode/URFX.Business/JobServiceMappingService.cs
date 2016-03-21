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
   public class JobServiceMappingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get Job Service Mapping RelationShip
        public List<JobServiceMappingModel> GetJobServiceMapping()
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            List<JobServiceMappingModel> jobServiceMappingModelList = new List<JobServiceMappingModel>();
            List<JobServiceMapping> jobServiceMapping = new List<JobServiceMapping>();
            AutoMapper.Mapper.Map(jobServiceMappingModelList, jobServiceMapping);
            jobServiceMapping = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, jobServiceMappingModelList);
            return jobServiceMappingModelList;
        }
        #endregion

        #region Get Job Service Mapping RelationShip By Id
        public JobServiceMappingModel GetJobServiceMappingById(int jobServiceMappingId)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
            JobServiceMapping jobServiceMapping = new JobServiceMapping();
            AutoMapper.Mapper.Map(jobServiceMappingModel, jobServiceMapping);
            jobServiceMapping = repo.GetAll().Where(x => x.JobServiceMappingId == jobServiceMappingId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, jobServiceMappingModel);
            return jobServiceMappingModel;
        }
        #endregion
        #region Get Job Service Mapping RelationShip By JobId
        public JobServiceMappingModel GetJobServiceMappingByJobId(int jobId)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            JobServiceMappingModel jobServiceMappingModel = new JobServiceMappingModel();
            JobServiceMapping jobServiceMapping = new JobServiceMapping();
            AutoMapper.Mapper.Map(jobServiceMappingModel, jobServiceMapping);
            jobServiceMapping = repo.GetAll().Where(x => x.JobId == jobId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, jobServiceMappingModel);
            return jobServiceMappingModel;
        }
        #endregion
        #region Get Job Service Mapping RelationShip By JobId
        public List<JobServiceMappingModel> GetJobServiceMappingListByJobId(int jobId)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
            List<JobServiceMapping> jobServiceMapping = new List<JobServiceMapping>();
            AutoMapper.Mapper.Map(jobServiceMappingModel, jobServiceMapping);
            jobServiceMapping = repo.GetAll().Where(x => x.JobId == jobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, jobServiceMappingModel);
            return jobServiceMappingModel;
        }
        #endregion

        #region Get Job Service Mapping RelationShip By JobId
        public List<JobServiceMappingModel> GetJobServiceMappingByJobIds(string[] jobIds)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            List<JobServiceMappingModel> jobServiceMappingModel = new List<JobServiceMappingModel>();
            List<JobServiceMapping> jobServiceMapping = new List<JobServiceMapping>();
            AutoMapper.Mapper.Map(jobServiceMappingModel, jobServiceMapping);
            jobServiceMapping = repo.GetAll().Where(x => jobIds.Contains(x.JobId.ToString())).ToList();
            //unitOfWork.Commit();
            //jobServiceMapping = repo.GetAll().Where(x => x.JobId == jobId).FirstOrDefault();
            AutoMapper.Mapper.Map(jobServiceMapping, jobServiceMappingModel);
            return jobServiceMappingModel;
        }
        #endregion

        #region Save Job Service Mapping RelationShip
        public JobServiceMappingModel SaveJobServiceMapping(JobServiceMappingModel model)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            JobServiceMapping jobServiceMapping = new JobServiceMapping();
            AutoMapper.Mapper.Map(model, jobServiceMapping);
            repo.Insert(jobServiceMapping);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, model);
            return model;
        }

        #endregion

        #region Update Job Service Mapping RelationShip
        public JobServiceMappingModel UpadteJobServiceMapping(JobServiceMappingModel model)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            JobServiceMapping jobServiceMapping = new JobServiceMapping();
            jobServiceMapping = repo.GetAll().Where(x => x.JobServiceMappingId == model.JobServiceMappingId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, jobServiceMapping);
            repo.Update(jobServiceMapping);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobServiceMapping, model);
            return model;
        }
        #endregion

        #region Delete Job Service Mapping RelationShip
        public void DeleteJobServiceMapping(int jobServiceMappingId)
        {
            //unitOfWork.StartTransaction();
            JobServiceMappingRepository repo = new JobServiceMappingRepository(unitOfWork);
            repo.Delete(x => x.JobServiceMappingId == jobServiceMappingId);
            //unitOfWork.Commit();

        }
        #endregion

        //#region Check Existance Of Employee By Id
        //public bool CheckExistance(string employeeId)
        //{
        //    EmployeeRepository repo = new EmployeeRepository(unitOfWork);
        //    var employee = repo.GetAll().Where(x => x.EmployeeId == employeeId).Count();
        //    if (employee > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region Paging For Employee
        //public List<EmployeeModel> Paging(PagingModel model)
        //{
        //    EmployeeRepository repo = new EmployeeRepository(unitOfWork);
        //    List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
        //    List<Employee> employeeList = new List<Employee>();
        //    //ResponseMessage responseMessage = new ResponseMessage();
        //    //PagingInfo Info = new PagingInfo();
        //    string searchparam = model.SearchText == null ? "" : model.SearchText;
        //    employeeList = repo.GetAll().Where(x => x.CompanyName.ToLower().Contains(searchparam.ToLower())).ToList();
        //    AutoMapper.Mapper.Map(employeeList, employeeModelList);
        //    return employeeModelList;
        //}
        //#endregion
    }
}
