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
    public class Data
    {

        public static string GetResponseJson( string sql)
        {
            string SQl = sql.Replace("--", "").Replace("'", "");
            string JSONresult;
            DataTable tblData = Koneksi.GetDataTable(SQl);
            JSONresult = JsonConvert.SerializeObject(tblData);
            return JSONresult;

        }

      

    }
}