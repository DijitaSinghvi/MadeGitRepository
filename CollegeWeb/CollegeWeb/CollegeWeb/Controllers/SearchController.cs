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
        public ActionResult GetFilters()
        {
            //create object of SearchModel.
            SearchModel model = new SearchModel();
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
           // Show list of all records from database.
            var showList = (from user in db.Users
                            join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
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
            model.ShowLists = showList;


            ////to compare filters' data in database.
            //var searchList = (from
            //                    user in db.Users
            //                  join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
            //                  where user.FirstName == model.FirstName
            //                  //&&
            //                  //user.LastName == objSearchModel.LastName &&
            //                  //user.Gender == objSearchModel.Gender &&
            //                  //user.DateOfBirth == objSearchModel.DateOfBirth &&
            //                  //user.Hobbies == objSearchModel.Hobbies &&
            //                  //user.Email == objSearchModel.Email &&
            //                  //user.IsEmailVerified == objSearchModel.IsEmailVerified &&
            //                  //user.IsActive == objSearchModel.IsActive &&
            //                  //user.Course.CourseName == objSearchModel.CourseName &&
            //                  //user.Address.AddressLine == objSearchModel.AddressLine &&
            //                  //user.Address.Country.CountryName == objSearchModel.CountryName &&
            //                  //user.Address.State.StateName == objSearchModel.StateName &&
            //                  //user.Address.City.CityName == objSearchModel.CityName &&
            //                  //user.Address.Pincode == objSearchModel.Pincode &&
            //                  //user.DateCreated == objSearchModel.DateCreated &&
            //                  //user.DateModified == objSearchModel.DateModified &&
            //                  //userInRole.Role.RoleName == objSearchModel.RoleName
            //                  select new ShowList
            //                  {
            //                      FirstName = user.FirstName,
            //                      LastName = user.LastName,
            //                      Gender = user.Gender,
            //                      DateOfBirth = user.DateOfBirth,
            //                      Hobbies = user.Hobbies,
            //                      Email = user.Email,
            //                      IsEmailVerified = user.IsEmailVerified,
            //                      IsActive = user.IsActive,
            //                      CourseName = user.Course.CourseName,
            //                      AddressLine = user.Address.AddressLine,
            //                      CountryName = user.Address.Country.CountryName,
            //                      StateName = user.Address.State.StateName,
            //                      CityName = user.Address.City.CityName,
            //                      Pincode = user.Address.Pincode,
            //                      DateCreated = user.DateCreated,
            //                      DateModified = user.DateModified,
            //                      RoleName = userInRole.Role.RoleName
            //                  }).ToList();

            //model.ShowLists = searchList;
            return View(model);
        }



        //public ActionResult SearchResult(SearchModel objSearchModel)
        //{
        //    var searchList = (from
        //                       user in db.Users
        //                      join userInRole in db.UserInRoles on user.UserId equals userInRole.UserId
        //                      where user.FirstName == objSearchModel.FirstName &&
        //                      user.LastName == objSearchModel.LastName &&
        //                      user.Gender == objSearchModel.Gender &&
        //                      user.DateOfBirth == objSearchModel.DateOfBirth &&
        //                      user.Hobbies == objSearchModel.Hobbies &&
        //                      user.Email == objSearchModel.Email &&
        //                      user.IsEmailVerified == objSearchModel.IsEmailVerified &&
        //                      user.IsActive == objSearchModel.IsActive &&
        //                      user.Course.CourseName == objSearchModel.CourseName &&
        //                      user.Address.AddressLine == objSearchModel.AddressLine &&
        //                      user.Address.Country.CountryName == objSearchModel.CountryName &&
        //                      user.Address.State.StateName == objSearchModel.StateName &&
        //                      user.Address.City.CityName == objSearchModel.CityName &&
        //                      user.Address.Pincode == objSearchModel.Pincode &&
        //                      user.DateCreated == objSearchModel.DateCreated &&
        //                      user.DateModified == objSearchModel.DateModified &&
        //                      userInRole.Role.RoleName == objSearchModel.RoleName
        //                      select new SearchModel
        //                      {
        //                          FirstName = user.FirstName,
        //                          LastName = user.LastName,
        //                          Gender = user.Gender,
        //                          DateOfBirth = user.DateOfBirth,
        //                          Hobbies = user.Hobbies,
        //                          Email = user.Email,
        //                          IsEmailVerified = user.IsEmailVerified,
        //                          IsActive = user.IsActive,
        //                          CourseName = user.Course.CourseName,
        //                          AddressLine = user.Address.AddressLine,
        //                          CountryName = user.Address.Country.CountryName,
        //                          StateName = user.Address.State.StateName,
        //                          CityName = user.Address.City.CityName,
        //                          Pincode = user.Address.Pincode,
        //                          DateCreated = user.DateCreated,
        //                          DateModified = user.DateModified,
        //                          RoleName = userInRole.Role.RoleName
        //                      }).ToList();
        //                return View(searchList);
        //}




    }
}