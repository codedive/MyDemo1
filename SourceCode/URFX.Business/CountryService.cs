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
  public  class CountryService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Countries
        public List<CountryModel> GetAllCountries()
        {
            CountryRepository repo = new CountryRepository(unitOfWork);
            List<CountryModel> countryModelList = new List<CountryModel>();
            List<Country> countryList = new List<Country>();
            AutoMapper.Mapper.Map(countryModelList, countryList);
            countryList = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(countryList, countryModelList);
            return countryModelList;
        }
        #endregion
    }
}
