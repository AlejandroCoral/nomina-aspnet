using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [Table]
using Microsoft.EntityFrameworkCore;

namespace SistemaNominaMVC.Models
{
    [Table("dept_manager")] // Esto obliga a EF a buscar la tabla exacta de tu script
    [PrimaryKey(nameof(emp_no), nameof(dept_no))]
    public class Dept_Manager
    {
        public int emp_no { get; set; }
        public int dept_no { get; set; }

        [MaxLength(50)]
        public string from_date { get; set; }

        [MaxLength(50)]
        public string to_date { get; set; }
    }
}