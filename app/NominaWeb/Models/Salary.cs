using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class Salary
{
    public int EmpNo { get; set; }

    public long Salary1 { get; set; }

    public string FromDate { get; set; } = null!;

    public string? ToDate { get; set; }

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
