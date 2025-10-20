using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace arduino_chata.Models
{
    public class Componente
    {
        public int IdComponente { get; set; }
        [StringLength(100)]
        public string Categoria { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public int CantidadTotal { get; set; }
    }

    public class Docente
    {
        public int IdDocente { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(100)]
        public string Curso { get; set; }
    }

    public class Kit
    {
        public int IdKit { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
        [Required, StringLength(32)]
        public string Estado { get; set; } // 'Completo' | 'Solo elementos específicos'
    }

    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public int? IdDocente { get; set; }
        [StringLength(10)]
        public string Semestre { get; set; }
        [StringLength(255)]
        public string TemaProyecto { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public int? IdKit { get; set; }
        [StringLength(32)]
        public string EstadoKit { get; set; }
        [StringLength(100)]
        public string PersonalSoporte { get; set; }
        public string EstadoSolicitud { get; set; }
        public virtual Docente Docente { get; set; }
        public virtual Kit Kit { get; set; }
        public virtual ICollection<SolicitudComponente> Componentes { get; set; } = new List<SolicitudComponente>();
    }

    public class SolicitudComponente
    {
        public int IdSolicitud { get; set; }
        public int IdComponente { get; set; }
        public int? Cantidad { get; set; }

        public virtual Solicitud Solicitud { get; set; }
        public virtual Componente Componente { get; set; }
    }
}
