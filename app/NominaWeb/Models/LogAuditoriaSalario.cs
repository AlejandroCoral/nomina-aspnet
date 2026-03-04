using System;
using System.Collections.Generic;

namespace NominaWeb.Models;

public partial class LogAuditoriaSalario
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public DateOnly FechaActualiz { get; set; }

    public string DetalleCambio { get; set; } = null!;

    public long Salario { get; set; }

    public int EmpNo { get; set; }

    public virtual Employee EmpNoNavigation { get; set; } = null!;
}
