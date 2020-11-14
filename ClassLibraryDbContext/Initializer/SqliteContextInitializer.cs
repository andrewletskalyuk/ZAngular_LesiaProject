using Microsoft.EntityFrameworkCore.Internal;
using Models.Model;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibraryDbContext.Initializer
{
    public static class SqliteContextInitializer
    {
        public static void Initialize(SqliteContext sqliteContext)
        {
            if (!sqliteContext.Objectives.Any())
            {
                Pacient pacient1 = new Pacient
                {
                    Name = "Andrii",
                    Patronymic = "Viacheslavovuch",
                    Surname = "Letskaliuk",
                    Birthday = DateTime.Now,
                    AddDay = DateTime.Now,
                    Anamnesis = "tempdata",
                    Cycle = "tempdata",
                    StatusLocalic = "Status localis",
                    Survey = "bla bla bla obstejenia"
                };
                Pacient pacient2 = new Pacient
                {
                    Name = "Lesia",
                    Patronymic = "Viacheslavivna",
                    Surname = "Gogol",
                    Birthday = DateTime.Now,
                    AddDay = DateTime.Now,
                    Anamnesis = "tempdata",
                    Cycle = "tempdata",
                    StatusLocalic = "Status localis",
                    Survey = "bla bla bla obstejenia"
                };

                Pacient pacient3 = new Pacient
                {
                    Name = "Viacheslav",
                    Patronymic = "Vasuliovuch",
                    Surname = "Letskaluik",
                    Birthday = DateTime.Now,
                    AddDay = DateTime.Now,
                    Anamnesis = "tempdata",
                    Cycle = "tempdata",
                    StatusLocalic = "Status localis",
                    Survey = "bla bla bla obstejenia"
                };
                sqliteContext.Pacients.AddRange(new List<Pacient> { pacient1, pacient2, pacient3 });
                sqliteContext.SaveChanges();

                Diagnosis diagnosis1 = new Diagnosis { Name = "diagnosis1", Description = "description for diagnosis" };
                Diagnosis diagnosis2 = new Diagnosis { Name = "diagnosis3", Description = "description for diagnosis" };
                Diagnosis diagnosis3 = new Diagnosis { Name = "diagnosis3", Description = "description for diagnosis" };


                Complaint complaint1 = new Complaint { Name = "скарга1", PacientId = 1 };
                Complaint complaint2 = new Complaint { Name = "скарга2", PacientId = 2 };
                Complaint complaint3 = new Complaint { Name = "скарга234234", PacientId = 1 };
                Complaint complaint4 = new Complaint { Name = "скарга123123", PacientId = 2 };

                pacient1.Complaints.Add(complaint1);
                pacient1.Complaints.AddRange(new List<Complaint> { complaint1, complaint2, complaint3, complaint4 });
                pacient2.Complaints.AddRange(new List<Complaint> { complaint1, complaint2, complaint3, complaint4 });
                pacient3.Complaints.AddRange(new List<Complaint> { complaint1, complaint2, complaint3, complaint4 });

                sqliteContext.Diagnoses.Add(diagnosis1);
                sqliteContext.Diagnoses.Add(diagnosis2);
                sqliteContext.Diagnoses.Add(diagnosis3);
                sqliteContext.SaveChanges();

                sqliteContext.PacientDiagnoses.Add(new PacientDiagnosis { Diagnosis = diagnosis1, Pacient = pacient1 });
                sqliteContext.PacientDiagnoses.Add(new PacientDiagnosis { Diagnosis = diagnosis2, Pacient = pacient2 });
                sqliteContext.PacientDiagnoses.Add(new PacientDiagnosis { Diagnosis = diagnosis3, Pacient = pacient3 });

                sqliteContext.SaveChanges();

                Objective objective1 = new Objective { Height = 143, Weight = 23, Pacient = pacient2, IMT = "imt1" }; //індекс маси тіла = ріст в метрах в квадраті  / масу в кг
                Objective objective2 = new Objective { Height = 145, Weight = 88, Pacient = pacient1, IMT = "imttra" };
                Objective objective3 = new Objective { Height = 66, Weight = 98,  Pacient = pacient3, IMT = "try and will be" };

                sqliteContext.Objectives.AddRange(new List<Objective> { objective1, objective2, objective3 });
                sqliteContext.SaveChanges();

                pacient1.Objective = objective1;
                pacient2.Objective = objective2;
                pacient3.Objective = objective3;

                sqliteContext.Pacients.UpdateRange(pacient1, pacient2, pacient3);
                sqliteContext.SaveChanges();

                Appointment appointment1 = new Appointment { Name = "Andrii", Email = "andr@gmail.com", Message = "Pruvit", Phone = "0673840953", DateWhenAdded = DateTime.Now };
                Appointment appointment2 = new Appointment { Name = "Valia", Email = "valia@gmail.com", Message = "ASP", Phone = "0972264556", DateWhenAdded = new DateTime(2020, 10, 10) };
                Appointment appointment3 = new Appointment { Name = "Valia", Email = "valia@gmail.com", Message = "ASP", Phone = "0972264556", DateWhenAdded = new DateTime(2020, 10, 10) };

                sqliteContext.Appointments.AddRange(new List<Appointment> { appointment1, appointment2, appointment3 });
                sqliteContext.SaveChanges();

            }
        }
    }
}