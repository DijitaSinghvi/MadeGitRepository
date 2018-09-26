using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("Course")]
    public class Course
    {[Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        [Required(ErrorMessage ="Please enter your course name.")]
        public string CourseName { get; set; }
        public ICollection<SubjectInCourse> SubjectInCourses { get; set; }
        public ICollection<User> Users { get; set; }
    }
}