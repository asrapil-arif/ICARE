using I_Care.Classes;
using I_Care.Models;
using I_Care.ServiceDoc;
using I_Care.ServiceSSO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Linq;
using System.Globalization;

namespace I_Care.Controllers
{
    public class ContractOfGeneralAffairController : Controller
    {
        // GET: ContractOfGeneralAffair
        public ActionResult Index()
        {
            return View();
        }
    }
}