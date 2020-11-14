using AutoMapper;
using LesiaWebApi.Models;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LesiaWebApi.Utils
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //це для запису на прийом
            CreateMap<AppointmentDTO, Appointment>();
            //.ForMember(ad=> ad.DateWhenAdded, opt=>opt.MapFrom(src=>src.DateWhenAdded.ToShortDateString()));
            CreateMap<Appointment, AppointmentDTO>();
            //.ForMember(ad => ad.DateWhenAdded, opt => opt.MapFrom(s => DateTime.ParseExact(s.DateWhenAdded, "dd.mm.yyyy", CultureInfo.InvariantCulture)));

            //Скарги
            CreateMap<Complaint, ComplaintDTO>();
            CreateMap<ComplaintDTO, Complaint>();

            //Діагнози
            CreateMap<Diagnosis, DiagnosisDTO>();
            CreateMap<DiagnosisDTO, Diagnosis>();

            //Особисті дані про пацієнта (вага, ріст ІМТ)
            CreateMap<Objective, ObjectiveDTO>();
            CreateMap<ObjectiveDTO, Objective>();

            //Pacient
            CreateMap<Pacient, PacientDTO>();
            CreateMap<PacientDTO, Pacient>();
        }
    }
}
