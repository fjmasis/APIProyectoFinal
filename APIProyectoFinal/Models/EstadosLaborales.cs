namespace APIProyectoFinal.Models
{
    public class EstadoLaboral
    {
        public int ID_EstadoLaboral { get; set; }
        public int ID_Empleado { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
