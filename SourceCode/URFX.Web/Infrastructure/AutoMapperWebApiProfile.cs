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
            AutoMapper.Mapper.CreateMap<EmployeeBindingModel, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<UserPlanBindingModel, TransactionHistoryModel>();
            AutoMapper.Mapper.CreateMap<UserPlanModel, TransactionHistoryModel>();
            AutoMapper.Mapper.CreateMap<JobServiceMappingBindingModel, JobServiceMappingModel>();
            AutoMapper.Mapper.CreateMap<JobBindingModel, JobModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMappingBindingModel, SubServiceModel>();
            AutoMapper.Mapper.CreateMap<ClientRatingBindingModel, ClientRatingModel>();
            AutoMapper.Mapper.CreateMap<RegisterServiceProviderBindingModel, IndividualBindingModel>();
            AutoMapper.Mapper.CreateMap<CarEmployeeMappingBindingModel, CarEmployeeMappingModel>();
            AutoMapper.Mapper.CreateMap<CarTypeBindingModel, CarTypeModel>();
            AutoMapper.Mapper.CreateMap<ComplaintBindingModel, ComplaintModel>();
            AutoMapper.Mapper.CreateMap<EmployeeScheduleBindingModel, EmployeeScheduleModel>();
            AutoMapper.Mapper.CreateMap<UserLocationBindingModel, UserLocationModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistoryBindingModel, TransactionHistoryModel>();
            AutoMapper.Mapper.CreateMap<SubServiceBindingModel,SubServiceModel>();
            #endregion

            #region DomainModel to ViewModel
            AutoMapper.Mapper.CreateMap<ClientModel, RegisterClientBindingModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderModel, RegisterServiceProviderBindingModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMappingModel, ServiceProviderServiceMappingBindingModel>();
            AutoMapper.Mapper.CreateMap<CityModel, CityBindingModel>();
            AutoMapper.Mapper.CreateMap<EmployeeModel, EmployeeBindingModel > ();
            AutoMapper.Mapper.CreateMap<TransactionHistoryModel, UserPlanBindingModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistoryModel, UserPlanModel>();
            AutoMapper.Mapper.CreateMap<JobServiceMappingModel, JobServiceMappingBindingModel>();
            AutoMapper.Mapper.CreateMap<JobModel, JobBindingModel>();
            AutoMapper.Mapper.CreateMap<SubServiceModel, ServiceProviderServiceMappingBindingModel>();
            AutoMapper.Mapper.CreateMap<ClientRatingModel, ClientRatingBindingModel>();
            AutoMapper.Mapper.CreateMap<RegisterServiceProviderBindingModel, IndividualBindingModel>();
            AutoMapper.Mapper.CreateMap<CarEmployeeMappingModel, CarEmployeeMappingBindingModel>();
            AutoMapper.Mapper.CreateMap<CarTypeModel, CarTypeBindingModel>();
            AutoMapper.Mapper.CreateMap<ComplaintModel, ComplaintBindingModel>();
            AutoMapper.Mapper.CreateMap<EmployeeScheduleModel, EmployeeScheduleBindingModel>();
            AutoMapper.Mapper.CreateMap<UserLocationModel, UserLocationBindingModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistoryModel, TransactionHistoryBindingModel>();
            AutoMapper.Mapper.CreateMap<SubServiceModel, SubServiceBindingModel>();
            #endregion

        }
    }
}