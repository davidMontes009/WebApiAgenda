using System.ComponentModel.DataAnnotations;

namespace WebApiAgenda.Models
{
    /// <summary>
    /// Modelo que contiene las propiedades de un contacto 
    /// </summary>
    public class ContactModel
    {
        /// <summary>
        /// Identificador unico del contacto 
        /// </summary>
       [Required(ErrorMessage = "* Campo obligatorio")] 
        public int IdContact { get; set; }
        /// <summary>
        /// Nombre del contacto 
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellidos del contacto 
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Numero telefonico del contacto 
        /// </summary>
        [Required(ErrorMessage = "* Campo obligatorio")]
        public string Telefono { get; set; }
        /// <summary>
        /// Tipo de teléfono  
        /// </summary>
        public string TipoTel { get; set; }
        /// <summary>
        /// Correo electrónico del contacto 
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "El formato del corre es invalido")]
        public string email { get; set; }
    }
}
