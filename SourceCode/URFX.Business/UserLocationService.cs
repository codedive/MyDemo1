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
   public class UserLocationService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Insert User Location
        public UserLocationModel InsertUserLocation(UserLocationModel locationModel)
        {
            //unitOfWork.StartTransaction();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            UserLocation userLocation = new UserLocation();
            AutoMapper.Mapper.Map(locationModel, userLocation);
            userLocation = repo.Insert(userLocation);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, locationModel);
            return locationModel;
        }
        #endregion

        #region Update User Location
        public UserLocationModel UpadteUserLocation(UserLocationModel model)
        {
            //unitOfWork.StartTransaction();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            UserLocation userLocation = new UserLocation();
            userLocation = repo.GetAll().Where(x => x.UserLocationId == model.UserLocationId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, userLocation);
            repo.Update(userLocation);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, model);
            return model;
        }
        #endregion

        #region Delete User Location
        public void DeleteUserLocation(int id)
        {
            //unitOfWork.StartTransaction();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            UserLocation userLocation = new UserLocation();
            repo.Delete(x=>x.UserLocationId==id);
            //unitOfWork.Commit();
        }
        #endregion

        #region Find Location According To User Id
        public UserLocationModel FindLocationById(string id)
        {
            //unitOfWork.StartTransaction();
            UserLocationModel locationModel = new UserLocationModel();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            UserLocation userLocation = new UserLocation();
            userLocation = repo.GetAll(x => x.UserId == id).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, locationModel);
            return locationModel;

        }
        #endregion
        #region Find Location According To User Id and City Id
        public UserLocationModel FindLocationByUserAndCityId(string userId,int cityId)
        {
            //unitOfWork.StartTransaction();
            UserLocationModel locationModel = new UserLocationModel();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            UserLocation userLocation = new UserLocation();
            userLocation = repo.GetAll(x => x.UserId == userId && x.CityId==cityId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, locationModel);
            return locationModel;

        }
        #endregion
        #region Find Location List According To User Id
        public List<UserLocationModel> FindLocationListById(string id)
        {
            //unitOfWork.StartTransaction();
            List<UserLocationModel> locationModelList = new List<UserLocationModel>();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            List<UserLocation> userLocation = new List<UserLocation>();
            userLocation = repo.GetAll(x => x.UserId == id).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, locationModelList);
            return locationModelList;

        }
        #endregion
        #region Find Location According To User Id
        public UserLocationModel FindUserLocationById(string id)
        {
            //unitOfWork.StartTransaction();
            UserLocationModel locationModel = new UserLocationModel();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            UserLocation userLocation = new UserLocation();
            userLocation = repo.GetAll(x => x.UserId == id).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userLocation, locationModel);
            return locationModel;

        }
        #endregion
        #region Check Existance Of Location  By UserId and City Id
        public bool CheckExistance(string userId)
        {
            //unitOfWork.StartTransaction();
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            UserLocation userLocation = new UserLocation();
            userLocation = repo.GetAll(x => x.UserId == userId).FirstOrDefault();
            //unitOfWork.Commit();
            if (userLocation !=null)
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
