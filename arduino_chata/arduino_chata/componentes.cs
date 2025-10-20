namespace arduino_chata
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class componentes
    {
        [Key]
        public int id_componente { get; set; }

        [StringLength(100)]
        public string categoria { get; set; }

        [StringLength(100)]
        public string nombre { get; set; }

        public int cantidad_total { get; set; }
    }
}
