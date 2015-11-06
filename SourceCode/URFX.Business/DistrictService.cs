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
   public class DistrictService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All District
        public List<DistrictModel> GetAllDistrict()
        {
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            List<DistrictModel> districtModelList = new List<DistrictModel>();
            List<District> districtList = new List<District>();
            AutoMapper.Mapper.Map(districtModelList, districtList);
            districtList = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(districtList, districtModelList);
            return districtModelList;
        }
        #endregion

        #region Get District By City Id
        public List<DistrictModel> GetDistrictByCityId(int cityId)
        {
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            List<DistrictModel> districtModelList = new List<DistrictModel>();
            List<District> districtList = new List<District>();
            AutoMapper.Mapper.Map(districtModelList, districtList);
            districtList = repo.GetAll().Where(x=>x.CityId== cityId).ToList();
            AutoMapper.Mapper.Map(districtList, districtModelList);
            return districtModelList;
        }
        #endregion
    }
}
