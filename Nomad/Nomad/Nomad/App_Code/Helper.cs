using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace Nomad.App_Code
{
    public class Helper
    {
        public static string GetCon()
        {
            return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
        }
        /// <summary>
        /// Returns a hash value using a secured hash algorithm (SHA-2)
        /// </summary>
        public static string Hash(string phrase)
        {
            SHA512Managed HashTool = new SHA512Managed();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(phrase));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        public static void SendEmail(string email, string subject, string message)
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.From = new MailAddress("benilde.web.development@gmail.com", "no-reply");
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            emailMessage.Priority = MailPriority.Normal;
            SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587);
            MailClient.EnableSsl = true;
            MailClient.Credentials = new System.Net.NetworkCredential("benilde.web.development@gmail.com", "!thisisalongpassword1234567890");
            MailClient.Send(emailMessage);
        }
        public static double GetPrice(string productID)
        {
            using (SqlConnection cn = new SqlConnection(GetCon()))
            {
                cn.Open();
                string query = @"SELECT Price
                                 FROM Products
                                 WHERE ProductID!=@ProductID";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return Convert.ToDouble((decimal)cmd.ExecuteScalar());
                }
            }
        }
        public static bool IsExisting(string productID)
        {
            using (SqlConnection cn = new SqlConnection(GetCon()))
            {
                cn.Open();
                string query = @"SELECT ProductID
                                 FROM OrderDetails
                                 WHERE OrderNo=@OrderNo AND UserID=@UserID AND ProductID=@ProductID";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }
        public static void AddToCartRecord(string productID, int qty)
        {
            using (SqlConnection cn = new SqlConnection(GetCon()))
            {
                cn.Open();
                string query = "";
                if (IsExisting(productID))
                {
                    query = @"UPDATE OrderDetails
                              SET Quantity = Quantity + @Quantity, Amount = Amount + @Amount
                              WHERE OrderNo=@OrderNo AND UserID=@UserID AND ProductID=@ProductID";
                }
                else
                {
                    query = @"INSERT INTO OrderDetails
                                 VALUES (@OrderNo, @UserID, @ProductID, @Quantity, @Amount, @Status)";
                }
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    //cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.Parameters.AddWithValue("@Amount", GetPrice(productID) * qty);
                    cmd.Parameters.AddWithValue("@Status", "In Cart");
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}