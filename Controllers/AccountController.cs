using JobPortal_Database.DB.Job;
using JobPortal_Database.DB.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace JobPortal.Controllers;

public class AccountController : Controller
{
    public static string connectionstring = "User Id=C##VEDANT;Password=Work@2025;Data Source=localhost:1521/ORCL;";
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SignUp(User model)
    {
        if (model.Name == null || model.Email == null || model.Password == null) return View(model);
        //model.Role = model.Password.Contains("Admin") ? "Admin" : "Student";
        if (model.Password.Contains("Admin"))
        {
            model.Role = "Admin";
        }

        if (!IsValidEmail(model.Email))
        {
            ModelState.AddModelError("Email", "Invalid Email Format");
            return View(model);
        }


        var passwordhash = PasswordHasher.Hash(model.Password);
        using var con = new OracleConnection(connectionstring);
        try
        {
            if (model.Role == null)
            {
                return View(model);
            }
            //con.Open();
            //var cmd = new OracleCommand("INSERT INTO JOBPORTAL_USERS(Email,User_Password,Role) VALUES (:email,:password,:role)", con);
            //cmd.Parameters.Add(new OracleParameter("email", model.Email));
            //cmd.Parameters.Add(new OracleParameter("password", passwordhash));
            //cmd.Parameters.Add(new OracleParameter("role", model.Role));
            CRUD_Users.User_Add(model, passwordhash);
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Invalid Login");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(User model)
        {
        CRUD_Users.User_Get();
        if (model.Email == null || model.Password == null) return View(model);

        using var con = new OracleConnection(connectionstring);
        con.Open();
        var cmd = new OracleCommand("Select Id,Name,User_Password,Role From JOBPORTAL_USERS WHERE Email = :email", con);
        cmd.Parameters.Add(new OracleParameter("email", model.Email));

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var storedHash = reader.GetString(2);
            var role = reader.GetString(3);

            if (storedHash == PasswordHasher.Hash(model.Password))
            {
                model.Description = CRUD_Users.GetUserDescription(id) as string;
                string ID = id.ToString();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.NameIdentifier, ID),
                    new Claim(ClaimTypes.Email,model.Email),
                    new Claim(ClaimTypes.Role,role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetInt32("Id", id);
                HttpContext.Session.SetString("Name", name);

                if (role == "Recruiter")
                {
                    if (model.Description != null)
                    {
                        return RedirectToAction("Recruiter_Index", "Employer");
                    }
                    return RedirectToAction("GetRecruiterDetails");
                }
                else
                if (role == "Applicant")
                {
                    if (model.Description != null)
                    {
                        return RedirectToAction("Applicant_Index", "JobSeeker");
                    }
                    else
                    {
                        return RedirectToAction("GetApplicantDetails");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("", "Invalid login");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize(Roles = "Applicant")]
    public IActionResult GetApplicantDetails()
    {
        User user = new User();
        return View(user);
    }

    [HttpPost]
    [Authorize(Roles = "Applicant")]
    public IActionResult GetApplicantDetails(User user)
    {
        if (user.Description != null && user.Skills != null && user.Experience != null && user.DOB != null && user.JobRole != null)
        {
            ViewBag.Alert = CRUD_Users.Applicant_DetailsAdd(user, HttpContext);
            if (ViewBag.Alert != null)
            {
                return RedirectToAction("Applicant_Index", "JobSeeker");
            }
        }
        return View(user);
    }
    [HttpGet]
    [Authorize(Roles = "Recruiter")]
    public IActionResult GetRecruiterDetails()
    {
        User user = new User();
        return View(user);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
    public IActionResult GetRecruiterDetails(User user)
    {
        if (user.Description != null && user.CName != null && user.IType != null && user.Address != null && user.CSize != null &&
            user.ContactNo != null)
        {
            ViewBag.Alert = CRUD_Users.Recruiter_DetailsAdd(user, HttpContext);
            if (ViewBag.Alert != null)
            {
                return RedirectToAction("Recruiter_Index", "Employer");
            }
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult EditProfileApplicant(int id)
    {
        User user = CRUD_Users.User_GetApplicantById(id);
        return View(user);
    }

    [HttpPost]
    public IActionResult EditProfileApplicant(User user)
    {
        if (User.IsInRole("Applicant"))
        {
            ViewBag.Alert = CRUD_Users.User_Update(user, HttpContext);
            if (ViewBag.Alert != null)
            {
                return RedirectToAction("Applicant_Index", "JobSeeker");
            }
        }
        return View(user);
    }
    [HttpGet]
    public IActionResult EditProfileRecruiter(int id)
    {
        User user = CRUD_Users.User_GetRecruiterById(id);
        return View(user);
    }

    [HttpPost]
    public IActionResult EditProfileRecruiter(User user)
    {
        if (User.IsInRole("Recruiter"))
        {
            ViewBag.Alert = CRUD_Users.User_UpdateRecruiter(user, HttpContext);
            if (ViewBag.Alert != null)
            {
                return RedirectToAction("Recruiter_Index", "Employer");
            }
        }
        return View(user);
    }
}
