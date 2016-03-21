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
   public class ServiceProviderEmployeeMappingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Save Employee By ServiceProviderId In ServiceProviderEmployeeMapping
        public ServiceProviderEmployeeMappingModel SaveEmployeeAccordingToServiceProvider(ServiceProviderEmployeeMappingModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderEmployeeMappingRepository repo = new ServiceProviderEmployeeMappingRepository(unitOfWork);
            ServicePoviderEmployeeMapping serviceProviderEmployeeMapping = new ServicePoviderEmployeeMapping();
            AutoMapper.Mapper.Map(model, serviceProviderEmployeeMapping);
            repo.Insert(serviceProviderEmployeeMapping);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderEmployeeMapping, model);
            return model;
        }

        #endregion

        #region Get Employee By Service Provider Id
        public List<ServiceProviderEmployeeMappingModel> GetEmployeeByServiceProviderId(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
            ServiceProviderEmployeeMappingRepository repo = new ServiceProviderEmployeeMappingRepository(unitOfWork);
            List<ServicePoviderEmployeeMapping> serviceProviderEmployeeMapping = new List<ServicePoviderEmployeeMapping>();
            serviceProviderEmployeeMapping= repo.GetAll().Where(x=>x.ServiceProviderId== serviceProviderId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceProviderEmployeeMapping, serviceProviderEmployeeMappingModel);
            return serviceProviderEmployeeMappingModel;
        }

        #endregion

        #region Delete ServiceProviderEmployeeMapping
        public void DeleteServiceProviderEmployeeMapping(string employeeId)
        {
            //unitOfWork.StartTransaction();
            ServiceProviderEmployeeMappingRepository repo = new ServiceProviderEmployeeMappingRepository(unitOfWork);
            ServicePoviderEmployeeMapping serviceProviderEmployeeMapping = new ServicePoviderEmployeeMapping();
            serviceProviderEmployeeMapping = repo.GetAll().Where(x => x.EmployeeId == employeeId).FirstOrDefault();
            if (serviceProviderEmployeeMapping != null)
            {
                repo.Delete(x => x.Id == serviceProviderEmployeeMapping.Id);
            }
            //unitOfWork.Commit();
        }
        #endregion

        #region GetServiceProviderByEmployeeId
        public List<EmployeeModel> GetServiceProviderByEmployeeId(List<EmployeeModel> model)
        {
            //unitOfWork.StartTransaction();
            List<ServiceProviderEmployeeMappingModel> serviceProviderEmployeeMappingModel = new List<ServiceProviderEmployeeMappingModel>();
            ServiceProviderEmployeeMappingRepository repo = new ServiceProviderEmployeeMappingRepository(unitOfWork);
            ServicePoviderEmployeeMapping serviceProviderEmployeeMapping = new ServicePoviderEmployeeMapping();
            model.ForEach(x =>
            {
                serviceProviderEmployeeMapping = repo.GetAllIncluding("ServiceProvider").Where(y => y.EmployeeId == x.EmployeeId).FirstOrDefault();
                if(serviceProviderEmployeeMapping!=null)
                x.ServiceProvider = serviceProviderEmployeeMapping.ServiceProvider.CompanyName;
            });
            //unitOfWork.Commit();
            //  AutoMapper.Mapper.Map(serviceProviderEmployeeMapping, serviceProviderEmployeeMappingModel);
            return model;
        }
        #endregion
    }
}
