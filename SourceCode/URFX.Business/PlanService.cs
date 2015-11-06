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
        public List<Plan> GetPlans()
        {
            PlanRepository planRepository = new PlanRepository(unitOfWork);
            return planRepository.GetAll().ToList();
        }
        #endregion

        
    }
}
