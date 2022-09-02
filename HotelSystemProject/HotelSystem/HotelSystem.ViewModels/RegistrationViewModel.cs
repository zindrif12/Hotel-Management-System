using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.ViewModels
{
    public class RegistrationViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }

        [Display(Name ="Email")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string ContactNum { get; set; }
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public System.Guid ActivationCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public bool RememberMe { get; set; }
    }
}
