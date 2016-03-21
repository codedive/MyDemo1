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
            AutoMapper.Mapper.CreateMap<CityModel, City>();
            AutoMapper.Mapper.CreateMap<DistrictModel, District>();
            AutoMapper.Mapper.CreateMap<UserLocationModel, UserLocation>();
            AutoMapper.Mapper.CreateMap<EmployeeModel, Employee>();
            AutoMapper.Mapper.CreateMap<TransactionHistoryModel, TransactionHistory>();
            AutoMapper.Mapper.CreateMap<UserPlanModel, TransactionHistoryModel>();
            AutoMapper.Mapper.CreateMap<UserPlanModel, UserPlan>();
            AutoMapper.Mapper.CreateMap<ServiceProviderEmployeeMappingModel, ServicePoviderEmployeeMapping>();
            AutoMapper.Mapper.CreateMap<JobServiceMappingModel, JobServiceMapping>();
            AutoMapper.Mapper.CreateMap<JobModel, Job>();
            AutoMapper.Mapper.CreateMap<RatingModel, Rating>();
            AutoMapper.Mapper.CreateMap<ClientRatingModel, ClientRating>();
            AutoMapper.Mapper.CreateMap<CarTypeModel, CarType>();
            AutoMapper.Mapper.CreateMap<CarEmployeeMappingModel, CarEmployeeMapping>();
            AutoMapper.Mapper.CreateMap<JobServicePictureMappingModel, JobServicePicturesMapping>();
            AutoMapper.Mapper.CreateMap<PlanModel, Plan>();
            AutoMapper.Mapper.CreateMap<EmployeeScheduleModel, EmployeeSchedule>();
            AutoMapper.Mapper.CreateMap<ComplaintModel, Complaint>();
            AutoMapper.Mapper.CreateMap<UserLocationModel, UserLocation>();
                AutoMapper.Mapper.CreateMap<TransactionHistoryModel, TransactionHistory>();

            #endregion

            #region entity to business

            AutoMapper.Mapper.CreateMap<Client, ClientModel>();
            AutoMapper.Mapper.CreateMap<ServiceProvider, ServiceProviderModel>();
            AutoMapper.Mapper.CreateMap<ServiceCategory, ServiceCategoryModel>();
            AutoMapper.Mapper.CreateMap<Service, SubServiceModel>();
            AutoMapper.Mapper.CreateMap<ServiceProviderServiceMapping, ServiceProviderServiceMappingModel>();
            AutoMapper.Mapper.CreateMap<City, CityModel>();
            AutoMapper.Mapper.CreateMap<District, DistrictModel>();
            AutoMapper.Mapper.CreateMap<UserLocation, UserLocationModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistory, TransactionHistoryModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistoryModel, UserPlanModel>();
            AutoMapper.Mapper.CreateMap<UserPlan, UserPlanModel>();
            AutoMapper.Mapper.CreateMap<ServicePoviderEmployeeMapping, ServiceProviderEmployeeMappingModel>();
            AutoMapper.Mapper.CreateMap<JobServiceMapping, JobServiceMappingModel>();
            AutoMapper.Mapper.CreateMap<Job, JobModel>();
            AutoMapper.Mapper.CreateMap<Rating, RatingModel>();
            AutoMapper.Mapper.CreateMap<ClientRating, ClientRatingModel>();
            AutoMapper.Mapper.CreateMap<CarType, CarTypeModel>();
            AutoMapper.Mapper.CreateMap<CarEmployeeMapping, CarEmployeeMappingModel>();
            AutoMapper.Mapper.CreateMap<JobServicePicturesMapping, JobServicePictureMappingModel>();
            AutoMapper.Mapper.CreateMap<Plan, PlanModel>();
            AutoMapper.Mapper.CreateMap<EmployeeSchedule, EmployeeScheduleModel>();
            AutoMapper.Mapper.CreateMap<Complaint, ComplaintModel>();
            AutoMapper.Mapper.CreateMap<UserLocation, UserLocationModel>();
            AutoMapper.Mapper.CreateMap<TransactionHistory,TransactionHistoryModel >();
            #endregion

        }
    }
}
