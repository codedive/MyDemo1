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
  public  class CityService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Countries
        public List<CityModel> GetAllCiites()
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            List<CityModel> cityModelList = new List<CityModel>();
            List<City> cityList = new List<City>();
            AutoMapper.Mapper.Map(cityModelList, cityList);
            cityList = repo.GetAll().OrderByDescending(x=>x.CityId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(cityList, cityModelList);
            return cityModelList;
        }
        #endregion

        #region Get City By Id
        public CityModel GetCityById(int cityId)
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            CityModel cityModel = new CityModel();
            City city = new City();
            AutoMapper.Mapper.Map(cityModel, city);
            city = repo.GetAll().Where(x => x.CityId == cityId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(city, cityModel);
            return cityModel;
        }
        #endregion

        #region Save City
        public CityModel SaveCity(CityModel model)
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            City city = new City();
            AutoMapper.Mapper.Map(model, city);
            repo.Insert(city);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(city, model);
            return model;
        }

        #endregion

        #region Update City
        public CityModel UpadteCity(CityModel model)
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            City city = new City();
            city = repo.GetAll().Where(x => x.CityId == model.CityId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, city);
            repo.Update(city);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(city, model);
            return model;
        }
        #endregion

        #region Delete City
        public void DeleteCity(int cityId)
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            repo.Delete(x => x.CityId == cityId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of City By Id
        public bool CheckExistance(int cityId)
        {
            //unitOfWork.StartTransaction();
            CityRepository repo = new CityRepository(unitOfWork);
            var city = repo.GetAll().Where(x => x.CityId == cityId).Count();
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
