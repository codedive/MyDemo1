using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Web.Utility
{
    public static class Constants
    {
        public  const string BASE_FILE_UPLOAD_PATH = "~/Uploads/";

        public const string COMPANY_LOGO_PATH = "CompanyLogo";

        public const string EMPLOYEE_PROFILE_PATH = "EmployeeProfile"; 

        public const string NATIONAL_ID_PATH = "NationalId";

        public const string IQAMA_NUMBER_PATH = "IqamaNumber";
        
        public const string REGISTRATION_CERTIFICATE_PATH = "RegistrationCertificate";
        public const string THUMBNAILSIMAGES = "ThumbnailImages";

        public const string CLIENT_PROFILE_IMAGE_PATH = "ClientProfileImage";

        public const string JOB_SERVICE_IMAGES_PATH = "JobServiceImage";

        public const string SERVICES_LOGO = "ServicesLogo";

        public const string GOSI_CERTIFICATE_PATH = "GosiCertificate";

        public const string LINK_FOR_RESETPASSWORD = "<p><a href='http://112.196.23.162/#/resetpwd'>Click Here</a>";

        public const string LINK_FOR_CONFIRMATIONEMAIL = "<p><a href='http://112.196.23.162/#/confirmemail'>Click Here</a>";

        public const string TEXT_FOR_RESETPASSWORD = "You will be redirected to website page to change your password</br>";

        public const string TEXT_FOR_CONFIRMATION = "You will be redirected to website page to change your password</br>";

        public const string RESET_PASSWORD_SUBJECT = "Reset your password";

        public const string CONFIRMATION_SUBJECT = "Confirmation Email From Site.";

        public const string SMTPUSER = "wayne7896@gmail.com";

        public const string SMTPPASSWORD = "zxcvb12#$";

        #region Mail Template
        public const string CONFIRMATION_EMAIL_PATH = "~/MailTemplate/WelcomeEmail.html";
        public const string EMPLOYEE_USERNAME_PASSWORD_PATH = "~/MailTemplate/WelcomeEmailWithCredentials.html";
        public const string CONFIRMATION_OTP_PATH = "~/MailTemplate/OTPConfirmation.html";

        public const string FORGOT_PASSWORD_PATH = "~/MailTemplate/ForgotPassword.html";
        public const string FORGOT_PASSWORD_PATH_CLIENT = "~/MailTemplate/ForgotPasswordClient.txt";
        #endregion

        #region Rating Constants For Client
       // public const int RATING_FOR_COMMUNICATION = 5;
       // public const int RATING_FOR_UNDERSTANDING = 15;
        public const int RATING_FOR_CORPORATION = 15;
        public const int RATING_FOR_BEHAVIOUR = 50;
        public const int RATING_FOR_FRIENDLINESS = 10;
        public const int RATING_FOR_OVERALLSATSIFACTION = 5;
        #endregion
        #region Rating Constants For Employee and SerciceProvider
        public const int RATING_FOR_COMMUNICATION = 5;
        public const int RATING_FOR_UNDERSTANDING = 15;
        public const int RATING_FOR_ONTIME = 15;
        public const int RATING_FOR_QUALITY = 50;
        public const int RATING_FOR_CLEANLINESS = 10;
        public const int RATING_FOR_CONDUCT = 5;
        public const int MAX_RATING = 5;
        #endregion

        #region Notification 
        public const string SERVER_API_KEY = "AIzaSyA2tbpo1JYtnPAS7jnWrXnszvFNdQ4beeU";
        public const string SENDER_ID = "3733756368";
        #endregion

        #region Payment Section
        public const string COMMAND = "PURCHASE";
        public const string ACCESS_CODE = "aVMXe0Hkte2rreONsVzg";
        public const string MERCHANT_IDENTIFIER= "gCZWIKrf";
        public const string PHRASE= "pjlhjrureew2345";
        public const string SUCCESS_STATUS = "14";
        public const string PAYMENT_URL= "https://sbcheckout.payfort.com/FortAPI/paymentPage";
        #endregion
        #region Refund Section
        public const string REFUND_COMMAND = "REFUND";
        #endregion

        public const string SUB_SERVICE_TEXT = "SubService";
        public const string SERVICE_TEXT = "Service";
        #region filter Region
        public const string LOCATION_FILTER = "Location";
        public const string COST_FILTER = "Cost";
        public const string RATING_FILTER = "Rating";
        public const string SERVICE_FILTER = "Service";
        public const string ASCENDING = "Ascending";
        #endregion

        #region IOS Notification
        public const string DEVICE_TYPE_ANDROID = "A";
        public const string IOS_HOST ="gateway.sandbox.push.apple.com";
        public const int IOS_PORT = 2195;
        public const string CERTIFICATE_PATH = "/App_Data/Certificates_URFXEmp_Dev_Push.p12";
        #endregion

        #region Android Notification
        public const string DEVICE_TYPE_IOS = "I";
        public const string GOOGLE_API_URL= "https://android.googleapis.com/gcm/send";
        public const string METHOD = "post";
        #endregion

        public const int NotificationTime = 30;
        public const string MESSAGE_FOR_APPOINTMENT = "You have an appointment after 30 minutes";
        public const string APPOINTMENT_TYPE = "Appointment";
        public const string MESSAGE_FOR_CLIENTNOTIFY = "Your job is not accepted please contact other service provider";
        public const string CLIENT_NOTIFICATION_TYPE = "ClientNotification";
        public const string MESSAGE_FOR_JOBASSIGN = "This is your new task";
        public const string JOB_ASSING_TYPE = "JobAssign";
        public const string MESSAGE_FOR_TRACKING = "User Latitude and Longitude.";
        public const string TRACKING_TYPE = "Tracking";
        public const string MESSAGE_FOR_JOB_ACCEPT = "Your job is accepted";
        public const string JOB_ACCEPT_TYPE = "Accept";
        public const string MESSAGE_FOR_JOB_REJECT = "Your job is rejected please contact to other service provider";
        public const string JOB_REJECT_TYPE = "Reject";
        public const string MESSAGE_FOR_JOB_COMPLETED = "Your task is completed";
        public const string JOB_COMPLETED_TYPE = "Completed";
        public const string MESSAGE_FOR_JOB_CREATED = "Your job is created successfully please wait for service provider response.";
        public const string JOB_CREATED_TYPE = "JobCreated";
        public const string MESSAGE_FOR_ADD_ADDITIONAL = "Your payment for additional task is successfully done.";
        public const string ADD_ADDITIONAL_TYPE = "Additional";




    }
}