using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class SearchController : Controller
    {
        /// <summary>
        /// Get the filters section on the search page.
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchPage()
        {
           
           
            return View();
        }
    }
}