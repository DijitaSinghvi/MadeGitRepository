using CollegeWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CollegeWeb
{
    public class CollegeContext : DbContext
    {
        public CollegeContext() : base("CollegeWeb")
        {


        }
        public DbSet<User> Users { get; set; }

        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<TeacherInSubject> TeacherInSubjects { get; set; }
        public DbSet<SubjectInCourse> SubjectInCourses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}