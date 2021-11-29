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
    public class UserController : Controller
    {
        public ActionResult Create()
        {
            //HOW TO DO ACCESS LEVELS
            if (Session["TypeID"] != null)
            {
                if (Session["TypeID"].ToString() != "1" && Session["TypeID"].ToString() != "2")
                {
                    return RedirectToAction("MyProfile");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            var record = new UserModel();
            record.Types = GetUsersTypes();
            return View(record);
        }
        //@Html.DropDownListFor(model => model.TypeID, new SelectList(Model.Types, "TypeID", "Type"), "Select from the list...", new { @class = "form-control" })

        [HttpPost]
        public ActionResult Create(UserModel record)
        {
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"INSERT INTO Users
                                 VALUES (@TypeID, @FirstName, @MiddleName, @LastName, @Email, @Mobile, @Username, @Password, @Status, @DateCreated, @LastLogin);";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@TypeID", record.TypeID);
                    cmd.Parameters.AddWithValue("@FirstName", record.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", record.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", record.LastName);
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Mobile", record.Mobile);
                    cmd.Parameters.AddWithValue("@Username", record.Username);
                    cmd.Parameters.AddWithValue("@Password", record.Password);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LastLogin", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        public List<UserTypeModel> GetUsersTypes()
        {
            var list = new List<UserTypeModel>();
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"SELECT TypeID, Type
                                 FROM UserType
                                 ORDER BY Type";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new UserTypeModel
                            {
                                TypeID = int.Parse(dr["TypeID"].ToString()),
                                Type = dr["Type"].ToString()

                            });

                        }
                        return list;
                    }
                }
            }
        }
        public ActionResult Details(int? id)
        {
            //IF NO RECORD IS SELECTED
            if (id == null)
            {
                //DO THIS
                return RedirectToAction("Index");
            }
            //Instantiating SQL Connection
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                //Opens Database Connection
                cn.Open();
                //status!=status changes it into uneditable
                string query = @"SELECT UserID, TypeID, FirstName, MiddleName, LastName, Email, Mobile, Username, Password   
                                 FROM Users
                                 WHERE UserID=@UserID AND Status!=@Status";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)// if Record Exists
                        {
                            var record = new UserModel();
                            record.Types = GetUsersTypes();

                            while (dr.Read())
                            {
                                record.UserID = int.Parse(dr["UserID"].ToString());
                                record.TypeID = int.Parse(dr["TypeID"].ToString());
                                record.FirstName = dr["FirstName"].ToString();
                                record.MiddleName = dr["MiddleName"].ToString();
                                record.LastName = dr["LastName"].ToString();
                                record.Email = dr["Email"].ToString();
                                record.Mobile = dr["Mobile"].ToString();
                                record.Username = dr["Username"].ToString();
                                record.Password = dr["Password"].ToString();
                            }
                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
        }
        [HttpPost]
        public ActionResult Details(int? id, UserModel record)
        {
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"UPDATE Users
                                 SET TypeID=@TypeID, FirstName=@FirstName, MiddleName=@MiddleName,LastName=@LastName, Email=@Email, Mobile=@Mobile, Username=@Username, Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@TypeID", record.TypeID);
                    cmd.Parameters.AddWithValue("@FirstName", record.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", record.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", record.LastName);
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Mobile", record.Mobile);
                    cmd.Parameters.AddWithValue("@Username", record.Username);
                    cmd.Parameters.AddWithValue("@Password", record.Password);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"UPDATE Users
                                 SET Status=@Status
                                 WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        // GET: User
        public ActionResult Index()
        {
            var list = new List<UserModel>();
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"SELECT u.UserID, ut.Type, u.Email, u.FirstName, u.MiddleName, u.LastName, u.Email, u.Mobile, u.Username, u.Password, u.Status, u.DateCreated, u.LastLogin
                                 FROM Users u
                                 INNER JOIN UserType ut ON u.TypeID = ut.TypeID
                                 WHERE u.Status=@Status";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new UserModel
                            {
                                UserID = int.Parse(dr["UserID"].ToString()),
                                Type = dr["Type"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                MiddleName = dr["MiddleName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                Mobile = dr["Mobile"].ToString(),
                                Username = dr["Username"].ToString(),
                                Password = dr["Password"].ToString(),
                                Status = dr["Status"].ToString(),
                                DateCreated = DateTime.Parse(dr["DateCreated"].ToString())
                            });
                        }

                    }
                }
            }

            return View(list);
        }
    }
}