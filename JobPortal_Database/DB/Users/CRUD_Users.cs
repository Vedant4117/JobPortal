using Microsoft.AspNetCore.Http;
using Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JobPortal_Database.DB.Users
{
    public class CRUD_Users
    {
        public static string connectionString = BusinessLogic.connectionString;
        public static string User_Add(User user, string passwordhash)
        {
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                string str = "JOBPORTAL_USERS_ADD";
                OracleCommand command = new OracleCommand(str, oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("PIN_NAME", OracleDbType.Varchar2).Value = user.Name;
                command.Parameters.Add("PIN_USER_PASSWORD", OracleDbType.Varchar2).Value = passwordhash;
                command.Parameters.Add("PIN_EMAIL", OracleDbType.Varchar2).Value = user.Email;
                command.Parameters.Add("PIN_ROLE", OracleDbType.Varchar2).Value = user.Role;
                command.Parameters.Add("PIN_CREATEDON", OracleDbType.Date).Value = user.CreatedOn = DateTime.Now;
                command.Parameters.Add("PIN_CREATEDBY", OracleDbType.Varchar2).Value = user.CreatedBy = user.Name;
                //command.Parameters.Add("PIN_MODIFIEDON", OracleDbType.Date).Value = user.ModifiedOn;
                //command.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Varchar2).Value = user.ModifiedBy;

                command.Parameters.Add("POUT_ID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                command.ExecuteNonQuery();

                user.Id = Convert.ToInt32(command.Parameters["POUT_ID"].Value.ToString());

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

            return "Student added successfully.";
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("JOBPORTAL_USERS_GET", oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("POUT_cursor", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Role = reader.GetString(3),
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

            return users;
        }
        public static List<User> User_Get()
        {
            List<User> users = new List<User>();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("JOBPORTAL_USERS_GETALL", oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("POUT_cursor", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3),
                        Role = reader.GetString(4),
                        CreatedOn = reader.GetDateTime(5),
                        CreatedBy = reader.GetString(6),
                        Description = reader.GetString(7),
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

            return users;
        }

        public static string User_Update(User user,HttpContext httpContext)
        {
            int? id = httpContext.Session.GetInt32("Id");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("JOBPORTAL_USER_UPDATE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_ID", OracleDbType.Int64).Value = id;
                cmd.Parameters.Add("PIN_NAME", OracleDbType.Varchar2).Value = user.Name;
                cmd.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Int64).Value = user.ModifiedBy;
                cmd.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = user.Description;
                cmd.Parameters.Add("PIN_SKILLS", OracleDbType.Varchar2).Value = user.Skills;
                cmd.Parameters.Add("PIN_EXPERIENCE", OracleDbType.Varchar2).Value = user.Experience;    
                cmd.Parameters.Add("PIN_DOB", OracleDbType.Varchar2).Value = user.DOB;
                cmd.Parameters.Add("PIN_JOBROLE", OracleDbType.Varchar2).Value = user.JobRole;


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

            return "Student updated successfully";
        }

        public static string User_Delete(User user,int id)
        {
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("JOBPORTAL_USERS_DELETE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_ID", OracleDbType.Int64).Value = id;


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

            return "User deleted successfully";
        }

        public static User User_GetApplicantById(int id)
        {
            User user = new User();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT ID,NAME,USER_PASSWORD,EMAIL,ROLE,DESCRIPTION,SKILLS,EXPERIENCE,DOB,JOBROLE FROM JOBPORTAL_USERS where id = " + id, oracleConnection);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Password = reader.GetString(2),
                        Email = reader.GetString(3),
                        Role = reader.GetString(4),
                        Description = reader.GetString(5),
                        Skills = reader.GetString(6),
                        Experience = reader.GetString(7),
                        DOB = reader.GetString(8),
                        JobRole = reader.GetString(9)
                    };
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

            return user;
        }
        public static User User_GetRecruiterById(int id)
        {
            User user = new User();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT ID,NAME,USER_PASSWORD,EMAIL,ROLE,DESCRIPTION,CNAME,ITYPE,ADDRESS,CSIZE,CONTACTNO FROM JOBPORTAL_USERS where id = " + id, oracleConnection);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = new User()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Password = reader.GetString(2),
                        Email = reader.GetString(3),
                        Role = reader.GetString(4),
                        Description = reader.GetString(5),
                        CName = reader.GetString(6),
                        IType = reader.GetString(7),
                        Address = reader.GetString(8),
                        CSize = reader.GetString(9),
                        ContactNo = reader.GetString(10)
                    };
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

            return user;
        }
        public static User GetUserByName(string name)
        {
            User user = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("SELECT Id, Name, Email, Role FROM JOBPORTAL_USERS WHERE Name = :name", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("name", name));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            return user;
        }

        public static List<User> ApplicantUserDetails(int? id)
            {
            List<User> users = new List<User>();
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                string query = "Select NAME,EMAIL,DESCRIPTION,SKILLS,EXPERIENCE,DOB,JOBROLE from JOBPORTAL_USERS where ID = :id";
                OracleCommand cmd = new OracleCommand(query, oracleConnection);
                cmd.Parameters.Add(new OracleParameter("id", id));
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Name = reader.GetString(0),
                        Email = reader.GetString(1),
                        Description = reader.GetString(2),
                        Skills = reader.GetString(3),
                        Experience = reader.GetString(4),
                        DOB = reader.GetString(5),
                        JobRole = reader.GetString(6)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
            return users;
        }

        public static string Applicant_DetailsAdd(User user, HttpContext httpContext)
        {
            int? id = httpContext.Session.GetInt32("Id");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                string str = "APPLICANT_DETAILS_ADD";
                OracleCommand command = new OracleCommand(str, oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = user.Description;
                command.Parameters.Add("PIN_SKILLS", OracleDbType.Varchar2).Value = user.Skills;
                command.Parameters.Add("PIN_EXPERIENCE", OracleDbType.Varchar2).Value = user.Experience;
                command.Parameters.Add("PIN_DOB", OracleDbType.Varchar2).Value = user.DOB;
                command.Parameters.Add("PIN_JOBROLE", OracleDbType.Varchar2).Value = user.JobRole;
                command.Parameters.Add("PIN_USERID", OracleDbType.Int32).Value = id;
                //command.Parameters.Add("PIN_MODIFIEDON", OracleDbType.Date).Value = user.ModifiedOn;
                //command.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Varchar2).Value = user.ModifiedBy;


                command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                command.ExecuteNonQuery();

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

            return "Student added successfully.";
        }
        public static string Recruiter_DetailsAdd(User user, HttpContext httpContext)
        {
            int? id = httpContext.Session.GetInt32("Id");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                string str = "RECRUITER_DETAILS_ADD";
                OracleCommand command = new OracleCommand(str, oracleConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("PIN_ID,", OracleDbType.Int32).Value = id;
                command.Parameters.Add("PIN_CNAME,", OracleDbType.Varchar2).Value = user.CName;
                command.Parameters.Add("PIN_ITYPE", OracleDbType.Varchar2).Value = user.IType;
                command.Parameters.Add("PIN_ADDRESS", OracleDbType.Varchar2).Value = user.Address;
                command.Parameters.Add("PIN_CSIZE", OracleDbType.Varchar2).Value = user.CSize;
                command.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = user.Description;
                command.Parameters.Add("PIN_CONTACTNO", OracleDbType.Varchar2).Value = user.ContactNo;
                
                command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

                // Execute
                command.ExecuteNonQuery();

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

            return "Student added successfully.";
        }


        public static object? GetUserDescription(int id)
        {
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();
            var cmd = new OracleCommand("SELECT Description FROM JOBPORTAL_USERS WHERE Id = :id", oracleConnection);
            cmd.Parameters.Add(new OracleParameter("id", id));
            var result = cmd.ExecuteScalar();
            return result;
        }

        public static string User_UpdateRecruiter(User user, HttpContext httpContext)
        {
            int? id = httpContext.Session.GetInt32("Id");
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {

                OracleCommand cmd = new OracleCommand("RECRUITER_UPDATE", oracleConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PIN_ID", OracleDbType.Int64).Value = id;
                cmd.Parameters.Add("PIN_NAME", OracleDbType.Varchar2).Value = user.Name;
                cmd.Parameters.Add("PIN_MODIFIEDBY", OracleDbType.Int64).Value = user.ModifiedBy;
                cmd.Parameters.Add("PIN_DESCRIPTION", OracleDbType.Varchar2).Value = user.Description;
                cmd.Parameters.Add("PIN_CNAME", OracleDbType.Varchar2).Value = user.CName;
                cmd.Parameters.Add("PIN_ITYPE", OracleDbType.Varchar2).Value = user.IType;
                cmd.Parameters.Add("PIN_ADDRESS", OracleDbType.Varchar2).Value = user.Address;
                cmd.Parameters.Add("PIN_CSIZE", OracleDbType.Varchar2).Value = user.CSize;
                cmd.Parameters.Add("PIN_CONTACTNO", OracleDbType.Varchar2).Value = user.ContactNo;


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

            return "Student updated successfully";
        }

        public static User GetUsersById(int id)
        {
            User user = new User();

            OracleConnection oracleConnection = new OracleConnection(connectionString);
            oracleConnection.Open();

            try
            {
                OracleCommand command = new OracleCommand("SELECT Name,Email,Role FROM JOBPORTAL_USERS where ID = :id", oracleConnection);
                command.Parameters.Add(new OracleParameter("id", id));
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.Name = reader.GetString(0);
                    user.Email = reader.GetString(1);
                    user.Role = reader.GetString(2);
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

            return user;
        }
    }
}
