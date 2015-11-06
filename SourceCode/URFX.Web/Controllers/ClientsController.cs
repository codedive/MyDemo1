using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Data.Infrastructure;
using URFX.Web.Models;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    public class ClientsController : ApiController
    {
        ClientService clientService = new ClientService();
        private ApplicationUserManager _userManager;

        public ClientsController()
        {

        }
        public ClientsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #region Get All Clients
        // GET: api/Clients
        public List<ClientModel> GetClient()
        {
            List<ClientModel> clientList = new List<ClientModel>();
            clientList = clientService.GetAllClients().Select(x=>new ClientModel {
             ClientId=x.ClientId,
             FirstName= CommonFunctions.ReadResourceValue(x.FirstName),
             LastName=CommonFunctions.ReadResourceValue(x.LastName),
             NationalIdNumber=CommonFunctions.ReadResourceValue(x.NationalIdNumber),
             NationaltIDType=x.NationaltIDType,
             IsActive=x.IsActive,
             IsDeleted=x.IsDeleted
            }).ToList();
            return clientList;
        }
        #endregion

        #region Get Client By Id
        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(string id)
        {
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(id);
            clientModel.FirstName = CommonFunctions.ReadResourceValue(clientModel.FirstName);
            clientModel.LastName = CommonFunctions.ReadResourceValue(clientModel.LastName);
            clientModel.NationalIdNumber = CommonFunctions.ReadResourceValue(clientModel.NationalIdNumber);
            if (clientModel == null)
            {
                return NotFound();
            }
            return Ok(clientModel);
        }
        #endregion

        #region Update Client
         // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(string id, RegisterClientBindingModel client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != client.ClientId)
            //{
            //    return BadRequest();
            //}

            try
            {
                ApplicationUser user = UserManager.FindById(id);
                user.Email = client.Email;
                IdentityResult result = UserManager.Update(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    client.ClientId = id;
                    ClientModel clientModel = new ClientModel();

                    AutoMapper.Mapper.Map(client, clientModel);
                    clientService.UpadteClient(clientModel);
                    AutoMapper.Mapper.Map(clientModel, client);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!clientService.CheckExistance(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion

        #region Delete Client
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(string id)
        {
            ClientModel clientModel = new ClientModel();
            clientModel = clientService.GetClientById(id);
            if (clientModel == null)
            {
                return NotFound();
            }
            clientService.DeleteClient(id);
            ApplicationUser user = UserManager.FindById(id);

            IdentityResult result = UserManager.Delete(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
             return Ok();
        }
        #endregion

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}