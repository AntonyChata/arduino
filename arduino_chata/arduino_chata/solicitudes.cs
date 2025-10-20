namespace arduino_chata
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class solicitudes
    {
        [Key]
        public int id_solicitud { get; set; }

        public int? id_docente { get; set; }

        [StringLength(10)]
        public string semestre { get; set; }

        [StringLength(255)]
        public string tema_proyecto { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public TimeSpan? hora_entrada { get; set; }

        public TimeSpan? hora_salida { get; set; }

        public int? id_kit { get; set; }

        [StringLength(32)]
        public string estado_kit { get; set; }

        [StringLength(100)]
        public string personal_soporte { get; set; }

        public virtual docentes docentes { get; set; }

        public virtual kits kits { get; set; }
    }
}
