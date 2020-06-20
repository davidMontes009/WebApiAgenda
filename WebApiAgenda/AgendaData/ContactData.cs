using Agenda.Shared;
using Agenda.Shared.Enums;
using AgendaEntities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;

namespace AgendaData.Contracts
{
    public class ContactData: IContactData
    {
        private readonly IConfiguration _configuration;
        public ContactData(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Metodo para registrar un contacto 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        public async Task<ContactDto> InsertContact(ContactDto contact)
        {
            try
            {
                ContactDto result = new ContactDto();
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_CONTACT"],OperationBD.INSERT))
                        {
                            command.Parameters.Add(new SqlParameter() { ParameterName = "@NOMBRE", SqlDbType = SqlDbType.VarChar, Value = contact.Nombre });

                            if (contact.Apellidos == "")
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@APELLIDOS", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });
                            else
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@APELLIDOS", SqlDbType = SqlDbType.VarChar, Value = contact.Apellidos });

                            command.Parameters.Add(new SqlParameter() { ParameterName = "@TELEFONO", SqlDbType = SqlDbType.VarChar, Value = contact.Telefono });

                            command.Parameters.Add(new SqlParameter() { ParameterName = "@TIPO_TEL", SqlDbType = SqlDbType.VarChar, Value = contact.TipoTel });

                            if (contact.email == "")
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@EMAIL", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });
                            else
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@EMAIL", SqlDbType = SqlDbType.VarChar, Value = contact.email });

                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet);

                            ///Mapea errores 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                connection.Close();
                                ErrorViewModel errorView = new ErrorViewModel()
                                {
                                    ErrorMessage = row.ItemArray[0].ToString()
                                };

                                result.Validations.Add(errorView);
                                return await Task.FromResult(result);
                            }

                            ///Mapea respuesta 
                            foreach (DataRow row in _dataSet.Tables[1].Rows)
                            {
                                result = new ContactDto()
                                {
                                    IdContact = int.Parse(row.ItemArray[0].ToString())
                                };
                            }
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para actualizar un contacto 
        /// </summary>
        /// <param name="contact">Modelo con la info del contacto a actualizar </param>
        /// <returns>Retorna info del usuario en caso de exito o erroe a mostrar </returns>
        public async Task<ContactDto> UpdateContact(ContactDto contact)
        {
            try
            {
                ContactDto result = new ContactDto();
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_CONTACT"], OperationBD.UPDATE))
                        {
                            command.Parameters.Add(new SqlParameter() { ParameterName = "@ID_CONTACTO", SqlDbType = SqlDbType.VarChar, Value = contact.IdContact });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "@NOMBRE", SqlDbType = SqlDbType.VarChar, Value = contact.Nombre });

                            if (contact.Apellidos == "")
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@APELLIDOS", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });
                            else
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@APELLIDOS", SqlDbType = SqlDbType.VarChar, Value = contact.Apellidos });

                            command.Parameters.Add(new SqlParameter() { ParameterName = "@TELEFONO", SqlDbType = SqlDbType.VarChar, Value = contact.Telefono });

                            command.Parameters.Add(new SqlParameter() { ParameterName = "@TIPO_TEL", SqlDbType = SqlDbType.VarChar, Value = contact.TipoTel });

                            if (contact.email == "")
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@EMAIL", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });
                            else
                                command.Parameters.Add(new SqlParameter() { ParameterName = "@EMAIL", SqlDbType = SqlDbType.VarChar, Value = contact.email });


                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet);

                            ///Mapea errores 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                connection.Close();
                                ErrorViewModel errorView = new ErrorViewModel()
                                {
                                    ErrorMessage = row.ItemArray[0].ToString()
                                };

                                result.Validations.Add(errorView);
                                return await Task.FromResult(result);
                            }

                            ///Mapea respuesta 
                            foreach (DataRow row in _dataSet.Tables[1].Rows)
                            {
                                result = new ContactDto()
                                {
                                    IdContact = int.Parse(row.ItemArray[0].ToString())
                                };
                            }
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para eliminar un contacto 
        /// </summary>
        /// <param name="IdContact">Id del contacto a eliminar</param>
        /// <returns>Resultado de la accion</returns>
        public async Task<ContactDto> DeleteContact(int IdContact)
        {
            try
            {
                ContactDto result = new ContactDto();
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_CONTACT"], OperationBD.DELETE))
                        {
                            command.Parameters.Add(new SqlParameter() { ParameterName = "@ID_CONTACTO", SqlDbType = SqlDbType.Int, Value = IdContact });


                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet);

                            ///Mapea errores 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                connection.Close();
                                ErrorViewModel errorView = new ErrorViewModel()
                                {
                                    ErrorMessage = row.ItemArray[0].ToString()
                                };

                                result.Validations.Add(errorView);
                                return await Task.FromResult(result);
                            } 
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para seleccionar info de un solo contacto 
        /// </summary>
        /// <param name="IdContact">id del contacto</param>
        /// <returns>Resultado de la accion</returns>
        public async Task<ContactDto> SelectFristContact(int IdContact)
        {
            try
            {
                ContactDto result = new ContactDto();
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_CONTACT"], OperationBD.SELECTFIRST))
                        {
                            command.Parameters.Add(new SqlParameter() { ParameterName = "@ID_CONTACTO", SqlDbType = SqlDbType.VarChar, Value = IdContact });


                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet);

                            ///Mapea errores 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                connection.Close();
                                ErrorViewModel errorView = new ErrorViewModel()
                                {
                                    ErrorMessage = row.ItemArray[0].ToString()
                                };

                                result.Validations.Add(errorView);
                                return await Task.FromResult(result);
                            }

                            ///Mapea respuesta 
                            foreach (DataRow row in _dataSet.Tables[1].Rows)
                            {
                                result = new ContactDto()
                                {
                                    IdContact       = int.Parse(row.ItemArray[0].ToString()),
                                    Nombre          = row.ItemArray[1].ToString(),
                                    Apellidos       = row.ItemArray[2].ToString(),
                                    Telefono        = row.ItemArray[4].ToString(),
                                    TipoTel         = row.ItemArray[5].ToString(),
                                    email           = row.ItemArray[6].ToString(),
                                    nombreTipoTel   = row.ItemArray[7].ToString()
                                };
                            }
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para consultar todos los contactos 
        /// </summary>
        /// <returns>Lista de contactos</returns>
        public async Task<IList<ContactDto>> SelectAllContacts()
        {
            ContactDto constact = new ContactDto();
            IList<ContactDto> listContacts = new List<ContactDto>();
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_CONTACT"], OperationBD.SELECTALL))
                        { 

                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet); 

                            ///Mapea respuesta 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                constact = new ContactDto()
                                {
                                    IdContact       = int.Parse(row.ItemArray[0].ToString()),
                                    Nombre          = row.ItemArray[1].ToString(),
                                    Apellidos       = row.ItemArray[2].ToString(),
                                    Telefono        = row.ItemArray[3].ToString(),
                                    TipoTel         = row.ItemArray[4].ToString(),
                                    email           = row.ItemArray[5].ToString(),
                                    nombreTipoTel   = row.ItemArray[6].ToString()
                                };

                                listContacts.Add(constact);
                            }
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }
                return listContacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para consulat los tipos de telefonos 
        /// </summary>
        /// <returns>retorna lista de tipos</returns>
        public async Task<IList<TipoTelefonoDto>> SelectAllTypePhone()
        {
            TipoTelefonoDto typePhone = new TipoTelefonoDto();
            IList<TipoTelefonoDto> listTypePhone = new List<TipoTelefonoDto>();
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(15), TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = new SqlConnection(_configuration["ConnectionStrings:ConnectionDev"]))
                    {
                        using (var command = connection.CreateSpCommand(_configuration["StoresProcedures:SP_TIPO_TEL"], OperationBD.SELECTALL))
                        {

                            ///Valida que la conexion este abiertaSP_Usuario
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();

                            ///Inicializa nueva instancia de la clase SqlDataAdapter con el objeto SqlCommand como parametro 
                            var _adapter = new SqlDataAdapter(command);
                            var _dataSet = new DataSet();
                            _adapter.Fill(_dataSet);

                            ///Mapea respuesta 
                            foreach (DataRow row in _dataSet.Tables[0].Rows)
                            {
                                typePhone = new TipoTelefonoDto()
                                {
                                    idTipoTel = int.Parse(row.ItemArray[0].ToString()),
                                    nombre    = row.ItemArray[1].ToString()

                                };

                                listTypePhone.Add(typePhone);
                            }
                        }
                        connection.Close();
                    }
                    transactionScope.Complete();
                }
                return listTypePhone;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
