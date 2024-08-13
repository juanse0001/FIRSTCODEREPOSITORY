namespace BibliotecaWebApplication.Models
{
    public class Libro
    {
        public string LibroId { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public int NumeroPaginas { get; set; }

        public Libro() 
        {
            this.LibroId = Guid.NewGuid().ToString();
        }
    }
}
