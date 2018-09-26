using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("State")]
    public class State
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        [Required(ErrorMessage ="Please enter your state.")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public bool IsActive { get; set; }
     
        public ICollection<Address> Addresses { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}