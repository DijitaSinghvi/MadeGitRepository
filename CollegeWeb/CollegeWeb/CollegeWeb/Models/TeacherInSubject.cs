using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("TeacherInSubject")]
    public class TeacherInSubject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherInSubjectId { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}