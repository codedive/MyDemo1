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
   public class UserPlanService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Insert User Plan
        public UserPlanModel InsertUserPlan(UserPlanModel planModel)
        {
            //unitOfWork.StartTransaction();
            UserPlanRepository repo = new UserPlanRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            UserPlan userPlan = new UserPlan();
            AutoMapper.Mapper.Map(planModel, userPlan);
            userPlan = repo.Insert(userPlan);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userPlan, planModel);
            return planModel;
        }
        #endregion
        #region Get User Plan By UserId
        public UserPlanModel GetUserPlanByUserId(string userId)
        {
            //unitOfWork.StartTransaction();
            UserPlanRepository repo = new UserPlanRepository(unitOfWork);
            UserPlanModel userPlanModel = new UserPlanModel();
            UserPlan userPlan = new UserPlan();
            userPlan = repo.GetAll().Where(x=>x.UserId==userId && x.ExpiredDate > DateTime.UtcNow).FirstOrDefault();
            //if (userPlan == null)
            //{
            //    UserPlan checkPlan = new UserPlan();
            //    checkPlan = repo.GetAll().Where(x => x.UserId == userId).FirstOrDefault();
            //    repo.Delete(x=>x.Id== checkPlan.Id);
            //}
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(userPlan, userPlanModel);
            return userPlanModel;
        }
        #endregion
        #region Check Existance Of User Plan
        public bool CheckExistanceOfUserPlan(string userId)
        {
            //unitOfWork.StartTransaction();
            UserPlanRepository repo = new UserPlanRepository(unitOfWork);
            UserPlan userPlan = new UserPlan();
            userPlan = repo.GetAll().Where(x => x.UserId == userId).FirstOrDefault();
            //unitOfWork.Commit();
            if (userPlan != null)
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
