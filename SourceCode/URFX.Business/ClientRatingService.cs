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
   public class ClientRatingService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();


        #region Get All ClientRatings
        public List<ClientRatingModel> GetAllClientRatings()
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            List<ClientRatingModel> clientRatingModelList = new List<ClientRatingModel>();
            List<ClientRating> rating = new List<ClientRating>();
            AutoMapper.Mapper.Map(clientRatingModelList, rating);
            rating = repo.GetAll().OrderByDescending(x=>x.ClientRatingId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, clientRatingModelList);
            return clientRatingModelList;
        }
        #endregion

        #region Get ClientRating By RatingId
        public ClientRatingModel GetClientRatingById(int clientRatingId)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            ClientRatingModel clientRatingModel = new ClientRatingModel();
            ClientRating rating = new ClientRating();
            AutoMapper.Mapper.Map(clientRatingModel, rating);
            rating = repo.GetAll().Where(x => x.ClientRatingId == clientRatingId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, clientRatingModel);
            return clientRatingModel;
        }
        #endregion

        //#region Get Rating By ServiceProviderId
        //public RatingModel GetRatingByServiceProviderId(string ServiceProviderId)
        //{
        //    RatingRepository repo = new RatingRepository(unitOfWork);
        //    RatingModel ratingModel = new RatingModel();
        //    Rating rating = new Rating();
        //    AutoMapper.Mapper.Map(ratingModel, rating);
        //    //rating = repo.GetAll().Where(x => x.ServiceProviderId == ServiceProviderId).FirstOrDefault();
        //    AutoMapper.Mapper.Map(rating, ratingModel);
        //    return ratingModel;
        //}
        //#endregion
        //#region Get Rating List By ServiceProviderId
        //public List<RatingModel> GetRatingListByServiceProviderId(int jobid)
        //{
        //    RatingRepository repo = new RatingRepository(unitOfWork);
        //    List<RatingModel> ratingModelList = new List<RatingModel>();
        //    List<Rating> ratingList = new List<Rating>();
        //    AutoMapper.Mapper.Map(ratingModelList, ratingList);
        //    ratingList = repo.GetAll().Where(x => x.JobId == jobid).ToList();
        //    AutoMapper.Mapper.Map(ratingList, ratingModelList);
        //    return ratingModelList;
        //}
        //#endregion

        //#region Get Rating By EmployeeId
        //public RatingModel GetRatingByEmployeeId(string EmployeeId)
        //{
        //    RatingRepository repo = new RatingRepository(unitOfWork);
        //    RatingModel ratingModel = new RatingModel();
        //    Rating rating = new Rating();
        //    AutoMapper.Mapper.Map(ratingModel, rating);
        //    //rating = repo.GetAll().Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
        //    AutoMapper.Mapper.Map(rating, ratingModel);
        //    return ratingModel;
        //}
        //#endregion

        #region Get ClientRating By JobId
        public ClientRatingModel GetClientRatingByJobId(int JobId)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            ClientRatingModel ratingModel = new ClientRatingModel();
            ClientRating rating = new ClientRating();
            AutoMapper.Mapper.Map(ratingModel, rating);
            rating = repo.GetAll().Where(x => x.JobId == JobId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Get ClientRating List By JobId
        public List<ClientRatingModel> GetClientRatingListByJobId(int JobId)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            List<ClientRatingModel> ratingModel = new List<ClientRatingModel>();
            List<ClientRating> rating = new List<ClientRating>();
            AutoMapper.Mapper.Map(ratingModel, rating);
            rating = repo.GetAll().Where(x => x.JobId == JobId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Get ClientRating List By JobIds
        public List<ClientRatingModel> GetClientRatingListByJobIds(string[] JobIds)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repoClient = new ClientRatingRepository(unitOfWork);            
            List<ClientRatingModel> ratingModel = new List<ClientRatingModel>();
            List<ClientRating> rating = new List<ClientRating>();
            rating = repoClient.GetAllIncluding("Job.Employee").Where(x => JobIds.Contains(x.JobId.ToString())).ToList();
            //unitOfWork.Commit();
            //rating = repo.GetAll().Where(x => x.JobId == JobId).ToList();
            AutoMapper.Mapper.Map(rating, ratingModel);
            return ratingModel;
        }
        #endregion
        #region Save ClientRating
        public ClientRatingModel SaveClientRating(ClientRatingModel model)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            ClientRating rating = new ClientRating();
            AutoMapper.Mapper.Map(model, rating);
            repo.Insert(rating);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, model);
            return model;
        }

        #endregion

        #region Update ClientRating
        public ClientRatingModel UpadteClientRating(ClientRatingModel model)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            ClientRating rating = new ClientRating();
            rating = repo.GetAll().Where(x => x.ClientRatingId == model.ClientRatingId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, rating);
            repo.Update(rating);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(rating, model);
            return model;
        }
        #endregion

        #region Delete ClientRating
        public void DeleteClientRating(int clientRatingId)
        {
            //unitOfWork.StartTransaction();
            ClientRatingRepository repo = new ClientRatingRepository(unitOfWork);
            repo.Delete(x => x.ClientRatingId == clientRatingId);
            //unitOfWork.Commit();

        }
        #endregion
    }
}
