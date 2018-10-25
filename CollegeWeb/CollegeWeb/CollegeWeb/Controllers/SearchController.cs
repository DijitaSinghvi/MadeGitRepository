using CollegeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class SearchController : Controller
    {
        CollegeContext db = new CollegeContext();

        /// <summary>
        /// Get the filters section on the search page.
        /// </summary>
        /// <returns></returns>
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
                                  where user.IsActive == model.IsActive || model.IsActive==false
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
            catch(Exception er)
            {
                Console.Write(er.Message);
                return View();
            }


        }
    }
}