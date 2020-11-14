using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZVersionUsersDTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Некоректна email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть будь ласка пароль!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введіть ПІБ")]
        [StringLength(100, ErrorMessage = "Введіть корректні дані", MinimumLength = 3)]
        [Display(Name = "Ваше ПІБ")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Неправильний номер телефону")]
        [Display(Name = "Формат даних 0771112233 без пробілів")]
        [RegularExpression(@"^(0[5-9][0-9]\d{7})$")]
        public string Phone { get; set; }
    }
}
