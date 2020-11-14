using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Models
{
    public class DiagnosisDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } //назва діагнозу
        public string Description { get; set; } //опис для діагнозу
        public List<PacientDiagnosis> PacientDiagnoses { get; set; }
    }
}
