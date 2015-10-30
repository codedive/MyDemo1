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

        #region Save Services for Service Provider
        public ServiceProviderServiceMappingModel InsertServicesForServiceProvider(ServiceProviderServiceMappingModel model)
        {
            ServiceProviderServiceMappingRepository repo = new ServiceProviderServiceMappingRepository(unitOfWork);
            ServiceProviderServiceMapping serviceProviderServiceMappingModel = new ServiceProviderServiceMapping();
            AutoMapper.Mapper.Map(model, serviceProviderServiceMappingModel);
            repo.Insert(serviceProviderServiceMappingModel);
            AutoMapper.Mapper.Map(serviceProviderServiceMappingModel, model);
            return model;
        }

        #endregion
    }
}
