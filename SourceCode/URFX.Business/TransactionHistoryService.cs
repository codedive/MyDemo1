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
   public class TransactionHistoryService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Insert Transaction History
        public TransactionHistoryModel InsertTransactionHistory(TransactionHistoryModel transactionHistoryModel)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.Insert(transactionHistory);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion

        #region Get Transaction History By Merchant Regrence
        public TransactionHistoryModel FindTransactionHistoryByMerchantRegrence(string merchantRegrence)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
             AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.GetAll().Where(x => x.MerchantReference == merchantRegrence).SingleOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion

        #region Get All Transaction History 
        public List<TransactionHistoryModel> GetAllTransactionHistory()
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            List<TransactionHistoryModel> transactionHistoryModelList = new List<TransactionHistoryModel>();
            List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();
            AutoMapper.Mapper.Map(transactionHistoryModelList, transactionHistoryList);
            transactionHistoryList = repo.GetAll().OrderByDescending(x=>x.CartId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistoryList, transactionHistoryModelList);
            return transactionHistoryModelList;
        }
        #endregion

        #region Update Transaction History
        public TransactionHistoryModel UpdateTransactionHistory(TransactionHistoryModel transactionHistoryModel)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
           
            transactionHistory = repo.GetAll().Where(x => x.CartId == transactionHistoryModel.CartId).SingleOrDefault();
           
            transactionHistoryModel.CartId = transactionHistory.CartId;
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            repo.Update(transactionHistory);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion

        #region Paging For Transaction History
        public List<TransactionHistoryModel> Paging(PagingModel model)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            List<TransactionHistoryModel> transactionHistoryModelList = new List<TransactionHistoryModel>();
            List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();
            //ResponseMessage responseMessage = new ResponseMessage();
            //PagingInfo Info = new PagingInfo();
            string searchparam = model.SearchText == null ? "" : model.SearchText;
            transactionHistoryList = repo.GetAll().Where(x => !string.IsNullOrEmpty(x.CustomerEmail) && x.CustomerEmail.ToLower().Contains(searchparam.ToLower())).OrderByDescending(x=>x.CartId).ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistoryList, transactionHistoryModelList);
            return transactionHistoryModelList;
        }
        #endregion

        #region Get Transaction History By Cart Id
        public TransactionHistoryModel GetTransactionHistoryByCartId(int id)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.GetAll().Where(x => x.CartId == id).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion
        #region Get Transaction History List By User Id
        public TransactionHistoryModel GetTransactionHistoryByUserId(string id)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.GetAll().Where(x => x.UserId == id).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion
        #region Get Transaction History By job Id
        public TransactionHistoryModel GetTransactionHistoryByJobId(int jobid)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.GetAll().Where(x => x.JobId == jobid && !string.IsNullOrEmpty(x.CustomerEmail)).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion
        #region Get Transaction History List By job Id
        public TransactionHistoryModel GetTransactionHistoryListByJobId(int jobid)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            TransactionHistoryModel transactionHistoryModel = new TransactionHistoryModel();
            //UserLocationModel userLocationModel = new UserLocationModel();
            TransactionHistory transactionHistory = new TransactionHistory();
            AutoMapper.Mapper.Map(transactionHistoryModel, transactionHistory);
            transactionHistory = repo.GetAll().Where(x => x.JobId == jobid).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(transactionHistory, transactionHistoryModel);
            return transactionHistoryModel;
        }
        #endregion

        #region Delete Transaction
        public void DeleteTransaction(int id)
        {
            //unitOfWork.StartTransaction();
            TransactionHistoryRepository repo = new TransactionHistoryRepository(unitOfWork);
            repo.Delete(x => x.CartId == id);
            //unitOfWork.Commit();
        }
        #endregion
    }
}
