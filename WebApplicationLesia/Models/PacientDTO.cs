using Models.Model;
using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Models
{
    public class PacientDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; } //по батькові
        public string Surname { get; set; }
        public string Birthday { get; set; }
        public string AddDay { get; set; } //дата додавання (створення) пацієнта
        public List<PacientDiagnosis> PacientDiagnoses { get; set; } //Діагнози
        public string Anamnesis { get; set; } //Анамнез
        public string Cycle { get; set; } //цикл для дівчат (регулярний або не регулярний)
        public List<ComplaintDTO> ComplaintsDTO { get; set; } //Скарги
        public string StatusLocalic { get; set; } //Status localic
        public string Survey { get; set; } //Обстеження
        public int? ObjectiveId { get; set; }
        public PacientDTO()
        {
            PacientDiagnoses = new List<PacientDiagnosis>();
            ComplaintsDTO = new List<ComplaintDTO>();
        }
    }
}
