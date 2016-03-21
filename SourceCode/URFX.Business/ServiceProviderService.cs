using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
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
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProvider);
            serviceProvider = repo.GetAll().OrderByDescending(x=>x.ServiceProviderId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderList);
            return serviceProviderList;
        }
        #endregion

        #region Get All Service Providers As Individual
        public List<ServiceProviderModel> GetAllServiceProvidersAsIndividual()
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            List<ServiceProviderModel> serviceProviderList = new List<ServiceProviderModel>();
            List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProvider);
            serviceProvider = repo.GetAll().Where(x=>x.ServiceProviderType== ServiceProviderType.Individual).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderList);
            return serviceProviderList;
        }
        #endregion

        #region Get Service Provider By Id
        public ServiceProviderModel GetServiceProviderById(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
            ServiceProvider serviceProvider = new ServiceProvider();
            AutoMapper.Mapper.Map(serviceProviderModel, serviceProvider);
            serviceProvider = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderModel);
            return serviceProviderModel;
        }
        #endregion


        #region Get Service Provider By ServiceProviderId
        public List<ServiceProviderModel> GetServiceProviderListById(string[] serviceProviderIds)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            List<ServiceProviderModel> serviceProviderModel = new List<ServiceProviderModel>();
            List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
            AutoMapper.Mapper.Map(serviceProviderModel, serviceProvider);
            serviceProvider = repo.GetAll().Where(x => serviceProviderIds.Contains(x.ServiceProviderId) && x.IsActive==true).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, serviceProviderModel);
            return serviceProviderModel;
        }
        #endregion

        #region Save Service Provider
        public ServiceProviderModel SaveServiceProvider(ServiceProviderModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProvider serviceProvider = new ServiceProvider();
            AutoMapper.Mapper.Map(model, serviceProvider);
            repo.Insert(serviceProvider);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, model);
            return model;
        }

        #endregion

        #region Update Service Provider
        public ServiceProviderModel UpadteServiceProvider(ServiceProviderModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider = repo.GetAll().Where(x => x.ServiceProviderId == model.ServiceProviderId).SingleOrDefault();
            AutoMapper.Mapper.Map(model, serviceProvider);
            //serviceProvider.IsActive = model.IsActive;
            repo.Update(serviceProvider);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProvider, model);
            return model;
        }
        #endregion

        #region Delete Service Provider
        public void DeleteServiceProvider(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId).SingleOrDefault();
            repo.Delete(x => x.ServiceProviderId == serviceProvider.ServiceProviderId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of ServiceProvider By Id
        public bool CheckExistance(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            var client = repo.GetAll().Where(x => x.ServiceProviderId == serviceProviderId).Count();
            //unitOfWork.Commit();
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

        #region Paging For Service Provider
        public List<ServiceProviderModel> Paging(PagingModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderRepository repo = new ServiceProviderRepository(unitOfWork);
            List<ServiceProviderModel> serviceProviderModelList = new List<ServiceProviderModel>();
            List<ServiceProvider> serviceProviderList = new List<ServiceProvider>();
            //ResponseMessage responseMessage = new ResponseMessage();
            //PagingInfo Info = new PagingInfo();
            string searchparam = model.SearchText == null ? "" : model.SearchText;
            serviceProviderList = repo.GetAll().Where(x => x.CompanyName.ToLower().Contains(searchparam.ToLower())).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderList, serviceProviderModelList);
            return serviceProviderModelList;
        }
        #endregion
    }
}
