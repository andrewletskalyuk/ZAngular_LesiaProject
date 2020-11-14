using Microsoft.EntityFrameworkCore;
using Models.Model;
using Models.Models;

namespace ClassLibraryDbContext
{
    public class SqliteContext : DbContext, IDataBaseContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<PacientDiagnosis> PacientDiagnoses { get; set; }
        public SqliteContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Sqlite.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Objective>().ToTable("Objective")
                .HasKey(z => z.PacientId);
            base.OnModelCreating(modelBuilder);


            //вяжемо багато до багатьох
            modelBuilder.Entity<PacientDiagnosis>()
            .HasKey(pd => new { pd.DiagnosisId, pd.PacientId });

            modelBuilder.Entity<PacientDiagnosis>()
                .HasOne(pd => pd.Pacient)
                .WithMany(p => p.PacientDiagnoses)
                .HasForeignKey(pd => pd.PacientId);

            modelBuilder.Entity<PacientDiagnosis>()
                .HasOne(pd => pd.Diagnosis)
                .WithMany(p => p.PacientDiagnoses)
                .HasForeignKey(pd => pd.DiagnosisId);
        }
    }
}
