using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Models;
using Oracle.ManagedDataAccess.Client;
using SquareOne.Database.Db;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal_Database.DB.Job
{
    public class CRUD_JOBS
    {
        public static string connectionString = BusinessLogic.connectionString;
        public static string Jobs_Add(Jobs jobs, HttpContext httpContext)
        {
            int? id = httpContext.Session.GetInt32("Id");
            string name = httpContext.Session.GetString("Name");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();


            try
            {
                string str = "JOBPORTAL_JOBS_ADD";
                OracleCommand command = new OracleCommand(str, oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.Add("PIN_EMPLOYERID", OracleDbType.Varchar2).Value = jobs.EmployerId;
                command.Parameters.Add("PIN_TITLE", OracleDbType.Varchar2).Value = jobs.Title;
                command.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = jobs.Description;
                command.Parameters.Add("PIN_LOCATION", OracleDbType.Varchar2).Value = jobs.Location;
                command.Parameters.Add("PIN_SALARY", OracleDbType.Int32).Value = jobs.Salary;
                jobs.Status = "Open";
                command.Parameters.Add("PIN_STATUS", OracleDbType.Varchar2).Value = jobs.Status;
                command.Parameters.Add("PIN_CREATEDBY", OracleDbType.Varchar2).Value = jobs.CreatedBy = name;
                command.Parameters.Add("PIN_EMPLOYER_ID", OracleDbType.Int32).Value = id;
                command.Parameters.Add("POUT_JOBID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                command.ExecuteNonQuery();

                jobs.JobId = Convert.ToInt32(command.Parameters["POUT_JOBID"].Value.ToString());

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

        public static List<Jobs> Jobs_Get()
        {
            List<Jobs> jobs = new List<Jobs>();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("JOBPORTAL_JOBS_GETALL", oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("POUT_cursor", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    jobs.Add(new Jobs
                    {
                        JobId = reader.GetInt32(0),
                        EmployerId = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Description = reader.GetString(3),
                        Location = reader.GetString(4),
                        Salary = reader.GetInt32(5),
                        Status = reader.GetString(6),
                        PostedDate = reader.GetDateTime(7),
                        CreatedOn = reader.GetDateTime(8),
                        CreatedBy = reader.GetString(9),
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

            return jobs;
        }

        public static string Jobs_Update(Jobs jobs, HttpContext httpContext, int id)
        {
            string name = httpContext.Session.GetString("Name");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("JOBPORTAL_JOBS_UPDATE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_JOBID", OracleDbType.Int32).Value = id;
                cmd.Parameters.Add("PIN_TITLE", OracleDbType.Varchar2).Value = jobs.Title;
                cmd.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = jobs.Description;
                cmd.Parameters.Add("PIN_LOCATION", OracleDbType.Varchar2).Value = jobs.Location;
                cmd.Parameters.Add("PIN_SALARY", OracleDbType.Int32).Value = jobs.Salary;
                cmd.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Varchar2).Value = jobs.ModifiedBy = name;
                cmd.Parameters.Add("PIN_STATUS", OracleDbType.Varchar2).Value = jobs.Status;


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

            return "Job updated successfully";
        }

        public static string Jobs_Delete(Jobs jobs, int id)
        {
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand cmd = new OracleCommand("JOBPORTAL_JOBS_DELETE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_JOBID", OracleDbType.Int32).Value = jobs.JobId;


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

        public static Jobs Jobs_GetJobById(int id)
        {
            Jobs jobs = new Jobs();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT Jobid,Title,Description,Location,Salary,Status FROM JOBPORTAL_JOBS where Jobid = :Jobid", oracleConnection);
                command.Parameters.Add(new OracleParameter("Jobid", id));
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    jobs.JobId = reader.GetInt32(0);
                    jobs.Title = reader.GetString(1);
                    jobs.Description = reader.GetString(2);
                    jobs.Location = reader.GetString(3);
                    jobs.Salary = reader.GetInt32(4);
                    jobs.Status = reader.GetString(5);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUser Error: " + ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

            return jobs;
        }
        public static List<Jobs> PersonalJobsListed(HttpContext httpContext)
        {
            int? recruiterId = httpContext.Session.GetInt32("Id");

            List<Jobs> jobs = new List<Jobs>();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT JOBID,TITLE, DESCRIPTION, LOCATION, SALARY, STATUS, POSTEDDATE FROM JOBPORTAL_JOBS WHERE EMPLOYERID = :id", oracleConnection);
                command.Parameters.Add(new OracleParameter("id", recruiterId));
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    Console.WriteLine($"JobId: {reader.GetInt32(0)}, Title: {reader.GetString(1)}");

                    jobs.Add(new Jobs
                    {
                        JobId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Location = reader.GetString(3),
                        Salary = reader.GetInt32(4),
                        Status = reader.GetString(5),
                        PostedDate = reader.GetDateTime(6),
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

            return jobs;
        }

        public static List<Jobs> GetJobsAppliedByJobSeeker(int jobSeekerId)
        {
            List<Jobs> jobs = new List<Jobs>();

            OracleConnection connection = new OracleConnection(connectionString);
            connection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT J.JOBID,J.TITLE,J.DESCRIPTION,J.LOCATION,J.SALARY,J.POSTEDDATE,A.STATUS FROM JOBPORTAL_JOBS J JOIN JOBPORTAL_APPLICATION A ON J.JOBID = A.JOBID WHERE A.JOBSEEKERID = :jobSeekerId", connection);
                command.Parameters.Add(new OracleParameter("jobSeekerId", jobSeekerId));
                OracleDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    jobs.Add(new Jobs
                    {
                        JobId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Location = reader.GetString(3),
                        Salary = reader.GetInt32(4),
                        PostedDate = reader.GetDateTime(5),
                        Status = reader.GetString(6),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUsers Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return jobs;
        }

        public static bool CheckIfApplied(int? jobSeekerId,int? JobId)
        {
            Jobs jobs = new Jobs();
            OracleConnection connection = new OracleConnection(connectionString);
            connection.Open();
            bool hasApplied = false;

            try
            {
                string checkQuery = "SELECT COUNT(*) FROM JOBPORTAL_APPLICATION WHERE JobId = :jobId AND JobSeekerId = :JobSeekerId";



                OracleCommand command = new OracleCommand(checkQuery, connection);
                command.Parameters.Add(new OracleParameter("jobId", JobId));
                command.Parameters.Add(new OracleParameter("JobSeekerId", jobSeekerId));

                int count = Convert.ToInt32(command.ExecuteScalar());
                hasApplied = count > 0;

                jobs.JobId = (int)JobId;
                jobs.hasApplied = count > 0;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("GetUsers Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUsers Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return hasApplied;
        }

        public static List<User> GetApplicants(int id)
        {
            List<User> user = new List<User>();
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("SELECT U.NAME, U.EMAIL,U.DESCRIPTION,U.SKILLS,U.EXPERIENCE,U.DOB,U.JOBROLE,U.ID,A.Status FROM JobPortal_Application A JOIN JobPortal_Users U ON A.JOBSEEKERID = U.ID WHERE A.JOBID = :jobId", oracleConnection);
                cmd.Parameters.Add(new OracleParameter("jobId", id));
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Add(new User
                    {
                        Name = reader.GetString(0),
                        Email = reader.GetString(1),
                        Description = reader.GetString(2),
                        Skills = reader.GetString(3),
                        Experience = reader.GetString(4),
                        DOB = reader.GetString(5),
                        JobRole = reader.GetString(6),
                        Id = reader.GetInt32(7),
                        Status = reader.GetString(8)
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
            return user;
        }
    }
}