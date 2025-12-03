using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VideoclubWebApp.Models.Articulos; // Asegúrate de tener este using

namespace VideoClubWebApp.Models
{
    public class Rentas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoRenta { get; set; }

        [Required]
        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual Empleado? Empleado { get; set; } // <--- Nueva conexión

        [Required]
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; } // <--- Nueva conexión

        [Required]
        public int ArticuloId { get; set; }
        [ForeignKey("ArticuloId")]
        public virtual Articulo? Articulo { get; set; } // <--- Nueva conexión

        [Required]
        public DateTime FechaRenta { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoPorDia { get; set; }

        [Required]
        public int CantidadDias { get; set; }

        [StringLength(250)]
        public string Comentario { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activa";
    }
}