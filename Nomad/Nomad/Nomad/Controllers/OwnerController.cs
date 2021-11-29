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
    public class OwnerController : Controller
    {
        public ActionResult Create()
        {
            //if (Session["TypeID"] != null) // user already logged in
            //{
            //    if (Session["TypeID"].ToString() != "3")
            //    {
            //        return RedirectToAction("MyProfile");
            //    }
            //}
            //else
            //    return RedirectToAction("Login", "Account"); 

            var record = new LotModel();
            return View(record);
        }
        [HttpPost]
        public ActionResult Create(LotModel record, HttpPostedFileBase image)
        {
            using (SqlConnection cn = new SqlConnection(Helper.GetCon()))
            {
                cn.Open();
                string query = @"INSERT INTO Lot
                                 VALUES (@Name, @UserID, @Location, @Image, @Price, @Availability, @Upvotes, @Downvotes, @DateCreated, @Status);";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Name", record.Name);
                    cmd.Parameters.AddWithValue("@UserID", int.Parse(Session["UserID"].ToString()));// SESSION NEEDED HERE
                    cmd.Parameters.AddWithValue("@Location", record.Location);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName; // TO ADD DATE TIME TO THE FILE NAME
                    cmd.Parameters.AddWithValue("@Image", fileName);
                    image.SaveAs(Server.MapPath("~/Images/Lots/" + fileName)); // FILE LOCATION OF THE DOCUMENT
                    cmd.Parameters.AddWithValue("@Price", record.Price);
                    cmd.Parameters.AddWithValue("@Availability", record.Availability);
                    cmd.Parameters.AddWithValue("@Upvotes", record.Upvotes);
                    cmd.Parameters.AddWithValue("@Downvotes", record.Downvotes);
                    cmd.Parameters.AddWithValue("@DateCreated", record.Downvotes);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
        // GET: Owner
        public ActionResult Index()
        {
            var list = new List<LotModel>();
            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"SELECT LotID, Name, Location,
                    Image, Price, Availability, Upvotes, Downvotes,
                    DateCreated, Status
                    FROM Lot
                    WHERE Status!=@Status";

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@Status", "Archived");
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            list.Add(new LotModel
                            {
                                LotID = int.Parse(sqlDr["LotID"].ToString()),
                                Name = sqlDr["Name"].ToString(),
                                Location = sqlDr["Location"].ToString(),
                                Image = sqlDr["Image"].ToString(),
                                Price = decimal.Parse(sqlDr["Price"].ToString()),
                                Availability = int.Parse(sqlDr["Availability"].ToString()),
                                Upvotes = int.Parse(sqlDr["Upvotes"].ToString()),
                                Downvotes = int.Parse(sqlDr["Downvotes"].ToString()),
                                DateCreated = DateTime.Parse(sqlDr["DateAdded"].ToString()),
                                Status = sqlDr["Status"].ToString()
                            });
                        }
                    }
                }
            }
            return View(list);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            using (SqlConnection sqlCon = new SqlConnection(Helper.GetCon()))
            {
                sqlCon.Open();
                string query = @"UPDATE Lot SET Status=@Status, DateCreated=@DateCreated
                    WHERE LotID=@LotID";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                {
                    sqlCmd.Parameters.AddWithValue("@Status", "Archived");
                    sqlCmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@LotID", id);
                    sqlCmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}