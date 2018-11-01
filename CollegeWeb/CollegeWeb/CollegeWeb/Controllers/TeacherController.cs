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
            Session["tId"] = id;
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
            List<ViewModel> results = new List<ViewModel>();
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
                    foreach (var item in innerQuery)
                    {
                        List<ViewModel> result = new List<ViewModel>();
                        result = (from user in db.Users

                                  where user.CourseId == item
                                  select new ViewModel
                                  {
                                      UserId = user.UserId,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      CourseName = user.Course.CourseName,


                                  }
                                       ).ToList();

                        results.AddRange(result);
                    }
                }
                return View(results);

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

        public ActionResult GetFilters(SearchModel model)
        {
            try
            {

                //get list of courses from database.
                var courseList = db.Courses.Select(x => new CourseClass
                {
                    CourseId = x.CourseId,
                    CourseName = x.CourseName
                }).ToList();
                //send list of courses to model's property courses.
                model.Courses = courseList;
                //get list ofcountries from database.
                var countryList = db.Countries.Select(x => new CountryClass
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName
                }).ToList();
                //send countryList to Countries property of model.
                model.Countries = countryList;
                //get state list from database.
                var stateList = db.States.Select(x => new StateClass
                {
                    StateId = x.StateId,
                    StateName = x.StateName
                }).ToList();
                //send state list to States property of model. 
                model.States = stateList;
                //get city list from database.
                var cityList = db.Cities.Select(x => new CityClass
                {
                    CityId = x.CityId,
                    CityName = x.CityName
                }).ToList();
                //send city list to Cities property of model.
                model.Cities = cityList;
                //get role list from database.
                var roleList = db.Roles.Select(x => new RoleClass
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName
                }).ToList();
                //send role list to Roles property of model.
                model.Roles = roleList;

                //to filter data according to the data entered in the filters.
                var searchList = (from
                                  user in db.Users
                                  join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                  where user.FirstName == model.FirstName || string.IsNullOrEmpty(model.FirstName)
                                  where user.LastName == model.LastName || string.IsNullOrEmpty(model.LastName)
                                  where user.Gender == model.Gender || string.IsNullOrEmpty(model.Gender)
                                  where user.DateOfBirth == model.DateOfBirth || model.DateOfBirth == null
                                  where user.Hobbies == model.Hobbies || string.IsNullOrEmpty(model.Hobbies)
                                  where user.Email == model.Email || string.IsNullOrEmpty(model.Email)
                                  where user.IsEmailVerified == model.IsEmailVerified || string.IsNullOrEmpty(model.IsEmailVerified)
                                  where user.IsActive == model.IsActive || model.IsActive == false
                                  where user.Course.CourseName == model.CourseName || string.IsNullOrEmpty(model.CourseName)
                                  where user.Address.AddressLine == model.AddressLine || string.IsNullOrEmpty(model.AddressLine)
                                  where user.Address.Country.CountryName == model.CountryName || string.IsNullOrEmpty(model.CountryName)
                                  where user.Address.State.StateName == model.StateName || string.IsNullOrEmpty(model.StateName)
                                  where user.Address.City.CityName == model.CityName || string.IsNullOrEmpty(model.CityName)
                                  where user.Address.Pincode == model.Pincode || model.Pincode == null
                                  where user.DateCreated == model.DateCreated || model.DateCreated == null
                                  where user.DateModified == model.DateModified || model.DateModified == null
                                  where userInRole.Role.RoleName == model.RoleName || string.IsNullOrEmpty(model.RoleName)
                                  select new ShowList
                                  {
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Gender = user.Gender,
                                      DateOfBirth = user.DateOfBirth,
                                      Hobbies = user.Hobbies,
                                      Email = user.Email,
                                      IsEmailVerified = user.IsEmailVerified,
                                      IsActive = user.IsActive,
                                      CourseName = user.Course.CourseName,
                                      AddressLine = user.Address.AddressLine,
                                      CountryName = user.Address.Country.CountryName,
                                      StateName = user.Address.State.StateName,
                                      CityName = user.Address.City.CityName,
                                      Pincode = user.Address.Pincode,
                                      DateCreated = user.DateCreated,
                                      DateModified = user.DateModified,
                                      RoleName = userInRole.Role.RoleName
                                  }).ToList();


                model.ShowLists = searchList;

                return View(model);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }
    }
}