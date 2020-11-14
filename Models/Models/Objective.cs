using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Model
{
    public class Objective //дані про падієнта
    {
        [Key]
        [ForeignKey("Pacient")] //ключ один до одного
        public int Id { get; set; }

        //[Required]
        [Display(Name = "Вага")]
        public float Weight { get; set; }

        //[Required]
        [Display(Name = "Ріст")]
        public float Height { get; set; }

        //[Required]
        [Display(Name = "ІМТ")]
        public string IMT { get; set; }

        [JsonIgnore]
        public virtual Pacient Pacient { get; set; }
        public int? PacientId { get; set; }
    }
}
