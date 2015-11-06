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
            UserLocationRepository repo = new UserLocationRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            UserLocation userLocation = new UserLocation();
            AutoMapper.Mapper.Map(locationModel, userLocation);
            userLocation = repo.Insert(userLocation);
            AutoMapper.Mapper.Map(userLocation, locationModel);
            return locationModel;
        }
        #endregion
    }
}
