namespace APIProyectoFinal.Models
{
    public class Evaluacion
    {
        public int ID_Evaluacion { get; set; }
        public int ID_Empleado { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public string Comentarios { get; set; }
        public int Puntuacion { get; set; }
    }

}
