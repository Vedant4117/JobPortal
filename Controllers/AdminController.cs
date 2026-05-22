using JobPortal_Database;
using JobPortal_Database.DB.Job;
using JobPortal_Database.DB.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace JobPortal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            List<User> user = CRUD_Users.GetUsers();
            return View(user);
        }
        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Update(int id)
        //{
        //    User user = CRUD_Users.GetUsersById(id);
        //    return View(user);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Update(User user, int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ViewBag.Alert = CRUD_Users.User_Update(user, HttpContext, id);
        //        if (ViewBag.Alert != null)
        //        {
        //            return RedirectToAction("Recruiter_Index");
        //        }
        //    }
        //    return View(user);
        //}
        public IActionResult Delete(int id)
        {
            var user = CRUD_Users.GetUsersById(id);
            TempData["Alert"] = CRUD_Users.User_Delete(user, id);
            return RedirectToAction("Index");
        }

        public IActionResult ExportUsersToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Vedant");
            var users = new List<User>();

            using var connection = new OracleConnection(BusinessLogic.connectionString);
            connection.Open();

            using var command = new OracleCommand("SELECT Id, Name, Email, Role FROM JOBPORTAL_USERS", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Role = reader["Role"].ToString()
                });
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Role";

            for (int i = 0; i < users.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = users[i].Id;
                worksheet.Cells[i + 2, 2].Value = users[i].Name;
                worksheet.Cells[i + 2, 3].Value = users[i].Email;
                worksheet.Cells[i + 2, 4].Value = users[i].Role;
            }

            var range = worksheet.Cells[1, 1, users.Count + 1, 4];
            var table = worksheet.Tables.Add(range, "UsersTable");
            table.TableStyle = TableStyles.Medium9;

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
        }

    }
}
