using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace arduino_chata.Models
{
    public class PrestamosContext : DbContext
    {
        public PrestamosContext() : base("PrestamosContext") { }

        public DbSet<Componente> Componentes { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<SolicitudComponente> SolicitudComponentes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Sin pluralización automática
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Tabla dbo.componentes
            modelBuilder.Entity<Componente>()
                .ToTable("componentes", "dbo")
                .HasKey(c => c.IdComponente);

            modelBuilder.Entity<Componente>()
                .Property(c => c.IdComponente).HasColumnName("id_componente");
            modelBuilder.Entity<Componente>()
                .Property(c => c.Categoria).HasColumnName("categoria").IsOptional().HasMaxLength(100);
            modelBuilder.Entity<Componente>()
                .Property(c => c.Nombre).HasColumnName("nombre").IsOptional().HasMaxLength(100);
            modelBuilder.Entity<Componente>()
                .Property(c => c.CantidadTotal).HasColumnName("cantidad_total");

            // Tabla dbo.docentes
            modelBuilder.Entity<Docente>()
                .ToTable("docentes", "dbo")
                .HasKey(d => d.IdDocente);
            modelBuilder.Entity<Docente>().Property(d => d.IdDocente).HasColumnName("id_docente");
            modelBuilder.Entity<Docente>().Property(d => d.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Docente>().Property(d => d.Curso).HasColumnName("curso").IsOptional().HasMaxLength(100);

            // Tabla dbo.kits
            modelBuilder.Entity<Kit>()
                .ToTable("kits", "dbo")
                .HasKey(k => k.IdKit);
            modelBuilder.Entity<Kit>().Property(k => k.IdKit).HasColumnName("id_kit");
            modelBuilder.Entity<Kit>().Property(k => k.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Kit>().Property(k => k.Estado).HasColumnName("estado").IsRequired().HasMaxLength(32);

            // Tabla dbo.solicitudes
            modelBuilder.Entity<Solicitud>()
                .ToTable("solicitudes", "dbo")
                .HasKey(s => s.IdSolicitud);
            modelBuilder.Entity<Solicitud>().Property(s => s.EstadoSolicitud).HasColumnName("estado_solicitud").HasMaxLength(16);

            modelBuilder.Entity<Solicitud>().Property(s => s.IdSolicitud).HasColumnName("id_solicitud");
            modelBuilder.Entity<Solicitud>().Property(s => s.IdDocente).HasColumnName("id_docente");
            modelBuilder.Entity<Solicitud>().Property(s => s.Semestre).HasColumnName("semestre").HasMaxLength(10);
            modelBuilder.Entity<Solicitud>().Property(s => s.TemaProyecto).HasColumnName("tema_proyecto").HasMaxLength(255);
            modelBuilder.Entity<Solicitud>().Property(s => s.Fecha).HasColumnName("fecha");
            modelBuilder.Entity<Solicitud>().Property(s => s.HoraEntrada).HasColumnName("hora_entrada");
            modelBuilder.Entity<Solicitud>().Property(s => s.HoraSalida).HasColumnName("hora_salida");
            modelBuilder.Entity<Solicitud>().Property(s => s.IdKit).HasColumnName("id_kit");
            modelBuilder.Entity<Solicitud>().Property(s => s.EstadoKit).HasColumnName("estado_kit").HasMaxLength(32);
            modelBuilder.Entity<Solicitud>().Property(s => s.PersonalSoporte).HasColumnName("personal_soporte").HasMaxLength(100);

            modelBuilder.Entity<Solicitud>()
                .HasOptional(s => s.Docente).WithMany().HasForeignKey(s => s.IdDocente);
            modelBuilder.Entity<Solicitud>()
                .HasOptional(s => s.Kit).WithMany().HasForeignKey(s => s.IdKit);

            // Tabla dbo.solicitud_componentes (sin PK -> definimos compuesta lógica)
            modelBuilder.Entity<SolicitudComponente>()
                .ToTable("solicitud_componentes", "dbo");

            modelBuilder.Entity<SolicitudComponente>()
                .HasKey(sc => new { sc.IdSolicitud, sc.IdComponente });

            modelBuilder.Entity<SolicitudComponente>().Property(sc => sc.IdSolicitud).HasColumnName("id_solicitud");
            modelBuilder.Entity<SolicitudComponente>().Property(sc => sc.IdComponente).HasColumnName("id_componente");
            modelBuilder.Entity<SolicitudComponente>().Property(sc => sc.Cantidad).HasColumnName("cantidad");

            modelBuilder.Entity<SolicitudComponente>()
                .HasRequired(sc => sc.Solicitud)
                .WithMany(s => s.Componentes)
                .HasForeignKey(sc => sc.IdSolicitud);

            modelBuilder.Entity<SolicitudComponente>()
                .HasRequired(sc => sc.Componente)
                .WithMany()
                .HasForeignKey(sc => sc.IdComponente);

            base.OnModelCreating(modelBuilder);
        }
    }
}
