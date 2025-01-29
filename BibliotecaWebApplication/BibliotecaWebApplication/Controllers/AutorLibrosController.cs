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

        // GET: AutorLibro/Edit/5
        public async Task<IActionResult> Edit(Guid? AutorId, Guid? LibroId)
        {
            if (AutorId == null || LibroId == null)
            {
                TempData["ErrorMessage"] = "IDs de Autor o Libro inválidos."; // Mensaje más descriptivo
                return RedirectToAction(nameof(Index)); // Redirige al Index en caso de error
            }

            var autorLibro = await _context.AutorLibros
                .Include(al => al.Autor) // Incluir información del Autor
                .Include(al => al.Libro) // Incluir información del Libro
                .FirstOrDefaultAsync(m => m.AutorId == AutorId && m.LibroId == LibroId);

            if (autorLibro == null)
            {
                TempData["ErrorMessage"] = "No se encontró la relación Autor-Libro.";
                return RedirectToAction(nameof(Index)); // Redirige al Index si no se encuentra
                                                        // Alternativa: return NotFound();
            }

            return View(autorLibro);
        }

        // POST: AutorLibro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid AutorIdOriginal, Guid LibroIdOriginal, [Bind("AutorId,LibroId")] AutorLibro autorLibro)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, verifique los datos ingresados.";
                return View(autorLibro);
            }

            try
            {
                // 1. Verificar si la relación original existe
                var existingRelacion = await _context.AutorLibros
                    .FirstOrDefaultAsync(al => al.AutorId == AutorIdOriginal && al.LibroId == LibroIdOriginal);

                if (existingRelacion == null)
                {
                    TempData["ErrorMessage"] = "No se encontró la relación Autor-Libro original.";
                    return RedirectToAction(nameof(Index));
                }

                // 2. Verificar si los valores nuevos son diferentes
                if (AutorIdOriginal == autorLibro.AutorId && LibroIdOriginal == autorLibro.LibroId)
                {
                    TempData["InfoMessage"] = "No se detectaron cambios en la relación.";
                    return RedirectToAction(nameof(Index));
                }

                // 3. Verificar si la nueva relación ya existe para evitar duplicados
                var relacionDuplicada = await _context.AutorLibros
                    .AnyAsync(al => al.AutorId == autorLibro.AutorId && al.LibroId == autorLibro.LibroId);

                if (relacionDuplicada)
                {
                    TempData["ErrorMessage"] = "Ya existe una relación con los valores seleccionados.";
                    return View(autorLibro);
                }

                // 4. Eliminar la relación existente
                _context.AutorLibros.Remove(existingRelacion);

                // 5. Crear una nueva relación con los valores actualizados
                var nuevaRelacion = new AutorLibro
                {
                    AutorId = autorLibro.AutorId,
                    LibroId = autorLibro.LibroId
                };
                _context.AutorLibros.Add(nuevaRelacion);

                // 6. Guardar los cambios
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Relación Autor-Libro actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Error al guardar en la base de datos: " + ex.Message;
                return View(autorLibro);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                return View(autorLibro);
            }
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