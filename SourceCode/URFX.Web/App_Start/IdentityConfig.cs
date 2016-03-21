using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using URFX.Data.DataEntity;
using System.Configuration;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Globalization;
using System.Net.Mail;

namespace URFX.Web
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<URFXDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity")) {
                    TokenLifespan = TimeSpan.FromHours(3)
                };
            }
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService=new EmailService();
            //manager.SmsService = new SmsService();
            return manager;
        }
    }
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //Credentials:
            var credentialUserName = ConfigurationManager.AppSettings["FromEmail"];
            var sentFrom = ConfigurationManager.AppSettings["FromEmail"];
            var pwd = ConfigurationManager.AppSettings["pwd"];

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["Host"]);

            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SslFalse"]);

            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;
            // Send:
            return client.SendMailAsync(mail);










        }
    }

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        var Twilio = new TwilioRestClient(
    //          Keys.SMSAccountIdentification,
    //          Keys.SMSAccountPassword);
    //        var result = Twilio.SendMessage(
    //          Keys.SMSAccountFrom,
    //          message.Destination, message.Body
    //        );
    //        //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
    //         Trace.TraceInformation(result.Status);
    //       // Twilio doesn't currently have an async API, so return success.
    //         return Task.FromResult(0);
    //    }

        
    //}
    public static class Keys
    {
        public static string SMSAccountIdentification = "ACaac07e100f984bfb3dbd8cc9030c607f";
        public static string SMSAccountPassword = "9f9780861b294eda61fa508400f6af99";
        public static string SMSAccountFrom = "+17577932839";
    }

    public class SendNotificationService
    {
        public string SendNotificationForAndroid(string postData)
        {
            string SERVER_API_KEY = ConfigurationManager.AppSettings["SERVER_API_KEY"];
            var SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"];
            
            WebRequest tRequest;
            tRequest = WebRequest.Create(Utility.Constants.GOOGLE_API_URL);
            tRequest.Method = Utility.Constants.METHOD;
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();


            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }


        public  void SendNotificationForIOS(string deviceID,string payload)
        {
            int port = Utility.Constants.IOS_PORT;
            String hostname = Utility.Constants.IOS_HOST;
            String certificatePath = System.Web.Hosting.HostingEnvironment.MapPath(Utility.Constants.CERTIFICATE_PATH);
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath));
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try
            {
                 sslStream.AuthenticateAsClient(hostname, certificatesCollection,SslProtocols.Default| SslProtocols.Tls, false);
                //sslStream.AuthenticateAsServer(clientCertificate, certificatesCollection, SslProtocols.Tls, false);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(HexStringToByteArray(deviceID.ToUpper()));
                //String payload = "{\"aps\":{\"alert\":\"" + "Hi,, "+ message + "" + "\",\"badge\":1,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
            }
            catch (Exception e)
            {
                client.Close();
            }

            
        }

        

        private static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length/2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }


    }


    }

       
