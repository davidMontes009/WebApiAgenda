using System;
using System.Collections.Generic;
using System.Text;

namespace AgendaEntities
{
    /// <summary>
    /// Modelo para recuperar los tipos de telefono 
    /// </summary>
    public class TipoTelefonoDto
    {
        /// <summary>
        /// Identificador unico
        /// </summary>
        public int idTipoTel { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string nombre { get; set; }
    }
}
