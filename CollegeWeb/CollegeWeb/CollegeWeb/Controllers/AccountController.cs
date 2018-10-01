using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollegeWeb.Models;

namespace CollegeWeb.Controllers
{
    public class AccountController : Controller
    {
        CollegeContext db = new CollegeContext();

        // GET: Account
        public ActionResult Register()
        {
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();
            List<SelectListItem> courseListNew = new List<SelectListItem>();
            List<SelectListItem> roleListNew = new List<SelectListItem>();
            courseListNew = courseList.Select(x => new SelectListItem
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList();
            roleListNew = roleList.Select(x => new SelectListItem
            {
                Text = x.RoleName,
                Value = x.RoleId.ToString()

            }).ToList();
            ViewBag.CourseShow = courseListNew;
            ViewBag.RoleShow = roleListNew;


            return View();
        }
    }
}