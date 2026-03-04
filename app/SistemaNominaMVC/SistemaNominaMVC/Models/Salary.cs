using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaNominaMVC.Models
{
    [PrimaryKey(nameof(emp_no), nameof(from_date))]
    public class Salary
    {
        public int emp_no { get; set; }
        public long salary { get; set; }

        [MaxLength(50)]
        public string from_date { get; set; }

        [MaxLength(50)]
        public string? to_date { get; set; }
    }
}