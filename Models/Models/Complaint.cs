using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Model
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } //назва скарги

        //public virtual Pacient Pacient { get; set; }
        public int PacientId { get; set; }
        
    }
}