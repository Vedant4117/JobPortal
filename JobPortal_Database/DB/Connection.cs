using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace SquareOne.Database.Db
{
    public class Connection
    {
        OracleConnection con;

        public Connection()
        {
            string constr = "User Id=C##VEDANT;Password=Work@2025;Data Source=localhost:1521/ORCL;";
            con = new OracleConnection(constr);
        }

        public void Open()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (OracleException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }

        public void Close()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (OracleException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }

        public OracleConnection Config()
        {
            return con;
        }
    }
}
