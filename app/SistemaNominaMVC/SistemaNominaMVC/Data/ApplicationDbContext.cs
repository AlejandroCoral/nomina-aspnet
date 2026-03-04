using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Models;

namespace SistemaNominaMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Log_AuditoriaSalarios> Log_AuditoriaSalarios { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Dept_Emp> Dept_Emps { get; set; }
        public DbSet<Dept_Manager> Dept_Managers { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}