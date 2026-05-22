using Models;
using Oracle.ManagedDataAccess.Client;
using SquareOne.Database.Db;
using System.Data;
using System.Numerics;
using System.Reflection;

namespace JobPortal_Database
{
    public class BusinessLogic
    {
        public static string connectionString = "User Id=C##VEDANT;Password=Work@2025;Data Source=localhost:1521/ORCL;";

        //public static void GetConnection(IConfiguration configuration)
        //{
        //    connectionString = configuration.GetConnectionString("OracleDb");
        //}

        //public static string User_Add(User user)
        //{
        //    int count = 0;
        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        string str = "JOBPORTAL_USERS_ADD";
        //        OracleCommand command = new OracleCommand(str, oracleConnection);
        //        command.Parameters.Add("Name", user.Name);
        //        command.Parameters.Add("USER_PASSWORD", user.Password);
        //        command.Parameters.Add("Email", user.Email);
        //        command.Parameters.Add("Role", user.Role);
        //        command.Parameters.Add("CreatedON", user.CreatedOn);
        //        command.Parameters.Add("CreatedBy", user.CreatedBy);
        //        command.Parameters.Add("ModifiedOn", user.ModifiedOn);
        //        command.Parameters.Add("ModifiedBy", user.ModifiedBy);
        //        count = command.ExecuteNonQuery();

        //        if (count == 0)
        //        {
        //            return "Unable to add";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Add Error: " + ex.Message);

        //        return "Unable to add";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return "Student added successfully.";
        //}

