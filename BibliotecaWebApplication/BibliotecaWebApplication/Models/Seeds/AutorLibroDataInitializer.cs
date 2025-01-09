using BibliotecaWebApplication.Data;

namespace BibliotecaWebApplication.Models.Seeds
{
    public static class AutorLibroDataInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Autores.Any() && !context.Libros.Any() && !context.AutorLibros.Any())
            {
                // Crear autores
                var autores = new List<Autor>
                {
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "García Márquez", Nombres = "Gabriel", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Mutis", Nombres = "Álvaro", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Vallejo", Nombres = "Fernando", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Zapata Olivella", Nombres = "Manuel", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Restrepo", Nombres = "Laura", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Henao", Nombres = "Juan Gabriel", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Bustos", Nombres = "Harold", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Echeverri", Nombres = "Pilar", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Santos", Nombres = "Jorge Enrique", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Ospina", Nombres = "William", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Rojas Herazo", Nombres = "Héctor", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Carrera Andrade", Nombres = "Jorge", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Acosta de Samper", Nombres = "Soledad", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Rengifo", Nombres = "Eduardo", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Zapata", Nombres = "Manuel", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Gamboa", Nombres = "Santiago", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Fajardo", Nombres = "Antonio", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Caicedo", Nombres = "Andrés", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Torres", Nombres = "Mario", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Gómez Jattin", Nombres = "Raúl", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Pombo", Nombres = "Rafael", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Mallorquín", Nombres = "Alejandra", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Sánchez Baute", Nombres = "Alberto", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Ordóñez", Nombres = "Germán", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Pardo", Nombres = "Eduardo", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Arenas", Nombres = "Jaime", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Vargas", Nombres = "José Luis", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Cobo Borda", Nombres = "Juan Gustavo", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Quintero", Nombres = "Margarita", Nacionalidad = "Colombiana" },
                    new Autor { AutorId = Guid.NewGuid(), Apellidos = "Arciniegas", Nombres = "Germán", Nacionalidad = "Colombiana" }
                };

                // Crear libros
                var libros = new List<Libro>
                {
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-84-376-0494-7", Titulo = "Cien años de soledad", NumeroPaginas = 471 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-84-376-0495-4", Titulo = "La hojarasca", NumeroPaginas = 182 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-8943-77-0", Titulo = "El amor en los tiempos del cólera", NumeroPaginas = 348 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-0891-8", Titulo = "Delirio", NumeroPaginas = 307 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-06-0093-4", Titulo = "La casa grande", NumeroPaginas = 159 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-8969-31-3", Titulo = "Satanás", NumeroPaginas = 224 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-06-0093-5", Titulo = "La rebelión de las ratas", NumeroPaginas = 193 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-42-5701-3", Titulo = "María", NumeroPaginas = 286 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-42-2874-6", Titulo = "Los ejércitos", NumeroPaginas = 150 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-545-622-1", Titulo = "El ruido de las cosas al caer", NumeroPaginas = 254 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-544-646-8", Titulo = "Rosario Tijeras", NumeroPaginas = 182 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-894-826-6", Titulo = "El olvido que seremos", NumeroPaginas = 366 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-891-625-8", Titulo = "La carroza de Bolívar", NumeroPaginas = 365 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-598-426-7", Titulo = "La novia oscura", NumeroPaginas = 422 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-8160-25-5", Titulo = "El desbarrancadero", NumeroPaginas = 199 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-3301-4", Titulo = "Paraíso Travel", NumeroPaginas = 315 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-4552-0", Titulo = "Que viva la música", NumeroPaginas = 223 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-8951-60-5", Titulo = "Cóndores no entierran todos los días", NumeroPaginas = 285 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-3835-5", Titulo = "Sin remedio", NumeroPaginas = 402 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-5503-1", Titulo = "El oro y la oscuridad", NumeroPaginas = 284 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-2874-5", Titulo = "La tejedora de coronas", NumeroPaginas = 364 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-5456-22-2", Titulo = "Los elegidos", NumeroPaginas = 269 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-4646-5", Titulo = "El general en su laberinto", NumeroPaginas = 285 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-45-1234-6", Titulo = "Los días azules", NumeroPaginas = 297 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-42-1234-5", Titulo = "Memorias de mis putas tristes", NumeroPaginas = 112 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-544-923-0", Titulo = "El coronel no tiene quien le escriba", NumeroPaginas = 90 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-42-4545-0", Titulo = "El otoño del patriarca", NumeroPaginas = 270 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-43-9898-0", Titulo = "Los funerales de la Mamá Grande", NumeroPaginas = 130 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-07-2978-6", Titulo = "La mala hora", NumeroPaginas = 233 },
                    new Libro { LibroId = Guid.NewGuid(), ISBN = "978-958-06-0087-1", Titulo = "Crónica de una muerte anunciada", NumeroPaginas = 124 }
                };

                // Agregar autores y libros al contexto
                context.Autores.AddRange(autores);
                context.Libros.AddRange(libros);

                // Crear relaciones AutorLibro
                var autorLibros = new List<AutorLibro>
                {
                    new AutorLibro { AutorId = autores[0].AutorId, LibroId = libros[0].LibroId },
                    new AutorLibro { AutorId = autores[0].AutorId, LibroId = libros[1].LibroId },
                    new AutorLibro { AutorId = autores[0].AutorId, LibroId = libros[2].LibroId },
                    new AutorLibro { AutorId = autores[1].AutorId, LibroId = libros[3].LibroId },
                    new AutorLibro { AutorId = autores[2].AutorId, LibroId = libros[4].LibroId },
                    new AutorLibro { AutorId = autores[3].AutorId, LibroId = libros[5].LibroId },
                    new AutorLibro { AutorId = autores[4].AutorId, LibroId = libros[6].LibroId },
                    new AutorLibro { AutorId = autores[5].AutorId, LibroId = libros[7].LibroId },
                    new AutorLibro { AutorId = autores[6].AutorId, LibroId = libros[8].LibroId },
                    new AutorLibro { AutorId = autores[7].AutorId, LibroId = libros[9].LibroId },
                    new AutorLibro { AutorId = autores[8].AutorId, LibroId = libros[10].LibroId },
                    new AutorLibro { AutorId = autores[9].AutorId, LibroId = libros[11].LibroId },
                    new AutorLibro { AutorId = autores[10].AutorId, LibroId = libros[12].LibroId },
                    new AutorLibro { AutorId = autores[11].AutorId, LibroId = libros[13].LibroId },
                    new AutorLibro { AutorId = autores[12].AutorId, LibroId = libros[14].LibroId },
                    new AutorLibro { AutorId = autores[13].AutorId, LibroId = libros[15].LibroId },
                    new AutorLibro { AutorId = autores[14].AutorId, LibroId = libros[16].LibroId },
                    new AutorLibro { AutorId = autores[15].AutorId, LibroId = libros[17].LibroId },
                    new AutorLibro { AutorId = autores[16].AutorId, LibroId = libros[18].LibroId },
                    new AutorLibro { AutorId = autores[17].AutorId, LibroId = libros[19].LibroId },
                    new AutorLibro { AutorId = autores[18].AutorId, LibroId = libros[20].LibroId },
                    new AutorLibro { AutorId = autores[19].AutorId, LibroId = libros[21].LibroId },
                    new AutorLibro { AutorId = autores[20].AutorId, LibroId = libros[22].LibroId },
                    new AutorLibro { AutorId = autores[21].AutorId, LibroId = libros[23].LibroId },
                    new AutorLibro { AutorId = autores[22].AutorId, LibroId = libros[24].LibroId },
                    new AutorLibro { AutorId = autores[23].AutorId, LibroId = libros[25].LibroId },
                    new AutorLibro { AutorId = autores[24].AutorId, LibroId = libros[26].LibroId },
                    new AutorLibro { AutorId = autores[25].AutorId, LibroId = libros[27].LibroId },
                    new AutorLibro { AutorId = autores[26].AutorId, LibroId = libros[28].LibroId },
                    new AutorLibro { AutorId = autores[27].AutorId, LibroId = libros[29].LibroId },
                };

                // Agregar relaciones AutorLibro al contexto
                context.AutorLibros.AddRange(autorLibros);

                // Guardar cambios en la base de datos
                await context.SaveChangesAsync();
            }
        }
    }
}
