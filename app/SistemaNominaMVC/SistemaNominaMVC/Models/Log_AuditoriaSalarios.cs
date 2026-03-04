using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaNominaMVC.Models
{
    [Table("Log_AuditoriaSalarios")] // <--- ESTO ES LO MÁS IMPORTANTE
    public class Log_AuditoriaSalarios
    {
        [Key]
        public int id { get; set; }

        public string usuario { get; set; }

        public DateTime fechaActualiz { get; set; } // En SQL es 'date', aquí DateTime

        public string DetalleCambio { get; set; }

        public long salario { get; set; }

        public int emp_no { get; set; }
    }
}