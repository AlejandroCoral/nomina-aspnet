using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <--- AGREGADO PARA MAPEO

namespace SistemaNominaMVC.Models
{
    [Table("users")] // <--- ESTO ES LO QUE SOLUCIONA EL ERROR
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Como no es IDENTITY en tu script
        public int emp_no { get; set; }

        [Required, MaxLength(100)]
        public string usuario { get; set; }

        [Required, MaxLength(100)]
        public string clave { get; set; }
    }
}