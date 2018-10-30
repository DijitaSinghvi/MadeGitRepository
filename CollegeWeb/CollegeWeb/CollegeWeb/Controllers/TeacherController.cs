using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollegeWeb.Models;

namespace CollegeWeb.Controllers
{
    public class TeacherController : Controller
    {
        CollegeContext db = new CollegeContext();
        // GET: Teacher
        /// <summary>
        /// To get teacher homepage.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult TeacherHomePage(int? id)
        {

            try
            {
                var teacherDetails = (from
                                  user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId

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

                return View(teacherDetails);

            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        public ActionResult ShowStudentsList(int? id)
        {
            List<ViewModel> result = new List<ViewModel>();
            try
            {
                ViewBag.SendId = id;
                TempData["TeacherId"] = id;
                var innerQuery =

                         (from user in db.Users
                          join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                          join subjectInCourse in db.SubjectInCourses on teacherInSubject.SubjectId equals subjectInCourse.SubjectId
                          where user.UserId == id
                          select subjectInCourse.CourseId
                          ).ToList();


                if (innerQuery.Count > 0)
                {
                    result = (from user in db.Users

                                  where innerQuery.Contains(Convert.ToInt32(user.CourseId))
                                  select new ViewModel
                                  {
                                      UserId = user.UserId,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      CourseName = user.Course.CourseName,


                                  }
                                   ).ToList();


                  
                }
                return View(result);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return View();
            }
        }
        public ActionResult ViewStudentProfile(int? id)
        {
            try
            {
                ViewBag.Data = TempData["TeacherId"];
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




                //To get country dropdown from database.
                var countryList = db.Countries.Select(x => new CountryModel
                {
                    CountryName = x.CountryName,
                    CountryId = x.CountryId
                }).ToList();



                //To get state dropdown from database.
                var stateList = db.States.Select(x => new StateModel
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }
                ).ToList();




                //To get city dropdown from database.
                var cityList = db.Cities.Select(x => new CityModel
                {
                    CityName = x.CityName,
                    CityId = x.CityId
                }).ToList();

                var studentDetails = (from
                                 user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId

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


                                          FirstName = user.FirstName,
                                          LastName = user.LastName,
                                          Gender = user.Gender,
                                          DateOfBirth = user.DateOfBirth,
                                          Hobbies = user.Hobbies,
                                          Email = user.Email,
                                          IsEmailVerified = user.IsEmailVerified,
                                          Password = user.Password,
                                          ConfirmPassword = user.ConfirmPassword,
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
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }

        /// <summary>
        /// Teacher view his profile.
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTeacherDetails(int? id)
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

                //To get country dropdown from database.
                var countryList = db.Countries.Select(x => new CountryModel
                {
                    CountryName = x.CountryName,
                    CountryId = x.CountryId
                }).ToList();


                //To get state dropdown from database.
                var stateList = db.States.Select(x => new StateModel
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }
                ).ToList();


                //To get city dropdown from database.
                var cityList = db.Cities.Select(x => new CityModel
                {
                    CityName = x.CityName,
                    CityId = x.CityId
                }).ToList();

                var teacherDetails = (from
                                 user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId

                                      where user.UserId == id

                                      select new EditViewModel
                                      {

                                          UserId = user.UserId,
                                          CountryId = user.Address.CountryId,
                                          AddressId = user.AddressId,
                                          StateId = user.Address.StateId,
                                          CityId = user.Address.CityId,
                                          CourseId = user.CourseId,
                                          RoleId = userInRole.RoleId,
                                          RoleName = userInRole.Role.RoleName,


                                          FirstName = user.FirstName,
                                          LastName = user.LastName,
                                          Gender = user.Gender,
                                          DateOfBirth = user.DateOfBirth,
                                          Hobbies = user.Hobbies,
                                          Email = user.Email,
                                          IsEmailVerified = user.IsEmailVerified,
                                          Password = user.Password,
                                          ConfirmPassword = user.ConfirmPassword,
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

                teacherDetails.Countries = countryList;
                teacherDetails.States = stateList;
                teacherDetails.Cities = cityList;
                teacherDetails.Courses = courseList;
                teacherDetails.Roles = roleList;

                return View(teacherDetails);


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
        public ActionResult EditTeacherDetails(EditViewModel objEditViewModel)
        {
            try
            {
                //Update User table.

                var userRecord = (from user in db.Users
                                  where user.UserId == objEditViewModel.UserId
                                  select user).FirstOrDefault();
                if (userRecord != null)

                {
                    userRecord.DateCreated = DateTime.Now;
                    userRecord.DateModified = DateTime.Now;
                    userRecord.UserId = objEditViewModel.UserId;
                    userRecord.FirstName = objEditViewModel.FirstName;
                    userRecord.LastName = objEditViewModel.LastName;
                    userRecord.Gender = objEditViewModel.Gender;
                    userRecord.DateOfBirth = objEditViewModel.DateOfBirth;
                    userRecord.Hobbies = objEditViewModel.Hobbies;
                    userRecord.Email = objEditViewModel.Email;
                    userRecord.IsEmailVerified = objEditViewModel.IsEmailVerified;
                    userRecord.Password = objEditViewModel.Password;
                    userRecord.ConfirmPassword = objEditViewModel.ConfirmPassword;
                    userRecord.IsActive = objEditViewModel.IsActive;
                    userRecord.CourseId = objEditViewModel.CourseId;
                }

                //Update Address table.
                var addressRecord = (from address in db.Addresses
                                     where address.AddressId == objEditViewModel.AddressId
                                     select address
                              ).FirstOrDefault();
                if (addressRecord != null)
                {

                    addressRecord.AddressLine = objEditViewModel.AddressLine;
                    addressRecord.CityId = objEditViewModel.CityId;
                    addressRecord.CountryId = objEditViewModel.CountryId;
                    addressRecord.Pincode = objEditViewModel.Pincode;
                    addressRecord.StateId = objEditViewModel.StateId;
                }

                //Update UserInRole Table.
                var userInRoleRecord = (from userInRole in db.UserInRoles
                                        where userInRole.UserId == objEditViewModel.UserId
                                        select userInRole
                                      ).FirstOrDefault();

                if (userInRoleRecord != null)
                {
                    userInRoleRecord.RoleId = objEditViewModel.RoleId;
                    userInRoleRecord.UserId = objEditViewModel.UserId;
                }

                //Save to database.
                db.SaveChanges();



                return RedirectToAction("TeacherHomePage", new { id = objEditViewModel.UserId });
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
    }
}