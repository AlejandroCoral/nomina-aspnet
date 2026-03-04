using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaNominaMVC.Models
{
    [PrimaryKey(nameof(emp_no), nameof(title), nameof(from_date))]
    public class Title
    {
        public int emp_no { get; set; }

        [MaxLength(50)]
        public string title { get; set; }

        [MaxLength(50)]
        public string from_date { get; set; }

        [MaxLength(50)]
        public string? to_date { get; set; }
    }
}