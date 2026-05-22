using JobPortal_Database.DB.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal_Database.DB.Applications
{
    public class CRUD_APPLICATION
    {
        public static string connectionString = BusinessLogic.connectionString;
        public static string Application_Add(Application application, HttpContext httpContext)
        {
            Jobs job = CRUD_JOBS.Jobs_GetJobById(application.JobId);
            int? id = httpContext.Session.GetInt32("Id");
            //string name = httpContext.Session.GetString("Name");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();


            try
            {
                string str = "JOBPORTAL_APPLICATION_ADD";
                OracleCommand command = new OracleCommand(str, oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.Add("PIN_EMPLOYERID", OracleDbType.Varchar2).Value = jobs.EmployerId;
                command.Parameters.Add("PIN_JOBSEEKERID", OracleDbType.Int32).Value = application.JobSeekerId = (int)id;
                command.Parameters.Add("PIN_JOBID", OracleDbType.Int32).Value = application.JobId;
                command.Parameters.Add("PIN_STATUS", OracleDbType.Varchar2).Value = application.Status = "Pending";
                command.Parameters.Add("POUT_APPLICATIONID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                command.ExecuteNonQuery();

                application.ApplicationId = Convert.ToInt32(command.Parameters["POUT_APPLICATIONID"].Value.ToString());

                string result = command.Parameters["POUT_RESULT"].Value.ToString();


                if (result == "FAILURE")
                {
                    return "Unable to add";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error: " + ex.Message);

                return "Unable to add";
            }
            finally
            {
                oracleConnection.Close();
            }

            return "Job added successfully.";
        }

        public static List<Application> Application_Get()
        {
            List<Application> applications = new List<Application>();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("JOBPORTAL_APPLICATION_GETALL", oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("POUT_cursor", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    applications.Add(new Application
                    {
                        JobSeekerId = reader.GetInt32(0),
                        JobId = reader.GetInt32(1),
                        AppliedDate = reader.GetDateTime(2),
                        Status = reader.GetString(3),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUsers Error: " + ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return applications;
        }

        public static string Application_Update(Application applications, HttpContext httpContext, int id)
        {
            string name = httpContext.Session.GetString("Name");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("JOBPORTAL_APPLICATION_UPDATE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_APPLICATIONID", OracleDbType.Int32).Value = id;
                cmd.Parameters.Add("PIN_STATUS", OracleDbType.Varchar2).Value = applications.Status;
                cmd.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Varchar2).Value = name;
                cmd.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["POUT_RESULT"].Value.ToString();


                if (result == "FAILURE")
                {
                    return "Unable to add";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error: " + ex.Message);
                return "Unable to Edit";
            }
            finally
            {
                oracleConnection.Close();
            }

            return "Application updated successfully";
        }

        public static string Application_Delete(Application applications, int id)
        {
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("JOBPORTAL_Application_DELETE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_JOBID", OracleDbType.Int32).Value = applications.ApplicationId;
                cmd.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["POUT_RESULT"].Value.ToString();


                if (result == "FAILURE")
                {
                    return "Unable to delete";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error: " + ex.Message);
                return "Unable to delete";
            }
            finally
            {
                oracleConnection.Close();
            }

            return "Job deleted successfully";
        }

        //public static Jobs Jobs_GetJobById(int id)
        //{
        //    Jobs jobs = new Jobs();

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        OracleCommand command = new OracleCommand("SELECT Jobid,Title,Description,Location,Salary,Status FROM JOBPORTAL_JOBS where Jobid = :Jobid", oracleConnection);
        //        command.Parameters.Add(new OracleParameter("Jobid", id));
        //        OracleDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            jobs.JobId = reader.GetInt32(0);
        //            jobs.Title = reader.GetString(1);
        //            jobs.Description = reader.GetString(2);
        //            jobs.Location = reader.GetString(3);
        //            jobs.Salary = reader.GetInt32(4);
        //            jobs.Status = reader.GetString(5);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetUser Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return jobs;
        //}
        //public static List<Jobs> PersonalJobsListed(HttpContext httpContext)
        //{
        //    int? recruiterId = httpContext.Session.GetInt32("Id");

        //    List<Jobs> jobs = new List<Jobs>();

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        OracleCommand command = new OracleCommand("SELECT JOBID,TITLE, DESCRIPTION, LOCATION, SALARY, STATUS, POSTEDDATE FROM JOBPORTAL_JOBS WHERE EMPLOYERID = :id", oracleConnection);
        //        command.Parameters.Add(new OracleParameter("id", recruiterId));
        //        OracleDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            jobs.Add(new Jobs
        //            {
        //                JobId = reader.GetInt32(0),
        //                Title = reader.GetString(1),
        //                Description = reader.GetString(2),
        //                Location = reader.GetString(3),
        //                Salary = reader.GetInt32(4),
        //                Status = reader.GetString(5),
        //                PostedDate = reader.GetDateTime(6),
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetUsers Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return jobs;
        //}


    }
}