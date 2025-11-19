namespace VideoclubWebApp.Models.Elenco
{
    public class ElencoAsignadoViewModel
    {
        public int ElencoId { get; set; }
        public string Nombre { get; set; }
        public bool Asignado { get; set; } // ¿Está en este artículo? (checkbox)
        public string? Rol { get; set; }    // ¿Qué rol tiene? (textbox)
    }
}