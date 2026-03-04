using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class Employee
{
    public int EmpNo { get; set; }

    public string Ci { get; set; } = null!;

    public string BirthDate { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string HireDate { get; set; } = null!;

    public string? Correo { get; set; }

    public virtual ICollection<DeptEmp> DeptEmps { get; set; } = new List<DeptEmp>();

    public virtual ICollection<DeptManager> DeptManagers { get; set; } = new List<DeptManager>();

    public virtual ICollection<LogAuditoriaSalario> LogAuditoriaSalarios { get; set; } = new List<LogAuditoriaSalario>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Title> Titles { get; set; } = new List<Title>();

    public virtual User? User { get; set; }
}
