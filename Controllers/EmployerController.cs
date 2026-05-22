using JobPortal_Database;
using JobPortal_Database.DB.Job;
using JobPortal_Database.DB.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Oracle.ManagedDataAccess.Client;

namespace JobPortal.Controllers
{
    public class EmployerController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Recruiter,Admin")]
        public IActionResult Recruiter_Index()
        {
            if (User.IsInRole("Admin"))
            {
                List<Jobs> job = CRUD_JOBS.Jobs_Get();
                return View(job);
            }
            else
            {
                User user = CRUD_Users.GetUserByName(User.Identity.Name);
                List<Jobs> jobs = CRUD_JOBS.PersonalJobsListed(HttpContext);
                ViewBag.Role = user.Role;
                return View(jobs);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Recruiter,Admin")]
        public IActionResult Create()
        {
            Jobs jobs = new Jobs();
            return View(jobs);
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter,Admin")]
        public IActionResult Create(Jobs jobs)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Alert = CRUD_JOBS.Jobs_Add(jobs, HttpContext);
                if (ViewBag.Alert != null)
                {
                    return RedirectToAction("Recruiter_Index");
                }
            }
            return View(jobs);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Recruiter")]
        public IActionResult Update(int id)
        {
            Jobs jobs = CRUD_JOBS.Jobs_GetJobById(id);
            return View(jobs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Recruiter")]
        public IActionResult Update(Jobs jobs, int id)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Alert = CRUD_JOBS.Jobs_Update(jobs, HttpContext, id);
                if (ViewBag.Alert != null)
                {
                    return RedirectToAction("Recruiter_Index");
                }
            }
            return View(jobs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Recruiter")]
        public IActionResult Delete(int id)
        {
            var job = CRUD_JOBS.Jobs_GetJobById(id);
            TempData["Alert"] = CRUD_JOBS.Jobs_Delete(job, id);
            return RedirectToAction("Recruiter_Index");
        }

        [HttpGet]
        public IActionResult Applicants(int id)
        {
            List<User> users = CRUD_JOBS.GetApplicants(id);
            ViewBag.JobId = id;
            return View(users);
        }

        [HttpGet]
        public IActionResult ApplicantUserDetails(int id)
        {
            //int? UserId = HttpContext.Session.GetInt32("Id");
            List<User> user = CRUD_Users.ApplicantUserDetails(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateApplicantStatus(int id,int jobId, string status)
        {
            using (var connection = new OracleConnection(BusinessLogic.connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {

                    try
                    {

                        using (var cmd1 = new OracleCommand("UPDATE JOBPORTAL_APPLICATION SET STATUS = :status WHERE JOBID = :jobId AND JOBSEEKERID = :jobSeekerId", connection))
                        {
                            cmd1.Parameters.Add(new OracleParameter("status", status));
                            cmd1.Parameters.Add(new OracleParameter("jobId", jobId));
                            cmd1.Parameters.Add(new OracleParameter("jobSeekerId", id));
                            cmd1.Transaction = transaction;
                            cmd1.ExecuteNonQuery();
                        }

                        using (var cmd2 = new OracleCommand("UPDATE JOBPORTAL_USERS SET Status = :status WHERE ID = :id", connection))
                        {
                            cmd2.Parameters.Add(new OracleParameter("status", status));
                            cmd2.Parameters.Add(new OracleParameter("id", id));
                            cmd2.Transaction = transaction;
                            cmd2.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log error or handle accordingly
                        ModelState.AddModelError("", "Error updating status: " + ex.Message);
                    }
                }
            }
            return RedirectToAction("Recruiter_Index"); // or your actual view name
        }

        public IActionResult ExportUsersToExcel(int Id)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Vedant");
            var users = new List<User>();

            using var connection = new OracleConnection(BusinessLogic.connectionString);
            connection.Open();

            using var command = new OracleCommand("SELECT U.NAME, U.EMAIL,A.Status FROM JobPortal_Application A JOIN JobPortal_Users U ON A.JOBSEEKERID = U.ID WHERE A.JOBID = :jobId", connection);
            command.Parameters.Add(new OracleParameter("jobId", Id));
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Status = reader["Status"].ToString()
                });
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");

            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Status";

            for (int i = 0; i < users.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = users[i].Name;
                worksheet.Cells[i + 2, 2].Value = users[i].Email;
                worksheet.Cells[i + 2, 3].Value = users[i].Status;
            }

            var range = worksheet.Cells[1, 1, users.Count + 1, 3];
            var table = worksheet.Tables.Add(range, "ApplicantsTable");
            table.TableStyle = TableStyles.Medium9;

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Applicants.xlsx");
        }
    }
}
