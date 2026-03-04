using System.ComponentModel.DataAnnotations;

namespace SistemaNominaMVC.Models
{
    public class Employee
    {
        [Key]
        public int emp_no { get; set; }

        [Required, MaxLength(50)]
        public string ci { get; set; }

        [Required, MaxLength(50)]
        public string birth_date { get; set; }

        [Required, MaxLength(50)]
        public string first_name { get; set; }

        [Required, MaxLength(50)]
        public string last_name { get; set; }

        [Required, MaxLength(1)]
        public string gender { get; set; }

        [Required, MaxLength(50)]
        public string hire_date { get; set; }

        [MaxLength(100)]
        public string correo { get; set; }
    }
}