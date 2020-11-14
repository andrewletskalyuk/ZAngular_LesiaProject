using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Model
{
    [Table("Pacients")]
    public class Pacient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Patronymic { get; set; } //по батькові
        [Required]
        public string Surname { get; set; }
        
        [Display(Name = "Дата народження")]
        [Required]
        public DateTime Birthday { get; set; }
        
        public DateTime AddDay { get; set; } //дата додавання (створення) пацієнта
        
        //[Required]
        public List<PacientDiagnosis> PacientDiagnoses { get; set; } //Діагнози
        
        [Required]
        [Display(Name = "Анамнез")]
        public string Anamnesis { get; set; } //Анамнез
        
        //[Required]
        [Display(Name="Циклічність")]
        public string Cycle { get; set; }
        
        //[Required]
        [Display(Name="На що скаржимось")]
        public List<Complaint> Complaints { get; set; } //Скарги
        
        //[Required]
        [Display(Name="Status Localic")]
        public string StatusLocalic { get; set; } //Status localic
        
        [ForeignKey(nameof(ObjectiveId))]
        public virtual Objective Objective { get; set; } //дані про пацієнта ріст маса і т.д.
        
        [Display(Name="Обстеження")]
        public string Survey { get; set; } //Обстеження
        public int? ObjectiveId { get; set; }
        public Pacient()
        {
            PacientDiagnoses = new List<PacientDiagnosis>();
            Complaints = new List<Complaint>();
            //Objective = new Objective();
        }
    }
}
