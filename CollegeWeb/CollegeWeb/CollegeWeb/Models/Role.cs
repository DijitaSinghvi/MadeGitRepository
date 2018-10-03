using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("Role")]
    public class Role
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Please enter your role.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage ="Please enter your role.")]
        public string RoleName { get; set; }
    }
}