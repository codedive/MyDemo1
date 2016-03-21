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

        #region Get District By Id
        public DistrictModel GetDistrictById(int districtId)
        {
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            DistrictModel districtModel = new DistrictModel();
            District city = new District();
            AutoMapper.Mapper.Map(districtModel, city);
            city = repo.GetAll().Where(x => x.DistrictId == districtId).FirstOrDefault();
            AutoMapper.Mapper.Map(city, districtModel);
            return districtModel;
        }
        #endregion

        #region Save District
        public DistrictModel SaveDistrict(DistrictModel model)
        {
            //unitOfWork.StartTransaction();
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            District district = new District();
            AutoMapper.Mapper.Map(model, district);
            repo.Insert(district);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(district, model);
            return model;
        }

        #endregion

        #region Update District
        public DistrictModel UpadteDistrict(DistrictModel model)
        {
            //unitOfWork.StartTransaction();
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            District district = new District();
            AutoMapper.Mapper.Map(model, district);
            repo.Update(district);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(district, model);
            return model;
        }
        #endregion

        #region Delete District
        public void DeleteDistrict(int districtId)
        {
            //unitOfWork.StartTransaction();
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            repo.Delete(x => x.CityId == districtId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of District By Id
        public bool CheckExistance(int districtId)
        {
            DistrictRepository repo = new DistrictRepository(unitOfWork);
            var district = repo.GetAll().Where(x => x.DistrictId == districtId).Count();
            if (district > 0)
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
