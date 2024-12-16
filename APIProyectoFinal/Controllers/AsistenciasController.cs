using APIProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciasController : ControllerBase
    {
        private readonly string connectionString = "Data Source=DESKTOP-8VM77JH\\UNIVERSIDAD;Initial Catalog=ProyectoFinal;Integrated Security=True;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetAsistencias()
        {
            List<Asistencia> asistencias = new List<Asistencia>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spObtenerAsistencias", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    asistencias.Add(new Asistencia
                    {
                        ID_Asistencia = Convert.ToInt32(reader["ID_Asistencia"]),
                        ID_Empleado = Convert.ToInt32(reader["ID_Empleado"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        HoraEntrada = reader["Hora_Entrada"] as TimeSpan?,
                        HoraSalida = reader["Hora_Salida"] as TimeSpan?,
                        Estado = reader["Estado"].ToString(),
                        Observaciones = reader["Observaciones"].ToString()
                    });
                }
            }

            return Ok(asistencias);
        }

        [HttpPost]
        public IActionResult AddAsistencia([FromBody] Asistencia asistencia)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAgregarAsistencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", asistencia.ID_Empleado);
                cmd.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                cmd.Parameters.AddWithValue("@Hora_Entrada", asistencia.HoraEntrada);
                cmd.Parameters.AddWithValue("@Hora_Salida", asistencia.HoraSalida);
                cmd.Parameters.AddWithValue("@Estado", asistencia.Estado);
                cmd.Parameters.AddWithValue("@Observaciones", asistencia.Observaciones);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Asistencia agregada exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAsistencia(int id, [FromBody] Asistencia asistencia)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spActualizarAsistencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Asistencia", id);
                cmd.Parameters.AddWithValue("@Hora_Salida", asistencia.HoraSalida);
                cmd.Parameters.AddWithValue("@Estado", asistencia.Estado);
                cmd.Parameters.AddWithValue("@Observaciones", asistencia.Observaciones);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Asistencia no encontrada.");
            }
            return Ok("Asistencia actualizada exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAsistencia(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spEliminarAsistencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Asistencia", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Asistencia no encontrada.");
            }
            return Ok("Asistencia eliminada exitosamente.");
        }
    }


}
