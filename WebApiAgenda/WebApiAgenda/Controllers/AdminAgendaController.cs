using AgendaBusisness;
using AgendaBusisness.Contract;
using AgendaEntities;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiAgenda.Models;

namespace WebApiAgenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAgendaController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminAgendaController> _logger;
        private readonly IMapper _mapper;
        private readonly IContactBusisness _contactBusisness;
        public  AdminAgendaController(IConfiguration configuration,ILogger<AdminAgendaController> logger,IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _contactBusisness = new ContactBusisness(_configuration);
        }


        /// <summary>
        /// Servicio para registrar un nuevo cotacto 
        /// </summary>
        /// <param name="contact"> Modelo con la información del contacto  </param>
        /// <returns>Mensaje y estatus de la acción </returns>  
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST    
        ///     {
        ///     "nombre": "david",
        ///     "apellidos": "Montes Rodriguez",
        ///     "telefono": "5534443491",
        ///     "tipoTel": 3,
        ///     "email": "davidmr_1312@hotmail.com"
        ///     }
        ///     
        /// Sample Response
        /// 
        ///     { 
        ///     "message": "El contacto se registró con éxito"
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("InsertCotact")]
        public async Task<IActionResult> InsertCotact(ContactModel contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactDto result = await _contactBusisness.InsertContact(_mapper.Map<ContactModel, ContactDto>(contact));
                    if (result.Validations.Count > 0)
                        return Ok(new { status = false, message = result.Validations[0].ErrorMessage.ToString() });

                    return Ok(new { status = false , message = "El contacto se registró con éxito" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            } 
        }

        /// <summary>
        /// Api para actualizar informaci´ñon de un contacto
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        ///     
        ///     PUT
        ///     {
        ///      "idContact": 1,
        ///     "nombre": "david",
        ///     "apellidos": "Montes Rodriguez",
        ///     "telefono": "5534443491",
        ///     "tipoTel": "3",
        ///     "email": "davidmr_1312@hotmail.com"
        ///     }
        ///     
        /// Sample Response
        /// 
        ///     { 
        ///     "message": "El contacto se actualizo con éxito"
        ///     }
        /// </remarks>
        [HttpPut]
        [Route("UpdateCotact")]
        public async Task<IActionResult> UpdateCotact(ContactModel contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactDto result = await _contactBusisness.UpdateContact(_mapper.Map<ContactModel, ContactDto>(contact));
                    if (result.Validations.Count > 0)
                        return Ok(new { status = false, mensaje = result.Validations[0].ErrorMessage.ToString() });

                    return Ok(new { mesage = "El contacto se actualizo con éxito" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Api para eliminar un contacto
        /// </summary>
        /// <param name="idContact">Id del contacto</param>
        /// <returns>Mensaje de respuesta</returns>
        /// <remarks>
        /// Sample Request 
        /// 
        ///     DELETE
        ///     idContact = 5
        ///     
        /// Sample Response 
        /// 
        ///     {
        ///     "status": false,
        ///     "mensaje": "El contacto se elimino con éxito"
        ///     }
        /// </remarks>
        [HttpDelete]
        [Route("DeleteCotact")]
        public async Task<IActionResult> DeleteCotact(int idContact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactDto result = await _contactBusisness.DeleteContact(idContact);
                    if (result.Validations.Count > 0)
                        return Ok(new { status = false, mensaje = result.Validations[0].ErrorMessage.ToString() });

                    return Ok(new {status= true, message = "El contacto se elimino con éxito" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("SelectFristCotact")]
        public async Task<IActionResult> SelectFristCotact(int Idcontact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactDto result = await _contactBusisness.SelectFristContact(Idcontact);
                    if (result.Validations.Count > 0)
                        return Ok(new { status = false, mensaje = result.Validations[0].ErrorMessage.ToString() });

                    return Ok(new { mesage = "El contacto se elimino con éxito" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Api para consultar todos los contactos
        /// </summary>
        /// <returns>Lista de contactos</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST
        ///     El servicio no requiere parametros 
        ///     
        /// Sample Response
        /// 
        ///     [
        ///      {
        ///       "idContact": 1,
        ///       "nombre": "david",
        ///       "apellidos": "Montes Rodriguez",
        ///       "telefono": "5534443491",
        ///       "tipoTel": 3,
        ///       "email": "davidmr_1312@hotmail.com",
        ///       "nombreTipoTel": "Iphone",
        ///       "validations": [],
        ///       "isValid": true
        ///      }
        ///     ]
        /// </remarks>
        [HttpGet]
        [Route("SelectAllContacts")]
        public async Task<ActionResult<IList<ContactDto>>> SelectAllContacts()
        {
            try
            {
                if (ModelState.IsValid)
                {
                   IList<ContactDto> result = await _contactBusisness.SelectAllContacts();
                     
                    return Ok(result);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Api para consultar todos los tipos de teléfono 
        /// </summary>
        /// <returns>Retorna lista de tipos</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST
        ///     El servicio no requiere parametros 
        ///     
        /// Sample Response
        /// 
        ///     [
        ///      {
        ///       "idTipoTel": 1,
        ///       "nombre": "Celular"
        ///      },
        ///      {
        ///       "idTipoTel": 2,
        ///       "nombre": "Casa"
        ///      },
        ///      {
        ///       "idTipoTel": 3,
        ///        "nombre": "Iphone"
        ///      },
        ///      {
        ///       "idTipoTel": 4,
        ///       "nombre": "Oficina"
        ///      }
        ///     ]
        /// </remarks>
        [HttpGet]
        [Route("SelectAllTypePhone")]
        public async Task<ActionResult<IList<TipoTelefonoDto>>> SelectAllTypePhone()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IList<TipoTelefonoDto> result = await _contactBusisness.SelectAllTypePhone();

                    return Ok(result);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Excepción en el controlador AdminAgendaController método InsertCotact:", ex);
                return BadRequest(ex);
            }
        }
    }
}
