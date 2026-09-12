using EMS_Core.Domain.Entities;
using EMS_Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMP_Infrastructure.SqlOperation
{
    public class EMSDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options)
            : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureEmployees(builder);
            ConfigureDepartment(builder);
            ConfigureDesignation(builder);
            ConfigureAttendance(builder);
            ConfigureLeave(builder);

        }
        public void ConfigureEmployees(ModelBuilder builder)
        {
            var emp = builder.Entity<Employee>();
            emp.HasKey(x => x.EmployeeId);
            emp.Property(x => x.EmployeeId).ValueGeneratedOnAdd();
            emp.Property(x => x.FullName).HasMaxLength(100);
            emp.Property(x => x.Email).HasMaxLength(200);
            emp.Property(x => x.Phone).HasMaxLength(15);
            emp.Property(x => x.Salary).HasColumnType("decimal(10,2)");
            emp.Property(x=>x.DateOfJoining).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            emp.HasIndex(x => x.Email).IsUnique();
            emp.HasIndex(x => x.DepartmentId);
            emp.HasIndex(x=>x.DesignationId);
            emp.HasOne(x => x.Department).WithMany(x => x.Employees).
                HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            emp.HasOne(x => x.Designation).WithMany(x => x.Employees).
            HasForeignKey(x => x.DesignationId).OnDelete(DeleteBehavior.Restrict);
        }
        public void ConfigureDesignation(ModelBuilder builder)
        {
            var des = builder.Entity<Designation>();
            des.HasKey(x => x.DesignationId);
            des.Property(x => x.DesignationId).ValueGeneratedOnAdd();
            des.Property(x => x.Title).HasMaxLength(100);
            des.HasIndex(x => x.DepartmentId);
            des.HasOne(x => x.Department).WithMany(x => x.Designations).
                HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        }
        public void ConfigureAttendance(ModelBuilder builder)
        {
            var att = builder.Entity<Attendance>();
            att.HasKey(x => x.AttendanceId);
            att.Property(x => x.AttendanceId).ValueGeneratedOnAdd();
            att.Property(x => x.Date).HasColumnType("date").HasDefaultValueSql("GetDate()");
            att.Property(x => x.Status).HasMaxLength(20).HasDefaultValue(AttendenceStatus.Present);
            att.Property(x => x.CheckIn).HasColumnType("time").HasDefaultValueSql("CAST(GETDATE() AS TIME)");
            att.Property(x => x.CheckOut).HasColumnType("time");
            att.HasIndex(x => x.EmployeeId);
            att.HasOne(x => x.Employee).WithMany(x => x.Attendances).
                HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
        public void ConfigureDepartment(ModelBuilder builder)
        {
            var dept = builder.Entity<Department>();
            dept.HasKey(x => x.DepartmentId);
            dept.Property(x => x.DepartmentId).ValueGeneratedOnAdd();
            dept.Property(x => x.Name).HasMaxLength(100);
        }
        public void ConfigureLeave(ModelBuilder builder)
        {
            var leave = builder.Entity<Leave>();
            leave.HasKey(x => x.LeaveId);
            leave.Property(x => x.LeaveId).ValueGeneratedOnAdd();
            leave.Property(x => x.FromDate).HasColumnType("date");
            leave.Property(x => x.ToDate).HasColumnType("date");
            leave.Property(x => x.Reason).HasMaxLength(200);
            leave.HasIndex(x => x.EmployeeId);
            leave.HasOne(x => x.Employee).WithMany(x => x.Leaves).
                HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
