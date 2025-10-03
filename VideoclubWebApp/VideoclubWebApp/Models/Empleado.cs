namespace VideoClubWebApp.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string TandaLabor { get; set; }
        public decimal PorcientoComision { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Estado { get; set; }
    }
}
