using arduino_chata.Models;
using arduino_chata.Models.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace arduino_chata.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly PrestamosContext db = new PrestamosContext();

        // GET: Solicitudes
        public ActionResult Index()
        {
            var q = db.Solicitudes
                      .Include(s => s.Docente)
                      .Include(s => s.Kit)
                      .OrderByDescending(s => s.IdSolicitud)
                      .Take(100)
                      .ToList();
            return View(q);
        }

        // GET: Solicitudes/Create
        public ActionResult Create()
        {
            var vm = BuildCreateVM();
            return View(vm);
        }

        // POST: Solicitudes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudCreateVM vm)
        {
            if (vm.Componentes == null)
                vm.Componentes = new System.Collections.Generic.List<ComponenteCantidadVM>();

            if (!ModelState.IsValid)
            {
                // recargar combos y mapa de cursos
                vm.Docentes = db.Docentes.OrderBy(d => d.Nombre)
                    .Select(d => new SelectListItem { Value = d.IdDocente.ToString(), Text = d.Nombre }).ToList();
                vm.Kits = db.Kits.OrderBy(k => k.Nombre)
                    .Select(k => new SelectListItem { Value = k.IdKit.ToString(), Text = k.Nombre }).ToList();
                vm.EstadosKit = GetEstadosKit();

                ViewBag.DocenteCursoMap = db.Docentes
                    .Select(d => new { id = d.IdDocente, curso = d.Curso })
                    .ToList();

                return View(vm);
            }

            var entity = new Solicitud
            {
                IdDocente = vm.IdDocente,
                Semestre = vm.Semestre,
                TemaProyecto = vm.TemaProyecto,
                Fecha = vm.Fecha,
                HoraEntrada = vm.HoraEntrada,
                HoraSalida = vm.HoraSalida,
                IdKit = vm.IdKit,
                EstadoKit = vm.EstadoKit,
                PersonalSoporte = vm.PersonalSoporte,
                // ⬇️ NUEVO: estado inicial
                EstadoSolicitud = "Pendiente"
            };

            // Agregar componentes seleccionados con cantidad > 0
            var seleccion = vm.Componentes
                              .Where(c => c.Seleccionado && c.Cantidad > 0)
                              .ToList();

            foreach (var c in seleccion)
            {
                entity.Componentes.Add(new SolicitudComponente
                {
                    IdComponente = c.IdComponente,
                    Cantidad = c.Cantidad
                });
            }

            db.Solicitudes.Add(entity);
            db.SaveChanges();

            TempData["ok"] = "Solicitud registrada correctamente.";
            return RedirectToAction("Details", new { id = entity.IdSolicitud });
        }

        // POST: Solicitudes/Aprobar/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Aprobar(int id)
        {
            var s = db.Solicitudes.Find(id);
            if (s == null) return HttpNotFound();

            s.EstadoSolicitud = "Aprobada";
            db.SaveChanges();
            TempData["ok"] = $"Solicitud #{id} aprobada.";
            return RedirectToAction("Index");
        }

        // POST: Solicitudes/Rechazar/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Rechazar(int id)
        {
            var s = db.Solicitudes.Find(id);
            if (s == null) return HttpNotFound();

            s.EstadoSolicitud = "Rechazada";
            db.SaveChanges();
            TempData["ok"] = $"Solicitud #{id} rechazada.";
            return RedirectToAction("Index");
        }

        // GET: Renderiza la vista previa para el modal de eliminación
        [HttpGet]
        public ActionResult _DeletePreview(int id)
        {
            var s = db.Solicitudes
                      .Include(x => x.Docente)
                      .Include(x => x.Kit)
                      .Include(x => x.Componentes.Select(c => c.Componente))
                      .FirstOrDefault(x => x.IdSolicitud == id);
            if (s == null) return HttpNotFound();
            return PartialView("_DeletePreview", s);
        }

        // POST: Elimina definitivamente (desde el modal)
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var s = db.Solicitudes
                      .Include(x => x.Componentes)
                      .FirstOrDefault(x => x.IdSolicitud == id);

            if (s == null) return HttpNotFound();

            // eliminar hijos primero
            db.SolicitudComponentes.RemoveRange(s.Componentes);
            db.Solicitudes.Remove(s);
            db.SaveChanges();

            TempData["ok"] = $"Solicitud #{id} eliminada.";
            return RedirectToAction("Index");
        }

        // GET: Solicitudes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var s = db.Solicitudes
                      .Include(x => x.Docente)
                      .Include(x => x.Kit)
                      .Include(x => x.Componentes.Select(sc => sc.Componente))
                      .FirstOrDefault(x => x.IdSolicitud == id);

            if (s == null) return HttpNotFound();
            return View(s);
        }

        // Utilidades
        private SolicitudCreateVM BuildCreateVM()
        {
            var vm = new SolicitudCreateVM
            {
                Docentes = db.Docentes
                    .OrderBy(d => d.Nombre)
                    .Select(d => new SelectListItem { Value = d.IdDocente.ToString(), Text = d.Nombre })
                    .ToList(),
                Kits = db.Kits
                    .OrderBy(k => k.Nombre)
                    .Select(k => new SelectListItem { Value = k.IdKit.ToString(), Text = k.Nombre })
                    .ToList(),
                EstadosKit = GetEstadosKit(),
            };

            // Componentes disponibles
            vm.Componentes = db.Componentes
                               .OrderBy(c => c.Categoria).ThenBy(c => c.Nombre)
                               .Select(c => new ComponenteCantidadVM
                               {
                                   IdComponente = c.IdComponente,
                                   Categoria = c.Categoria,
                                   Nombre = c.Nombre,
                                   CantidadTotal = c.CantidadTotal,
                                   Seleccionado = false,
                                   Cantidad = 0
                               }).ToList();

            // Mapa IdDocente -> Curso (para autocompletar en la vista Create)
            ViewBag.DocenteCursoMap = db.Docentes
                .Select(d => new { id = d.IdDocente, curso = d.Curso })
                .ToList();

            return vm;
        }

        private static SelectList GetEstadosKit()
        {
            return new SelectList(new[]
            {
                new { Value = "Completo", Text = "Completo" },
                new { Value = "Solo elementos específicos", Text = "Solo elementos específicos" }
            }, "Value", "Text");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
