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

        //#region Get All Category Services
        //public List<SubServiceModel> GetAllSubServicesByCategoryServiceId(int Id)
        //{
        //    SubServiceRepository repo = new SubServiceRepository(unitOfWork);
        //    List<SubServiceModel> subServiceList = new List<SubServiceModel>();
        //    List<Service> service = new List<Service>();
        //    AutoMapper.Mapper.Map(subServiceList, service);
        //    service = repo.GetAll().Where(x => x.ServiceCategoryId == Id).ToList();
        //    AutoMapper.Mapper.Map(service, subServiceList);
        //    return subServiceList;
        //}
        //#endregion

        //#region Get All Sub Services By ServiceId
        //public List<SubServiceModel> GetAllSubServicesByServiceId(int Id)
        //{
        //    SubServiceRepository repo = new SubServiceRepository(unitOfWork);
        //    List<SubServiceModel> subServiceList = new List<SubServiceModel>();
        //    List<Service> service = new List<Service>();
        //    AutoMapper.Mapper.Map(subServiceList, service);
        //    service = repo.GetAll().Where(x => x.ParentServiceId == Id).ToList();
        //    AutoMapper.Mapper.Map(service, subServiceList);
        //    return subServiceList;
        //}
        //#endregion
    }
}
