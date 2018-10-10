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
        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult StudentIndex()
        {
            //var studentList = (from user in db.Users
            //         join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
            //         orderby user.UserId
            //         select new
            //         {
            //             .OrderID,
            //             pd.ProductID,
            //             pd.Name,
            //             pd.UnitPrice,
            //             od.Quantity,
            //             od.Price,
            //         }).ToList();
            //return View(studentList);

        }

    }
}