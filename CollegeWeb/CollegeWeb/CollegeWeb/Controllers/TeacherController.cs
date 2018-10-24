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
        //to get teacher homepage.
        public ActionResult TeacherHomePage(int?id)
        {
            return View();
        }
        //public ActionResult ShowStudentsList(int? id)
        //{
        //    try
        //    {
        //        //select* from[dbo].[user] where[dbo].[user].CourseId IN(select SubjectInCourse.CourseId from [dbo].[user]
        //        //     join TeacherInSubject on TeacherInSubject.UserId=[dbo].[user].UserId

        //        //join SubjectInCourse on TeacherInSubject.SubjectId= SubjectInCourse.SubjectId


        //        //    where[dbo].[user].UserId= 26)






        //        //var studentList =from users in db.Users where users.CourseId
        //        //                 let courseList=
        //        //                 from user in db.Users
        //        //                 join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
        //        //                 join subjectInCourse in db.SubjectInCourses on teacherInSubject.SubjectId equals subjectInCourse.SubjectId
        //        //                 where user.UserId == id
        //        //                 select new ViewModel
        //        //                 {
        //        //                     CourseName = subjectInCourse.Course.CourseName,
        //        //                     CourseId = subjectInCourse.CourseId
        //        //                 };


        //    }
        //    catch (Exception er)
        //    {
        //        Console.WriteLine(er.Message);
        //        return View();
        //    }
        //}
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

                //Sending data from roleList and courseList to Roles and Courses properties of ViewModel.  
                

                //To get country dropdown from database.
                var countryList = db.Countries.Select(x => new CountryModel
                {
                    CountryName = x.CountryName,
                    CountryId = x.CountryId
                }).ToList();

                //Sending countrie's data to ViewModel's property, Countries.
              
                //To get state dropdown from database.
                var stateList = db.States.Select(x => new StateModel
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }
                ).ToList();

                //Send state's data to ViewModel's property,States.
             


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