using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoClubWebApp.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; } // Identificador

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } // Nombre

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(11)]
        public string Cedula { get; set; } // Cedula

        [Required]
        [Display(Name = "Tanda de Labor")]
        [StringLength(50)]
        public string TandaLabor { get; set; } // Tanda labor

        [Required]
        [Column(TypeName = "decimal(5, 2)")] // ej: 10.50%
        [Display(Name = "% de Comisión")]
        public decimal PorcientoComision { get; set; } // Porciento Comision

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; } // Fecha Ingreso

        public bool Estado { get; set; } // Estado
    }
}