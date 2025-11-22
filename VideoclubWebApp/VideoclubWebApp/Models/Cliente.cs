using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoClubWebApp.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(11)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El No. de Tarjeta es obligatorio")]
        [Display(Name = "Tarjeta de Crédito")]
        [StringLength(16)]
        public string NoTarjetaCR { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        [Display(Name = "Límite de Crédito")]
        public decimal LimiteCredito { get; set; }

        [Required]
        [Display(Name = "Tipo de Persona")]
        public string TipoPersona { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";
    }
}