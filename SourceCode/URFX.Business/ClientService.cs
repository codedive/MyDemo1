﻿using System;
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
   public class ClientService
    {

        private URFXDbContext db = new URFXDbContext();
        UnitOfWork unitOfWork = new UnitOfWork();

        #region Get All Clients
        public List<ClientModel> GetAllClients()
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            List<ClientModel> clientList = new List<ClientModel>();
            List<Client> client = new List<Client>();
            AutoMapper.Mapper.Map(clientList, client);
            client = repo.GetAll().ToList();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(client, clientList);
            return clientList;
        }
        #endregion

        #region Get Client By Id
        public ClientModel GetClientById(string clientId)
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            ClientModel clientModel = new ClientModel();
            Client client = new Client();
            AutoMapper.Mapper.Map(clientModel, client);
             client = repo.GetAll().Where(x => x.ClientId == clientId).FirstOrDefault();
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(client, clientModel);
            return clientModel;
        }
        #endregion


        #region Save Client
        public ClientModel SaveClient(ClientModel model)
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            Client client = new Client();
            AutoMapper.Mapper.Map(model, client);
            repo.Insert(client);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(client, model);
            return model;
        }

        #endregion

        #region Update Client
        public ClientModel UpadteClient(ClientModel model)
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            Client client = new Client();
            client = repo.GetAll().Where(x => x.ClientId == model.ClientId).FirstOrDefault();
            AutoMapper.Mapper.Map(model, client);
            repo.Update(client);
            //unitOfWork.Commit();
            AutoMapper.Mapper.Map(client, model);
            return model;
        }
        #endregion

        #region Delete Client
        public void DeleteClient(string clientId)
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            repo.Delete(x=>x.ClientId== clientId);
            //unitOfWork.Commit();

        }
        #endregion

        #region Check Existance Of Client By Id
        public bool CheckExistance(string clientId )
        {
            //unitOfWork.StartTransaction();
            ClientRepository repo = new ClientRepository(unitOfWork);
            var client = repo.GetAll().Where(x => x.ClientId == clientId).Count();
            //unitOfWork.Commit();
            if (client > 0)
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
