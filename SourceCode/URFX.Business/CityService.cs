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
            CityRepository repo = new CityRepository(unitOfWork);
            List<CityModel> cityModelList = new List<CityModel>();
            List<City> cityList = new List<City>();
            AutoMapper.Mapper.Map(cityModelList, cityList);
            cityList = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(cityList, cityModelList);
            return cityModelList;
        }
        #endregion
    }
}
