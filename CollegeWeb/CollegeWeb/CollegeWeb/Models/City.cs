using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeWeb.Models
{[Table("City")]
    public class City
    {[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        [Required]
        public string CityName { get; set; }

        public int StateId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<State> States { get; set; }
    }
}
