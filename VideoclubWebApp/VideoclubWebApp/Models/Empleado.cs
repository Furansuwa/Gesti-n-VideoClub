using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoClubWebApp.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(11)]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tanda de Labor")]
        [StringLength(50)]
        public string TandaLabor { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        [Display(Name = "% de Comisión")]
        public decimal PorcientoComision { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";
    }
}