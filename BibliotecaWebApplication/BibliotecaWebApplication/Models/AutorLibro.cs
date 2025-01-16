using System.ComponentModel.DataAnnotations;

namespace BibliotecaWebApplication.Models
{
    public class AutorLibro
    {
        [Required]
        public Guid AutorId { get; set; } 

        public Autor? Autor { get; set; }

        [Required]
        public Guid LibroId { get; set; }

        public Libro? Libro { get; set; }
    }
}
