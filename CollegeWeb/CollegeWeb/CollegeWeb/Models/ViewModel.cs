using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    public class ViewModel
    {
        
       
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter your firstname.")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your last name.")]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your gender.")]
        public string Gender { get; set; }

        [Display(Name = "DoBs")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]
        public System.DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Enter your hobbies.")]
        public string Hobbies { get; set; }
        [Required(ErrorMessage = "Enter your email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
      //  [Required(ErrorMessage = "Your email address is not verified.")]
        public string IsEmailVerified { get; set; }
        [Required(ErrorMessage = "Enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        //[Required(ErrorMessage = "Your account is inactive.")]
        public bool IsActive { get; set; }

        public int CourseId { get; set; }
       
        public int AddressId { get; set; }
       
        [Required(ErrorMessage = "Enter date created.")]
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage = "Enter date modified.")]
        public DateTime DateModified { get; set; }
      
        public int RoleId { get; set; }
      
        public string RoleName { get; set; }
    }
}