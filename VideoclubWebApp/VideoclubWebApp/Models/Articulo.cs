namespace VideoClubWebApp.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int TipoArticuloId { get; set; }
        public int IdiomaId { get; set; }
        public int GeneroId { get; set; }
        public decimal RentaPorDia { get; set; }
        public int DiasRenta { get; set; }
        public decimal MontoEntregaTardia { get; set; }
        public string Estado { get; set; }
    }
}
