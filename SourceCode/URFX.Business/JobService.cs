using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Data.Infrastructure;

namespace URFX.Business
{
   public class JobService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Jobs
        public List<JobModel> GetAllJobs()
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);            
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> jobList = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, jobList);
            jobList = repo.GetAllIncluding("ServiceProvider").Where(x=>x.IsPaid==true).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobList, jobModelList);            
            return jobModelList;
        }
        #endregion

        #region Get Job By Id
        public JobModel GetJobById(int jobId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            JobModel jobModel = new JobModel();
            Job job = new Job();
            AutoMapper.Mapper.Map(jobModel, job);
            job = repo.GetAllIncluding("ServiceProvider").Where(x => x.JobId == jobId).OrderByDescending(x=>x.JobId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModel);
            return jobModel;
        }
        #endregion
        #region Get JobList By ServiceProvider Id
        public List<JobModel> GetJobListByServiceProviderId(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAllIncluding("Employee").Where(x => x.ServiceProviderId == serviceProviderId && x.IsPaid==true).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion

        #region Get Completed Tasks By ServiceProvider Id
        public List<JobModel> GetCompletedJobListByServiceProviderId(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId && x.Status==JobStatus.Completed).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get JobList By Employee Id
        public List<JobModel> GetJobListByEmployeeId(string employeeId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAllIncluding("Client").Where(x => x.EmployeeId == employeeId).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get Job By Employee Id
        public JobModel GetJobByEmployeeId(string employeeId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            JobModel jobModel = new JobModel();
            Job job = new Job();
            AutoMapper.Mapper.Map(jobModel, job);
            job = repo.GetAll().Where(x => x.EmployeeId == employeeId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModel);
            return jobModel;
        }
        #endregion
        #region Get Status Of Employee
        public List<JobModel> GetStatusOfEmployee(string employeeId,string status)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            //job = repo.GetAll().Where(x => status.Contains(x.Status.ToString())).ToList();
            job = repo.GetAll().Where(x => x.EmployeeId == employeeId && x.Status.ToString()==status).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get JobList By Client Id
        public List<JobModel> GetJobListByClientId(string clientId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAll().Where(x => x.ClientId == clientId).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get JobList By Client Id and status
        public List<JobModel> GetJobListByClientId(string clientId, JobStatus status)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAll().Where(x => x.ClientId == clientId &&x.Status==status && x.IsPaid == true).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get Jobs By EmployeeId and Status
        public List<JobModel> GetJobsByEmployeeId(string employeeId,JobStatus status)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAll().Where(x => x.EmployeeId == employeeId && x.Status == status && x.IsPaid==true ).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get Jobs By Employees Id
        public List<JobModel> GetJobListByEmployeeIds(string[] employeeIds)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAll().Where(x => employeeIds.Contains(x.EmployeeId.ToString())).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            // job = repo.GetAll().Where(x => x.EmployeeId == employeeId && x.Status == status).ToList();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Save Job
        public JobModel SaveJob(JobModel model)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            Job job = new Job();
            AutoMapper.Mapper.Map(model, job);
            repo.Insert(job);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, model);
            return model;
        }

        #endregion

        #region Update Job
        public JobModel UpadteJob(JobModel model)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            Job job = new Job();
            job = repo.GetAll().Where(x => x.JobId == model.JobId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, job);
            repo.Update(job);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, model);

            return model;
        }
        #endregion

        #region Delete Job
        public void DeleteJob(int jobId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            repo.Delete(x => x.JobId == jobId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Job By Id
        public bool CheckExistance(int jobId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            var job = repo.GetAll().Where(x => x.JobId == jobId).Count();
            //unitOfWork.Commit();
            if (job > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Paging For Jobs
        public List<JobModel> Paging(PagingModel model)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> jobList = new List<Job>();
            //ResponseMessage responseMessage = new ResponseMessage();
            //PagingInfo Info = new PagingInfo();
            string searchparam = model.SearchText == null ? "" : model.SearchText;
            jobList = repo.GetAll().Where(x => x.Description.ToLower().Contains(searchparam.ToLower())).OrderByDescending(x=>x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(jobList, jobModelList);
            return jobModelList;
        }
        #endregion
        #region Get JobList By ServiceProvider Id With client
        public List<JobModel> GetJobListByServiceProviderIdWithClient(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            JobRepository repo = new JobRepository(unitOfWork);
            List<JobModel> jobModelList = new List<JobModel>();
            List<Job> job = new List<Job>();
            AutoMapper.Mapper.Map(jobModelList, job);
            job = repo.GetAllIncluding("Client").Where(x => x.ServiceProviderId == serviceProviderId && x.IsPaid == true).OrderByDescending(x => x.JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(job, jobModelList);
            return jobModelList;
        }
        #endregion

    }
}
