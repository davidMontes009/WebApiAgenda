using System;
using System.Collections.Generic;
using System.Text;

namespace Agenda.Shared
{
    /// <summary>
    /// Entidad que representa un error en las validaciones de otra entidad
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Nombre de la propiedad en el cual se presenta el error
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Descripción del error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
