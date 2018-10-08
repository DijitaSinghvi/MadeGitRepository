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

        //To show registration form to a new user.
        // GET: Account
        public ActionResult Register()
        {
            ViewModel model = new ViewModel();
            var courseList = db.Courses.Select(x => new CourseModel
            {
                CourseName = x.CourseName,
                CourseId = x.CourseId
            }).ToList();
            // ViewBag.CourseShow = courseList;

            var roleList = db.Roles.Select(x => new RoleModel
            {
                RoleName = x.RoleName,
                RoleId = x.RoleId
            }).ToList();

            model.Roles = roleList;
            model.Courses = courseList;
            //ViewBag.RoleShow = roleList;





            return View(model);
        }

        //To post the values of the registration form to the database.
        [HttpPost]
        public ActionResult Register(ViewModel objViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(objViewModel);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //try to insert user details in registration form.
                    User objUser = new User
                    {
                        UserId = objViewModel.UserId,
                        FirstName = objViewModel.FirstName,
                        LastName = objViewModel.LastName,
                        Gender = objViewModel.Gender,
                        DateOfBirth = objViewModel.DateOfBirth,
                        Hobbies = objViewModel.Hobbies,
                        Email = objViewModel.Email,
                        IsEmailVerified = objViewModel.IsEmailVerified,
                        Password = objViewModel.Password,
                        ConfirmPassword = objViewModel.ConfirmPassword,
                        IsActive = objViewModel.IsActive,
                        CourseId = objViewModel.CourseId,
                        AddressId = objViewModel.AddressId,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now


                    };
                    db.Users.Add(objUser);
                    db.SaveChanges();

                    //RoleId for the respective UserId gets saved in database.
                    UserInRole objUserInRole = new UserInRole
                    {
                        RoleId = objViewModel.RoleId,
                        UserId = objUser.UserId
                    };
                    db.UserInRoles.Add(objUserInRole);
                    db.SaveChanges();
                    //Everything looks fine,so save the data permanently.
                    transaction.Commit();
                    ModelState.Clear();
                    ViewBag.ResultMessage = objViewModel.FirstName + "" + objViewModel.LastName + "" + "is successfully registered.";

                }
                catch (Exception)
                {
                    //roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
                return View();
        }



               
            }


        }

      