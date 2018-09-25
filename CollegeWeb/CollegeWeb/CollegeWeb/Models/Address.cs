using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        [MaxLength(255)][Required(ErrorMessage ="Please enter your address")]
        public string AddressLine { get; set; }
        [Display(Name ="Country")]
        [Required(ErrorMessage ="Please select your country")]
        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
             [Display(Name ="Country")]
             [Required(ErrorMessage ="Please enter your state")]
        public string StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State state { get; set; }
        [Display(Name ="City")]
        [Required(ErrorMessage ="please enter your city")]

        public string CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        
        [Required(ErrorMessage ="please enter your pincode")]
        public int Pincode { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public ICollection<Country> Countries { get; set; }
        public ICollection<State> States { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<User> Users { get; set; }

    }
}