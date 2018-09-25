using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("UserInRole")]
    public class UserInRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserInRoleId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<User> Users { get; set; }

    }
}