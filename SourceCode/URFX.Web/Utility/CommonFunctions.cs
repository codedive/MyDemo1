using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Resources;
using URFX.Web.Models;

namespace URFX.Web.Utility
{
    public static class CommonFunctions
    {
        public static string ReadResourceValue(string Value)
        {
            string resourceValue = string.Empty;
            try
            {
                resourceValue = Resources.ResourceManager.GetString(Value);
                return resourceValue;
                
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                resourceValue = Value;
            }
            return resourceValue;
        }
        public static decimal GetTotalFeedbackForClient(ClientRatingModel model)
        {
            decimal totalRating;
            int maxRating = Constants.MAX_RATING;

                totalRating =Math.Ceiling(Convert.ToDecimal((Constants.RATING_FOR_CORPORATION * model.Corporation / maxRating + Constants.RATING_FOR_COMMUNICATION * model.Communication / maxRating +
                    Constants.RATING_FOR_UNDERSTANDING * model.UnderStanding / maxRating + Constants.RATING_FOR_BEHAVIOUR * model.Behaivor / maxRating +
                    Constants.RATING_FOR_FRIENDLINESS * model.FriendLiness / maxRating + Constants.RATING_FOR_OVERALLSATSIFACTION * model.OverallSatisfaction / maxRating) * maxRating/100));
                return totalRating;
        }
        public static decimal GetTotalFeedback(RatingModel model)
        {
            decimal totalRating;
            int maxRating = Constants.MAX_RATING; ;

            totalRating = Math.Ceiling(Convert.ToDecimal((Constants.RATING_FOR_ONTIME * model.OnTime / maxRating + Constants.RATING_FOR_COMMUNICATION * model.Communication / maxRating +
                Constants.RATING_FOR_UNDERSTANDING * model.UnderStandingOfServiceRequired / maxRating + Constants.RATING_FOR_QUALITY * model.Quality / maxRating +
                Constants.RATING_FOR_CLEANLINESS * model.Cleanliness / maxRating + Constants.RATING_FOR_CONDUCT * model.Conduct / maxRating) * maxRating / 100));
            return totalRating;
        }

        public static string Decrypt(string TextToBeDecrypted)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            string Password = "CSC";
            string DecryptedData;

            try
            {
                byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted);

                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
                //Making of the key for decryption
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                //Creates a symmetric Rijndael decryptor object(chahal).
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                //Defines the cryptographics stream for decryption.THe stream contains decrpted data
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();

                //Converting to string
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch(Exception ex)
            {
                DecryptedData = TextToBeDecrypted;
            }
            return DecryptedData;
        }

        public static string Encrypt(string TextToBeEncrypted)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            string Password = "CSC";
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(TextToBeEncrypted);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            //Creates a symmetric encryptor object(chahal). 
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;
        }

    }
}
