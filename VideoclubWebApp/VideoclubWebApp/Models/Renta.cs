namespace VideoClubWebApp.Models
{
    public class Rentas
    {
        public int Id { get; set; }
        public int NoRenta { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public int ArticuloId { get; set; }
        public DateTime FechaRenta { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public decimal MontoPorDia { get; set; }
        public int CantidadDias { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
    }
}

