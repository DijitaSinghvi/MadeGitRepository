using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("Subject")]
    public class Subject
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }
        [Required(ErrorMessage ="Please enter subject name.")]
        public string SubjectName { get; set; }
        public ICollection<SubjectInCourse> SubjectInCourses { get; set; }
        public ICollection<TeacherInSubject> TeacherInSubjects { get; set; }

    }
}