        //public static string AddUsingProcedure(students)
        //{

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {

        //        OracleCommand command = new OracleCommand("student_add", oracleConnection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add("PIN_FIRST_NAME", OracleDbType.Varchar2).Value = students.FirstName;
        //        command.Parameters.Add("PIN_LAST_NAME", OracleDbType.Varchar2).Value = students.LastName;
        //        command.Parameters.Add("PIN_DOB", OracleDbType.Date).Value = students.DOB;
        //        command.Parameters.Add("PIN_EMAIL", OracleDbType.Varchar2).Value = students.Email;
        //        command.Parameters.Add("PIN_COURSE", OracleDbType.Varchar2).Value = students.Course;
        //        command.Parameters.Add("PIN_GENDER", OracleDbType.Varchar2).Value = students.Gender;
        //        command.Parameters.Add("PIN_PHONE", OracleDbType.Int64).Value = students.Phone;

        //        command.Parameters.Add("POUT_ID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
        //        command.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

        //        // Execute
        //        command.ExecuteNonQuery();

        //        students.Id = Convert.ToInt32(command.Parameters["POUT_ID"].Value.ToString());

        //        string result = command.Parameters["POUT_RESULT"].Value.ToString();


        //        if (result == "FAILURE")
        //        {
        //            return "Unable to add";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Add Error: " + ex.Message);

        //        return "Unable to add";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return "Student added successfully.";
        //}


        //public static List<Students> GetStudents()
        //{
        //    List<Students> students = new List<Students>();

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        OracleCommand command = new OracleCommand("SELECT * FROM STUDENT", oracleConnection);
        //        OracleDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            students.Add(new Students
        //            {
        //                Id = reader.GetInt32(0),
        //                FirstName = reader.GetString(1),
        //                LastName = reader.GetString(2),
        //                DOB = reader.GetDateTime(3),
        //                Email = reader.GetString(4),
        //                Course = reader.GetString(5),
        //                Gender = reader.GetString(6),
        //                Phone = reader.GetString(7),
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetStudents Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return students;
        //}
        //public static List<Students> GetStudentsUsingProcedure()
        //{
        //    List<Students> students = new List<Students>();

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        OracleCommand command = new OracleCommand("student_get_all", oracleConnection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add("POUT_cursor", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
        //        OracleDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            students.Add(new Students
        //            {
        //                Id = reader.GetInt32(0),
        //                FirstName = reader.GetString(1),
        //                LastName = reader.GetString(2),
        //                DOB = reader.GetDateTime(3),
        //                Email = reader.GetString(4),
        //                Course = reader.GetString(5),
        //                Gender = reader.GetString(6),
        //                Phone = reader.GetString(7),
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetStudents Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return students;
        //}


        //public static string UpdateUsingStoredProcedure(Students students)
        //{
        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {

        //        OracleCommand cmd = new OracleCommand("STUDENT_UPDATE", oracleConnection);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("PIN_ID", OracleDbType.Int64).Value = students.Id;
        //        cmd.Parameters.Add("PIN_FIRST_NAME", OracleDbType.Varchar2).Value = students.FirstName;
        //        cmd.Parameters.Add("PIN_LAST_NAME", OracleDbType.Varchar2).Value = students.LastName;
        //        cmd.Parameters.Add("PIN_DOB", OracleDbType.Date).Value = students.DOB;
        //        cmd.Parameters.Add("PIN_EMAIL", OracleDbType.Varchar2).Value = students.Email;
        //        cmd.Parameters.Add("PIN_COURSE", OracleDbType.Varchar2).Value = students.Course;
        //        cmd.Parameters.Add("PIN_GENDER", OracleDbType.Varchar2).Value = students.Gender;
        //        cmd.Parameters.Add("PIN_PHONE", OracleDbType.Int64).Value = students.Phone;


        //        cmd.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

        //        // Execute
        //        cmd.ExecuteNonQuery();

        //        string result = cmd.Parameters["POUT_RESULT"].Value.ToString();


        //        if (result == "FAILURE")
        //        {
        //            return "Unable to add";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Update Error: " + ex.Message);
        //        return "Unable to Edit";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return "Student updated successfully";
        //}
        //public static string Update(Students students)
        //{
        //    int count = 0;
        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        string str = "UPDATE STUDENT SET FIRSTNAME=:FIRSTNAME,LASTNAME=:LASTNAME,DOB=:DOB,EMAIL=:EMAIL,COURSE=:COURSE,GENDER=:GENDER,PHONE=:PHONE WHERE ID = :ID";
        //        OracleCommand cmd = new OracleCommand(str, oracleConnection);
        //        cmd.Parameters.Add("FIRSTNAME", students.FirstName);
        //        cmd.Parameters.Add("LASTNAME", students.LastName);
        //        cmd.Parameters.Add("DOB", students.DOB);
        //        cmd.Parameters.Add("EMAIL", students.Email);
        //        cmd.Parameters.Add("COURSE", students.Course);
        //        cmd.Parameters.Add("GENDER", students.Gender);
        //        cmd.Parameters.Add("PHONE", students.Phone);
        //        cmd.Parameters.Add("ID", students.Id);
        //        count = cmd.ExecuteNonQuery();

        //        if (count == 0)
        //        {
        //            return "Unable to Edit";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Update Error: " + ex.Message);
        //        return "Unable to Edit";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return "Student updated successfully";
        //}


        //public static string Delete(Students students)
        //{
        //    int count = 0;
        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();
        //    try
        //    {
        //        string str = "DELETE FROM STUDENT WHERE ID = :ID";
        //        OracleCommand command = new OracleCommand(str, oracleConnection);
        //        command.Parameters.Add(new OracleParameter("ID", students.Id));
        //        count = command.ExecuteNonQuery();

        //        if (count == 0)
        //        {
        //            return "Unable to delete employee";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Delete Error: " + ex.Message);
        //        return "Unable to delete employee";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }
        //    return "Employee Deleted Successfully";
        //}
        //public static string DeleteUsingStoredProcedure(Students students)
        //{
        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {

        //        OracleCommand cmd = new OracleCommand("STUDENT_DELETE", oracleConnection);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("PIN_ID", OracleDbType.Int64).Value = students.Id;


        //        cmd.Parameters.Add("POUT_RESULT", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;

        //        // Execute
        //        cmd.ExecuteNonQuery();

        //        string result = cmd.Parameters["POUT_RESULT"].Value.ToString();


        //        if (result == "FAILURE")
        //        {
        //            return "Unable to delete";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Update Error: " + ex.Message);
        //        return "Unable to delete";
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return "Student deleted successfully";
        //}

        //public static Students GetStudentsById(int id)
        //{
        //    Students students = new Students();

        //    OracleConnection oracleConnection = new OracleConnection(connectionString);
        //    oracleConnection.Open();

        //    try
        //    {
        //        OracleCommand command = new OracleCommand("SELECT * FROM STUDENT where id = " + id, oracleConnection);
        //        OracleDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            students.Id = reader.GetInt32(0);
        //            students.FirstName = reader.GetString(1);
        //            students.LastName = reader.GetString(2);
        //            students.DOB = reader.GetDateTime(3);
        //            students.Email = reader.GetString(4);
        //            students.Course = reader.GetString(5);
        //            students.Gender = reader.GetString(6);
        //            students.Phone = reader.GetString(7);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetStudents Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //    }

        //    return students;
        //}

    }
}
