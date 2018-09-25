using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{[Table("SubjectInCourse")]
    public class SubjectInCourse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectInCourseId { get; set; }
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Subject> Subjects { get; set; }


    }
}