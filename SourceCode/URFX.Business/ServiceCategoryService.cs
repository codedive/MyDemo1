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
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            List<ServiceCategoryModel> serviceCategoryList = new List<ServiceCategoryModel>();
            List<ServiceCategory> serviceCategory = new List<ServiceCategory>();
            AutoMapper.Mapper.Map(serviceCategoryList, serviceCategory);
            serviceCategory = repo.GetAll().OrderByDescending(x=>x.ServiceCategoryId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceCategory, serviceCategoryList);
            return serviceCategoryList;
        }
        #endregion

        #region Get Category Service By Id
        public ServiceCategoryModel GetCategoryServiceById(int categoryServiceId)
        {
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            ServiceCategoryModel serviceCategoryModel = new ServiceCategoryModel();
            ServiceCategory serviceCategory = new ServiceCategory();
            AutoMapper.Mapper.Map(serviceCategoryModel, serviceCategory);
            serviceCategory = repo.GetAll().Where(x => x.ServiceCategoryId == categoryServiceId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceCategory, serviceCategoryModel);
            return serviceCategoryModel;
        }
        #endregion

        #region Save Category Service
        public ServiceCategoryModel SaveCategoryService(ServiceCategoryModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            ServiceCategory serviceCategory = new ServiceCategory();
            AutoMapper.Mapper.Map(model, serviceCategory);
            repo.Insert(serviceCategory);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceCategory, model);
            return model;
        }

        #endregion

        #region Update Category Service
        public ServiceCategoryModel UpadteCategoryService(ServiceCategoryModel model)
        {
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            ServiceCategory serviceCategory = new ServiceCategory();
            AutoMapper.Mapper.Map(model, serviceCategory);
            repo.Update(serviceCategory);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(serviceCategory, model);
            return model;
        }
        #endregion

        #region Delete Category Service
        public void DeleteCategoryService(int categoryServiceId)
        {
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            repo.Delete(x => x.ServiceCategoryId == categoryServiceId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Category Service By Id
        public bool CheckExistance(int categoryServiceId)
        {
            //unitOfWork.StartTransaction();
            ServiceCatagoryRepository repo = new ServiceCatagoryRepository(unitOfWork);
            var categoryService = repo.GetAll().Where(x => x.ServiceCategoryId == categoryServiceId).Count();
            //unitOfWork.Commit();
            if (categoryService > 0)
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
