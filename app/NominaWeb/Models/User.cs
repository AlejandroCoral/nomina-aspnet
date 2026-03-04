using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class User
{
    public int EmpNo { get; set; }

    public string Usuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
