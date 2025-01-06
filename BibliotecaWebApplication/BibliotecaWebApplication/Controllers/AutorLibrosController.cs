using BibliotecaWebApplication.Data;
using BibliotecaWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaWebApplication.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AutorLibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutorLibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AutorLibros/Index
        public async Task<IActionResult> Index()
        {
            var autorLibros = await _context.AutorLibros
                .Include(al => al.Autor)
                .Include(al => al.Libro)
                .ToListAsync();
            return View(autorLibros);
        }

        // GET: AutorLibros/Details/5
        public async Task<IActionResult> Details(Guid autorId, Guid libroId)
        {
            if (autorId == null || libroId == null)
            {
                return NotFound();
            }

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

        // GET: AutorLibros/Create
        public async Task <IActionResult> Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "Nombres");
            ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo");
            return View();
        }

        // POST: AutorLibros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,LibroId")] AutorLibro autorLibro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar si la relación ya existe con las llaves compuestas. 
                    var existingRelation = await _context.AutorLibros
                        .AnyAsync(al => al.AutorId == autorLibro.AutorId && al.LibroId == autorLibro.LibroId);

                    if (existingRelation)
                    {
                        TempData["ErrorMessage"] = "La relación entre el autor y el libro ya existe.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Si no existe la relación, agregarla a la base de datos. 
                    _context.Add(autorLibro);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Relación creada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "Nombres", autorLibro.AutorId);
                ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Error de base de datos: {ex.InnerException?.Message ?? ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
            }

            return View(autorLibro);
        }




        // GET: AutorLibros/Edit
        public async Task<IActionResult> Edit(Guid autorId, Guid libroId)
        {
            var autorLibro = await _context.AutorLibros
                .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

            if (autorLibro == null)
            {
                return NotFound();
            }

            ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "Nombres", autorLibro.AutorId);
            ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
            return View(autorLibro);
        }


        // POST: AutorLibros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid autorId, Guid libroId, [Bind("AutorId,LibroId")] AutorLibro autorLibro)
        {
            if (autorId != autorLibro.AutorId || libroId != autorLibro.LibroId)
            {
                // Si las claves originales no coinciden con los valores enviados, retorna un error.
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Paso 1: Buscar la relación original en la base de datos.
                var existingRelation = await _context.AutorLibros
                    .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

                if (existingRelation != null)
                {
                    // Paso 2: Eliminar la relación original.
                    _context.AutorLibros.Remove(existingRelation);
                }

                // Paso 3: Crear una nueva relación con los datos actualizados.
                _context.AutorLibros.Add(autorLibro);

                // Paso 4: Guardar los cambios.
                await _context.SaveChangesAsync();

                // Redirigir al índice después de completar la edición.
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, recargar las listas desplegables y volver a mostrar la vista.
            ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "Nombres", autorLibro.AutorId);
            ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
            return View(autorLibro);
        }



        // GET: AutorLibros/Delete/5
        public async Task<IActionResult> Delete(Guid autorId, Guid libroId)
        {
            if (autorId == null || libroId == null)
            {
                return NotFound();
            }

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

        // POST: AutorLibros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid autorId, Guid libroId)
        {
            var autorLibro = await _context.AutorLibros
                .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);
            if (autorLibro != null)
            {
                _context.AutorLibros.Remove(autorLibro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AutorLibroExists(Guid autorId, Guid libroId)
        {
            return _context.AutorLibros.Any(e => e.AutorId == autorId && e.LibroId == libroId);
        }
    }
}
