using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    public class SearchModel
    {
        public int UserId { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
       
        public string Gender { get; set; }

       
        public DateTime DateOfBirth { get; set; }


       
        public string Hobbies { get; set; }
       
        public string Email { get; set; }
        
        public string IsEmailVerified { get; set; }
      
        public bool IsActive { get; set; }

        public int? CourseId { get; set; }

        public int? AddressId { get; set; }
        [DisplayName("Address")]
        public string AddressLine { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? Pincode { get; set; }


       
        public DateTime DateCreated { get; set; }
       
        public DateTime DateModified { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; }
        public string CourseName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public List<CourseClass> Courses { get; set; }
        public List<CountryClass> Countries { get; set; }
        public List<StateClass> States { get; set; }
        public List<CityClass> Cities { get; set; }
        public List<RoleClass> Roles { get; set; }

       

    }
    public class CourseClass
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
    public class CountryClass
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class StateClass
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class CityClass
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    public class RoleClass
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}