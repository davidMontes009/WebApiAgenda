using AgendaBusisness.Contract;
using AgendaData.Contracts;
using AgendaEntities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaBusisness
{
    public class ContactBusisness: IContactBusisness
    {
        private readonly IConfiguration _configuration;
        private readonly IContactData _contactData;
        public ContactBusisness(IConfiguration configuration)
        {
            _configuration = configuration;
            _contactData = new ContactData(_configuration);
        }


        /// <summary>
        /// Metodo para registrar un contacto 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        public async Task<ContactDto> InsertContact(ContactDto contact)
            => await _contactData.InsertContact(contact);

        /// <summary>
        /// Metodo para actualizar un contacto 
        /// </summary>
        /// <param name="contact">Modelo con la info del contacto a actualizar </param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        public async Task<ContactDto> UpdateContact(ContactDto contact)
            => await _contactData.UpdateContact(contact);

        /// <summary>
        /// Metodo para eliminar un contacto 
        /// </summary>
        /// <param name="IdContact">Id del contacto a eliminar</param>
        /// <returns>Resultado de la accion</returns>
        public async Task<ContactDto> DeleteContact(int IdContact)
            => await _contactData.DeleteContact(IdContact);

        /// <summary>
        /// Metodo para seleccionar info de un solo contacto 
        /// </summary>
        /// <param name="IdContact">id del contacto</param>
        /// <returns>Resultado de la accion</returns>
        public async Task<ContactDto> SelectFristContact(int IdContact)
            => await _contactData.SelectFristContact(IdContact);

        /// <summary>
        /// Metodo para consultar todos los contactos 
        /// </summary>
        /// <returns>Lista de contactos</returns>
        public async Task<IList<ContactDto>> SelectAllContacts()
            => await _contactData.SelectAllContacts();

        /// <summary>
        /// Metodo para consulat los tipos de telefonos 
        /// </summary>
        /// <returns>retorna lista de tipos</returns>
        public async Task<IList<TipoTelefonoDto>> SelectAllTypePhone()
            => await _contactData.SelectAllTypePhone();
    }
}
