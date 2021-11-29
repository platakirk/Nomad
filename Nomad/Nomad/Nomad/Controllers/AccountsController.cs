using Nomad.App_Code;
using Nomad.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nomad.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel record)
        {
            //Step 1: Open database
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT UserID, TypeID FROM Users
                    WHERE Username=@Username AND Password=@Password
                    AND Status!=@Status";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@Username", record.Username);
                    sqlCmd.Parameters.AddWithValue("@Password", record.Password);
                    sqlCmd.Parameters.AddWithValue("@Status", "Archived");
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.HasRows)
                        {
                            while (sqlDr.Read())
                            {
                                Session["userid"] = sqlDr["UserId"].ToString();
                                Session["typeid"] = sqlDr["TypeId"].ToString();
                            }
                            return RedirectToAction("MyProfile");
                        }
                        else // email and password dont match 
                        {
                            ViewBag.Error = "<div class='alert alert-danger col-lg-5'> Invalid Credentials</div>";
                            return View(record);
                        }
                    }
                }
            }

            //Step 2: Execute query to check if email + password match
            //Step 3: If account match, proceed to main page
            //Step 4: If account not match, display error message 
        }

        public ActionResult MyProfile()
        {
            if (Session["UserID"] == null) // user did not log in 
            {
                return RedirectToAction("Login");
            }

            var record = new UserModel();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT FirstName, MiddleName, LastName, Email,
                    Mobile, Username, Password
                    FROM Users
                    Where UserID=@UserID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            record.FirstName = sqlDr["FirstName"].ToString();
                            record.MiddleName = sqlDr["MiddleName"].ToString();
                            record.LastName = sqlDr["LastName"].ToString();
                            record.Email = sqlDr["Email"].ToString();
                            record.Mobile = sqlDr["Mobile"].ToString();
                            record.Username = sqlDr["Username"].ToString();
                            record.Password = sqlDr["Password"].ToString();
                        }
                        return View(record);
                    }
                }
            }
        }
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }
    }

}