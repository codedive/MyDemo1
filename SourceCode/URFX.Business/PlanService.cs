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
    public class PlanService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();
       

        #region Get All Plans
        public List<PlanModel> GetPlans()
        {
            //unitOfWork.StartTransaction();
            List<PlanModel> planModel = new List<PlanModel>();
            List<Plan> plan = new List<Plan>();
            PlanRepository planRepository = new PlanRepository(unitOfWork);
            plan = planRepository.GetAll().OrderByDescending(x=>x.PlanId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(plan,planModel);
            return planModel;
           
        }
        #endregion
        #region Get All Plan By Id
        public PlanModel GetPlanById(int planId)
        {
            //unitOfWork.StartTransaction();
            PlanRepository planRepository = new PlanRepository(unitOfWork);
            PlanModel planModel = new PlanModel();
            Plan plan = new Plan();
            plan = planRepository.GetAll().Where(x => x.PlanId == planId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(plan, planModel);
            return planModel;
        }
        #endregion

      

       
    }
}
