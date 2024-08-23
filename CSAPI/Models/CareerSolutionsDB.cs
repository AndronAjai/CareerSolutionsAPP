using System;
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
        // Syntax based  on ClassName(Entity) table name
        public DbSet<BranchOffice> BranchOffices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
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

            // Define foreign key relationships and behaviors
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

        // Navigation properties

        //public ICollection<User> Users { get; set; } = new List<User>();

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

        [StringLength(25)]

        public string Password { get; set; }

        [Required]

        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        [ForeignKey("BranchOffices")]
        public int? BranchOfficeID { get; set; }

        // Navigation properties

        //public BranchOffice BranchOffice { get; set; }

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

        [Required]

        public string ResumePath { get; set; }

        public string? AcademicDetails { get; set; }

        public string? ProfessionalDetails { get; set; }

        public string? PreferredIndustry { get; set; }

        public string? PreferredSpecialization { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        // Navigation properties

        //public ICollection<Application> Applications { get; set; } = new List<Application>();

        //public User User { get; set; } // Navigation property to User

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

        // Navigation properties

        //public ICollection<Job> Jobs { get; set; } = new List<Job>();

        //public ICollection<Application> Applications { get; set; } = new List<Application>();

        //public User User { get; set; } // Navigation property to User

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

        // Navigation properties

        //public Employer Employer { get; set; }

        //public ICollection<Application> Applications { get; set; } = new List<Application>();

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

        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; }

        // Navigation properties

        //public Job Job { get; set; }

        //public JobSeeker JobSeeker { get; set; }
        //public Employer Employer { get; set; }

    }

}