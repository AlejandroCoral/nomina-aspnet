using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NominaWeb.Models;

namespace NominaWeb.Data;

public partial class NominaDbContext : DbContext
{
    public NominaDbContext()
    {
    }

    public NominaDbContext(DbContextOptions<NominaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DeptEmp> DeptEmps { get; set; }

    public virtual DbSet<DeptManager> DeptManagers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LogAuditoriaSalario> LogAuditoriaSalarios { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-O61A6S3;Database=nomina_db;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptNo).HasName("PK__departme__DCA63FA6156C7484");

            entity.ToTable("departments");

            entity.Property(e => e.DeptNo)
                .ValueGeneratedNever()
                .HasColumnName("dept_no");
            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dept_name");
        });

        modelBuilder.Entity<DeptEmp>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.DeptNo }).HasName("PK__dept_emp__6F52330022561878");

            entity.ToTable("dept_emp");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.DeptNo).HasColumnName("dept_no");
            entity.Property(e => e.FromDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("to_date");

            entity.HasOne(d => d.DeptNoNavigation).WithMany(p => p.DeptEmps)
                .HasForeignKey(d => d.DeptNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dept_emp__dept_n__3C69FB99");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.DeptEmps)
                .HasForeignKey(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dept_emp__emp_no__3B75D760");
        });

        modelBuilder.Entity<DeptManager>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.DeptNo }).HasName("PK__dept_man__6F52330057BC2543");

            entity.ToTable("dept_manager");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.DeptNo).HasColumnName("dept_no");
            entity.Property(e => e.FromDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("to_date");

            entity.HasOne(d => d.DeptNoNavigation).WithMany(p => p.DeptManagers)
                .HasForeignKey(d => d.DeptNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dept_mana__dept___403A8C7D");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.DeptManagers)
                .HasForeignKey(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__dept_mana__emp_n__3F466844");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpNo).HasName("PK__employee__129850FAA0557938");

            entity.ToTable("employees");

            entity.Property(e => e.EmpNo)
                .ValueGeneratedNever()
                .HasColumnName("emp_no");
            entity.Property(e => e.BirthDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("birth_date");
            entity.Property(e => e.Ci)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ci");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.HireDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<LogAuditoriaSalario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log_Audi__3213E83F340B9C21");

            entity.ToTable("Log_AuditoriaSalarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DetalleCambio)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.FechaActualiz).HasColumnName("fechaActualiz");
            entity.Property(e => e.Salario).HasColumnName("salario");
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.LogAuditoriaSalarios)
                .HasForeignKey(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log_Audit__emp_n__4BAC3F29");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.FromDate }).HasName("PK__salaries__BF7C09537EA741E4");

            entity.ToTable("salaries");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.FromDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.Salary1).HasColumnName("salary");
            entity.Property(e => e.ToDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("to_date");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__salaries__emp_no__45F365D3");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.Title1, e.FromDate }).HasName("PK__titles__B614B4DB5CF1C1D9");

            entity.ToTable("titles");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.Title1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.FromDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("to_date");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.Titles)
                .HasForeignKey(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__titles__emp_no__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.EmpNo).HasName("PK__users__129850FA05DFFEFD");

            entity.ToTable("users");

            entity.Property(e => e.EmpNo)
                .ValueGeneratedNever()
                .HasColumnName("emp_no");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Usuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.EmpNoNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.EmpNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__emp_no__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
