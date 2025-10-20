using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace arduino_chata.Models.ViewModels
{
    public class ComponenteCantidadVM
    {
        public int IdComponente { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public int CantidadTotal { get; set; }

        // Lo que selecciona el usuario
        public bool Seleccionado { get; set; }
        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; }
    }

    public class SolicitudCreateVM
    {
        // Datos de cabecera
        public int? IdDocente { get; set; }
        [StringLength(10)]
        public string Semestre { get; set; }
        [StringLength(255)]
        public string TemaProyecto { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? HoraEntrada { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? HoraSalida { get; set; }
        public int? IdKit { get; set; }
        [StringLength(32)]
        public string EstadoKit { get; set; }
        [StringLength(100)]
        public string PersonalSoporte { get; set; }

        // Listas para combos
        public IEnumerable<SelectListItem> Docentes { get; set; }
        public IEnumerable<SelectListItem> Kits { get; set; }
        public IEnumerable<SelectListItem> EstadosKit { get; set; }

        // Componentes (checkbox + cantidad)
        public List<ComponenteCantidadVM> Componentes { get; set; } = new List<ComponenteCantidadVM>();
    }
}
