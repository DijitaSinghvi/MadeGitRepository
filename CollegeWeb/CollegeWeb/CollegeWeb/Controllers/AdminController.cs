using CollegeWeb.Models;
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
                                   UserId = user.UserId,
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
                    Console.Write(ex.Message);
                    //Roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
            return RedirectToAction("ViewStudents");
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
        /// 
        public ActionResult EditStudent(int? id)
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
                             where user.UserId == id

                             select new ViewModel
                             {
                                 UserId = user.UserId,
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
                                 Pincode = user.Address.Pincode,
                                 AddressId = user.AddressId,
                                 CityId = user.Address.CityId,
                                 StateId = user.Address.StateId,
                                 CountryId = user.Address.CountryId,
                                 CourseId = user.CourseId,
                                 ConfirmPassword = user.ConfirmPassword

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
        public ActionResult EditStudent(ViewModel objViewModel)
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
                    userRecord.IsEmailVerified = objViewModel.IsEmailVerified;
                    userRecord.IsActive = objViewModel.IsActive;
                    userRecord.ConfirmPassword = objViewModel.ConfirmPassword;


                    db.SaveChanges();


                }
                return RedirectToAction("ViewStudents");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }

        /// <summary>
        /// Delete student.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deleteRecord = (from
                                    user in db.Users
                                where user.UserId == id
                                select user).FirstOrDefault();
            var userInRoleRecord = (from
                                      userInRole in db.UserInRoles
                                    where userInRole.UserId == deleteRecord.UserId
                                    select userInRole).FirstOrDefault();



            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        /// <summary>
        /// Delete a student record from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        public ActionResult DeleteStudent(int id)
        {
            try
            {

                //Delete selected record from database.
                var deleteRecord = (from
                                    user in db.Users
                                    where user.UserId == id
                                    select user).FirstOrDefault();
                //Get userInRole record from database.
                var userInRoleRecord = (from
                                        userInRole in db.UserInRoles
                                        where userInRole.UserId == deleteRecord.UserId
                                        select userInRole).FirstOrDefault();
                //Delete UserInRole record from table.
                if (userInRoleRecord != null)
                {
                    db.UserInRoles.Remove(userInRoleRecord);
                    db.SaveChanges();
                }


                //Delete user record from table.
                if (deleteRecord != null)
                {
                    db.Users.Remove(deleteRecord);
                    db.SaveChanges();
                }


                return RedirectToAction("ViewStudents", "Admin");
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// View list of teachers.
        /// </summary>
        /// <returns></returns>
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
                                   UserId = user.UserId,

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

                                   AddressLine = user.Address.AddressLine,
                                   CityName = user.Address.City.CityName,
                                   StateName = user.Address.State.StateName,
                                   CountryName = user.Address.Country.CountryName,
                                   Pincode = user.Address.Pincode
                               }).ToList();

            return View(TeacherList);
        }
        /// <summary>
        /// Add new student record in website.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddTeacher()
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
        public ActionResult AddTeacher(ViewModel objViewModel)
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
                    Console.Write(ex.Message);
                    //Roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
            return RedirectToAction("ViewTeachers");
        }

        /// <summary>
        ///   To edit teacher record, get details of the teacher from database. 
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult EditTeacher(int? id)
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
                             where user.UserId == id

                             select new ViewModel
                             {
                                 UserId = user.UserId,
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
                                 Pincode = user.Address.Pincode,
                                 AddressId = user.AddressId,
                                 CityId = user.Address.CityId,
                                 StateId = user.Address.StateId,
                                 CountryId = user.Address.CountryId,
                                 CourseId = user.CourseId,
                                 ConfirmPassword = user.ConfirmPassword

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
        public ActionResult EditTeacher(ViewModel objViewModel)
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
                    userRecord.IsEmailVerified = objViewModel.IsEmailVerified;
                    userRecord.IsActive = objViewModel.IsActive;
                    userRecord.ConfirmPassword = objViewModel.ConfirmPassword;


                    db.SaveChanges();


                }
                return RedirectToAction("ViewTeachers");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// Delete teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteTeacher(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deleteRecord = (from
                                    user in db.Users
                                where user.UserId == id
                                select user).FirstOrDefault();
            var userInRoleRecord = (from
                                      userInRole in db.UserInRoles
                                    where userInRole.UserId == deleteRecord.UserId
                                    select userInRole).FirstOrDefault();



            return View();
        }
        [HttpPost, ActionName("DeleteTeacher")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Delete a student record from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 

        public ActionResult DeleteTeacher(int id)
        {
            try
            {

                //Delete selected record from database.
                var deleteRecord = (from
                                    user in db.Users
                                    where user.UserId == id
                                    select user).FirstOrDefault();
                //Get userInRole record from database.
                var userInRoleRecord = (from
                                        userInRole in db.UserInRoles
                                        where userInRole.UserId == deleteRecord.UserId
                                        select userInRole).FirstOrDefault();
                //Delete UserInRole record from table.
                if (userInRoleRecord != null)
                {
                    db.UserInRoles.Remove(userInRoleRecord);
                    db.SaveChanges();
                }


                //Delete user record from table.
                if (deleteRecord != null)
                {
                    db.Users.Remove(deleteRecord);
                    db.SaveChanges();
                }


                return RedirectToAction("ViewTeachers", "Admin");
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// View list of Subjects.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewSubjects()
        {
            //Get list of students from database.
            var subjectList = (from
                               subject in db.Subjects

                               select new ViewModel
                               {
                                   SubjectId = subject.SubjectId,
                                   SubjectName = subject.SubjectName


                               }).ToList();

            return View(subjectList);
        }

        /// <summary>
        /// Add new subject record in website.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSubject()
        {
            //Creating an object of ViewModel.


            //Return object of ViewModel in the view.
            return View();

        }

        /// <summary>
        ///To post subject details to database. 
        /// </summary>
        /// <param name="objViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSubject(ViewModel objViewModel)
        {


            try
            {
                //Subject added in database.
                Subject objSubject = new Subject
                {
                    SubjectId = objViewModel.SubjectId,
                    SubjectName = objViewModel.SubjectName,


                };
                db.Subjects.Add(objSubject);
                db.SaveChanges();




            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return View();
            }
            return RedirectToAction("ViewSubjects");


        }
        /// <summary>
        ///   To edit subject record, get names of the subject from database. 
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult EditSubject(int? id)
        {
            try
            {

                var model = (from subject in db.Subjects
                             where subject.SubjectId == id

                             select new ViewModel
                             {
                                 SubjectId = subject.SubjectId,
                                 SubjectName = subject.SubjectName,

                             }).FirstOrDefault();



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
        public ActionResult EditSubject(ViewModel objViewModel)
        {
            try
            {
                //Raw data sent to address table.

                var subjectRecord = (from subject in db.Subjects
                                     where subject.SubjectId == objViewModel.SubjectId
                                     select subject).FirstOrDefault();
                if (subjectRecord != null)
                {
                    subjectRecord.SubjectId = objViewModel.SubjectId;
                    subjectRecord.SubjectName = objViewModel.SubjectName;

                    db.SaveChanges();


                }
                return RedirectToAction("ViewSubjects");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// Delete subject record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from subject in db.Subjects
                         where subject.SubjectId == id

                         select new ViewModel
                         {
                             SubjectId = subject.SubjectId,
                             SubjectName = subject.SubjectName,

                         }).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("DeleteSubject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {


                //Delete selected record from database.
                var deleteRecord = (from
                                    subject in db.Subjects
                                    where subject.SubjectId == id
                                    select subject).FirstOrDefault();


                if (deleteRecord != null)
                {
                    db.Subjects.Remove(deleteRecord);
                    db.SaveChanges();
                }
                return RedirectToAction("ViewSubjects", "Admin");
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// View list of Subjects.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCourses()
        {
            //Get list of students from database.
            var courseList = (from
                               course in db.Courses

                              select new ViewModel
                              {
                                  CourseId = course.CourseId,
                                  CourseName = course.CourseName


                              }).ToList();

            return View(courseList);
        }

        /// <summary>
        /// Add new course record in website.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCourse()
        {

            return View();

        }

        /// <summary>
        ///To post course details to database. 
        /// </summary>
        /// <param name="objViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCourse(ViewModel objViewModel)
        {


            try
            {
                //Subject added in database.
                Course objCourse = new Course
                {
                    CourseId = objViewModel.CourseId,
                    CourseName = objViewModel.CourseName,


                };
                db.Courses.Add(objCourse);
                db.SaveChanges();




            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return View();
            }
            return RedirectToAction("ViewCourses");


        }
        /// <summary>
        ///   To edit subject record, get names of the subject from database. 
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult EditCourse(int? id)
        {
            try
            {

                var model = (from course in db.Courses
                             where course.CourseId == id

                             select new ViewModel
                             {
                                 CourseId = course.CourseId,
                                 CourseName = course.CourseName,

                             }).FirstOrDefault();



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
        public ActionResult EditCourse(ViewModel objViewModel)
        {
            try
            {
                //Raw data sent to address table.

                var courseRecord = (from course in db.Courses
                                    where course.CourseId == objViewModel.CourseId
                                    select course).FirstOrDefault();
                if (courseRecord != null)
                {
                    courseRecord.CourseId = objViewModel.CourseId;
                    courseRecord.CourseName = objViewModel.CourseName;

                    db.SaveChanges();


                }
                return RedirectToAction("ViewCourses");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }
        /// <summary>
        /// Delete subject record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = (from course in db.Courses
                         where course.CourseId == id

                         select new ViewModel
                         {
                             CourseId = course.CourseId,
                             CourseName = course.CourseName,

                         }).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(int id)
        {
            try
            {


                //Delete selected record from database.
                var deleteRecord = (from
                                    course in db.Courses
                                    where course.CourseId == id
                                    select course).FirstOrDefault();


                if (deleteRecord != null)
                {
                    db.Courses.Remove(deleteRecord);
                    db.SaveChanges();
                }
                return RedirectToAction("ViewCourses", "Admin");
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }

        /// <summary>
        ///View list of Users and roles.      
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewUserAndRoles()
        {
            //Get list of students from database.
            var studentList = (from
                               user in db.Users
                               join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId

                               orderby user.UserId
                               select new ViewModel
                               {
                                   UserId = user.UserId,
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
                                   Pincode = user.Address.Pincode,
                                   RoleId = userInRole.RoleId,
                                   RoleName = userInRole.Role.RoleName
                               }).ToList();

            return View(studentList);
        }

        /// <summary>
        /// Assign role of teacher or student.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AssignRole(int? id)
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
                             where user.UserId == id

                             select new ViewModel
                             {
                                 UserId = user.UserId,
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
                                 Pincode = user.Address.Pincode,
                                 AddressId = user.AddressId,
                                 CityId = user.Address.CityId,
                                 StateId = user.Address.StateId,
                                 CountryId = user.Address.CountryId,
                                 CourseId = user.CourseId,
                                 ConfirmPassword = user.ConfirmPassword,
                                 RoleId = userInRole.RoleId,
                                 RoleName = userInRole.Role.RoleName


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
        /// Save assigned role in database.
        /// </summary>
        [HttpPost]
        public ActionResult AssignRole(ViewModel objViewModel)
        {
            try
            {
                //Raw data sent to address table.

                //UserInRole objUserInRole = new UserInRole();

                var userRecord = (from user in db.Users
                                  join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                  where user.UserId == objViewModel.UserId
                                  select user
                                  ).FirstOrDefault();

                if (userRecord != null)
                {
                    userRecord.FirstName = objViewModel.FirstName;

                    var UserRecord = (from User in db.Users
                                         where User.UserId == objViewModel.UserId
                                         select User
                             ).FirstOrDefault();

                    UserRecord.DateCreated = DateTime.Now;
                    UserRecord.DateModified = DateTime.Now;
                    UserRecord.UserId = objViewModel.UserId;
                    UserRecord.FirstName = objViewModel.FirstName;
                    UserRecord.LastName = objViewModel.LastName;
                    UserRecord.Gender = objViewModel.Gender;
                    UserRecord.DateOfBirth = objViewModel.DateOfBirth;
                    UserRecord.Hobbies = objViewModel.Hobbies;
                    UserRecord.Email = objViewModel.Email;
                    UserRecord.IsEmailVerified = objViewModel.IsEmailVerified;
                    UserRecord.Password = objViewModel.Password;
                    UserRecord.ConfirmPassword = objViewModel.ConfirmPassword;
                    UserRecord.IsActive = objViewModel.IsActive;
                    UserRecord.CourseId = objViewModel.CourseId;
                  





                    var addressRecord = (from address in db.Addresses
                                         where address.AddressId == objViewModel.AddressId
                                         select address
                                 ).FirstOrDefault();

                    addressRecord.AddressLine = objViewModel.AddressLine;
                    addressRecord.CityId = objViewModel.CityId;
                    addressRecord.CountryId = objViewModel.CountryId;
                    addressRecord.Pincode = objViewModel.Pincode;
                    addressRecord.StateId = objViewModel.StateId;


                    UserInRole objUserRole = new UserInRole
                    {
                        RoleId = objViewModel.RoleId,
                        UserId = objViewModel.UserId,

                    };

                    Role role = new Role()
                    {
                        RoleId = objViewModel.RoleId,
                        RoleName = objViewModel.RoleName
                    };


                    db.SaveChanges();


                }
                return RedirectToAction("ViewUserAndRoles");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }

        /// <summary>
        ///View list of Teachers and subjects.      
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewSubjectAndTeacher()
        {
            //Get list of subject and teacher from database.
            var subjectTeacherList = (from
                               user in db.Users
                                      join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                      where userInRole.RoleId == 3


                                      select new ViewModel
                                      {
                                          UserId = user.UserId,
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
                                          Pincode = user.Address.Pincode,
                                          RoleId = userInRole.RoleId,
                                          RoleName = userInRole.Role.RoleName,
                                          SubjectId = teacherInSubject.SubjectId,
                                          SubjectName = teacherInSubject.Subject.SubjectName
                                      }).ToList();

            return View(subjectTeacherList);
        }
        /// <summary>
        /// Assign role of teacher or student.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AssignTeacherToSubject(int? id)
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
                var subjectList = db.Subjects.Select(x => new SubjectModel
                {
                    SubjectName = x.SubjectName,
                    SubjectId = x.SubjectId
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
                             where user.UserId == id
                             join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                             join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId

                             select new ViewModel
                             {
                                 UserId = user.UserId,
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
                                 Pincode = user.Address.Pincode,
                                 AddressId = user.AddressId,
                                 CityId = user.Address.CityId,
                                 StateId = user.Address.StateId,
                                 CountryId = user.Address.CountryId,
                                 CourseId = user.CourseId,
                                 ConfirmPassword = user.ConfirmPassword,
                                 RoleId = userInRole.RoleId,
                                 RoleName = userInRole.Role.RoleName,
                                 SubjectId = teacherInSubject.SubjectId,
                                 SubjectName = teacherInSubject.Subject.SubjectName



                             }).FirstOrDefault();

                model.Countries = countryList;
                model.States = stateList;
                model.Cities = cityList;
                model.Courses = courseList;
                model.Roles = roleList;
                model.Subjects = subjectList;
                return View(model);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }


        }


        /// <summary>
        /// Save assigned subject in database.
        /// </summary>
        [HttpPost]
        public ActionResult AssignTeacherToSubject(ViewModel objViewModel)
        {
            try
            {

                User objUser = new User();
                UserInRole objUserInRole = new UserInRole();
                TeacherInSubject objTeacherInSubject = new TeacherInSubject();

                var userRecord = from user in db.Users
                                 join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
                                 join teacherInSubject in db.TeacherInSubjects on user.UserId equals teacherInSubject.UserId
                                 where objUser.UserId == objViewModel.UserId
                                 select new
                                 {
                                     objUser.UserId,
                                     objUser.FirstName,
                                     objUser.LastName,
                                     objUser.Gender,
                                     objUser.DateOfBirth,
                                     objUser.Hobbies,
                                     objUser.Email,
                                     objUser.IsEmailVerified,
                                     objUser.Password,
                                     objUser.IsActive,
                                     objUser.DateCreated,
                                     objUser.DateModified,
                                     objUser.Course.CourseName,
                                     objUser.Address.AddressLine,
                                     objUser.Address.City.CityName,
                                     objUser.Address.State.StateName,
                                     objUser.Address.Country.CountryName,
                                     objUser.Address.Pincode,
                                     objUser.AddressId,
                                     objUser.Address.CityId,
                                     objUser.Address.StateId,
                                     objUser.Address.CountryId,
                                     objUser.CourseId,
                                     objUser.ConfirmPassword,
                                     objUserInRole.RoleId,
                                     objUserInRole.Role.RoleName,
                                     objTeacherInSubject.SubjectId,
                                     objTeacherInSubject.Subject.SubjectName



                                 };
                if (userRecord != null)
                {

                    objUser.DateCreated = DateTime.Now;
                    objUser.DateModified = DateTime.Now;
                    objUser.UserId = objViewModel.UserId;
                    objUser.FirstName = objViewModel.FirstName;
                    objUser.LastName = objViewModel.LastName;
                    objUser.Gender = objViewModel.Gender;
                    objUser.DateOfBirth = objViewModel.DateOfBirth;
                    objUser.Hobbies = objViewModel.Hobbies;
                    objUser.Email = objViewModel.Email;
                    objUser.IsEmailVerified = objViewModel.IsEmailVerified;
                    objUser.Password = objViewModel.Password;
                    objUser.ConfirmPassword = objViewModel.ConfirmPassword;
                    objUser.IsActive = objViewModel.IsActive;
                    objUser.CourseId = objViewModel.CourseId;
                    objUser.Address.AddressLine = objViewModel.AddressLine;
                    objUser.Address.CityId = objViewModel.CityId;
                    objUser.Address.CountryId = objViewModel.CountryId;
                    objUser.Address.Pincode = objViewModel.Pincode;
                    objUser.Address.StateId = objViewModel.StateId;
                    objUser.IsEmailVerified = objViewModel.IsEmailVerified;
                    objUser.IsActive = objViewModel.IsActive;
                    objUser.ConfirmPassword = objViewModel.ConfirmPassword;
                    objUserInRole.RoleId = objViewModel.RoleId;
                    objUserInRole.Role.RoleName = objViewModel.RoleName;
                    objTeacherInSubject.SubjectId = objViewModel.SubjectId;
                    objTeacherInSubject.Subject.SubjectName = objViewModel.SubjectName;

                    db.SaveChanges();



                }
                return RedirectToAction("ViewSubjectAndTeacher");
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }


    }
}