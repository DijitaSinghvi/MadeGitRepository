using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        //to get teacher homepage.
        public ActionResult HomePage()
        {
            return View();
        }
    }
}