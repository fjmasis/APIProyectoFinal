using APIProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionesController : ControllerBase
    {
        private readonly string connectionString = "Data Source=DESKTOP-8VM77JH\\UNIVERSIDAD;Initial Catalog=ProyectoFinal;Integrated Security=True;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetEvaluaciones()
        {
            List<Evaluacion> evaluaciones = new List<Evaluacion>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spObtenerEvaluaciones", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    evaluaciones.Add(new Evaluacion
                    {
                        ID_Evaluacion = Convert.ToInt32(reader["ID_Evaluacion"]),
                        ID_Empleado = Convert.ToInt32(reader["ID_Empleado"]),
                        FechaEvaluacion = Convert.ToDateTime(reader["Fecha_Evaluacion"]),
                        Comentarios = reader["Comentarios"].ToString(),
                        Puntuacion = Convert.ToInt32(reader["Puntuacion"])
                    });
                }
            }

            return Ok(evaluaciones);
        }

        [HttpPost]
        public IActionResult AddEvaluacion([FromBody] Evaluacion evaluacion)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAgregarEvaluacion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", evaluacion.ID_Empleado);
                cmd.Parameters.AddWithValue("@Fecha_Evaluacion", evaluacion.FechaEvaluacion);
                cmd.Parameters.AddWithValue("@Comentarios", evaluacion.Comentarios);
                cmd.Parameters.AddWithValue("@Puntuacion", evaluacion.Puntuacion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Evaluación agregada exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvaluacion(int id, [FromBody] Evaluacion evaluacion)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spActualizarEvaluacion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Evaluacion", id);
                cmd.Parameters.AddWithValue("@Comentarios", evaluacion.Comentarios);
                cmd.Parameters.AddWithValue("@Puntuacion", evaluacion.Puntuacion);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Evaluación no encontrada.");
            }
            return Ok("Evaluación actualizada exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvaluacion(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spEliminarEvaluacion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Evaluacion", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Evaluación no encontrada.");
            }
            return Ok("Evaluación eliminada exitosamente.");
        }
    }

}
