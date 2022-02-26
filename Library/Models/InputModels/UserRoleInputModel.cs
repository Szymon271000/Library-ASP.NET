using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class UserRoleInputModel
    {
        [Required(ErrorMessage = "The email is mandatory")]
        [EmailAddress(ErrorMessage = "The email is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "The role is mandatory")]
        [Display(Name = "Role")]
        public Role Role { get; set; }
    }
}
