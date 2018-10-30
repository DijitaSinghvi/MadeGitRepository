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
        public ActionResult EditStudentDetails(int? id)
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

                var studentDetails = (from
                                 user in db.Users
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                     
                                      where user.UserId ==id

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
       

        /// <summary>
        /// Save updates in database.
        /// </summary>
        [HttpPost]
        public ActionResult EditStudentDetails(EditViewModel objEditViewModel)
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
                


                return RedirectToAction("StudentHomePage", new { id = objEditViewModel.UserId });
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }

        /// <summary>
        /// Student can see all the subjects and teachers of his course.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CourseDetails(int? id)
        {
            try
            {
                ViewBag.SendId = id;
                var courseDetails =
                    (from
                     user in db.Users
                      join subjectInCourse in db.SubjectInCourses on user.CourseId equals subjectInCourse.CourseId
                     join subject in db.Subjects on subjectInCourse.SubjectId equals subject.SubjectId
                     join teacherInSubject in db.TeacherInSubjects on subjectInCourse.SubjectId equals teacherInSubject.SubjectId
                     where user.UserId==id 
                     select new ViewModel
                     {
                         UserId = user.UserId,
                         SubjectId = teacherInSubject.SubjectId,
                         SubjectName = subject.SubjectName,
                         FirstName = teacherInSubject.User.FirstName,
                         LastName = teacherInSubject.User.LastName,
                         CourseId = user.CourseId,
                         CourseName = user.Course.CourseName

                     }).DefaultIfEmpty().ToList();
            
                return View(courseDetails);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }
    }
}