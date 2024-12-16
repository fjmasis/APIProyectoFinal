using APIProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestionesController : ControllerBase
    {
        private readonly string connectionString = "Data Source=DESKTOP-8VM77JH\\UNIVERSIDAD;Initial Catalog=ProyectoFinal;Integrated Security=True;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetGestiones()
        {
            List<Gestion> gestiones = new List<Gestion>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spObtenerGestiones", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    gestiones.Add(new Gestion
                    {
                        ID_Gestion = Convert.ToInt32(reader["ID_Gestion"]),
                        ID_Empleado = Convert.ToInt32(reader["ID_Empleado"]),
                        FechaInicio = Convert.ToDateTime(reader["Fecha_Inicio"]),
                        FechaFin = Convert.ToDateTime(reader["Fecha_Fin"]),
                        Tipo = reader["Tipo"].ToString(),
                        Motivo = reader["Motivo"].ToString()
                    });
                }
            }

            return Ok(gestiones);
        }

        [HttpPost]
        public IActionResult AddGestion([FromBody] Gestion gestion)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAgregarGestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", gestion.ID_Empleado);
                cmd.Parameters.AddWithValue("@Fecha_Inicio", gestion.FechaInicio);
                cmd.Parameters.AddWithValue("@Fecha_Fin", gestion.FechaFin);
                cmd.Parameters.AddWithValue("@Tipo", gestion.Tipo);
                cmd.Parameters.AddWithValue("@Motivo", gestion.Motivo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Gestión agregada exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGestion(int id, [FromBody] Gestion gestion)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spActualizarGestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Gestion", id);
                cmd.Parameters.AddWithValue("@Fecha_Fin", gestion.FechaFin);
                cmd.Parameters.AddWithValue("@Motivo", gestion.Motivo);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Gestión no encontrada.");
            }
            return Ok("Gestión actualizada exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGestion(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spEliminarGestion", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Gestion", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Gestión no encontrada.");
            }
            return Ok("Gestión eliminada exitosamente.");
        }
    }

}
