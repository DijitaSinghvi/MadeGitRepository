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
            return View();
        }


        // GET: Account
        public ActionResult Register()
        {
            var courseList = db.Courses.Select(x => new SelectListItem
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList();
            ViewBag.CourseShow = courseList;

            var roleList = db.Roles.Select(x => new SelectListItem
            {
                Text = x.RoleName,
                Value = x.RoleId.ToString()
            }).ToList();

            ViewBag.RoleShow = roleList;



            return View();
        }







        [HttpPost]
        public ActionResult Register(ViewModel user)
        {

            if (ModelState.IsValid)
            {
                User objUser = new User
                {
                     UserId=user.UserId,
                     FirstName=user.FirstName,
                     LastName=user.LastName,
                     Gender=user.Gender,
                     DateOfBirth=user.DateOfBirth,
                     Hobbies=user.Hobbies,
                     Email=user.Email,
                     IsEmailVerified=user.IsEmailVerified,
                     Password=user.Password,
                     ConfirmPassword=user.ConfirmPassword,
                     IsActive=user.IsActive,
                     CourseId=user.CourseId,
                     AddressId=user.AddressId,
                     DateCreated=DateTime.Now,
                     DateModified= DateTime.Now


                };
                db.Users.Add(objUser);

                UserInRole objUserInRole = new UserInRole
                {
                    RoleId = user.RoleId,
                    UserId = objUser.UserId
                };
                db.UserInRoles.Add(objUserInRole);
                using (CollegeContext db = new CollegeContext())
                {


                    db.SaveChanges();
                }

                ModelState.Clear();
                ViewBag.Message = user.FirstName + "" + user.LastName + "is successfully registered.";
            }

            return View(new User());


        }

    }  }   