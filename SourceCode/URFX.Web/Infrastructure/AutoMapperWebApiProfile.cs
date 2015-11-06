using URFX.Business.Infrastructure;
using URFX.Data.DataEntity.DomainModel;
using URFX.Web.Models;

namespace URFX.Web.Infrastructure
{
    public class AutoMapperWebApiProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWebApiProfile>();
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }
        protected override void Configure()
        {
            base.Configure();

            #region ViewModel to DomainModel
            AutoMapper.Mapper.CreateMap<RegisterClientBindingModel, ClientModel>();
            AutoMapper.Mapper.CreateMap<RegisterServiceProviderBindingModel, ServiceProviderModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMappingBindingModel, ServiceProviderServiceMappingModel>();
            AutoMapper.Mapper.CreateMap<CityBindingModel,CityModel>();
          


            #endregion

            #region DomainModel to ViewModel
            AutoMapper.Mapper.CreateMap<ClientModel, RegisterClientBindingModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderModel, RegisterServiceProviderBindingModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMappingModel, ServiceProviderServiceMappingBindingModel>();
            AutoMapper.Mapper.CreateMap<CityModel, CityBindingModel>();
            #endregion

        }
    }
}