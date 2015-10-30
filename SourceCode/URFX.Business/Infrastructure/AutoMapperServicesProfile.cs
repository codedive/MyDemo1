//using URFX.Data.DataEntity;
//using URFX.Data.Entities;

using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;

namespace URFX.Business.Infrastructure
{
    public class AutoMapperBusinessProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }
        protected override void Configure()
        {
            base.Configure();

            #region business to entity            
            AutoMapper.Mapper.CreateMap<ClientModel, Client>();
            AutoMapper.Mapper.CreateMap<ServiceProviderModel, ServiceProvider>();
            AutoMapper.Mapper.CreateMap<ServiceCategoryModel, ServiceCategory>();
            AutoMapper.Mapper.CreateMap<SubServiceModel, Service>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMappingModel, ServiceProviderServiceMapping>();
            AutoMapper.Mapper.CreateMap<CountryModel, Country>();
            #endregion

            #region entity to business

            AutoMapper.Mapper.CreateMap<Client, ClientModel>();
            AutoMapper.Mapper.CreateMap<ServiceProvider, ServiceProviderModel>();
            AutoMapper.Mapper.CreateMap<ServiceCategory, ServiceCategoryModel>();
            AutoMapper.Mapper.CreateMap<Service, SubServiceModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMapping, ServiceProviderServiceMappingModel>();
            AutoMapper.Mapper.CreateMap<Country, CountryModel>();
            #endregion

        }
    }
}
