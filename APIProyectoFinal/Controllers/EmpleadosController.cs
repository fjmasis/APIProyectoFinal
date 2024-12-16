using APIProyectoFinal.Database;
using APIProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APIProyectoFinal.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly string connectionString = "Data Source=DESKTOP-8VM77JH\\UNIVERSIDAD;Initial Catalog=ProyectoFinal;Integrated Security=True;TrustServerCertificate=True;";


        [HttpGet]
        public IActionResult GetEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spObtenerEmpleados", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empleados.Add(new Empleado
                    {
                        ID_Empleado = Convert.ToInt32(reader["ID_Empleado"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Email = reader["Email"].ToString(),
                        Cargo = reader["Cargo"].ToString(),
                        Departamento = reader["Departamento"].ToString(),
                        Salario = Convert.ToDecimal(reader["Salario"]),
                        Estado = reader["Estado"].ToString(),
                        FechaNacimiento = reader["Fecha_Nacimiento"] as DateTime?,
                        FechaIngreso = reader["Fecha_Ingreso"] as DateTime?
                    });
                }
            }

            return Ok(empleados);
        }

        [HttpPost]
        public IActionResult AddEmpleado([FromBody] Empleado empleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAgregarEmpleado", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Agrega todos los parámetros requeridos por el procedimiento almacenado
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", empleado.Telefono);
                cmd.Parameters.AddWithValue("@Email", empleado.Email);
                cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                cmd.Parameters.AddWithValue("@Departamento", empleado.Departamento);
                cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                cmd.Parameters.AddWithValue("@Estado", empleado.Estado); // Parámetro agregado
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", empleado.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Fecha_Ingreso", empleado.FechaIngreso);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Empleado agregado exitosamente.");
        }

        // PUT: Actualizar un empleado
        [HttpPut("{id}")]
        public IActionResult UpdateEmpleado(int id, [FromBody] Empleado empleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spActualizarEmpleado", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", id);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", empleado.Telefono);
                cmd.Parameters.AddWithValue("@Email", empleado.Email);
                cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                cmd.Parameters.AddWithValue("@Departamento", empleado.Departamento);
                cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                cmd.Parameters.AddWithValue("@Estado", empleado.Estado);
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", empleado.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Fecha_Ingreso", empleado.FechaIngreso);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Empleado no encontrado.");
            }
            return Ok("Empleado actualizado exitosamente.");
        }

        // DELETE: Eliminar un empleado
        [HttpDelete("{id}")]
        public IActionResult DeleteEmpleado(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spEliminarEmpleado", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Empleado", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound("Empleado no encontrado.");
            }
            return Ok("Empleado eliminado exitosamente.");
        }
    }
}
