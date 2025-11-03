using System;
using System.ComponentModel.DataAnnotations;

namespace VideoclubWebApp.Models
{
    public class Elenco
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public bool Estado { get; set; }

        // Colección para la relación muchos a muchos
        public virtual ICollection<ElencoArticulo> ElencoArticulos { get; set; }
    }
}