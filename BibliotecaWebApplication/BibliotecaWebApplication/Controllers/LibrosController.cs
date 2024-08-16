using BibliotecaWebApplication.Data;
using BibliotecaWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibliotecaWebApplication.Models;

namespace BibliotecaWebApplication.Controllers
{
    public class LibroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibroController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Libros.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var libro = await _context.Libros
                .Include(l => l.LibroAutores)
                .ThenInclude(a => a.Autor)
                .FirstOrDefaultAsync(m => m.LibroId == id);


            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            ViewBag.AutorSelectList = new SelectList(_context.Autores, "AutorId", "Nombre");
            return View();
        }

        // POST: Libro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,Titulo,NumeroPaginas,AutorIds")] Libro libro, Guid[] autorIds)
        {
            if (ModelState.IsValid)
            {
                libro.LibroId = Guid.NewGuid();
                _context.Add(libro);

                // Agregar autores al libro
                foreach (var autorId in autorIds)
                {
                    _context.Add(new AutorLibro { LibroId = libro.LibroId, AutorId = autorId });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AutoresSelectList = new SelectList(_context.Autores, "AutorId", "Nombre");
            return View(libro);
        }

        private bool LibroExists(Guid id)
        {
            return _context.Libros.Any(e => e.LibroId == id);
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.LibroAutores)
                .ThenInclude(la => la.Autor)
                .FirstOrDefaultAsync(m => m.LibroId == id);

            if (libro == null)
            {
                return NotFound();
            }

            ViewBag.AutoresSelectList = new SelectList(_context.Autores, "AutorId", "Nombre", libro.LibroAutores.Select(la => la.AutorId));
            return View(libro);
        }


        // POST: Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LibroId,ISBN,Titulo,NumeroPaginas,AutorIds")] Libro libro, Guid[] autorIds)
        {
            if (id != libro.LibroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar el libro
                    _context.Update(libro);

                    // Remover autores antiguos
                    var autoresExistentes = _context.AutorLibros.Where(al => al.LibroId == libro.LibroId).ToList();
                    _context.AutorLibros.RemoveRange(autoresExistentes);

                    // Agregar autores nuevos
                    foreach (var autorId in autorIds)
                    {
                        _context.AutorLibros.Add(new AutorLibro { LibroId = libro.LibroId, AutorId = autorId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.LibroId))
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
            ViewBag.AutoresSelectList = new SelectList(_context.Autores, "AutorId", "Nombre", autorIds);
            return View(libro);
        }
    }
}