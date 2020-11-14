using BaseProjectDataContext.Entity;
using ClassLibraryDbContext;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Model;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectDataContext
{
    public class SqLiteContextUsers : IdentityDbContext, IDataBaseContext
    {
        public DbSet<UserAdditionalInfo> UserAdditionalInfos { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Pacient> Pacients { get; set; }
        public DbSet<PacientDiagnosis> PacientDiagnoses { get; set; }

        public SqLiteContextUsers()
        {
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=SqliteUsers.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserAdditionalInfo)
                .WithOne(t => t.User)
                .HasForeignKey<UserAdditionalInfo>(q => q.Id);
            base.OnModelCreating(modelBuilder);

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
