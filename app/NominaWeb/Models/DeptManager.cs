using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class DeptManager
{
    public int EmpNo { get; set; }

    public int DeptNo { get; set; }

    public string FromDate { get; set; } = null!;

    public string ToDate { get; set; } = null!;

    public virtual Department DeptNoNavigation { get; set; } = null!;

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
