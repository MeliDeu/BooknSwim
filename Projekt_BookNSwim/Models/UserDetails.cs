using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_BookNSwim.Models
{
    public class UserDetails
    {
        public UserDetails() { } // empty constructor

        // properties of each instance

        public int userId { set; get; }

        [Required]
        [Display(Name = "Username")]
        public string userUserName { set; get; }

        [Required]
        [Display(Name = "Password")]
        public string userPassword { set; get; }

        [Required]
        [Display(Name = "Email")]
        public string userEmail { set; get; }

        [Required]
        [Display(Name = "First Name")]
        public string userFirstName { set; get; }

        [Required]
        [Display(Name = "Last Name")]
        public string userLastName { set; get; }

    }
}
