using Models.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Models
{
    //
    public class PacientDiagnosis
    {
        [Key]
        public int id { get; set; }

        public int DiagnosisId { get; set; }
        
        [JsonIgnore]
        public virtual Diagnosis Diagnosis { get; set; }
        
        public int PacientId { get; set; }

        [JsonIgnore]
        public virtual Pacient Pacient { get; set; }
    }
}
