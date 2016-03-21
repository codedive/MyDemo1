using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using URFX.Business;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Enums;

namespace URFX.Web.Scheduler
{
   
    public class JobTask : ITask, IRegisteredObject
    {
        private URFXDbContext db = new URFXDbContext();
        EmployeeService employeeService = new EmployeeService();
        JobService jobService = new JobService();
        SendNotificationService sendNotificationService = new SendNotificationService();
        private readonly object _lock = new object();

        private bool _shuttingDown;


        public JobTask()
        {
            // Register this task with the hosting environment.
            // Allows for a more graceful stop of the task, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;
                //get all employee
                List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
                employeeModelList = employeeService.GetAllEmployees();
                //Get all jobs for each employee
                List<JobModel> jobModelList = new List<JobModel>();
                employeeModelList.ForEach(x => {
                    jobModelList = jobService.GetJobsByEmployeeId(x.EmployeeId,JobStatus.Current);
                    ApplicationUser user = db.Users.Where(z => z.Id == x.EmployeeId).FirstOrDefault();
                   
                    jobModelList.ForEach(y => {
                        if (y.StartDate != null)
                        {
                            if (user != null)
                            {
                                TimeSpan difference = Convert.ToDateTime(y.StartDate) - DateTime.Now;
                                var minutes = difference.Minutes;
                                if (minutes == Utility.Constants.NotificationTime && minutes > 0)
                                {
                                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                                    {
                                        string postData= "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + y.ClientId + "&data.jobId=" + y.JobId + "&data.type=" + Utility.Constants.APPOINTMENT_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_APPOINTMENT + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                                    }
                                    else
                                    {
                                       
                                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_APPOINTMENT + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + y.JobId + ",\"userId\":\"" + y.ClientId + "\",\"type\":\"" + Utility.Constants.APPOINTMENT_TYPE + "\"}";
                                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    });
                   
                });

               
                //SmtpClient mailClient = new SmtpClient("smtpout.secureserver.net", 25);
                //mailClient.EnableSsl = false;
                //mailClient.Credentials = new System.Net.NetworkCredential("confirmation@urfxco.com", "123456aa");
                //MailMessage message = new MailMessage();
                //message.From = new MailAddress("confirmation@urfxco.com");
                //message.Subject = "dfgdg";
                //message.IsBodyHtml = true;
                //message.Body = "gdfgdfg";
                //message.To.Add("narenderkumar@csgroupchd.com");
                //mailClient.Send(message);
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }

    public class ClientNotifyTask : ITask, IRegisteredObject
    {
        private URFXDbContext db = new URFXDbContext();
        EmployeeService employeeService = new EmployeeService();
        JobService jobService = new JobService();
        SendNotificationService sendNotificationService = new SendNotificationService();
        ClientService clientService = new ClientService();
        private readonly object _lock = new object();

        private bool _shuttingDown;


        public ClientNotifyTask()
        {
            // Register this task with the hosting environment.
            // Allows for a more graceful stop of the task, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;
                //get all clients
                List<ClientModel> clientModelList = new List<ClientModel>();
                clientModelList = clientService.GetAllClients();
                //Get all jobs for each employee
                List<JobModel> jobModelList = new List<JobModel>();
                clientModelList.ForEach(x => {
                    jobModelList = jobService.GetJobListByClientId(x.ClientId, JobStatus.New);
                    ApplicationUser user = db.Users.Where(z => z.Id == x.ClientId).FirstOrDefault();

                    jobModelList.ForEach(y => {
                        if (y.CreatedDate != null)
                        {
                            if (user != null)
                            {
                                DateTime respondingDate = Convert.ToDateTime(y.CreatedDate).AddMinutes(3);
                               
                                if (respondingDate == DateTime.Now && y.Status==JobStatus.New )
                                {
                                    
                                    if (user.DeviceType == Utility.Constants.DEVICE_TYPE_ANDROID)
                                    {
                                        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.userId=" + y.ClientId + "&data.jobId=" + y.JobId + "&data.type=" + Utility.Constants.CLIENT_NOTIFICATION_TYPE + "&data.message=" + Utility.Constants.MESSAGE_FOR_CLIENTNOTIFY + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + user.DeviceToken + "";
                                        var response = sendNotificationService.SendNotificationForAndroid(postData);
                                    }
                                    else
                                    {
                                        string payload = "{\"aps\":{\"alert\":\"" + "Hi, " + Utility.Constants.MESSAGE_FOR_CLIENTNOTIFY + "" + "\",\"badge\":1,\"sound\":\"default\"},\"JobId\":" + y.JobId + ",\"userId\":\"" + y.ClientId + "\",\"type\":\"" + Utility.Constants.CLIENT_NOTIFICATION_TYPE + "\"}";
                                        sendNotificationService.SendNotificationForIOS(user.DeviceToken, payload);
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    });

                });


                
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}