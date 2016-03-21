using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.DataEntity;
using URFX.Data.Entities;
using URFX.Data.Infrastructure;
using URFX.Data;
using URFX.Data.DataEntity.DomainModel;
namespace URFX.Business
{
   public class EmployeeService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();


        #region Get All Employees
        public List<EmployeeModel> GetAllEmployees()
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            List<Employee> employee = new List<Employee>();
            AutoMapper.Mapper.Map(employeeList, employee);
            employee = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employee, employeeList);
            return employeeList;
        }
        #endregion

        #region Get Employee By Id
        public EmployeeModel GetEmployeeById(string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            EmployeeModel employeeModel = new EmployeeModel();
            Employee employee = new Employee();
            AutoMapper.Mapper.Map(employeeModel, employee);
            Dispose(true);
            employee = repo.GetAll().Where(x => x.EmployeeId == employeeId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employee, employeeModel);
            return employeeModel;
        }
        #endregion

        #region Get Employee List By  ServiceProviderId
        public List<EmployeeModel> GetEmployeeListByServiceProviderId(string[] employeeIds)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            List<Employee> employee = new List<Employee>();
            //AutoMapper.Mapper.Map(employeeModel, employee);
            employee = repo.GetAll().Where(x => employeeIds.Contains(x.EmployeeId.ToString())).ToList();
            //unitOfWork.Commit();
            //employee = repo.GetAll().Where(x => x.EmployeeId == serviceProviderId).ToList();
            AutoMapper.Mapper.Map(employee, employeeModel);
            return employeeModel;
        }
        #endregion

        #region Save Employee
        public EmployeeModel SaveEmployee(EmployeeModel model)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            Employee employee = new Employee();
            AutoMapper.Mapper.Map(model, employee);
            repo.Insert(employee);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employee, model);
            return model;
        }

        #endregion

        #region Update Employee
        public EmployeeModel UpadteEmployee(EmployeeModel model)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            Employee employee = new Employee();
            employee = repo.GetAll().Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, employee);
            repo.Update(employee);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employee, model);
            return model;
        }
        #endregion

        #region Delete Employee
        public void DeleteEmployee(string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            repo.Delete(x => x.EmployeeId == employeeId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Employee By Id
        public bool CheckExistance(string employeeId)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            var employee = repo.GetAll().Where(x => x.EmployeeId == employeeId).Count();
            //unitOfWork.Commit();
            if (employee > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Paging For Employee
        public List<EmployeeModel> Paging(PagingModel model)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repo = new EmployeeRepository(unitOfWork);
            List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
            List<Employee> employeeList = new List<Employee>();
            //ResponseMessage responseMessage = new ResponseMessage();
            //PagingInfo Info = new PagingInfo();
            string searchparam = model.SearchText == null ? "" : model.SearchText;
            if(searchparam!="")
            employeeList = repo.GetAll().Where(x=>x.FirstName.ToLower().Contains(searchparam.ToLower())).ToList();
          else
                employeeList = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(employeeList, employeeModelList);
            return employeeModelList;
        }
        #endregion

        #region Get Employee info List By employeeIds
        public List<EmployeeModel> GetEmployeeInfoListByEmployeeIds(string[] employeeIds)
        {
            //unitOfWork.StartTransaction();
            EmployeeRepository repoEmployee = new EmployeeRepository(unitOfWork);
            List<EmployeeModel> employeeModel = new List<EmployeeModel>();
            List<Employee> employee = new List<Employee>();
            employee = repoEmployee.GetAll().Where(x => employeeIds.Contains(x.EmployeeId.ToString())).ToList();
            //unitOfWork.Commit();
            //rating = repo.GetAll().Where(x => x.JobId == JobId).ToList();
            AutoMapper.Mapper.Map(employee, employeeModel);
            return employeeModel;
        }
        #endregion
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

        }

    }
}
