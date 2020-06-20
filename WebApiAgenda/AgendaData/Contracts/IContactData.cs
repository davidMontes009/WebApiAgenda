using AgendaEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaData.Contracts
{
    public interface IContactData
    {
        /// <summary>
        /// Metodo para registrar un contacto 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        Task<ContactDto> InsertContact(ContactDto contact);
        /// <summary>
        /// Metodo para actualizar un contacto 
        /// </summary>
        /// <param name="contact">Modelo con la info del contacto a actualizar </param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        Task<ContactDto> UpdateContact(ContactDto contact);
        /// <summary>
        /// Metodo para eliminar un contacto 
        /// </summary>
        /// <param name="IdContact">Id del contacto a eliminar</param>
        /// <returns>Resultado de la accion</returns>
        Task<ContactDto> DeleteContact(int IdContact);
        /// <summary>
        /// Metodo para seleccionar info de un solo contacto 
        /// </summary>
        /// <param name="IdContact">id del contacto</param>
        /// <returns>Resultado de la accion</returns>
        Task<ContactDto> SelectFristContact(int IdContact);
        /// <summary>
        /// Metodo para consultar todos los contactos 
        /// </summary>
        /// <returns>Lista de contactos</returns>
        Task<IList<ContactDto>> SelectAllContacts();
        /// <summary>
        /// Metodo para consulat los tipos de telefonos 
        /// </summary>
        /// <returns>retorna lista de tipos</returns>
        Task<IList<TipoTelefonoDto>> SelectAllTypePhone();
    }
}
