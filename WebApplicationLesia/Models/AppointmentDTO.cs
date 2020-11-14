using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Models
{
    public class AppointmentDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } //ім"я
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; } //повідомлення
        //дата на прийом під питанням
        public DateTime DateWhenAdded { get; set; }
    }
}
