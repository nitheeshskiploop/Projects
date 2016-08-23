using Pentagon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace Pentagon.Controllers
{
    public class SiteEnController : Controller
    {
        // GET: SiteEn
        public ActionResult Index()
        {
            return View();
        }
    }
}