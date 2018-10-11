using CollegeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class AdminController : Controller
    { //Showing homepage to admin.
        CollegeContext db = new CollegeContext();
        // GET: Admin
        public ActionResult HomePage()
        {
            return View();
        }
        //Admin can manage student list.
        //get list of students from database.
        public ActionResult ViewStudents()
        {
           
            var studentList = (from
                               user in db.Users 
                               join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                               where userInRole.RoleId == 4
                               orderby user.UserId
                               select new ViewModel
                               {
                                 FirstName=  user.FirstName,
                                 LastName = user.LastName,
                                   Gender=user.Gender,
                                   DateOfBirth=user.DateOfBirth,
                                   Hobbies=user.Hobbies,
                                   Email=user.Email,
                                   IsEmailVerified=user.IsEmailVerified,
                                   Password=user.Password,
                                   IsActive=user.IsActive,
                                   DateCreated=user.DateCreated,
                                   DateModified=user.DateModified,
                                  CourseName = user.Course.CourseName,
                                  AddressLine=  user.Address.AddressLine,
                                  CityName= user.Address.City.CityName,
                                  StateName=user.Address.State.StateName,
                                  CountryName=user.Address.Country.CountryName,
                                  Pincode=user.Address.Pincode
                               }).ToList();
                                  
                             return View(studentList);
        }
        [HttpGet]
        //Add new student record in website.
        public ActionResult AddStudent()
        {
            //Creating an object of ViewModel.
            ViewModel model = new ViewModel();
            //query to get the course dropdown from database.
            var courseList = db.Courses.Select(x => new CourseModel
            {
                CourseName = x.CourseName,
                CourseId = x.CourseId
            }).ToList();

            //query to get the role dropdown from database.
            var roleList = db.Roles.Select(x => new RoleModel
            {
                RoleName = x.RoleName,
                RoleId = x.RoleId
            }).ToList();
            //sending data from roleList and courseList to Roles and Courses properties of ViewModel.  
            model.Roles = roleList;
            model.Courses = courseList;
            //to get country dropdown from database.
            var countryList = db.Countries.Select(x => new CountryModel
            {
                CountryName = x.CountryName,
                CountryId = x.CountryId
            }).ToList();
            //seding countrie's data to ViewModel's property, Countries.
            model.Countries = countryList;


            //to get state dropdown from database.
            var stateList = db.States.Select(x => new StateModel
            {
                StateId = x.StateId,
                StateName = x.StateName
            }
            ).ToList();
            ////send state's data to ViewModel's property,States.
            model.States = stateList;


            //to get city dropdown from database.
            var cityList = db.Cities.Select(x => new CityModel
            {
                CityName = x.CityName,
                CityId = x.CityId
            }).ToList();

            ////send cities data to ViewModel's property,Cities.
            model.Cities = cityList;


            //return object of ViewModel in the view.
            return View(model);

        }
        //to post form details to database.
        [HttpPost]
        public ActionResult AddStudent(ViewModel objViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(objViewModel);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {//Raw data sent to address table.
                    Address objAddress = new Address
                    {
                        AddressLine = objViewModel.AddressLine,
                        CityId = objViewModel.CityId,
                        CountryId = objViewModel.CountryId,
                        Pincode = objViewModel.Pincode,
                        StateId = objViewModel.StateId,

                    };
                    db.Addresses.Add(objAddress);
                    db.SaveChanges();
                    //Raw data sent for IsEmailVerified property through ViewModel object.
                    objViewModel.IsEmailVerified = "Yes";
                    objViewModel.IsActive = true;
                    //try to insert user details of registration form in User table of database.
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
                        // Adding addresId 
                        AddressId = objAddress.AddressId,
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

                    ViewBag.ResultMessage = objViewModel.FirstName + "" + objViewModel.LastName + "" + "is successfully registered.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    //roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
            return RedirectToAction("Index", "Account");
        }
        //get states according to selected country.
        public JsonResult getState(int Id)
        {
            var states = db.States.Where(x => x.CountryId == Id).ToList();
            List<SelectListItem> stateList = new List<SelectListItem>();

            stateList.Add(new SelectListItem { Text = "", Value = "0" });
            if (states != null)
            {
                foreach (var x in states)
                {
                    stateList.Add(new SelectListItem { Text = x.StateName, Value = x.StateId.ToString() });

                }
            }
            return Json(new SelectList(stateList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        //get cities according to selected state.
        public JsonResult getCity(int id)
        {
            var cities = db.Cities.Where(x => x.StateId == id).ToList();
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Add(new SelectListItem { Text = "", Value = "0" });
            if (cities != null)
            {
                foreach (var x in cities)
                {
                    cityList.Add(new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
                }
            }
            return Json(new SelectList(cityList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        //To edit student record.
        //public ActionResult EditStudent()
        //{
            
        //}
            

        

    }


    
}