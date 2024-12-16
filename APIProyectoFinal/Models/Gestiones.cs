namespace APIProyectoFinal.Models
{
    public class Gestion
    {
        public int ID_Gestion { get; set; }
        public int ID_Empleado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Tipo { get; set; } // Vacación, Permiso, Licencia
        public string Motivo { get; set; }
    }

}
