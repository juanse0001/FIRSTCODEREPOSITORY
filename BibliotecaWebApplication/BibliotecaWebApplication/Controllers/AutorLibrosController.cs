using BibliotecaWebApplication.Data;
using BibliotecaWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaWebApplication.Controllers
{
    public class AutorLibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutorLibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Metodos utilizando ajax para optimizar las listas

        [HttpGet]
        public async Task<IActionResult> GetAutores()   
        {
            var autores = await _context.Autores
                .Select(a => new { a.AutorId, Nombre = $"{a.Nombres} {a.Apellidos}" })
                .ToListAsync();
            return Json(autores);
        }

        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            var libros = await _context.Libros
                .Select(l => new { l.LibroId, l.Titulo })
                .ToListAsync();
            return Json(libros);
        }

        // 1. Agregar una nueva relación entre autor y libro
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,LibroId")] AutorLibro autorLibro)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.AutorLibros
                    .AnyAsync(al => al.AutorId == autorLibro.AutorId && al.LibroId == autorLibro.LibroId);

                if (exists)
                {
                    ModelState.AddModelError("", "Esta relación ya existe.");
                    return View(autorLibro);
                }

                try
                {
                    _context.Add(autorLibro);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Relación creada con éxito.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error al crear la relación: " + ex.Message);
                }
            }

            return View(autorLibro);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid autorId, Guid libroId)
        {
            if (autorId == Guid.Empty || libroId == Guid.Empty)
            {
                return NotFound();
            }

            var autorLibro = await _context.AutorLibros
                .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

            if (autorLibro == null)
            {
                return NotFound();
            }

            // Obtener listas para los dropdowns usando SelectList
            ViewData["Autores"] = new SelectList(await _context.Autores
                .Select(a => new { a.AutorId, Nombre = $"{a.Nombres} {a.Apellidos}" })
                .ToListAsync(), "AutorId", "Nombre", autorLibro.AutorId); // Selecciona el valor actual

            ViewData["Libros"] = new SelectList(await _context.Libros
                .Select(l => new { l.LibroId, l.Titulo })
                .ToListAsync(), "LibroId", "Titulo", autorLibro.LibroId); // Selecciona el valor actual

            return View(autorLibro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid autorId, Guid libroId, AutorLibro autorLibro)
        {
            if (autorId != autorLibro.AutorId || libroId != autorLibro.LibroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Attach(autorLibro).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Relación actualizada con éxito.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorLibroExists(autorLibro.AutorId, autorLibro.LibroId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "El registro fue modificado por otro usuario.");
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la relación: " + ex.InnerException?.Message ?? ex.Message);
                }
            }

            // Recarga los datos en caso de error
            ViewBag.Autores = new SelectList(await _context.Autores
                .Select(a => new { a.AutorId, Nombre = $"{a.Nombres} {a.Apellidos}" })
                .ToListAsync(), "AutorId", "Nombre", autorLibro.AutorId);

            ViewBag.Libros = new SelectList(await _context.Libros
                .Select(l => new { l.LibroId, l.Titulo })
                .ToListAsync(), "LibroId", "Titulo", autorLibro.LibroId);

            return View(autorLibro);
        }



        // Acción para confirmar la eliminación
        public async Task<IActionResult> Delete(Guid autorId, Guid libroId)
        {
            var autorLibro = await _context.AutorLibros
                .Include(al => al.Autor)
                .Include(al => al.Libro)
                .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

            if (autorLibro == null)
            {
                return NotFound();
            }

            return View(autorLibro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid autorId, Guid libroId)
        {
            try
            {
                var autorLibro = await _context.AutorLibros
                    .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

                if (autorLibro != null)
                {
                    _context.AutorLibros.Remove(autorLibro);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Relación eliminada con éxito.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la relación: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        // 4. Ver los detalles de una relación
        public async Task<IActionResult> Details(Guid autorId, Guid libroId)
        {
            var autorLibro = await _context.AutorLibros
                .Include(al => al.Autor)
                .Include(al => al.Libro)
                .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

            if (autorLibro == null)
            {
                return NotFound();
            }

            return View(autorLibro);
        }

        // Index de todas las relaciones
        public async Task<IActionResult> Index()
        {
            var autorLibros = await _context.AutorLibros
                .Include(al => al.Autor)
                .Include(al => al.Libro)
                .ToListAsync();
            return View(autorLibros);
        }

        private async Task CargarDropdowns(AutorLibro autorLibro = null)
        {
            ViewData["AutorId"] = new SelectList(await _context.Autores.ToListAsync(), "AutorId", "Nombres", autorLibro?.AutorId);
            ViewData["LibroId"] = new SelectList(await _context.Libros.ToListAsync(), "LibroId", "Titulo", autorLibro?.LibroId);
        }

        private bool AutorLibroExists(Guid autorId, Guid libroId)
        {
            return _context.AutorLibros.Any(e => e.AutorId == autorId && e.LibroId == libroId);
        }
    }
}