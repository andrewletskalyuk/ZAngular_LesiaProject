using Microsoft.EntityFrameworkCore;
using Models.Model;

namespace ClassLibraryDbContext
{
    public interface IDataBaseContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
    }
}