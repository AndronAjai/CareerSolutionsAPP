﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public class CareerSolutionsDB : DbContext

    {

        public CareerSolutionsDB(DbContextOptions<CareerSolutionsDB> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<JobSeeker> JobSeekers { get; set; }

        public DbSet<Employer> Employers { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobApplication> Applications { get; set; }
        
        public DbSet<BranchOffice> BranchOffices { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SkillRelation> SkillRelations { get; set; }

        public DbSet<JobStatusNotification> JobStatusNotifications { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<JobSeeker>()
                .HasKey(js => js.JobSeekerID);

            modelBuilder.Entity<Employer>()
                .HasKey(e => e.EmployerID);

            modelBuilder.Entity<Job>()
                .HasKey(j => j.JobID);

            modelBuilder.Entity<JobApplication>()
                .HasKey(a => a.ApplicationID);

            modelBuilder.Entity<BranchOffice>()
                .HasKey(b => b.BranchOfficeID);

            modelBuilder.Entity<Notification>()
                .HasKey(n => n.NotificationID);

            modelBuilder.Entity<SkillRelation>()
                .HasNoKey();

            modelBuilder.Entity<JobStatusNotification>()
                .HasKey(jsn => jsn.SnID);


            modelBuilder.Entity<User>()
                .HasOne<BranchOffice>()
                .WithMany()
                .HasForeignKey(u => u.BranchOfficeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobSeeker>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<User>(js => js.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employer>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<User>(e => e.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasOne<Employer>()
                .WithMany()
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne<Job>()
                .WithMany()
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne<JobSeeker>()
                .WithMany()
                .HasForeignKey(a => a.JobSeekerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne<Employer>()
                .WithMany()
                .HasForeignKey(a => a.EmployerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobStatusNotification>()
                .HasOne<JobApplication>()
                .WithMany()
                .HasForeignKey(a => a.ApplicationID)
                .OnDelete(DeleteBehavior.Restrict);


        }


    }

    [Table("BranchOffices")]

    public class BranchOffice

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchOfficeID { get; set; }

        [Required]

        [StringLength(100)]

        public string BranchName { get; set; }

        [StringLength(255)]

        public string BranchAddress { get; set; }

        [StringLength(15)]

        public string? PhoneNumber { get; set; }


    }

    [Table("Users")]

    public class User

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]

        [StringLength(50)]

        public string Username { get; set; }

        [Required]

        
        [StringLength(int.MaxValue)]
        public string Password { get; set; }

        [Required]

        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        [ForeignKey("BranchOffices")]
        public int? BranchOfficeID { get; set; }
        

    }

    [Table("JobSeekers")]

    public class JobSeeker

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobSeekerID { get; set; }

        [Required]

        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]

        [StringLength(50)]

        public string LastName { get; set; }

        [StringLength(15)]

        public string? PhoneNumber { get; set; }

        [StringLength(255)]

        public string? Address { get; set; }

        [StringLength(1000)]

        public string? ProfileSummary { get; set; }

        [Required]

        public string KeySkills { get; set; }

        [Required]

        public string ExpertField { get; set; }

        public string? ResumePath { get; set; }

        public string? AcademicDetails { get; set; }

        public string? ProfessionalDetails { get; set; }

        public string? PreferredIndustry { get; set; }

        public string? PreferredSpecialization { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

    }

    [Table("Employers")]

    public class Employer

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployerID { get; set; }

        [Required]

        [StringLength(100)]

        public string CompanyName { get; set; }

        [Required]

        [StringLength(50)]

        public string ContactPerson { get; set; }

        [StringLength(15)]

        public string? PhoneNumber { get; set; }

        [StringLength(255)]

        public string? CompanyAddress { get; set; }

        [StringLength(50)]

        public string? IndustryType { get; set; }

        [StringLength(100)]

        public string? WebsiteURL { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }


    }

    [Table("Jobs")]

    public class Job

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int JobID { get; set; }

        [ForeignKey("Employers")]

        public int EmployerID { get; set; }

        [Required]

        [StringLength(100)]

        public string JobTitle { get; set; }

        [Required]

        public string JobDescription { get; set; }

        [Required]

        public string IndustryType { get; set; }

        [Required]

        public string Specialization { get; set; }

        [Required]

        public string RequiredSkills { get; set; }

        public string? ExperienceLevel { get; set; }

        [Required]

        public string Location { get; set; }

        public string? SalaryRange { get; set; }

        [Required]

        public DateTime PostedDate { get; set; }

        public DateTime? ApplicationDeadline { get; set; }

        [Required]

        public string JobType { get; set; }


    }

    [Table("Applications")]

    public class JobApplication

    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationID { get; set; }

        [ForeignKey("Jobs")]
        public int JobID { get; set; }

        [ForeignKey("JobSeekers")]
        public int JobSeekerID { get; set; }

        [ForeignKey("Jobs")]
        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; }

    }

    [Table("Notifications")]
     public class Notification

     {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }

        [ForeignKey("Employers")]
        public int EmployerID { get; set; }

        public int ApplicationID { get; set; }

        public string Message { get; set; }

     }

    [Table("JobStatusNotifications")]
    public class JobStatusNotification
    {
        [Key]
        public int SnID { get; set; }

        [ForeignKey("Applications")]
        public int ApplicationID { get; set; }

        [StringLength(int.MaxValue)]
        public string Msg { get; set; }

    }


    [Table("SkillRelations")]
    public class SkillRelation
    {
        [Key]
        public string KeySkill { get; set; }

        [StringLength(int.MaxValue)]
        public string SubSkill { get; set; }

    }
}