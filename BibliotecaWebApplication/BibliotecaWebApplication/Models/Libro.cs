using System.ComponentModel.DataAnnotations;

namespace BibliotecaWebApplication.Models
{
    public class Libro
    {
        [Key]
        public string LibroId { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public int NumeroPaginas { get; set; }

        public Libro() 
        {
            this.LibroId = Guid.NewGuid().ToString();
        }
        //Propiedades de navegacion 
         public ICollection<AutorLibro> LibroAutores { get; set; } = new List<AutorLibro>();
    }
}
