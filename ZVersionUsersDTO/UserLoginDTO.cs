using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZVersionUsersDTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Некоректна email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть будь ласка пароль!")]
        public string Password { get; set; }
    }
}
