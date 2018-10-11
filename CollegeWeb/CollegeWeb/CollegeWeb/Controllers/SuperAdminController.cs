using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class SuperAdminController : Controller
    {
        CollegeContext db = new CollegeContext();
        // GET: SuperAdmin
        //to get SuperAdmin homepage. 
        public ActionResult HomePage()
        {
            return View();
        }
        

    }
}