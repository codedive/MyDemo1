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
   public class SubServicesService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Sub Services By Category Service Id
        public List<SubServiceModel> GetAllSubServicesByCategoryServiceId(int CategoryServiceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            List<Service> service = new List<Service>();
            AutoMapper.Mapper.Map(subServiceList, service);
            service = repo.GetAll().Where(x => x.ServiceCategoryId == CategoryServiceId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceList);
            return subServiceList;
        }
        #endregion

        #region Get All Sub Services By ServiceId
        public List<SubServiceModel> GetAllSubServicesByServiceId(int ServiceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            List<Service> service = new List<Service>();
            AutoMapper.Mapper.Map(subServiceList, service);
            service = repo.GetAll().Where(x => x.ParentServiceId == ServiceId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceList);
            return subServiceList;
        }
        #endregion

        #region Get All Category Service By ServiceId
        public SubServiceModel GetAllCategoryServiceByServiceId(int ServiceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            SubServiceModel subServiceList = new SubServiceModel();
            Service service = new Service();
            AutoMapper.Mapper.Map(subServiceList, service);
            service = repo.GetAll().Where(x => x.ServiceId == ServiceId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceList);
            return subServiceList;
        }
        #endregion

        #region Get All Sub Services
        public List<SubServiceModel> GetAllSubServices()
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
           List<SubServiceModel> subServiceList = new List<SubServiceModel>();
           List<Service> service = new List<Service>();
            AutoMapper.Mapper.Map(subServiceList, service);
            service = repo.GetAll().Where(x=>x.ServiceCategoryId != null).OrderByDescending(X=>X.ServiceId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceList);
            return subServiceList;
        }

        public List<SubServiceModel> GetAllActualServiceByServiceId(int serviceId=0)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            List<SubServiceModel> subServiceList = new List<SubServiceModel>();
            List<Service> service = new List<Service>();
            AutoMapper.Mapper.Map(subServiceList, service);
            if (serviceId > 0)
            {
                service = repo.GetAll().Where(x => x.ServiceCategoryId == null && x.ParentServiceId == serviceId).OrderByDescending(X => X.ServiceId).ToList();
            }
            else
            {
                service = repo.GetAll().Where(x => x.ServiceCategoryId != null).OrderByDescending(X => X.ServiceId).ToList();
            }            
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceList);
            return subServiceList;
        }

        #endregion

        #region Get Sub Service By Id
        public SubServiceModel GetSubServiceById(int? subServiceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            SubServiceModel subServiceModel = new SubServiceModel();
            Service service = new Service();
            AutoMapper.Mapper.Map(subServiceModel, service);
            service = repo.GetAll().Where(x => x.ServiceId == subServiceId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, subServiceModel);
            return subServiceModel;
        }
        #endregion

        #region Save Sub Service
        public SubServiceModel SaveSubService(SubServiceModel model)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            Service service = new Service();
            AutoMapper.Mapper.Map(model, service);
            repo.Insert(service);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, model);
            return model;
        }

        #endregion

        #region Update Sub Service
        public SubServiceModel UpadteSubService(SubServiceModel model)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            Service service = new Service();
            service = repo.GetAll().Where(x => x.ServiceId == model.ServiceId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, service);
            repo.Update(service);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(service, model);
            return model;
        }
        #endregion

        #region Delete Sub Service
        public void DeleteSubService(int serviceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            repo.Delete(x => x.ServiceId == serviceId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Sub Service By Id
        public bool CheckExistance(int serviceId)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            var service = repo.GetAll().Where(x => x.ServiceId == serviceId).Count();
            //unitOfWork.Commit();
            if (service > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion



        #region Get_Service_As_Per_CategoryId
        public List<SubServiceModel> GetServicesByCategoryId(int categoryId = 0)
        {
            //unitOfWork.StartTransaction();
            SubServiceRepository repo = new SubServiceRepository(unitOfWork);
            List<SubServiceModel> subServicesModel = new List<SubServiceModel>();
            List<Service> services = new List<Service>();
            AutoMapper.Mapper.Map(subServicesModel, services);
            var subServices = repo.GetAll().Where(x => x.ServiceCategoryId == categoryId).ToList();
            var subServiceIds = subServices.Select(x => Convert.ToInt32(x.ServiceId)).ToArray();
            var actualServices = repo.GetAll().Where(x => x.ServiceCategoryId == null && subServiceIds.Contains(Convert.ToInt32(x.ParentServiceId))).ToList();
            services = subServices.Concat(actualServices).ToList();
            
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(services, subServicesModel);
            return subServicesModel;
        }
        #endregion

    }
}
