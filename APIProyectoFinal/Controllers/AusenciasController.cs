using APIProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AusenciasController : ControllerBase
    {
        private readonly string connectionString = "Data Source=DESKTOP-8VM77JH\\UNIVERSIDAD;Initial Catalog=ProyectoFinal;Integrated Security=True;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetAusencias()
        {
            List<Ausencia> ausencias = new List<Ausencia>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spObtenerAusencias", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ausencias.Add(new Ausencia
                    {
                        ID_Ausencia = Convert.ToInt32(reader["ID_Ausencia"]),
                        ID_Empleado = Convert.ToInt32(reader["ID_Empleado"]),
                        FechaInicio = Convert.ToDateTime(reader["Fecha_Inicio"]),
                        FechaFin = Convert.ToDateTime(reader["Fecha_Fin"]),
                        TipoAusencia = reader["Tipo_Ausencia"].ToString(),
                        Motivo = reader["Motivo"].ToString()
                    });
                }
            }

            return Ok(ausencias);
        }

        [HttpPost]
        public IActionResult AddAusencia([FromBody] Ausencia ausencia)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAgregarAusencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", ausencia.ID_Empleado);
                cmd.Parameters.AddWithValue("@Fecha_Inicio", ausencia.FechaInicio);
                cmd.Parameters.AddWithValue("@Fecha_Fin", ausencia.FechaFin);
                cmd.Parameters.AddWithValue("@Tipo_Ausencia", ausencia.TipoAusencia);
                cmd.Parameters.AddWithValue("@Motivo", ausencia.Motivo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Ausencia agregada exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAusencia(int id, [FromBody] Ausencia ausencia)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spActualizarAusencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Ausencia", id);
                cmd.Parameters.AddWithValue("@Fecha_Fin", ausencia.FechaFin);
                cmd.Parameters.AddWithValue("@Motivo", ausencia.Motivo);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Ausencia no encontrada.");
            }
            return Ok("Ausencia actualizada exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAusencia(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spEliminarAusencia", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Ausencia", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Ausencia no encontrada.");
            }
            return Ok("Ausencia eliminada exitosamente.");
        }
    }

}
