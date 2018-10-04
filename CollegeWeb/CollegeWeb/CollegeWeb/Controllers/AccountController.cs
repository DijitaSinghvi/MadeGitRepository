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
        public ActionResult Index()
        {
            return View(db.UserInRoles.ToList());
        }
           

        // GET: Account
            public ActionResult Register()
        { 
            var courseList = db.Courses.Select(x => new SelectListItem
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList();
            SelectList courseOutput = new SelectList(courseList, "CourseId", "CourseName");

           
            ViewBag.CourseShow = courseOutput;
         
           
          

            //var roleList = db.Roles.Select(x => new SelectListItem
            //{
            //    Text = x.RoleName,
            //    Value = x.RoleId.ToString()

            //}).ToList();
            //ViewBag.RoleShow = roleList;
             return View(new User());
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
           
            if (ModelState.IsValid)
            {

                using (CollegeContext db = new CollegeContext())
                {
                    
                    db.Users.Add(user);
                    db.SaveChanges();
                }

                ModelState.Clear();
                ViewBag.Message = user.FirstName + "" + user.LastName + "is successfully registered.";
            }
            var courseList = db.Courses.Select(x => new SelectListItem
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList();
            SelectList courseOutput = new SelectList(courseList, "CourseId", "CourseName");
            ViewBag.CourseShow = courseOutput;
            return View(new User());

        }
    }
}