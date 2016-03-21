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
   public class ServiceProviderServiceMappingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();


        #region GetListOfServiceProviderServiceMapping
        public List<ServiceProviderServiceMappingModel> GetAllServiceProviderServiceMapping()
        {
            //unitOfWork.StartTransaction();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            List<ServiceProviderServiceMappingModel> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMappingModel>();
            List<ServiceProviderServiceMapping> ServiceProviderServiceMapping = new List<ServiceProviderServiceMapping>();
            // AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, ServiceProviderServiceMapping);
            ServiceProviderServiceMapping = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(ServiceProviderServiceMapping, serviceProviderServiceMappingModel);
            return serviceProviderServiceMappingModel;
        }
        #endregion


        #region Save Services for Service Provider
        public ServiceProviderServiceMappingModel InsertServicesForServiceProvider(ServiceProviderServiceMappingModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            ServiceProviderServiceMapping serviceProviderServiceMappingModel = new ServiceProviderServiceMapping();
            AutoMapper.Mapper.Map(model, serviceProviderServiceMappingModel);
            repo.Insert(serviceProviderServiceMappingModel);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }

        #endregion

        #region Update Services for Service Provider
        public ServiceProviderServiceMappingModel UpadteServicesForServiceProvider(ServiceProviderServiceMappingModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            ServiceProviderServiceMapping serviceProviderServiceMappingModel = new ServiceProviderServiceMapping();
            serviceProviderServiceMappingModel = repo.GetAll().Where(x => x.ServiceProviderId == model.ServiceProviderId).SingleOrDefault();
            AutoMapper.Mapper.Map(model, serviceProviderServiceMappingModel);
            repo.Update(serviceProviderServiceMappingModel);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }
        #endregion


        #region Find Services for Service Provider By User Id
        public List<ServiceProviderServiceMappingModel> FindServiceByUserId(string Id)
        {
            //unitOfWork.StartTransaction();
            List<ServiceProviderServiceMappingModel> model = new List<ServiceProviderServiceMappingModel>();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
           List<ServiceProviderServiceMapping> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMapping>();

            serviceProviderServiceMappingModel= repo.GetAll().Where(x => x.ServiceProviderId == Id).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }
        #endregion

        #region Find Services for Services Provider By Service Id
        public List<ServiceProviderServiceMappingModel> FindServicesByServiceId(int Id)
        {
            //unitOfWork.StartTransaction();
            List<ServiceProviderServiceMappingModel> model = new List<ServiceProviderServiceMappingModel>();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            List<ServiceProviderServiceMapping> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMapping>();

            serviceProviderServiceMappingModel = repo.GetAll().Where(x => x.ServiceId == Id).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }
        #endregion

        #region Find Services for Service Provider By Service Id
        public bool FindServiceByServiceId(int Id,string serviceproviderId)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderServiceMappingModel model = new ServiceProviderServiceMappingModel();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
           ServiceProviderServiceMapping serviceProviderServiceMappingModel = new ServiceProviderServiceMapping();

            serviceProviderServiceMappingModel = repo.GetAll().Where(x => x.ServiceId == Id && x.ServiceProviderId== serviceproviderId).FirstOrDefault();
            //unitOfWork.Commit();
            //AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            if (serviceProviderServiceMappingModel == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        #endregion

        #region Delete Services for Service Provider By ServiceProviderServiceMapping Id
        public void DeleteServiceProviderServiceMapping(int Id)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderServiceMappingModel model = new ServiceProviderServiceMappingModel();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            ServiceProviderServiceMapping serviceProviderServiceMappingModel = new ServiceProviderServiceMapping();

            repo.Delete(x => x.Id == Id);
            //unitOfWork.Commit();

        }

        #endregion

        #region Find Services Provider By Service Id
        public List<ServiceProviderServiceMappingModel> FindServiceProviderByServiceId(int[] Ids)
        {
            //unitOfWork.StartTransaction();
            List<ServiceProviderServiceMappingModel> model = new List<ServiceProviderServiceMappingModel>();
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            List<ServiceProviderServiceMapping> serviceProviderServiceMappingModel = new List<ServiceProviderServiceMapping>();

            serviceProviderServiceMappingModel = repo.GetAll().Where(x => Ids.Contains(x.ServiceId)).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }
        #endregion
    }
}
