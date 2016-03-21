using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;
using URFX.Web.Models;
using URFX.Web.Utility;

namespace URFX.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Plans")]
    public class PlansController : ApiController
    {
        private URFXDbContext db = new URFXDbContext();
        PlanService planService = new PlanService();
        TransactionHistoryService transactionHistoryService = new TransactionHistoryService();
        UserPlanService userPlanService = new UserPlanService();
        SendNotificationService sendNotificationService = new SendNotificationService();
        ServiceProviderService serviceProviderService = new ServiceProviderService();
        JobService jobService = new JobService();
        private ApplicationUserManager _userManager;
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
        // GET: api/Plans
        public List<Plan> GetPlans()
        {
            List<Plan> planList = new List<Plan>();
            planList = planService.GetPlans().Select(x => new Plan
            {
                Description = CommonFunctions.ReadResourceValue(x.Description),
                ApplicationFee = x.ApplicationFee,
                PlanId = x.PlanId,
                CreatedDate = x.CreatedDate,
                Detail = CommonFunctions.ReadResourceValue(x.Detail),
                IsActive = x.IsActive,
                PerVisitPercentage = x.PerVisitPercentage,
                TeamRegistrationFee = x.TeamRegistrationFee,
                TeamRegistrationType = x.TeamRegistrationType,
                PlanFeatures=x.PlanFeatures
            }).ToList();
            return planList;
        }

        // GET: api/Plans/5
        [ResponseType(typeof(Plan))]
        public IHttpActionResult GetPlan(int id)
        {
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        // PUT: api/Plans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlan(int id, Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plan.PlanId)
            {
                return BadRequest();
            }

            db.Entry(plan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
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

        // POST: api/Plans
        [ResponseType(typeof(Plan))]
        public IHttpActionResult PostPlan(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Plans.Add(plan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = plan.PlanId }, plan);
        }

        // POST: api/Plans/ReturnSucceesCallback
        [Route("ReturnSucceesCallback")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult ReturnSucceesCallback(TransactionHistoryModel model)
        {
            try
            {
                //string headers = string.Empty;
                //foreach (var head in Request.Content.Headers)
                //{
                //    headers += "Key : " + head.Key + " , Value : " + head.Value;
                //}

                //string parameters = string.Empty;

                //foreach (var param in HttpContext.Current.Request.Params)
                //{
                var merchantRefrence = model.MerchantReference;
                TransactionHistoryModel historyModel = new TransactionHistoryModel();
                historyModel = transactionHistoryService.FindTransactionHistoryByMerchantRegrence(merchantRefrence);
                historyModel.Status = model.Status;
                historyModel.ResponseMessage = model.ResponseMessage;
                historyModel.Eci = model.Eci;
                historyModel.CardNumber = model.CardNumber;
                historyModel.FortId = model.FortId;
                historyModel.ResponseCode = model.ResponseCode;
                historyModel.CustomerEmail = model.CustomerEmail;
                historyModel.CustomerIp = model.CustomerIp;
                //historyModel.Amount = model.Amount;
                historyModel.Command = model.Command;
                historyModel.PaymentOption = model.PaymentOption;
                historyModel.ExpiryDate = model.ExpiryDate;
                historyModel.Signature = model.Signature;
                if (historyModel.URFXPaymentType == URFXPaymentType.PlanPayment)
                {

                    if (historyModel.Status == Utility.Constants.SUCCESS_STATUS)
                    {

                        var startDate = DateTime.Now;
                        UserPlanModel userPlanModel = new UserPlanModel();
                        userPlanModel.PlanId = historyModel.PlanId;
                        userPlanModel.UserId = historyModel.UserId;
                        userPlanModel.NumberOfTeams = historyModel.NumberOfTeams;
                        userPlanModel.CreatedDate = startDate;
                        userPlanModel.ExpiredDate = startDate.AddDays(30);
                        bool planExist = userPlanService.CheckExistanceOfUserPlan(userPlanModel.UserId);
                        if (!planExist)
                        {
                            userPlanModel = userPlanService.InsertUserPlan(userPlanModel);

                        }
                        transactionHistoryService.UpdateTransactionHistory(historyModel);
                        //parameters += "Key : " + param + " , value:" + HttpContext.Current.Request.Params[param.ToString()];
                    }
                    else
                    {
                        transactionHistoryService.UpdateTransactionHistory(historyModel);
                    }
                }
                else if(historyModel.URFXPaymentType == URFXPaymentType.JobPayment)
                {
                    ApplicationUser user = UserManager.FindById(historyModel.UserId);
                    transactionHistoryService.UpdateTransactionHistory(historyModel);
                    JobModel jobModel = new JobModel();
                    if (historyModel.Status == Utility.Constants.SUCCESS_STATUS)
                    {
                       
                        jobModel = jobService.GetJobById(historyModel.JobId);
                        jobModel.IsPaid = true;
                        jobModel.CreatedDate = DateTime.UtcNow;
                        jobService.UpadteJob(jobModel);
                    }

                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + historyModel.UserId + "&data.jobId=" + historyModel.JobId + "&data.type=" + Utility.Constants.JOB_CREATED_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_JOB_CREATED + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_JOB_CREATED + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + historyModel.JobId + ",\"userId\":\"" + historyModel.UserId + "\",\"type\":\"" + Utility.Constants.JOB_CREATED_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }
                else
                {
                    ApplicationUser user = UserManager.FindById(historyModel.UserId);
                    transactionHistoryService.UpdateTransactionHistory(historyModel);
                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                    {
                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + historyModel.UserId + "&data.jobId=" + historyModel.JobId + "&data.type=" + Utility.Constants.ADD_ADDITIONAL_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_ADD_ADDITIONAL + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                    }
                    else if (user.DeviceType == Utility.Constants.DEVICE_TYPE_IOS)
                    {
                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_ADD_ADDITIONAL + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + historyModel.JobId + ",\"userId\":\"" + historyModel.UserId + "\",\"type\":\"" + Utility.Constants.ADD_ADDITIONAL_TYPE + "\"}";
                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                    }
                }
                // parameters += "Key : " + param + " , value:" + HttpContext.Current.Request.Params[param.ToString()]; 
                // }

                // System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/ExceptionFile.txt"), headers + parameters);

                return Ok(historyModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/ExceptionFile.txt"), ex.Message);
                return Ok();
            }



        }



        //[Route("ReturnRefundCallbcak")]
        //[HttpGet]
        //[HttpPost]
        //public IHttpActionResult ReturnRefundCallbcak()
        //{
        //    try
        //    {
        //        string headers = string.Empty;
        //        foreach (var head in Request.Content.Headers)
        //        {
        //            headers += "Key : " + head.Key + " , Value : " + head.Value;
        //        }

        //        string parameters = string.Empty;

        //        foreach (var param in HttpContext.Current.Request.Params)
        //        {


        //            parameters += "Key : " + param + " , value:" + HttpContext.Current.Request.Params[param.ToString()];
        //        }


        //        // }

        //        System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/ExceptionFile.txt"), headers + parameters);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/ExceptionFile.txt"), ex.Message);
        //        return Ok();
        //    }



        //}

        // DELETE: api/Plans/5
        [ResponseType(typeof(Plan))]
        public IHttpActionResult DeletePlan(int id)
        {
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }

            db.Plans.Remove(plan);
            db.SaveChanges();

            return Ok(plan);
        }

        // POST: api/Plans/PayNow
        [Route("PayNow")]
        public IHttpActionResult PayNow(UserPlanBindingModel plan)
        {
            try
            {
                plan.UserId = User.Identity.GetUserId();
                plan.Email = User.Identity.GetUserName();
                plan.MerchantIdentifier = Utility.Constants.MERCHANT_IDENTIFIER;
                plan.AccessCode = Utility.Constants.ACCESS_CODE;
                plan.Command = Utility.Constants.COMMAND;
                //Guid id = Guid.NewGuid();
                Random r = new Random();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D20");
                plan.Amount = plan.TotalValue;
                plan.MerchantReference = sixDigitNumber;
                plan.URFXPaymentType = Data.Enums.URFXPaymentType.PlanPayment;
                TransactionHistoryModel historyModel = new TransactionHistoryModel();
                AutoMapper.Mapper.Map(plan, historyModel);
                historyModel.CreatedDate = DateTime.UtcNow;
                historyModel = transactionHistoryService.InsertTransactionHistory(historyModel);
                AutoMapper.Mapper.Map(historyModel, plan);
                plan.TotalValue = plan.TotalValue * 100;
                byte[] secretkey = new Byte[64];
                SHA256Managed mysha256 = new SHA256Managed();

                byte[] bytedText = System.Text.UTF8Encoding.UTF8.GetBytes("" + Utility.Constants.PHRASE + "access_code=" + plan.AccessCode + "amount=" + plan.TotalValue + "command=" + plan.Command + "currency=" + plan.Currency + "customer_email=" + plan.Email + "language=" + plan.Language + "merchant_identifier=" + plan.MerchantIdentifier + "merchant_reference=" + plan.MerchantReference + Utility.Constants.PHRASE);
                byte[] hashValue = mysha256.ComputeHash(bytedText);
                byte[] hash = HashHMAC(bytedText);
                plan.SecretKey = HashEncode(hash);


                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Delete Transaction
        //api/Plans/DeleteTransaction
        [Route("DeleteTransaction")]
        public IHttpActionResult DeleteTransaction(int Id)
        {
            try
            {
                transactionHistoryService.DeleteTransaction(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        //

        #region Get Plan By Id
        [Route("GetTranscationHistoryById")]
        public IHttpActionResult GetTranscationHistoryById(int Id)
        {
            //get transcation history
            TransactionHistoryModel model = transactionHistoryService.GetTransactionHistoryByCartId(Id);
            TransactionHistoryBindingModel bindingModel = new TransactionHistoryBindingModel();
            AutoMapper.Mapper.Map(model, bindingModel);
            //get plan info
            PlanModel planModel = planService.GetPlanById(bindingModel.PlanId);
            bindingModel.PlanName = planModel.Description;
            bindingModel.URFXPaymentTypeString = ((URFXPaymentType)bindingModel.URFXPaymentType).ToString();
            //get job detail
            JobModel jobModel = new JobModel();
            jobModel = jobService.GetJobById(model.JobId);
            bindingModel.JobDescription = jobModel.Description;
            return Ok(bindingModel);
        }
        #endregion

        #region Get Transcation History By Service Provider Id
        [Route("GetTranscationHistoryByServiceProviderId")]
        public IHttpActionResult GetTranscationHistoryByServiceProviderId(string Id)
        {
            
            
            //get job detail
            FinanceBindingModel financeBindingModel = new FinanceBindingModel();
            //get deatils for service provider
            ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
            serviceProviderModel = serviceProviderService.GetServiceProviderById(Id);
            financeBindingModel.ServiceProviderName = serviceProviderModel.CompanyName;
            List<JobBindingModel> jobBindingModel = new List<JobBindingModel>();
            List<JobModel> jobModel = new List<JobModel>();
            jobModel = jobService.GetJobListByServiceProviderIdWithClient(Id);
            AutoMapper.Mapper.Map(jobModel, jobBindingModel);
            //get transcation history
            jobBindingModel.ForEach(x =>
            {
                financeBindingModel.Description = x.Description;
                x.TransactionHistoryModel = transactionHistoryService.GetTransactionHistoryListByJobId(x.JobId);
                x.Email = x.TransactionHistoryModel.CustomerEmail;
                x.Amount = x.TransactionHistoryModel.Amount;
                x.PaymentType = x.TransactionHistoryModel.URFXPaymentType;
                x.ClientName = x.Client.FirstName +' '+ x.Client.LastName;
                x.TransactionDate = x.TransactionHistoryModel.CreatedDate;
                financeBindingModel.TotalEarnedAmount += x.TransactionHistoryModel.Amount;
            });
            financeBindingModel.JobModel = jobBindingModel;
            financeBindingModel.NumberOfJobs = jobBindingModel.Count();
            

            return Ok(financeBindingModel);
        }
        #endregion

        // POST: api/Plans/Refund
        //[Route("Refund")]
        //public async Task<IHttpActionResult> Refund(int cartId)
        //{
        //    TransactionHistoryModel historyModel = new TransactionHistoryModel();
        //    historyModel = transactionHistoryService.FindTransactionHistoryByMerchantRegrence(cartId);
        //    historyModel.Command = Utility.Constants.REFUND_COMMAND;
        //    byte[] secretkey = new Byte[64];
        //    SHA256Managed mysha256 = new SHA256Managed();

        //    byte[] bytedText = System.Text.UTF8Encoding.UTF8.GetBytes(""+Utility.Constants.PHRASE+"access_code=" + historyModel.AccessCode + "amount=" + historyModel.Amount + "command=" + historyModel.Command + "currency=" + historyModel.Currency + "fort_id="+historyModel.FortId+ "language=" + historyModel.Language + "merchant_identifier=" + historyModel.MerchantIdentifier+ Utility.Constants.PHRASE);
        //    byte[] hashValue = mysha256.ComputeHash(bytedText);
        //    byte[] hash = HashHMAC(bytedText);
        //    historyModel.Signature = HashEncode(hash);

        //   //var client = new RestClient("https://sbpaymentservices.payfort.com/FortAPI/paymentApi");
        //    //// client.Authenticator = new HttpBasicAuthenticator(username, password);

        //    //var request = new RestRequest("resource/{id}", Method.POST);
        //    //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
        //    //                                       // request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

        //    //// easily add HTTP Headers
        //    //request.AddHeader("header", "value");



        //    //// execute the request
        //    //IRestResponse response = client.Execute(request);
        //    //var content = response.Content; // raw content as string

        //    //// or automatically deserialize result
        //    //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
        //    //RestResponse<Person> response2 = client.Execute<Person>(request);
        //    //var name = response2.Data.Name;

        //    //// easy async support
        //    //client.ExecuteAsync(request, response =>
        //    //{
        //    //    Console.WriteLine(response.Content);
        //    //});

        //    //// async with deserialization
        //    //var asyncHandle = client.ExecuteAsync<Person>(request, response =>
        //    //{
        //    //    Console.WriteLine(response.Data.Name);
        //    //});

        //    //// abort the request on demand
        //    //asyncHandle.Abort();

        //    Uri theUri = new Uri("https://sbpaymentservices.payfort.com/FortAPI/paymentApi");

        //    //Create an Http client and set the headers we want
        //    HttpClient aClient = new HttpClient();

        //    aClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    // aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
        //    // aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
        //    aClient.DefaultRequestHeaders.Host = theUri.Host;
        //    RefundModel model = new RefundModel();
        //    model.access_code = historyModel.AccessCode;
        //    model.amount = historyModel.Amount;
        //    model.command = historyModel.Command;
        //    model.currency = historyModel.Currency;
        //    model.fort_id = historyModel.FortId;
        //    model.language = historyModel.Language;
        //    model.merchant_identifier = historyModel.MerchantIdentifier;
        //    model.merchant_reference = historyModel.MerchantReference;
        //    model.signature = historyModel.Signature;
        //    var json = JsonConvert.SerializeObject(model);

        //    //Create a Json Serializer for our type
        //    DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(RefundModel));

        //    // use the serializer to write the object to a MemoryStream
        //      MemoryStream ms = new MemoryStream();
        //    jsonSer.WriteObject(ms, model);
        //    ms.Position = 0;

        //    //use a Stream reader to construct the StringContent (Json)
        //     StreamReader sr = new StreamReader(ms);

        //    //var json = JsonConvert.SerializeObject(historyModel);

        //    // StreamReader sr = new StreamReader(json);
        //    // Note if the JSON is simple enough you could ignore the 5 lines above that do the serialization and construct it yourself
        //    // then pass it as the first argument to the StringContent constructor
        //    StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        //    //Post the data
        //    HttpResponseMessage aResponse = await aClient.PostAsync(theUri, theContent);

        //    if (aResponse.IsSuccessStatusCode)
        //    {

        //    }
        //    else
        //    {
        //        // show the response status code
        //        String failureMsg = "HTTP Status: " + aResponse.StatusCode.ToString() + " - Reason: " + aResponse.ReasonPhrase;
        //    }

        //    return Ok(historyModel);
        //}






        // POST: api/Plans/ClientPayment
        [Route("ClientPayment")]
        [AllowAnonymous]
        public IHttpActionResult ClientPayment(UserPlanBindingModel plan)
         {
            try
            {
                ApplicationUser user = UserManager.FindById(plan.UserId);
                if (user != null)
                {
                    plan.Email = user.Email;// User.Identity.GetUserName();  
                    plan.MerchantIdentifier = Utility.Constants.MERCHANT_IDENTIFIER;
                    plan.AccessCode = Utility.Constants.ACCESS_CODE;
                    plan.Command = Utility.Constants.COMMAND;//"AUTHORIZATION"; //
                                                             //Guid id = Guid.NewGuid();
                    Random r = new Random();
                    int randNum = r.Next(1000000);
                    string sixDigitNumber = randNum.ToString("D20");
                    plan.Amount = plan.TotalValue;
                    plan.MerchantReference = sixDigitNumber;
                    if(plan.PaymentType == URFXPaymentType.JobPayment.ToString())
                    {
                        plan.URFXPaymentType = Data.Enums.URFXPaymentType.JobPayment;
                    }
                    else
                    {
                        plan.URFXPaymentType = Data.Enums.URFXPaymentType.JobAdditionalPayment;
                    }
                    TransactionHistoryModel historyModel = new TransactionHistoryModel();
                    AutoMapper.Mapper.Map(plan, historyModel);
                    historyModel.CreatedDate = DateTime.UtcNow;
                    historyModel = transactionHistoryService.InsertTransactionHistory(historyModel);
                    AutoMapper.Mapper.Map(historyModel, plan);
                    plan.TotalValue = plan.TotalValue * 100;
                    byte[] secretkey = new Byte[64];
                    SHA256Managed mysha256 = new SHA256Managed();

                    byte[] bytedText = System.Text.UTF8Encoding.UTF8.GetBytes("" + Utility.Constants.PHRASE + "access_code=" + plan.AccessCode + "amount=" + plan.TotalValue + "command=" + plan.Command + "currency=" + plan.Currency + "customer_email=" + plan.Email + "language=" + plan.Language + "merchant_identifier=" + plan.MerchantIdentifier + "merchant_reference=" + plan.MerchantReference + Utility.Constants.PHRASE);
                    byte[] hashValue = mysha256.ComputeHash(bytedText);
                    byte[] hash = HashHMAC(bytedText);
                    plan.SecretKey = HashEncode(hash);
                    plan.Url = Utility.Constants.PAYMENT_URL;
                    return Json(plan);
                }
                else
                {
                    return BadRequest("User not found");
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return BadRequest(ex.Message);
            }

        }
        // Get: api/Plans/GetAllTransactions
        [HttpGet]
        [Route("GetAllTransactions")]
        public IHttpActionResult GetAllTransactions()
        {
            List<TransactionHistoryModel> transactionHistoryModelList = new List<TransactionHistoryModel>();
            transactionHistoryModelList = transactionHistoryService.GetAllTransactionHistory();
            return Ok(transactionHistoryModelList);
        }

        #region Paging For Transaction History
        [HttpPost]
        [AllowAnonymous]
        [Route("Paging")]
        public ResponseMessage Paging(PagingModel model)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<TransactionHistoryModel> transactionHistoryModel = new List<TransactionHistoryModel>();
            transactionHistoryModel = transactionHistoryService.Paging(model);
            responseMessage.totalRecords = transactionHistoryModel.Count();
            transactionHistoryModel = transactionHistoryModel.OrderBy(x => x.CustomerEmail).Skip((model.CurrentPageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            transactionHistoryModel.ForEach(x => {
                JobModel jobModel = new JobModel();
                ServiceProviderModel serviceProviderModel = new ServiceProviderModel();
                if (x.JobId > 0)
                {
                    
                    jobModel = jobService.GetJobById(x.JobId);
                }
                serviceProviderModel = serviceProviderService.GetServiceProviderById(jobModel.ServiceProviderId);
                x.ServiceProviderName = serviceProviderModel.CompanyName;
            });
            responseMessage.data = transactionHistoryModel;


            return responseMessage;
        }
        #endregion
        private static byte[] HashHMAC(byte[] message)
        {
            var hash = new SHA256Managed();
            return hash.ComputeHash(message);
        }
        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlanExists(int id)
        {
            return db.Plans.Count(e => e.PlanId == id) > 0;
        }


    }
}