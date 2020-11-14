using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Models
{
    public class ObjectiveDTO //
    {
        [Key]
        [ForeignKey("Pacient")]
        public int Id { get; set; }
        //від 0 до 18 років
        public float Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public string IMT { get; set; } //індекс маси тіла
        [Required]
        public int? PacientId { get; set; }
    }
}
