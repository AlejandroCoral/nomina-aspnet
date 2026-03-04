using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class Title
{
    public int EmpNo { get; set; }

    public string Title1 { get; set; } = null!;

    public string FromDate { get; set; } = null!;

    public string? ToDate { get; set; }

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
