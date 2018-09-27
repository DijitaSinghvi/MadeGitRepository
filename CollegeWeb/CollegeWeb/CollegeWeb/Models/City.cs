using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{[Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        [Required(ErrorMessage="Please enter your city" )]
        public string CityName { get; set; }
        
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        
        public bool IsActive { get; set; }
       
        public ICollection<Address> Addresses { get; set; }
    }
}
