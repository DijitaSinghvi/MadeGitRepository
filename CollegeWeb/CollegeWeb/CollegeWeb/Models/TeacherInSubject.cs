using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("TeacherInSubject")]
    public class TeacherInSubject
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherInSubjectId { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        
    }
}