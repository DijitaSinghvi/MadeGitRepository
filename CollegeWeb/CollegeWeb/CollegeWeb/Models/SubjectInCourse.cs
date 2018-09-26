using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{[Table("SubjectInCourse")]
    public class SubjectInCourse
    {
      [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectInCourseId { get; set; }
        [Required(ErrorMessage ="Enter the course.")]
        
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        [Required(ErrorMessage ="Enter the subject for course.")]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
       


    }
}