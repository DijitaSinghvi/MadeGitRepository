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
            //List<Course> courseList = new List<Course>();
            //IEnumerable<Role> roleList = new IEnumerable<Role>();
            //List<SelectListItem> courseListNew = new List<SelectListItem>();
            //List<SelectListItem> roleListNew = new List<SelectListItem>();
            var courseList = db.Courses.Select(x => new SelectListItem
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList(); 
           
            ViewBag.CourseShow = courseList;
            //ViewBag.CourseShow = new SelectList(courseListNew, "CourseId", "CourseName");
            //ViewBag.RoleShow = roleList;
          

            var roleList = db.Roles.Select(x => new SelectListItem
            {
                Text = x.RoleName,
                Value = x.RoleId.ToString()

            }).ToList();
            ViewBag.RoleShow = roleList;
             return View();
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
            return View();

        }
    }
}