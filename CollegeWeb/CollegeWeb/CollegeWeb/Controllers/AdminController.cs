﻿using CollegeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class AdminController : Controller
    { 
        CollegeContext db = new CollegeContext();

        /// <summary>
        /// GET: Admin
        /// Showing homepage to admin.
        /// </summary>
        /// <returns></returns>
         public ActionResult HomePage()
        {
            return View();
        }

        /// <summary>
        /// Admin can manage student list.      
        /// </summary>
        /// <returns></returns>
       public ActionResult ViewStudents()
        {
            //Get list of students from database.
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

        /// <summary>
        /// Add new student record in website.
        /// </summary>
        /// <returns></returns>
        [HttpGet]       
        public ActionResult AddStudent()
        {
            //Creating an object of ViewModel.
            ViewModel model = new ViewModel();

            //Query to get the course dropdown from database.
            var courseList = db.Courses.Select(x => new CourseModel
            {
                CourseName = x.CourseName,
                CourseId = x.CourseId
            }).ToList();

            //Query to get the role dropdown from database.
            var roleList = db.Roles.Select(x => new RoleModel
            {
                RoleName = x.RoleName,
                RoleId = x.RoleId
            }).ToList();

            //Sending data from roleList and courseList to Roles and Courses properties of ViewModel.  
            model.Roles = roleList;
            model.Courses = courseList;

            //To get country dropdown from database.
            var countryList = db.Countries.Select(x => new CountryModel
            {
                CountryName = x.CountryName,
                CountryId = x.CountryId
            }).ToList();

            //Sending countrie's data to ViewModel's property, Countries.
            model.Countries = countryList;

          //To get state dropdown from database.
            var stateList = db.States.Select(x => new StateModel
            {
                StateId = x.StateId,
                StateName = x.StateName
            }
            ).ToList();

            //Send state's data to ViewModel's property,States.
            model.States = stateList;


            //To get city dropdown from database.
            var cityList = db.Cities.Select(x => new CityModel
            {
                CityName = x.CityName,
                CityId = x.CityId
            }).ToList();

            //Send cities data to ViewModel's property,Cities.
            model.Cities = cityList;

           //Return object of ViewModel in the view.
            return View(model);

        }

        /// <summary>
        ///To post form details to database. 
        /// </summary>
        /// <param name="objViewModel"></param>
        /// <returns></returns>
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
                {   
                    //Raw data sent to address table.
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

                    // Raw data sent for IsEmailVerified property through ViewModel object.
                    objViewModel.IsEmailVerified = "Yes";
                    objViewModel.IsActive = true;

                    // Try to insert user details of registration form in User table of database.
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

                        // Adding addressId 
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
                    //Roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
            return View();
        }

        /// <summary>
        /// Get states according to selected country.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get cities according to selected state.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///   To edit student record, get details of the student from database. 
        /// </summary>
        /// <returns></returns>
        public ActionResult EditStudent(int? id)
        {
            var editRecord=
                from user in db.Users             
             where user.UserId == id
           
             select new ViewModel
             {
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 Gender = user.Gender,
                 DateOfBirth = user.DateOfBirth,
                 Hobbies = user.Hobbies,
                 Email = user.Email,
                 IsEmailVerified = user.IsEmailVerified,
                 Password = user.Password,
                 IsActive = user.IsActive,
                 DateCreated = user.DateCreated,
                 DateModified = user.DateModified,
                 CourseName = user.Course.CourseName,
                 AddressLine = user.Address.AddressLine,
                 CityName = user.Address.City.CityName,
                 StateName = user.Address.State.StateName,
                 CountryName = user.Address.Country.CountryName,
                 Pincode = user.Address.Pincode
             };
            return View(editRecord);

        }

        /// <summary>
        /// Save updates in database.
        /// </summary>
        //[HttpPost]
        //public ActionResult EditStudent(ViewModel objViewModel)
        //{
        //    //Updating address in address table.
        

        //    db.Addresses.Add(objAddress);
        //    db.SaveChanges();

        //    //Adding updates to User table.
           

        //    db.Users.Add(objUser);
        //    db.SaveChanges();
        //    return View(objViewModel);
        //}

        /// <summary>
        /// Get record to be deleted from database and pass to ViewModel. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteStudent(int? id)
        {
            //var deleteRecord = from
            //                   user in db.Users
            //                   where user.UserId == id
            //                   select new ViewModel
            //                   {
            //                       FirstName = user.FirstName,
            //                       LastName = user.LastName,
            //                       Gender = user.Gender,
            //                       DateOfBirth = user.DateOfBirth,
            //                       Hobbies = user.Hobbies,
            //                       Email = user.Email,
            //                       IsEmailVerified = user.IsEmailVerified,
            //                       Password = user.Password,
            //                       IsActive = user.IsActive,
            //                       DateCreated = user.DateCreated,
            //                       DateModified = user.DateModified,
            //                       CourseName = user.Course.CourseName,
            //                       AddressLine = user.Address.AddressLine,
            //                       CityName = user.Address.City.CityName,
            //                       StateName = user.Address.State.StateName,
            //                       CountryName = user.Address.Country.CountryName,
            //                       Pincode = user.Address.Pincode
            //                   };
            //   User objUser = new User;
            var deleteRecord = (from
                               user in db.Users
                                where user.UserId == id
                                select user).FirstOrDefault();

            if (deleteRecord != null)
            {
                db.Users.Remove(deleteRecord);
                db.SaveChanges();
            }


            return View();
                    
       }
        public ActionResult ViewTeachers()
        {
            //Get list of students from database.
            var TeacherList = (from
                               user in db.Users
                               join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                               where userInRole.RoleId == 3
                               orderby user.UserId
                               select new ViewModel
                               {
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   Gender = user.Gender,
                                   DateOfBirth = user.DateOfBirth,
                                   Hobbies = user.Hobbies,
                                   Email = user.Email,
                                   IsEmailVerified = user.IsEmailVerified,
                                   Password = user.Password,
                                   IsActive = user.IsActive,
                                   DateCreated = user.DateCreated,
                                   DateModified = user.DateModified,
                                   CourseName = user.Course.CourseName,
                                   AddressLine = user.Address.AddressLine,
                                   CityName = user.Address.City.CityName,
                                   StateName = user.Address.State.StateName,
                                   CountryName = user.Address.Country.CountryName,
                                   Pincode = user.Address.Pincode
                               }).ToList();

            return View(TeacherList);
        }

    }



}