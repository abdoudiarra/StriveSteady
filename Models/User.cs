using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StriveSteady.Models
{
    public class User
    {   
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email {get; set;}
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordRepeat { get; set; }
        public List<Goal> Goals { get; set; }
    }
}