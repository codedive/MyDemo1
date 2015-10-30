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
   public class ServiceProviderService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Service Providers
        public List<ServiceProviderModel> GetAllServiceProviders()
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProvider);
            serviceProvider = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderList);
            return serviceProviderList;
        }
        #endregion

        #region Get Service Provider By Id
        public ServiceProviderModel GetServiceProviderById(string serviceProviderId)
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
            ServiceProvider serviceProvider = new ServiceProvider();
            AutoMapper.Mapper.Map(serviceProviderModel, serviceProvider);
            serviceProvider = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId).FirstOrDefault();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderModel);
            return serviceProviderModel;
        }
        #endregion

        #region Save Service Provider
        public ServiceProviderModel SaveServiceProvider(ServiceProviderModel model)
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProvider serviceProvider = new ServiceProvider();
            AutoMapper.Mapper.Map(model, serviceProvider);
            repo.Insert(serviceProvider);
            AutoMapper.Mapper.Map(serviceProvider, model);
            return model;
        }

        #endregion

        #region Update Service Provider
        public ServiceProviderModel UpadteServiceProvider(ServiceProviderModel model)
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProvider serviceProvider = new ServiceProvider();
            AutoMapper.Mapper.Map(model, serviceProvider);
            repo.Update(serviceProvider);
            AutoMapper.Mapper.Map(serviceProvider, model);
            return model;
        }
        #endregion

        #region Delete Service Provider
        public void DeleteServiceProvider(string serviceProviderId)
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            repo.Delete(x => x.ServiceProviderId == serviceProviderId);

        }
        #endregion

        #region Check Existance Of ServiceProvider By Id
        public bool CheckExistance(string serviceProviderId)
        {
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            var client = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId).Count();
            if (client > 0)
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
