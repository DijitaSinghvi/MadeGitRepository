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
           try{ var teacherDetails = (from
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
            catch(Exception er)
            {
                Console.Write(er.Message);
                return View();
    }

}
        public ActionResult ShowStudentsList(int? id)
        {
            try
            {
                ViewModel model = new ViewModel();
                var innerQuery =

                         (from user in db.Users
                          join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                          join subjectInCourse in db.SubjectInCourses on teacherInSubject.SubjectId equals subjectInCourse.SubjectId
                          where user.UserId == id
                          select subjectInCourse.CourseId
                          ).ToList();



                var result = (from user in db.Users

                              where innerQuery.Contains(user.CourseId) select new ViewModel
                               {
                                    UserId=user.UserId,
                                    FirstName =user.FirstName,
                                    LastName=user.LastName,
                                    CourseName=user.Course.CourseName,
                                  
                                    
                                }
                              ).ToList();

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
    }
}