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
  public class EmployeeScheduleService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();


        #region Get All Employee Schedule
        public List<EmployeeScheduleModel> GetAllEmployeesSchedule()
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            List<EmployeeScheduleModel> employeeScheduleList = new List<EmployeeScheduleModel>();
            List<EmployeeSchedule> employeeSchedule = new List<EmployeeSchedule>();
            AutoMapper.Mapper.Map(employeeScheduleList, employeeSchedule);
            employeeSchedule = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, employeeScheduleList);
            return employeeScheduleList;
        }
        #endregion

        #region Get Employee Schedule By Employee Id
        public List<EmployeeScheduleModel> GetEmployeesScheduleByEmployeeId(string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            List<EmployeeScheduleModel> employeeScheduleList = new List<EmployeeScheduleModel>();
            List<EmployeeSchedule> employeeSchedule = new List<EmployeeSchedule>();
            AutoMapper.Mapper.Map(employeeScheduleList, employeeSchedule);
            employeeSchedule = repo.GetAll().Where(x=>x.EmployeeId== employeeId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, employeeScheduleList);
            return employeeScheduleList;
        }
        #endregion
        #region Get Employee Schedule By Job Id
        public EmployeeScheduleModel GetEmployeesScheduleByJobId(int jobid)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            EmployeeScheduleModel employeeScheduleList = new EmployeeScheduleModel();
           EmployeeSchedule employeeSchedule = new EmployeeSchedule();
            AutoMapper.Mapper.Map(employeeScheduleList, employeeSchedule);
            employeeSchedule = repo.GetAll().Where(x => x.JobId == jobid).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, employeeScheduleList);
            return employeeScheduleList;
        }
        #endregion

        #region Get Employee Schedule By Date And Time
        public List<EmployeeScheduleModel> GetEmployeesScheduleByDateAndTime(DateTime? start, DateTime? end,string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            List<EmployeeScheduleModel> employeeScheduleList = new List<EmployeeScheduleModel>();
            List<EmployeeSchedule> employeeSchedule = new List<EmployeeSchedule>();
            AutoMapper.Mapper.Map(employeeScheduleList, employeeSchedule);
            employeeSchedule = repo.GetAll().Where(x => x.Start >= start && x.End<=end && x.EmployeeId== employeeId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, employeeScheduleList);
            return employeeScheduleList;
        }
        #endregion

        #region Save Employee Schedule
        public EmployeeScheduleModel SaveEmployeeSchedule(EmployeeScheduleModel model)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            EmployeeSchedule employeeSchedule = new EmployeeSchedule();
            AutoMapper.Mapper.Map(model, employeeSchedule);
            repo.Insert(employeeSchedule);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, model);
            return model;
        }

        #endregion
        #region Update Employee Schedule
        public EmployeeScheduleModel UpdateEmployeeSchedule(EmployeeScheduleModel model)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            EmployeeSchedule employeeSchedule = new EmployeeSchedule();
            employeeSchedule = repo.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
            AutoMapper.Mapper.Map(model, employeeSchedule);
            repo.Update(employeeSchedule);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, model);
            return model;
        }

        #endregion

        #region Delete Employee Schedule
        public void DeleteEmployeeSchedule(int id)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            repo.Delete(x => x.Id == id);
            //unitOfWork.Commit();

        }
        #endregion

        #region Get Employee Schedule By Job Id and EmployeeId
        public EmployeeScheduleModel GetEmployeesScheduleByJobIdAndEmployeeId(int jobid,string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeScheduleRepository repo = new EmployeeScheduleRepository(unitOfWork);
            EmployeeScheduleModel employeeScheduleList = new EmployeeScheduleModel();
            EmployeeSchedule employeeSchedule = new EmployeeSchedule();
            AutoMapper.Mapper.Map(employeeScheduleList, employeeSchedule);
            employeeSchedule = repo.GetAll().Where(x => x.JobId == jobid && x.EmployeeId==employeeId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeSchedule, employeeScheduleList);
            return employeeScheduleList;
        }
        #endregion
    }
}
