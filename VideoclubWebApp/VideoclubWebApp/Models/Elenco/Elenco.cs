using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoclubWebApp.Models.Elenco
{
    [Table("Elenco")]
    public class Elenco
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        // Colección para la relación muchos a muchos
        public virtual ICollection<ElencoArticulo> ElencoArticulos { get; set; } = new List<ElencoArticulo>();
    }
}