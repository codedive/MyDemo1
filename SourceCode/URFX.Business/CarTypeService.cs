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
   public class CarTypeService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All CarTypes
        public List<CarTypeModel> GetAllCarTypes()
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            List<CarTypeModel> carTypeModelList = new List<CarTypeModel>();
            List<CarType> carTypeList = new List<CarType>();
            AutoMapper.Mapper.Map(carTypeModelList, carTypeList);
            carTypeList = repo.GetAll().OrderByDescending(x=>x.CarTypeId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carTypeList, carTypeModelList);
            return carTypeModelList;
        }
        #endregion

        #region Get CarType By Id
        public CarTypeModel GetCarTypeById(int carTypeId)
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            CarTypeModel carTypeModel = new CarTypeModel();
            CarType carType = new CarType();
            AutoMapper.Mapper.Map(carTypeModel, carType);
            carType = repo.GetAll().Where(x => x.CarTypeId == carTypeId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carType, carTypeModel);
            return carTypeModel;
        }
        #endregion

        #region Save Car Type
        public CarTypeModel SaveCarType(CarTypeModel model)
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            CarType carType = new CarType();
            AutoMapper.Mapper.Map(model, carType);
            repo.Insert(carType);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carType, model);
            return model;
        }

        #endregion

        #region Update Car Type
        public CarTypeModel UpadteCarType(CarTypeModel model)
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            CarType carType = new CarType();
            AutoMapper.Mapper.Map(model, carType);
            repo.Update(carType);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(carType, model);
            return model;
        }
        #endregion

        #region Delete CarType
        public void DeleteCarType(int carTypeId)
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            repo.Delete(x => x.CarTypeId == carTypeId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Car Type By Id
        public bool CheckExistance(int carTypeId)
        {
            //unitOfWork.StartTransaction();
            CarTypeRepository repo = new CarTypeRepository(unitOfWork);
            var city = repo.GetAll().Where(x => x.CarTypeId == carTypeId).Count();
            //unitOfWork.Commit();
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
