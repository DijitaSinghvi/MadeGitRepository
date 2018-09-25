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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required][MaxLength(50)]
        public string FirstName { get; set; }
        [Required][MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Hobbies { get; set; }
        [Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string IsEmailVerified { get; set; }
        [Required][DataType(DataType.Password)]
        public string Password { get; set; }
        [Required][DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        
        [Required]
        public bool IsActive { get; set; }
        
        public int CourseId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
        public ICollection<Address> Addresses { get; set; }
       
        public ICollection<Course> Courses { get; set; }



    }
}