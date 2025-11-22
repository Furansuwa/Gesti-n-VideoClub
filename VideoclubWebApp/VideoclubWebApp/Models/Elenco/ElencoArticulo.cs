using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VideoclubWebApp.Models.Articulos;

namespace VideoclubWebApp.Models.Elenco
{
    [Table("ElencoArticulo")]
    public class ElencoArticulo
    {
        public int ArticuloId { get; set; }
        public int ElencoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Rol { get; set; }

        // Propiedades de navegación (para que EF Core entienda la relación)
        [ForeignKey("ArticuloId")]
        public virtual Articulo Articulo { get; set; }

        [ForeignKey("ElencoId")]
        public virtual Elenco Elenco { get; set; }
    }
}