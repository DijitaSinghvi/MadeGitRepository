using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("User")]
    public class User
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage ="Enter your firstname.")][MaxLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Enter your last name.")][MaxLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please enter your gender.")]
        public string Gender { get; set; }
        [Required(ErrorMessage ="Enter your date of birth.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage ="Enter your hobbies.")]
        public string Hobbies { get; set; }
        [Required(ErrorMessage ="Enter your email address.")][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       // [Required(ErrorMessage ="Your email address is not verified.")]
        public string IsEmailVerified { get; set; }
        [Required(ErrorMessage ="Enter a password.")][DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Confirm your password.")][DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        
      //  [Required(ErrorMessage="Your account is inactive." )]
        public bool IsActive { get; set; }
        
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [Required(ErrorMessage ="Enter date created.")]
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage ="Enter date modified.")]
        public DateTime DateModified { get; set; }
        public ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
       



    }
}