namespace APIProyectoFinal.Models
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } // Admin, User, etc.
    }

}
