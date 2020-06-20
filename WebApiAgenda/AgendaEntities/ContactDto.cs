namespace AgendaEntities
{
    public class ContactDto:BaseValidateBulkOperationDto
    {
        /// <summary>
        /// Identificador unico del contacto 
        /// </summary>
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
        public string Telefono { get; set; }
        /// <summary>
        /// Tipo de teléfono  
        /// </summary>
        public string TipoTel { get; set; }
        /// <summary>
        /// Correo electrónico del contacto 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// descripcion tipo telefono 
        /// </summary>
        public string nombreTipoTel { get; set; }
    }
}
