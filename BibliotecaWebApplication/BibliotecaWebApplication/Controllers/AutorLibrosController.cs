using BibliotecaWebApplication.Data;
using BibliotecaWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class AutorLibrosController : Controller
{
    private readonly ApplicationDbContext _context;

    public AutorLibrosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: AutorLibros
    [Authorize(Roles = "Bibliotecario, Administrador")]
    public async Task<IActionResult> Index()
    {
        var autorLibros = await _context.AutorLibros
            .Include(a => a.Autor)
            .Include(a => a.Libro)
            .ToListAsync();
        return View(autorLibros);
    }

    // GET: AutorLibros/Details
    [Authorize(Roles = "Bibliotecario, Administrador")]
    public async Task<IActionResult> Details(Guid? autorId, Guid? libroId)
    {
        if (autorId == null || libroId == null)
        {
            return NotFound();
        }

        var autorLibro = await _context.AutorLibros
            .Include(a => a.Autor)
            .Include(a => a.Libro)
            .FirstOrDefaultAsync(m => m.AutorId == autorId && m.LibroId == libroId);

        if (autorLibro == null)
        {
            return NotFound();
        }

        return View(autorLibro);
    }

    // GET: AutorLibros/Create
    [Authorize(Roles = "Administrador")]
    public IActionResult Create()
    {
        ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "NombreCompleto");
        ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo");
        return View();
    }

    // POST: AutorLibros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create([Bind("AutorId,LibroId")] AutorLibro autorLibro)
    {
        if (ModelState.IsValid)
        {
            var existingRelation = await _context.AutorLibros
                .FirstOrDefaultAsync(al => al.AutorId == autorLibro.AutorId && al.LibroId == autorLibro.LibroId);

            if (existingRelation != null)
            {
                ModelState.AddModelError("", "Esta relación autor-libro ya existe.");
                ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "NombreCompleto", autorLibro.AutorId);
                ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
                return View(autorLibro);
            }

            _context.Add(autorLibro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "NombreCompleto", autorLibro.AutorId);
        ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
        return View(autorLibro);
    }

    // GET: AutorLibros/Edit
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(Guid? autorId, Guid? libroId)
    {
        if (autorId == null || libroId == null)
        {
            return NotFound();
        }

        var autorLibro = await _context.AutorLibros
            .FirstOrDefaultAsync(al => al.AutorId == autorId && al.LibroId == libroId);

        if (autorLibro == null)
        {
            return NotFound();
        }

        ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "NombreCompleto", autorLibro.AutorId);
        ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
        return View(autorLibro);
    }

    // POST: AutorLibros/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(Guid autorId, Guid libroId, [Bind("AutorId,LibroId")] AutorLibro autorLibro)
    {
        if (autorId != autorLibro.AutorId || libroId != autorLibro.LibroId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(autorLibro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorLibroExists(autorLibro.AutorId, autorLibro.LibroId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["AutorId"] = new SelectList(_context.Autores.ToList(), "AutorId", "NombreCompleto", autorLibro.AutorId);
        ViewData["LibroId"] = new SelectList(_context.Libros.ToList(), "LibroId", "Titulo", autorLibro.LibroId);
        return View(autorLibro);
    }

    // GET: AutorLibros/Delete
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(Guid? autorId, Guid? libroId)
    {
        if (autorId == null || libroId == null)
        {
            return NotFound();
        }

        var autorLibro = await _context.AutorLibros
            .Include(a => a.Autor)
            .Include(a => a.Libro)
            .FirstOrDefaultAsync(m => m.AutorId == autorId && m.LibroId == libroId);

        if (autorLibro == null)
        {
            return NotFound();
        }

        return View(autorLibro);
    }

    // POST: AutorLibros/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
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
