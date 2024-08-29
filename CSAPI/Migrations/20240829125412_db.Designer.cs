﻿// <auto-generated />
using System;
using CSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CSAPI.Migrations
{
    [DbContext(typeof(CareerSolutionsDB))]
    [Migration("20240829125412_db")]
    partial class db
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CSAPI.Models.BranchOffice", b =>
                {
                    b.Property<int>("BranchOfficeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchOfficeID"), 1L, 1);

                    b.Property<string>("BranchAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("BranchOfficeID");

                    b.ToTable("BranchOffices");
                });

            modelBuilder.Entity("CSAPI.Models.Employer", b =>
                {
                    b.Property<int>("EmployerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployerID"), 1L, 1);

                    b.Property<string>("CompanyAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IndustryType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("WebsiteURL")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployerID");

                    b.ToTable("Employers");
                });

            modelBuilder.Entity("CSAPI.Models.Job", b =>
                {
                    b.Property<int>("JobID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobID"), 1L, 1);

                    b.Property<DateTime?>("ApplicationDeadline")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployerID")
                        .HasColumnType("int");

                    b.Property<string>("ExperienceLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndustryType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequiredSkills")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalaryRange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobID");

                    b.HasIndex("EmployerID");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("CSAPI.Models.JobApplication", b =>
                {
                    b.Property<int>("ApplicationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationID"), 1L, 1);

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("JobID")
                        .HasColumnType("int");

                    b.Property<int>("JobSeekerID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApplicationID");

                    b.HasIndex("JobID");

                    b.HasIndex("JobSeekerID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("CSAPI.Models.JobSeeker", b =>
                {
                    b.Property<int>("JobSeekerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobSeekerID"), 1L, 1);

                    b.Property<string>("AcademicDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ExpertField")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("KeySkills")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PreferredIndustry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferredSpecialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfessionalDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileSummary")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ResumePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("JobSeekerID");

                    b.ToTable("JobSeekers");
                });

            modelBuilder.Entity("CSAPI.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"), 1L, 1);

                    b.Property<int>("ApplicationID")
                        .HasColumnType("int");

                    b.Property<int>("EmployerID")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationID");

                    b.HasIndex("ApplicationID");

                    b.HasIndex("EmployerID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("CSAPI.Models.SkillRelation", b =>
                {
                    b.Property<string>("KeySkill")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubSkill")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SkillRelations");
                });

            modelBuilder.Entity("CSAPI.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BranchOfficeID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.HasIndex("BranchOfficeID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CSAPI.Models.Job", b =>
                {
                    b.HasOne("CSAPI.Models.Employer", null)
                        .WithMany()
                        .HasForeignKey("EmployerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CSAPI.Models.JobApplication", b =>
                {
                    b.HasOne("CSAPI.Models.Job", null)
                        .WithMany()
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CSAPI.Models.JobSeeker", null)
                        .WithMany()
                        .HasForeignKey("JobSeekerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CSAPI.Models.Notification", b =>
                {
                    b.HasOne("CSAPI.Models.JobApplication", null)
                        .WithMany()
                        .HasForeignKey("ApplicationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CSAPI.Models.Employer", null)
                        .WithMany()
                        .HasForeignKey("EmployerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CSAPI.Models.User", b =>
                {
                    b.HasOne("CSAPI.Models.BranchOffice", null)
                        .WithMany()
                        .HasForeignKey("BranchOfficeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CSAPI.Models.Employer", null)
                        .WithOne()
                        .HasForeignKey("CSAPI.Models.User", "UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CSAPI.Models.JobSeeker", null)
                        .WithOne()
                        .HasForeignKey("CSAPI.Models.User", "UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
