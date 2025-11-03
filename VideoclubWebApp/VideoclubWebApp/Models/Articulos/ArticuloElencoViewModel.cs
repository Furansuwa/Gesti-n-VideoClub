using System.Collections.Generic;
using VideoclubWebApp.Models.Elenco;

namespace VideoclubWebApp.Models.Articulos
{
    // Este ViewModel es para las vistas Create y Edit de Artículos
    public class ArticuloElencoViewModel
    {
        // El artículo que estamos creando o editando
        public Articulo Articulo { get; set; }

        // La lista de TODOS los miembros del elenco disponibles
        public List<ElencoAsignadoViewModel> ElencosDisponibles { get; set; }

        public ArticuloElencoViewModel()
        {
            Articulo = new Articulo();
            ElencosDisponibles = new List<ElencoAsignadoViewModel>();
        }
    }
}