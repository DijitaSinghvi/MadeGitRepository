using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult HomePage()
        {
            return View();
        }
    }
}