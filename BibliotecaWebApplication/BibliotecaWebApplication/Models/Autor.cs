namespace BibliotecaWebApplication.Models
{
    public class Autor
    {
        public Guid AutorId { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Nacionalidad { get; set; }
        public Autor()
        {
            this.AutorId = Guid.NewGuid();
        } 
    }
    //Propiedades de navegacion
}
