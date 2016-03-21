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
   public class RatingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();


        #region Get All Ratings
        public List<RatingModel> GetAllRatings()
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            List<RatingModel> ratingModelList = new List<RatingModel>();
            List<Rating> rating = new List<Rating>();
            AutoMapper.Mapper.Map(ratingModelList, rating);
            rating = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModelList);
            return ratingModelList;
        }
        #endregion

        #region Get Rating By RatingId
        public RatingModel GetRatingById(int ratingid)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            RatingModel ratingModel = new RatingModel();
            Rating rating = new Rating();
            AutoMapper.Mapper.Map(ratingModel, rating);
            rating = repo.GetAll().Where(x => x.RatingId == ratingid).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion

        #region Get Rating By ServiceProviderId
        public RatingModel GetRatingByServiceProviderId(string ServiceProviderId)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            RatingModel ratingModel = new RatingModel();
            Rating rating = new Rating();
            AutoMapper.Mapper.Map(ratingModel, rating);
            //rating = repo.GetAll().Where(x => x. == ServiceProviderId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Get Rating List By ServiceProviderId
        public List<RatingModel> GetRatingListByServiceProviderId(int jobid)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            List<RatingModel> ratingModelList = new List<RatingModel>();
            List<Rating> ratingList = new List<Rating>();
            AutoMapper.Mapper.Map(ratingModelList, ratingList);
            ratingList = repo.GetAll().Where(x => x.JobId == jobid).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(ratingList, ratingModelList);
            return ratingModelList;
        }
        #endregion
        #region Get Rating List By jobIds
        public List<RatingModel> GetRatingListByjobIds(string[] jobIds)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            List<RatingModel> ratingModelList = new List<RatingModel>();
            List<Rating> ratingList = new List<Rating>();
            AutoMapper.Mapper.Map(ratingModelList, ratingList);
            ratingList = repo.GetAll().Where(x => jobIds.Contains(x.JobId.ToString())).ToList();
            //unitOfWork.Commit();
            //ratingList = repo.GetAll().Where(x => x.JobId == jobid).ToList();
            AutoMapper.Mapper.Map(ratingList, ratingModelList);
            return ratingModelList;
        }
        #endregion

        #region Get Rating By EmployeeId
        public RatingModel GetRatingByEmployeeId(string EmployeeId)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            RatingModel ratingModel = new RatingModel();
            Rating rating = new Rating();
            AutoMapper.Mapper.Map(ratingModel, rating);
            //rating = repo.GetAll().Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Get Rating List By JobId
        public List<RatingModel> GetRatingListByJobId(int JobId)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            List<RatingModel> ratingModel = new List<RatingModel>();
            List<Rating> rating = new List<Rating>();
            AutoMapper.Mapper.Map(ratingModel, rating);
            rating = repo.GetAll().Where(x => x.JobId == JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Get Rating By JobId
        public RatingModel GetRatingByJobId(int JobId)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            RatingModel ratingModel = new RatingModel();
            Rating rating = new Rating();
            AutoMapper.Mapper.Map(ratingModel, rating);
            rating = repo.GetAll().Where(x => x.JobId == JobId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion

        #region Save Rating
        public RatingModel SaveRating(RatingModel model)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            Rating rating = new Rating();
            AutoMapper.Mapper.Map(model, rating);
            repo.Insert(rating);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, model);
            return model;
        }

        #endregion

        #region Update Rating
        public RatingModel UpadteRating(RatingModel model)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            Rating rating = new Rating();
            rating = repo.GetAll().Where(x => x.RatingId == model.RatingId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, rating);
            repo.Update(rating);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, model);
            return model;
        }
        #endregion

        #region Delete Rating
        public void DeleteRating(int ratingId)
        {
            //unitOfWork.StartTransaction();
            RatingRepository repo = new RatingRepository(unitOfWork);
            repo.Delete(x => x.RatingId == ratingId);
            //unitOfWork.Commit();

        }
        #endregion

        

        
    }
}
