using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace I_Care.Classes
{
    public class Koneksi
    {
        public class SqlParamSp {

            public string Name { get; set; }
            public string Value { get; set; }
            
        }
        public static SqlConnection GetKoneksi()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
            else
            {
                conn.Open();
            }

            return conn;
        }

        public static DataTable GetDataTable(string SQL)
        {
            //SQL = SQL.Replace("--", "").Replace("'", "");
            DataTable dtTable = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();

            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
            else
            {
                cnn.Open();
            }


            SqlCommand sCommand = new SqlCommand(SQL, cnn);
            SqlDataReader dtReader = sCommand.ExecuteReader();
            dtTable.Load(dtReader);
            cnn.Close();

            return dtTable;

        }


        public static DataTable GetDataTable2(string ProcName,string Param = "")
        {
            DataTable dtTable = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();
            if (cnn.State == System.Data.ConnectionState.Open){cnn.Close();}else{cnn.Open();}
            SqlCommand sCommand = new SqlCommand(ProcName, cnn);
            sCommand.CommandType = CommandType.StoredProcedure;
            if (Param != "") {
                var list = JsonConvert.DeserializeObject<List<SqlParamSp>>(Param);
                foreach (var item in list)
                {
                    sCommand.Parameters.AddWithValue(item.Name, item.Value);
                }
            }
            SqlDataReader dtReader = sCommand.ExecuteReader();
            dtTable.Load(dtReader);
            cnn.Close();

            return dtTable;

        }
        public static Boolean execQuery(string SQL)
        {
            //SQL = SQL.Replace("--", "").Replace("'", "");
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();

            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
            else
            {
                cnn.Open();
            }

            try
            {

                SqlCommand sCommand = new SqlCommand(SQL, cnn);
                sCommand.ExecuteNonQuery();
                cnn.Close();
                return true;

            }
            catch
            {

                cnn.Close();
                return false;

            }

        }


        public static string getScalarValue(string SQL)
        {
            //SQL = SQL.Replace("--", "").Replace("'", "");
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();
            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
            else
            {
                cnn.Open();
            }

            try
            {

                SqlCommand sCommand = new SqlCommand(SQL, cnn);
                string sVAL = sCommand.ExecuteScalar().ToString();
                return sVAL;

            }
            catch
            {

                return null;

            }

        }


        public static string getScalarValue2(string SQL, string Param = "")
        {
            SqlConnection cnn = new SqlConnection();
            try
            {
                cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();
                if (cnn.State == System.Data.ConnectionState.Open) { cnn.Close(); } else { cnn.Open(); }
                SqlCommand sCommand = new SqlCommand(SQL, cnn);
                sCommand.CommandType = CommandType.StoredProcedure;
                if (Param != "")
                {
                    var list = JsonConvert.DeserializeObject<List<SqlParamSp>>(Param);
                    foreach (var item in list)
                    {
                        sCommand.Parameters.AddWithValue(item.Name, item.Value);
                    }
                }
                string sVAL = sCommand.ExecuteScalar().ToString();
                cnn.Close();
                return sVAL;
            }
            catch(Exception ex)
            {
                cnn.Close();
                return null;

            }

        }


        public static Boolean getAllowUser(string Nip, string Password)
        {

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();

            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
            else
            {
                cnn.Open();
            }

            try
            {

                SqlCommand sCommand = new SqlCommand("SELECT count(*) AS Ada FROM Survey_Responden WHERE RespondenNip = '" + Nip + "' --and RespondenBirth = '" + Password + "'", cnn);
                string sVAL = sCommand.ExecuteScalar().ToString();
                int ada = Int32.Parse(sVAL);
                cnn.Close();

                if (ada > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                cnn.Close();
                return false;

            }

        }

        public static Boolean CheckSession()
        {

            var customerId = HttpContext.Current.Session["Responden"];
            if (null != customerId)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static DataSet GetDataSet(string SQL, string TableName)
        {
            //SQL = SQL.Replace("--", "").Replace("'", "");
            DataSet dtSet = new DataSet();
            DataTable dtTable = new DataTable();
            dtTable.TableName = TableName;
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["FMSConnection"].ToString();

            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
            else
            {
                cnn.Open();
            }


            SqlCommand sCommand = new SqlCommand(SQL, cnn);
            SqlDataReader dtReader = sCommand.ExecuteReader();
            dtTable.Load(dtReader);
            dtSet.Tables.Add(dtTable);
            cnn.Close();

            return dtSet;

        }
    }
}