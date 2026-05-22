using JobPortal_Database;
using JobPortal_Database.DB.Applications;
using JobPortal_Database.DB.Job;
using JobPortal_Database.DB.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Oracle.ManagedDataAccess.Client;
namespace JobPortal.Controllers
{
    public class JobSeekerController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Applicant,Admin")]
        public IActionResult Applicant_Index()
        {
            List<Jobs> students = CRUD_JOBS.Jobs_Get();
            return View(students);
        }

        [HttpPost]
        public IActionResult Check(int JobId)
        {
            
                int? jobSeekerId = HttpContext.Session.GetInt32("Id");

                if (jobSeekerId == null)
                {
                    return BadRequest("JobSeeker ID not found.");
                }

                bool hasApplied = CRUD_JOBS.CheckIfApplied(jobSeekerId.Value, JobId); 

                if (hasApplied)
                {
                    TempData["Message"] = "You have already applied for this job.";
                    return RedirectToAction("Applicant_Index");
                }

                
                return RedirectToAction("Applications", new { jobId = JobId });
            }

        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public IActionResult Applications(Application application,int JobId)
        {
            
            application.JobId = JobId;
            int? jobSeekerId = HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                ViewBag.Alert = CRUD_APPLICATION.Application_Add(application, HttpContext);
                if (ViewBag.Alert != null)
                {
                    var appliedJobs = CRUD_JOBS.GetJobsAppliedByJobSeeker(jobSeekerId.Value);
                    return View("Application_Index", appliedJobs);

                }
            }
            return View(application);
        }

        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public IActionResult Application_Index()
        {
            List<Jobs> students = CRUD_JOBS.Jobs_Get();
            return View(students);
        }


    }
}
