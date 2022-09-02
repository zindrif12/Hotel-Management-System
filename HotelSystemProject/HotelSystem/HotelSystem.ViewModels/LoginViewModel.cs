 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        //[DataType(DataType.Password)]
        //[Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public int CusId { get; set; }
        public string LoginError { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
