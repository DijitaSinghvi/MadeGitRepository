using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [Required(ErrorMessage ="Please enter your country")]
        public string CountryName { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }
}