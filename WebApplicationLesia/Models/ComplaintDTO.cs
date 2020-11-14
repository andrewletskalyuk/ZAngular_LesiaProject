using Models.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Models
{
    public class ComplaintDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } //назва скарги
        //public virtual Pacient Pacient { get; set; }
        public int PacientId { get; set; }
    }
}
