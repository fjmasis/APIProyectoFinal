namespace APIProyectoFinal.Models
{
    public class Asistencia
    {
        public int ID_Asistencia { get; set; } // Clave primaria
        public int ID_Empleado { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string Observaciones { get; set; }
    }

}
