using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class Department
{
    public int DeptNo { get; set; }

    public string DeptName { get; set; } = null!;

    public virtual ICollection<DeptEmp> DeptEmps { get; set; } = new List<DeptEmp>();

    public virtual ICollection<DeptManager> DeptManagers { get; set; } = new List<DeptManager>();
}
