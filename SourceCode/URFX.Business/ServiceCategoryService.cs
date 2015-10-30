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
   public class ServiceCategoryService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Category Services
        public List<ServiceCategoryModel> GetAllServiceCategories()
        {
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            List<ServiceCategoryModel> serviceCategoryList = new List<ServiceCategoryModel>();
            List<ServiceCategory> serviceCategory = new List<ServiceCategory>();
            AutoMapper.Mapper.Map(serviceCategoryList, serviceCategory);
            serviceCategory = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(serviceCategory, serviceCategoryList);
            return serviceCategoryList;
        }
        #endregion
    }
}
