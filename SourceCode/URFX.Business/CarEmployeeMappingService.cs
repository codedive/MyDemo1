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
   public class CarEmployeeMappingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Car Employee Mapping
        public List<CarEmployeeMappingModel> GetAllCarEmployeeMapping()
        {
           // //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            List<CarEmployeeMappingModel> carEmployeeMappingModelList = new List<CarEmployeeMappingModel>();
            List<CarEmployeeMapping> carEmployeeMappingList = new List<CarEmployeeMapping>();
            AutoMapper.Mapper.Map(carEmployeeMappingModelList, carEmployeeMappingList);
            carEmployeeMappingList = repo.GetAll().ToList();
            ////unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMappingList, carEmployeeMappingModelList);
            return carEmployeeMappingModelList;
        }
        #endregion

        #region Get Car Employee Mapping By Id
        public CarEmployeeMappingModel GetCarEmployeeMappingById(int carEmployeeMappingId)
        {
            ////unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
            CarEmployeeMapping carEmployeeMapping = new CarEmployeeMapping();
            AutoMapper.Mapper.Map(carEmployeeMappingModel, carEmployeeMapping);
            carEmployeeMapping = repo.GetAll().Where(x => x.CarTypeId == carEmployeeMappingId).FirstOrDefault();
           // //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMapping, carEmployeeMappingModel);
            return carEmployeeMappingModel;
        }
        #endregion
        #region Get Car Employee Mapping By Employee Id
        public CarEmployeeMappingModel GetCarEmployeeMappingByEmployeeId(string employeeId)
        {
           // //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            CarEmployeeMappingModel carEmployeeMappingModel = new CarEmployeeMappingModel();
            CarEmployeeMapping carEmployeeMapping = new CarEmployeeMapping();
            AutoMapper.Mapper.Map(carEmployeeMappingModel, carEmployeeMapping);
            carEmployeeMapping = repo.GetAll().Where(x => x.EmployeeId == employeeId).FirstOrDefault();
////unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMapping, carEmployeeMappingModel);
            return carEmployeeMappingModel;
        }
        #endregion

        #region Get Car Employee Mapping List By Employee Id
        public List<CarEmployeeMappingModel> GetCarEmployeeMappingListByEmployeeId(string employeeId)
        {
           // //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            List<CarEmployeeMappingModel> carEmployeeMappingModel = new List<CarEmployeeMappingModel>();
            List<CarEmployeeMapping> carEmployeeMapping = new List<CarEmployeeMapping>();
            AutoMapper.Mapper.Map(carEmployeeMappingModel, carEmployeeMapping);
            carEmployeeMapping = repo.GetAll().Where(x => x.EmployeeId == employeeId).ToList();
          //  //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMapping, carEmployeeMappingModel);
            return carEmployeeMappingModel;
        }
        #endregion

        #region Save Car Employee Mapping
        public CarEmployeeMappingModel SaveCarEmployeeMapping(CarEmployeeMappingModel model)
        {
           // //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            CarEmployeeMapping carEmployeeMapping = new CarEmployeeMapping();
            AutoMapper.Mapper.Map(model, carEmployeeMapping);
            repo.Insert(carEmployeeMapping);
           // //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMapping, model);
            return model;
        }

        #endregion

        #region Update Car Employee Mapping
        public CarEmployeeMappingModel UpadteCarEmployeeMapping(CarEmployeeMappingModel model)
        {
            ////unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            CarEmployeeMapping carEmployeeMapping = new CarEmployeeMapping();
            carEmployeeMapping = repo.GetAll().Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, carEmployeeMapping);
            repo.Update(carEmployeeMapping);
            ////unitOfWork.Commit();
            AutoMapper.Mapper.Map(carEmployeeMapping, model);
            return model;
        }
        #endregion

        #region Delete Car Employee Mapping
        public void DeleteCarEmployeeMapping(int carEmployeeMappingId)
        {
           // //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            repo.Delete(x => x.CarEmployeeMappingId == carEmployeeMappingId);
           // //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Car Employee Mapping By Id
        public bool CheckExistance(string EmployeeMappingId)
        {
          //  //unitOfWork.StartTransaction();
            CarEmployeeMappingRepository repo = new CarEmployeeMappingRepository(unitOfWork);
            var city = repo.GetAll().Where(x => x.EmployeeId == EmployeeMappingId).Count();
           // //unitOfWork.Commit();
            if (city > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
