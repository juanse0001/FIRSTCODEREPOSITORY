using Microsoft.AspNetCore.Mvc.Rendering; // Importante para SelectList
using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaWebApplication.Models.ViewModels // Asegúrate de que este namespace coincida
{
    public class AutorLibroViewModel
    {
        [Required]
        public Guid AutorId { get; set; }
        [Required]
        public Guid LibroId { get; set; }
        public SelectList Autores { get; set; }
        public SelectList Libros { get; set; }
        // Eliminar la propiedad Id que no se usa
    }
}