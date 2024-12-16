using APIProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProyectoFinal.Database
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { } 

        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        public DbSet<Ausencia> Ausencia { get; set; }

        public DbSet <Evaluacion> evaluacions { get; set; }

        public DbSet<Gestion> Gestions { get; set; }

        public DbSet<Usuario>   Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>().HasKey(e => e.ID_Empleado);

        }

    }
}
