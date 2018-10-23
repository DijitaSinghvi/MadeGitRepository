using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollegeWeb.Models;

namespace CollegeWeb.Controllers
{
    public class StudentController : Controller
    {
        CollegeContext db = new CollegeContext();
        // GET: Student

        /// <summary>
        /// Student homepage.
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentHomePage(int? id)
        {
            try
            {
                var studentDetails = (from
                                  user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                      join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                                      where user.UserId == id

                                      select new ViewModel
                                      {
                                          UserId = user.UserId,
                                          CountryId = user.Address.CountryId,
                                          AddressId = user.AddressId,
                                          StateId = user.Address.StateId,
                                          CityId = user.Address.CityId,
                                          CourseId = user.CourseId,
                                          RoleId = userInRole.RoleId,
                                          RoleName = userInRole.Role.RoleName,
                                          SubjectId = teacherInSubject.SubjectId,
                                          SubjectName = teacherInSubject.Subject.SubjectName,

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
                                      }).FirstOrDefault();

                return View(studentDetails);
            }
            catch(Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }
            
        
        /// <summary>
        /// Student view his profile.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewStudentDetails(ViewModel objViewModel)
        {
            try
            {
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
                // model.Roles = roleList;
                //model.Courses = courseList;

                //To get country dropdown from database.
                var countryList = db.Countries.Select(x => new CountryModel
                {
                    CountryName = x.CountryName,
                    CountryId = x.CountryId
                }).ToList();

                //Sending countrie's data to ViewModel's property, Countries.
                //model.Countries = countryList;

                //To get state dropdown from database.
                var stateList = db.States.Select(x => new StateModel
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }
                ).ToList();

                //Send state's data to ViewModel's property,States.
                //model.States = stateList;


                //To get city dropdown from database.
                var cityList = db.Cities.Select(x => new CityModel
                {
                    CityName = x.CityName,
                    CityId = x.CityId
                }).ToList();

                var studentDetails = (from
                                 user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                      join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                                      where user.UserId == objViewModel.UserId

                                      select new ViewModel
                                      {

                                          UserId = user.UserId,
                                          CountryId = user.Address.CountryId,
                                          AddressId = user.AddressId,
                                          StateId = user.Address.StateId,
                                          CityId = user.Address.CityId,
                                          CourseId = user.CourseId,
                                          RoleId = userInRole.RoleId,
                                          RoleName = userInRole.Role.RoleName,
                                          SubjectId = teacherInSubject.SubjectId,
                                          SubjectName = teacherInSubject.Subject.SubjectName,

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

                                      }).FirstOrDefault();

                studentDetails.Countries = countryList;
                studentDetails.States = stateList;
                studentDetails.Cities = cityList;
                studentDetails.Courses = courseList;
                studentDetails.Roles = roleList;

                return View(studentDetails);

               
            }
            catch(Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }
        public ActionResult EditStudentDetails(int? id)
        {
            try
            {
                //ViewModel model = new ViewModel();
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
                // model.Roles = roleList;
                //model.Courses = courseList;

                //To get country dropdown from database.
                var countryList = db.Countries.Select(x => new CountryModel
                {
                    CountryName = x.CountryName,
                    CountryId = x.CountryId
                }).ToList();

                //Sending countrie's data to ViewModel's property, Countries.
                //model.Countries = countryList;

                //To get state dropdown from database.
                var stateList = db.States.Select(x => new StateModel
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }
                ).ToList();

                //Send state's data to ViewModel's property,States.
                //model.States = stateList;


                //To get city dropdown from database.
                var cityList = db.Cities.Select(x => new CityModel
                {
                    CityName = x.CityName,
                    CityId = x.CityId
                }).ToList();

                //Send cities data to ViewModel's property,Cities.
                //model.Cities = cityList;



                var model = (from user in db.Users
                             join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                             join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                             where user.UserId == id

                             select new ViewModel
                             {
                                 UserId = user.UserId,
                                 CountryId = user.Address.CountryId,
                                 AddressId = user.AddressId,
                                 StateId = user.Address.StateId,
                                 CityId = user.Address.CityId,
                                 CourseId = user.CourseId,
                                 RoleId = userInRole.RoleId,
                                 RoleName = userInRole.Role.RoleName,
                                 SubjectId = teacherInSubject.SubjectId,
                                 SubjectName = teacherInSubject.Subject.SubjectName,

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

                             }).FirstOrDefault();

                model.Countries = countryList;
                model.States = stateList;
                model.Cities = cityList;
                model.Courses = courseList;
                model.Roles = roleList;

                return View(model);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }


        }

        /// <summary>
        /// Save updates in database.
        /// </summary>
        [HttpPost]
        public ActionResult EditStudentDetails(ViewModel objViewModel)
        {
            try
            {
                //Raw data sent to address table.

                var userRecord = (from user in db.Users
                                  where user.UserId == objViewModel.UserId
                                  select user).FirstOrDefault();
                if (userRecord != null)
                {
                    userRecord.DateCreated = DateTime.Now;
                    userRecord.DateModified = DateTime.Now;
                    userRecord.UserId = objViewModel.UserId;
                    userRecord.FirstName = objViewModel.FirstName;
                    userRecord.LastName = objViewModel.LastName;
                    userRecord.Gender = objViewModel.Gender;
                    userRecord.DateOfBirth = objViewModel.DateOfBirth;
                    userRecord.Hobbies = objViewModel.Hobbies;
                    userRecord.Email = objViewModel.Email;
                    userRecord.IsEmailVerified = objViewModel.IsEmailVerified;
                    userRecord.Password = objViewModel.Password;
                    userRecord.ConfirmPassword = objViewModel.ConfirmPassword;
                    userRecord.IsActive = objViewModel.IsActive;
                    userRecord.CourseId = objViewModel.CourseId;
                    userRecord.Address.AddressLine = objViewModel.AddressLine;
                    userRecord.Address.CityId = objViewModel.CityId;
                    userRecord.Address.CountryId = objViewModel.CountryId;
                    userRecord.Address.Pincode = objViewModel.Pincode;
                    userRecord.Address.StateId = objViewModel.StateId;


                  


                    db.SaveChanges();


                }
                return RedirectToAction("ViewStudentDetails");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }






    }
}