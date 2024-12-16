namespace APIProyectoFinal.Models
{
    public class Ausencia
    {
        public int ID_Ausencia { get; set; }
        public int ID_Empleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoAusencia { get; set; }
        public string Motivo { get; set; }
    }

}
