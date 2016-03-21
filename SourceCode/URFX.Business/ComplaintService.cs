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
    public class ComplaintService
    {
        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get Complaints 
        public List<ComplaintModel> GetComplaints()
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            List<ComplaintModel> complaintModel = new List<ComplaintModel>();
            List<Complaint> complaint = new List<Complaint>();
            complaint = repo.GetAll().ToList();
            AutoMapper.Mapper.Map(complaint, complaintModel);
            //var complaints = (from data in db.Complaint
            //                  join e in db.Employee on data.EmployeeId equals e.EmployeeId into es
            //                  from e in es.DefaultIfEmpty()                              
            //                  join cl in db.Client on data.ClientId equals cl.ClientId into cls
            //                  from cl in cls.DefaultIfEmpty()
            //                  join ul in db.UserLocation on cl.ClientId equals ul.UserId into uls
            //                  from ul in uls.DefaultIfEmpty()
            //                  join u in db.Users on cl.ClientId equals u.Id into us
            //                  from u in us.DefaultIfEmpty()
            //                  join sp in db.ServiceProvider on data.ServiceProviderId equals sp.ServiceProviderId into sps
            //                  from sp in sps.DefaultIfEmpty()
            //                  join j in db.Job on data.JobId equals j.JobId into js
            //                  from j in js.DefaultIfEmpty()
            //                  select new
            //                  {
            //                      ComplainId = data.ComplaintId,
            //                      Description = data.Description,
            //                      ServiceProviderId = data.ServiceProviderId,
            //                      EmployeeName = e.FirstName,
            //                      ClientName = cl.FirstName,
            //                      ServiceProviderName = sp.CompanyName,
            //                      Status = data.Status,
            //                      jobDescription = j.Description,
            //                      ClientAddress = ul.Address,
            //                      JobAddress = j.JobAddress,
            //                      ClientPhoneNumber = u.PhoneNumber,
            //                  }).OrderByDescending(x=>x.ComplainId).ToList();

            //foreach (var item in complaints)
            //{
            //    ComplaintModel model = new ComplaintModel();
            //    model.ComplaintId = item.ComplainId;
            //    model.Description = item.Description;
            //    model.EmployeeName = item.EmployeeName;
            //    model.ClientName = item.ClientName;
            //    model.ServiceProviderName = item.ServiceProviderName;
            //    model.Status = item.Status;
            //    model.JobDescription = item.jobDescription;
            //    model.JobAddress = item.JobAddress;
            //    model.ClientAddress = item.ClientAddress;
            //    model.ClientPhoneNumber = item.ClientPhoneNumber;
            //    complaintModel.Add(model);
            //}
            //unitOfWork.Commit();

            return complaintModel;
        }
        #endregion


        #region Get Complaint list By ServiceProvider Id
        public List<ComplaintModel> GetComplaintListByServiceProviderId(string serviceProviderId)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            List<ComplaintModel> complaintModelList = new List<ComplaintModel>();
            List<Complaint> complaintlist = new List<Complaint>();
            complaintlist = repo.GetAll().Where(x=>x.ServiceProviderId== serviceProviderId).ToList();
            AutoMapper.Mapper.Map(complaintlist, complaintModelList);
            //var complaints = (from data in db.Complaint
            //                  join e in db.Employee on data.EmployeeId equals e.EmployeeId
            //                  join cl in db.Client on data.ClientId equals cl.ClientId
            //                  join ul in db.UserLocation on cl.ClientId equals ul.UserId
            //                  join u in db.Users on cl.ClientId equals u.Id
            //                  join sp in db.ServiceProvider on data.ServiceProviderId equals sp.ServiceProviderId
            //                  join j in db.Job on data.JobId equals j.JobId
            //                  select new
            //                  {
            //                      ComplainId = data.ComplaintId,
            //                      Description = data.Description,
            //                      ServiceProviderId = data.ServiceProviderId,
            //                      EmployeeName = e.FirstName,
            //                      ClientName = cl.FirstName,
            //                      ClientAddress = ul.Address,
            //                      ClientPhoneNumber=u.PhoneNumber,
            //                      ServiceProviderName = sp.CompanyName,
            //                      Status = data.Status,
            //                      jobDescription=j.Description,
            //                      JobAddress=j.JobAddress
            //                  }).Where(x => x.ServiceProviderId == serviceProviderId).ToList();

            //foreach (var item in complaints)
            //{
            //    ComplaintModel model = new ComplaintModel();
            //    model.ComplaintId = item.ComplainId;
            //    model.Description = item.Description;
            //    model.EmployeeName = item.EmployeeName;
            //    model.ClientName = item.ClientName;
            //    model.ServiceProviderName = item.ServiceProviderName;
            //    model.Status = item.Status;
            //    model.JobDescription = item.jobDescription;
            //    model.JobAddress = item.JobAddress;
            //    model.ClientAddress = item.ClientAddress;
            //    model.ClientPhoneNumber = item.ClientPhoneNumber;
            //    complaintModelList.Add(model);
            //}
            //unitOfWork.Commit();
            return complaintModelList;
        }
        #endregion

        #region Get Complaint By Complaint Id
        public ComplaintModel GetComplaintDetailByComplaintId(int compliantId)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            ComplaintModel complaintModelList = new ComplaintModel();
            Complaint complaintlist = new Complaint();
            var complaints = (from data in db.Complaint
                              join e in db.Employee on data.EmployeeId equals e.EmployeeId
                              join cl in db.Client on data.ClientId equals cl.ClientId
                              join ul in db.UserLocation on cl.ClientId equals ul.UserId
                              join u in db.Users on cl.ClientId equals u.Id
                              join sp in db.ServiceProvider on data.ServiceProviderId equals sp.ServiceProviderId
                              join j in db.Job on data.JobId equals j.JobId
                              select new
                              {
                                  ComplainId = data.ComplaintId,
                                  Description = data.Description,
                                  ServiceProviderId = data.ServiceProviderId,
                                  EmployeeName = e.FirstName,
                                  ClientName = cl.FirstName,
                                  ClientAddress = ul.Address,
                                  ClientPhoneNumber = u.PhoneNumber,
                                  ServiceProviderName = sp.CompanyName,
                                  Status = data.Status,
                                  jobDescription = j.Description,
                                  JobAddress = j.JobAddress
                              }).Where(x => x.ComplainId == compliantId).FirstOrDefault();

           
                ComplaintModel model = new ComplaintModel();
                model.ComplaintId = complaints.ComplainId;
                model.Description = complaints.Description;
                model.EmployeeName = complaints.EmployeeName;
                model.ClientName = complaints.ClientName;
                model.ServiceProviderName = complaints.ServiceProviderName;
                model.Status = complaints.Status;
                model.JobDescription = complaints.jobDescription;
                model.JobAddress = complaints.JobAddress;
                model.ClientAddress = complaints.ClientAddress;
                model.ClientPhoneNumber = complaints.ClientPhoneNumber;
                
          
            //unitOfWork.Commit();
            return model;
        }
        #endregion

        #region Get Complaint By complaint Id
        public ComplaintModel GetComplaintByComplaintId(int complaintId)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            ComplaintModel complaintModel = new ComplaintModel();
            Complaint complaint = new Complaint();
            complaint = repo.GetAll().Where(x => x.ComplaintId == complaintId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(complaint, complaintModel);
            return complaintModel;
        }
        #endregion

        #region Update Complaint 
        public ComplaintModel UpdateComplaint(ComplaintModel model)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            ComplaintModel complaintModel = new ComplaintModel();
            Complaint complaint = new Complaint();
            complaint = repo.GetAll().Where(x => x.ComplaintId == model.ComplaintId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, complaint);
            repo.Update(complaint);
            //unitOfWork.Commit();
            return complaintModel;
        }
        #endregion

        #region Insert Complaint 
        public ComplaintModel InsertComplaint(ComplaintModel model)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            Complaint complaint = new Complaint();
            AutoMapper.Mapper.Map(model, complaint);
            repo.Insert(complaint);
            AutoMapper.Mapper.Map(complaint,model);
            //unitOfWork.Commit();
            return model;
        }
        #endregion

        #region Delete Complaint 
        public void DeleteComplaint(int id)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            ComplaintModel complaintModel = new ComplaintModel();
            Complaint complaint = new Complaint();
            complaint = repo.GetAll().Where(x => x.ComplaintId == id).SingleOrDefault();
            repo.Delete(x => x.ComplaintId == complaint.ComplaintId);
            //unitOfWork.Commit();


        }
        #endregion

        public void DeleteComplaintbyEmployeeId(string id)
        {
            //unitOfWork.StartTransaction();
            ComplaintRepository repo = new ComplaintRepository(unitOfWork);
            ComplaintModel complaintModel = new ComplaintModel();
           List<Complaint> complaints = new List<Complaint>();
            complaints = repo.GetAll().Where(x => x.EmployeeId == id).ToList();
            complaints.ForEach(x =>
            {
                repo.Delete(y => y.ComplaintId == x.ComplaintId);
            });
            //unitOfWork.Commit();


        }
    }
}
