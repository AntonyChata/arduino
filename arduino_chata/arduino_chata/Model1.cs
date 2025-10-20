using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace arduino_chata
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<componentes> componentes { get; set; }
        public virtual DbSet<docentes> docentes { get; set; }
        public virtual DbSet<kits> kits { get; set; }
        public virtual DbSet<solicitudes> solicitudes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<solicitudes>()
                .Property(e => e.hora_entrada)
                .HasPrecision(0);

            modelBuilder.Entity<solicitudes>()
                .Property(e => e.hora_salida)
                .HasPrecision(0);
        }
    }
}
