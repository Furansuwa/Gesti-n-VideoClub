using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoClubWebApp.Models
{
    public class Rentas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Clave primaria auto-incremental

        [Required]
        public int NoRenta { get; set; } // Número de renta (lo calculamos manualmente)

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ArticuloId { get; set; }

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